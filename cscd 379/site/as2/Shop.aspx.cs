using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Shop : System.Web.UI.Page {
	// constants for table/cells properties
	// table style, cell style
	// individual widths ?
	private readonly BorderStyle TABLE_BORDER_STYLE = BorderStyle.Dashed;
	private readonly BorderStyle CELL_BORDER_STYLE = BorderStyle.Solid;
	private readonly Unit CELL_BORDER_WIDTH = 1;

	// lists of customers/invoice line items
	private List<CustomerAS2> customers;
	private List<InvoiceLineItemAS2> invoiceLineItems;

	// page load event
	protected void Page_Load(object sender, EventArgs e) {
		// check if not from a previous page
		if (PreviousPage == null) {
			LabelStatus.Text = "invalid, must reach this page from another";
			return;
		}

		// check if postback
		if (Page.IsPostBack) {
			LabelStatus.Text = "please don't do that";
			return;
		}

		// grab info from previous page
		loadFromPreviousPage();

		// build final table
		buildTable();
	}

	// grabs info from previous page
	private void loadFromPreviousPage() {
		// _Default _prevPage = (_Default) PreviousPage;
		_Default _prevPage = PreviousPage as _Default;
		customers = _prevPage.getCustomers();
		invoiceLineItems = _prevPage.getInvoices();
	}

	// creates List<dynamic> from customers and invoice line items lists
	// performs a full outer join on both, stores references into an anonymous type of (Customer, InvoiceLineItem)
	private List<dynamic> createTableList() {
		// dynamic list for anonymous types
		List<dynamic> _list = new List<dynamic>();

		// sort lists first
		customers.Sort();
		invoiceLineItems.Sort();

		// match customers up first
		foreach (CustomerAS2 _c in customers) {
			_list.Add(new { Customer = _c });

			// match to invoices
			var _invoices = from _inv in invoiceLineItems
							where _inv.CustomerID == _c.CustomerID
							select new { Customer = _c, Invoice = _inv };

			foreach (var _v in _invoices) {
				_list.Add(_v);
			}
		}

		// grab any remaining invoices
		var _invoicesRemaining = from _inv in invoiceLineItems
								 where !(
									 from _c in customers
									 select _c.CustomerID
									 ).Contains(_inv.CustomerID)
								 select new { Invoice = _inv };

		_list.AddRange(_invoicesRemaining);

		return _list;
	}

	// builds final table
	private void buildTable() {
		LabelStatus.Text = string.Format("customers({0}) invoices({1})", customers.Count, invoiceLineItems.Count);

		// get list of customers and invoices
		List<dynamic> _list = createTableList();

		// create table
		Table _table = new Table();
		_table.EnableViewState = true;
		//_table.BorderStyle = TABLE_BORDER_STYLE;

		// build title row
		_table.Rows.Add(createTitleRow());

		// populate
		foreach (var _v in _list) {
			// get values to fill table
			CustomerAS2 _c;
			InvoiceLineItemAS2 _inv;

			getValuesFromVar(out _c, out _inv, _v);

			// create row
			TableRow _row = new TableRow();
			_row.BorderStyle = BorderStyle.Solid;

			// fill with customer
			fillRow(_row, _c, _inv);

			_table.Rows.Add(_row);
		}

		// add linebreak to controls before
		dynamicControls.Controls.Add(new Literal() { Text = "<br />" });

		// add table to controls
		dynamicControls.Controls.Add(_table);
	}

	// gets customer/invoice from an anonymous type
	private void getValuesFromVar(out CustomerAS2 _customer, out InvoiceLineItemAS2 _invoice, dynamic _v) {
		// get customer
		try {
			_customer = _v.Customer;
		} catch (RuntimeBinderException) {
			_customer = null;
		}
		// get invoice line item
		try {
			_invoice = _v.Invoice;
		} catch (RuntimeBinderException) {
			_invoice = null;
		}
	}

	// creates title row
	private TableRow createTitleRow() {
		TableRow _row = new TableRow();

		// Customer ID, Name, Street, City, State, Zip
		// Order Number, Sequence Number, Item SKU, Quantity, Price, Weight, Shipping cost
		_row.Cells.Add(createCell("Customer ID"));
		_row.Cells.Add(createCell("Name"));
		_row.Cells.Add(createCell("Street"));
		_row.Cells.Add(createCell("City"));
		_row.Cells.Add(createCell("State"));
		_row.Cells.Add(createCell("Zip"));

		_row.Cells.Add(createCell("Order Number"));
		_row.Cells.Add(createCell("Sequence Number"));
		_row.Cells.Add(createCell("Item SKU"));
		_row.Cells.Add(createCell("Quantity"));
		_row.Cells.Add(createCell("Price"));
		_row.Cells.Add(createCell("Weight"));
		_row.Cells.Add(createCell("Shipping Cost"));

		return _row;
	}

	// fill row with cells for given customer/invoice
	// if either null, empty cells
	private void fillRow(TableRow _row, CustomerAS2 _customer, InvoiceLineItemAS2 _invoice) {
		// grab info
		string _custID = (_customer != null) ? (_customer.CustomerID.ToString()) : ((_invoice != null) ? (_invoice.CustomerID.ToString()) : (""));
		string _name = (_customer != null) ? (_customer.Name) : ("");
		string _street = (_customer != null) ? (_customer.Street) : ("");
		string _city = (_customer != null) ? (_customer.City) : ("");
		string _state = (_customer != null) ? (_customer.State) : ("");
		string _zip = (_customer != null) ? (_customer.Zip) : ("");

		string _ordNum = (_invoice != null) ? (_invoice.OrderNumber.ToString()) : ("");
		string _seqNum = (_invoice != null) ? (_invoice.SequenceNumber.ToString()) : ("");
		string _itemsku = (_invoice != null) ? (_invoice.ItemSKU.ToString()) : ("");
		string _quantity = (_invoice != null) ? (_invoice.Quantity.ToString()) : ("");
		string _price = (_invoice != null) ? (_invoice.Price.ToString()) : ("");
		string _weight = (_invoice != null) ? (_invoice.Weight.ToString()) : ("");
		string _shipping = (_invoice != null) ? (_invoice.Shipping.ToString()) : ("");

		// Customer ID, Name, Street, City, State, Zip
		// Order Number, Sequence Number, Item SKU, Quantity, Price, Weight, Shipping cost

		// build cells
		_row.Cells.Add(createCell(_custID));

		_row.Cells.Add(createCell(_name));
		_row.Cells.Add(createCell(_street));
		_row.Cells.Add(createCell(_city));
		_row.Cells.Add(createCell(_state));
		_row.Cells.Add(createCell(_zip));

		_row.Cells.Add(createCell(_ordNum));
		_row.Cells.Add(createCell(_seqNum));
		_row.Cells.Add(createCell(_itemsku));
		_row.Cells.Add(createCell(_quantity));
		_row.Cells.Add(createCell(_price));
		_row.Cells.Add(createCell(_weight));
		_row.Cells.Add(createCell(_shipping));
	}

	// create cell
	private TableCell createCell(string _value) {
		TableCell _cell = new TableCell();
		_cell.Text = _value;
		_cell.BorderStyle = CELL_BORDER_STYLE;
		_cell.BorderWidth = CELL_BORDER_WIDTH;

		return _cell;
	}

	// div dynamicControls
	// LabelStatus
}