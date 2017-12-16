using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Globalization;
using System.Data.Common;
using System.Drawing;

// formMain

public partial class _Default : System.Web.UI.Page {
	// background color for the table
	private readonly Color COLOR_TABLE_BACK = Color.CadetBlue;


	// Page_Load event
	protected void Page_Load(object sender, EventArgs e) {
		if (!Page.IsPostBack) {
			// if not a postback

			// load info from DB
			LoadDB();

			// build table
			BuildTable();

			// debug table
			//BuildDebugTable();
		}
	}

	// builds final table to display
	private void BuildTable() {
		// sort invoices
		invoices.Sort();

		// totals
		int _totalOrders = 0;
		double _totalSales = 0;

		// build table
		Table _table = new Table();
		_table.EnableViewState = true;
		_table.Width = 1500;
		_table.BackColor = COLOR_TABLE_BACK;

		AddBlankRow(_table, COLOR_TABLE_BACK);

		// build section for each invoice
		foreach (Invoice_AS3 _i in invoices) {
			_totalOrders++;
			_totalSales += _i.TotalCost;
			AddInvoiceSection(_table, _i);
		}

		AddBlankRow(_table, COLOR_TABLE_BACK);

		// build totals section
		AddTotals(_table, _totalOrders, _totalSales);

		// add table to page
		contentArea.Controls.Add(_table);
		contentArea.Controls.Add(new Literal() { Text = "<br />" });
	}

	// adds totals section
	private void AddTotals(Table _table, int _orders, double _sales) {
		// Main title row
		TableRow _row = new TableRow();
		_row.BackColor = Color.White;

		// build text
		string _cellStr = @"<table>
				<tr>
					<td>Orders:</td>
				</tr>
				<tr >
					<td width=""5%"" />
					<td width=""95%"" align=""center"">
					<table cellpadding=""10"">
						<tr>
							<td width=""30%"" />
							<td width=""20%"">{0}</td>
							<td width=""20%"">{1}</td>
							<td width=""20%"">{2}</td>
						</tr>
						<tr>
							<td width=""30%"" />
							<td width=""20%"" align=""center"">{3}</td>
							<td width=""20%"" align=""center"">{4:c2}</td>
							<td width=""20%"" align=""center"">{5:c2}</td>
						</tr>
					</table>
					</td>
				</tr>
			</table>";

		string _cellTable = string.Format(_cellStr, "Total Orders", "Total Sales", "Sales Avg.", _orders, _sales, _sales / _orders);

		TableCell _cellMain = CreateCell(_cellTable, Color.White, HorizontalAlign.Left);
		_cellMain.ColumnSpan = 8;
		_cellMain.RowSpan = 1;
		_cellMain.BorderStyle = BorderStyle.Solid;
		_cellMain.BorderWidth = 2;
		_cellMain.Width = 1500;

		_row.Cells.Add(_cellMain);
		_table.Rows.Add(_row);
		
	}

	// builds section for an invoice
	private void AddInvoiceSection(Table _table, Invoice_AS3 _invoice) {

		// build top row
		AddInvoiceTopRow(_table, _invoice);

		// build address rows
		AddAddressRows(_table, _invoice);

		// build items section
		AddItemsTop(_table);
		
		// add lines
		foreach(LineItem_AS3 _line in _invoice.LineItems) {
			AddItemLine(_table, _line);
		}

		// add total
		AddItemLineTotal(_table, _invoice);
	}

	// builds top row for invoice section
	private void AddInvoiceTopRow(Table _table, Invoice_AS3 _invoice) {
		TableRow _row = new TableRow();

		Customer_AS3 _customer = _invoice.Customer;

		string _strCustomer = string.Format("Customer #{0} - {1}", _customer.Number, (_customer.Company.Equals("None", StringComparison.OrdinalIgnoreCase)) ? (_customer.Contact) : (_customer.Company));
		string _strOrder = string.Format("Order #{0}", _invoice.Number);
		string _strStatus = string.Format("Status: {0}", _invoice.Status);
		string _strOrdered = string.Format("Ordered: {0}", _invoice.Ordered.ToString("M/d/yyyy HH:mm:ss tt"));

		_row.Cells.Add(CreateCell(_strCustomer, Color.White, HorizontalAlign.Left, BorderStyle.Solid, 2));
		_row.Cells.Add(CreateCell(_strOrder, Color.White, HorizontalAlign.Left, BorderStyle.Solid, 2));
		_row.Cells.Add(CreateCell(_strStatus, Color.White, HorizontalAlign.Left, BorderStyle.Solid, 2));
		_row.Cells.Add(CreateCell(_strOrdered, Color.White, HorizontalAlign.Left, BorderStyle.Solid, 2));

		_table.Rows.Add(_row);
	}

	// adds address rows
	private void AddAddressRows(Table _table, Invoice_AS3 _invoice) {
		Address_AS3 _addrBill = _invoice.Customer.AddrBill;
		Address_AS3 _addrShip = _invoice.Customer.AddrShip;

		// Billing
		TableRow _rowBill = new TableRow();
		_rowBill.Cells.Add(CreateCell("Billing: "));
		_rowBill.Cells.Add(CreateCell(string.Format("{0} {1}, {2} {3}", _addrBill.Street, _addrBill.City, _addrBill.State, _addrBill.Zip)));

		// Shipping
		TableRow _rowShip = new TableRow();
		_rowShip.Cells.Add(CreateCell("Shipping: "));
		_rowShip.Cells.Add(CreateCell(string.Format("{0} {1}, {2} {3}", _addrShip.Street, _addrShip.City, _addrShip.State, _addrShip.Zip)));

		_table.Rows.Add(_rowBill);
		_table.Rows.Add(_rowShip);
	}

	// adds top portion for items of invoice
	private void AddItemsTop(Table _table) {
		TableRow _row = new TableRow();

		_row.Cells.Add(CreateCell("SKU", Color.White, HorizontalAlign.Left));
		_row.Cells.Add(CreateCell("Description", Color.White, HorizontalAlign.Left));
		_row.Cells.Add(CreateCell("Price", Color.White, HorizontalAlign.Right));
		_row.Cells.Add(CreateCell("Quantity", Color.White, HorizontalAlign.Right));
		_row.Cells.Add(CreateCell("Weight", Color.White, HorizontalAlign.Right));
		_row.Cells.Add(CreateCell("Ext. Price", Color.White, HorizontalAlign.Right));
		_row.Cells.Add(CreateCell("Ext. Weight", Color.White, HorizontalAlign.Right));
		_row.Cells.Add(CreateCell("Total", Color.White, HorizontalAlign.Right));

		_table.Rows.Add(_row);
	}

	// adds item line to invoice portion
	private void AddItemLine(Table _table, LineItem_AS3 _line) {
		TableRow _row = new TableRow();

		InventoryItem_AS3 _item = _line.Item;

		_row.Cells.Add(CreateCell(string.Format("{0}", _item.SKU)));
		_row.Cells.Add(CreateCell(_item.Description));

		_row.Cells.Add(CreateCell(string.Format("{0:c2}", _item.UnitPrice), HorizontalAlign.Right));
		_row.Cells.Add(CreateCell(string.Format("{0}", _line.Quantity), HorizontalAlign.Right));
		_row.Cells.Add(CreateCell(string.Format("{0:f2}", (double) _item.UnitWeight), HorizontalAlign.Right));

		_row.Cells.Add(CreateCell(string.Format("{0:c2}", (double) _item.UnitPrice * _line.Quantity), HorizontalAlign.Right));
		_row.Cells.Add(CreateCell(string.Format("{0:f2}", (double) _item.UnitWeight * _line.Quantity), HorizontalAlign.Right));

		_table.Rows.Add(_row);
	}

	// adds total line for invoice
	private void AddItemLineTotal(Table _table, Invoice_AS3 _invoice) {
		TableRow _row = new TableRow();

		_row.Cells.Add(CreateCell(""));
		_row.Cells.Add(CreateCell(""));
		_row.Cells.Add(CreateCell(""));
		_row.Cells.Add(CreateCell(""));
		_row.Cells.Add(CreateCell(""));
		_row.Cells.Add(CreateCell(""));
		_row.Cells.Add(CreateCell(""));

		_row.Cells.Add(CreateCell(string.Format("{0:c2}", _invoice.TotalCost), COLOR_TABLE_BACK, HorizontalAlign.Right));

		_table.Rows.Add(_row);
	}

	// creates a cell
	// overload options
	private TableCell CreateCell(string _text) {
		return CreateCell(_text, COLOR_TABLE_BACK, HorizontalAlign.Left);
	}
	private TableCell CreateCell(string _text, HorizontalAlign _align) {
		return CreateCell(_text, COLOR_TABLE_BACK, _align);
	}
	private TableCell CreateCell(string _text, Color _color, HorizontalAlign _align, BorderStyle _style = BorderStyle.None) {
		return CreateCell(_text, _color, _align, _style, 0);
	}

	// Creates a cell, all parameters
	private TableCell CreateCell(string _text, Color _color, HorizontalAlign _align, BorderStyle _style, Unit _width) {
		TableCell _cell = new TableCell();
		_cell.Text = _text;
		_cell.BackColor = _color;
		_cell.BorderStyle = _style;
		_cell.BorderWidth = _width;
		_cell.HorizontalAlign = _align;
		return _cell;
	}

	// adds a blank row
	private void AddBlankRow(Table _table, Color _color) {
		TableRow _row = new TableRow();
		// ForeColor
		_row.BackColor = _color;

		// blank cell to use
		TableCell _blankCell = new TableCell();
		_blankCell.Text = "_";
		_blankCell.BackColor = _color;
		_blankCell.ForeColor = _color;

		_row.Cells.Add(_blankCell);

		_table.Rows.Add(_row);
	}

	// builds debug table from internal lists
	// shows values read in
	private void BuildDebugTable() {
		// invoices
		//CreateLabel(string.Format("Invoices({0})", invoices.Count), 150, 30);
		CreateListBox("Invoices", invoices);

		// customers
		//CreateLabel(string.Format("Customers({0})", customers.Count), 150, 30);
		CreateListBox("Customers", customers);

		// addresses
		//CreateLabel(string.Format("Addresses({0})", addresses.Count), 150, 30);
		CreateListBox("Addresses", addresses);

		// inventories
		//CreateLabel(string.Format("Inventories({0})", inventories.Count), 150, 30);
		CreateListBox("Inventories", inventories);

		// suppliers
		//CreateLabel(string.Format("Suppliers({0})", suppliers.Count), 150, 30);
		CreateListBox("Suppliers", suppliers);
	}

	// Create list box
	private void CreateListBox<T>(string _title, List<T> _items) {
		// create label
		Label _label = new Label();
		_label.Text = string.Format("{0}({1})", _title, _items.Count);
		_label.Width = 150;
		_label.Height = 30;
		contentArea.Controls.Add(_label);
		contentArea.Controls.Add(new Literal() { Text = "<br />" });

		// create list
		ListBox _list = new ListBox();
		_list.Width = 500;
		_list.Height = 200;
		contentArea.Controls.Add(_list);
		contentArea.Controls.Add(new Literal() { Text = "<br />" });

		// populate list
		foreach(T _t in _items) {
			_list.Items.Add(_t.ToString());
		}
	}

	// list of objects for caching from DB
	private List<Supplier_AS3> suppliers;
	private List<InventoryItem_AS3> inventories;
	private List<Address_AS3> addresses;
	private List<Customer_AS3> customers;
	private List<Invoice_AS3> invoices;
	// connection object
	private OleDbConnection connection;
	

	// load info from database
	// mainly invoices and related data
	private void LoadDB() {
		// setup cache
		suppliers = new List<Supplier_AS3>();
		inventories = new List<InventoryItem_AS3>();
		addresses = new List<Address_AS3>();
		customers = new List<Customer_AS3>();
		invoices = new List<Invoice_AS3>();

		// build connection
		string _conStr = BuildConnectionString();
		connection = new OleDbConnection(_conStr);

		// attempt to open connection
		try{
			connection.Open();
		}catch (Exception _e) {
			Trace.Warn("shop errors", "failed to open db connection", _e);
			return;
		}

		// grab all invoices
		//OleDbCommand _cmd = new OleDbCommand("SELECT * FROM Invoice;", connection);
		OleDbCommand _cmd = new OleDbCommand("SELECT * FROM Invoice where (Status = \"open\");", connection);
		OleDbDataReader _reader = _cmd.ExecuteReader();

		// read all records
		while (_reader.Read()) {
			if (CreateInvoice(_reader) == null) {
				Trace.Warn("shop errors", "Invoice record failed to parse");
			}
		}

		// close connection
		connection.Close();
	}

	// creates an invoice from given record
	// adds to list of invoices
	private Invoice_AS3 CreateInvoice(OleDbDataReader _reader) {
		// parse info from reader
		int _number = (int)_reader["InvoiceNumber"];
		int _customerID = (int)_reader["CustNumber"];
		string _dateOrderStr = (string)_reader["DateOrder"];
		string _dateShippedStr = (string)_reader["DateShipped"];
		string _status = (string)_reader["Status"];

		// parse date strings
		DateTime _dateOrdered, _dateShipped;

		// formats
		string[] formats = { "dd-MM-yyyy", "d-MM-yyyy", "dd-M-yyyy", "d-M-yyyy" };

		// CultureInfo.InvariantCulture
		if (!DateTime.TryParseExact(_dateOrderStr, formats, null, DateTimeStyles.None, out _dateOrdered)) {
			Trace.Warn("shop errors", string.Format("Invoice({0}), DateTime Ordered parse failed (\"{1}\")", _number, _dateOrderStr));
			return null;
		}

		if (!DateTime.TryParseExact(_dateShippedStr, formats, null, DateTimeStyles.None, out _dateShipped)) {
			Trace.Warn("shop errors", string.Format("Invoice({0}), DateTime Shipped parse failed (\"{1}\")", _number, _dateShippedStr));
			return null;
		}

		// grab customer
		Customer_AS3 _customer = GetCreateCustomer(_customerID);
		if (_customer == null) {
			Trace.Warn("shop errors", string.Format("Invoice({0}), failed to get customer #{1}", _number, _customerID));
			return null;
		}

		// grab list of items
		List<LineItem_AS3> _items = GetCreateLines(_number);
		if (_items == null) {
			Trace.Warn("shop errors", string.Format("Invoice({0}), failed to get lines", _number));
			return null;
		}

		// build obj, add to list
		Invoice_AS3 _invoice = new Invoice_AS3(_number, _customer, _items, _dateOrdered, _dateShipped, _status);
		invoices.Add(_invoice);

		return _invoice;
	}

	// gets customer from given ID from cache
	// will create if does not exist (grab from DB)
	private Customer_AS3 GetCreateCustomer(int _id) {
		// check if in customer list
		foreach (Customer_AS3 _c in customers) {
			if (_id == _c.Number) {
				return _c;
			}
		}

		// create customer
		return CreateCustomer(_id);
	}

	// gets customer from DB, creates object
	// uses given ID
	private Customer_AS3 CreateCustomer(int _id) {
		// build command, get reader
		string _query = string.Format("SELECT * FROM Customer WHERE (CustNumber = {0});", _id);
		OleDbCommand _cmd = new OleDbCommand(_query, connection);
		OleDbDataReader _reader = _cmd.ExecuteReader();

		if (!_reader.Read()) {
			Trace.Warn("shop errors", string.Format("Customer, failed to find record with id={0}", _id));
			return null;
		}

		// parse information
		int _number = (int)_reader["CustNumber"];
		int _addrShipID = (int)_reader["AddrShip"];
		int _addrBillID = (int)_reader["AddrBill"];
		string _company = (string)_reader["Company"];
		string _contact = (string)_reader["Contact"];
		string _phone = (string)_reader["Phone"];

		// get addresses
		Address_AS3 _addrShip = GetCreateAddress(_addrShipID);
		if (_addrShip == null) {
			Trace.Warn("shop errors", string.Format("Customer({0}), failed to parse shipping address ({1})", _id, _addrShipID));
			return null;
		}

		Address_AS3 _addrBill = GetCreateAddress(_addrBillID);
		if (_addrBill == null) {
			Trace.Warn("shop errors", string.Format("Customer({0}), failed to parse billing address ({1})", _id, _addrBillID));
			return null;
		}

		// build object, add to list
		Customer_AS3 _customer = new Customer_AS3(_number, _addrShip, _addrBill, _company, _contact, _phone);
		customers.Add(_customer);

		return _customer;
	}

	// takes address ID
	// if not in local cache, loads from DB
	private Address_AS3 GetCreateAddress(int _id) {
		// check cache
		foreach(Address_AS3 _a in addresses) {
			if (_a.ID == _id) {
				return _a;
			}
		}

		return CreateAddress(_id);
	}

	// grab address w/ ID from DB
	// build obj
	private Address_AS3 CreateAddress(int _id) {
		// build command, get reader
		string _query = string.Format("SELECT * FROM Address WHERE (AddressID = {0});", _id);
		OleDbCommand _cmd = new OleDbCommand(_query, connection);
		OleDbDataReader _reader = _cmd.ExecuteReader();

		if (!_reader.Read()) {
			Trace.Warn("shop errors", string.Format("Address, failed to find record with id={0}", _id));
			return null;
		}

		// parse information
		int _addrID = (int)_reader["AddressID"];
		string _street = (string)_reader["Street"];
		string _city = (string)_reader["City"];
		string _state = (string)_reader["State"];
		string _zip = (string)_reader["Zip"];

		// create, add to cache
		Address_AS3 _address = new Address_AS3(_addrID, _street, _city, _state, _zip);
		addresses.Add(_address);

		return _address;
	}

	// gets line items for given invoice number
	private List<LineItem_AS3> GetCreateLines(int _invoiceNum) {
		// build command, get reader
		string _query = string.Format("SELECT * FROM LineItem WHERE (InvoiceNumber = {0});", _invoiceNum);
		OleDbCommand _cmd = new OleDbCommand(_query, connection);
		OleDbDataReader _reader = _cmd.ExecuteReader();

		// Parse records, build lines list
		List<LineItem_AS3> _lines = new List<LineItem_AS3>();

		while (_reader.Read()) {
			LineItem_AS3 _line = CreateLine(_reader);
			if (_line == null) {
				Trace.Warn("shop errors", "failed to parse line record");
			}else{
				_lines.Add(_line);
			}
		}

		return _lines;
	}

	// creates line for given record
	private LineItem_AS3 CreateLine(DbDataReader _reader) {
		// parse out info
		int _invoiceNumber = (int)_reader["InvoiceNumber"];
		int _lineNumber = (int)_reader["LineNumber"];
		int _sku = (int)_reader["SKU"];
		int _quantity = (int)_reader["QuantityOrdered"];

		// get inventory item
		InventoryItem_AS3 _item = GetCreateItem(_sku);		
		if (_item == null) {
			Trace.Warn("shop errors", string.Format("Invoice({0})Line#{1}, failed to get item #{2}", _invoiceNumber, _lineNumber, _sku));
			return null;
		}

		return new LineItem_AS3(_item, _quantity);
	}

	// gets or creates inventory item
	// that matches given sku
	private InventoryItem_AS3 GetCreateItem(int _skuID) {
		// check if in cache
		foreach(InventoryItem_AS3 _i in inventories) {
			if (_i.SKU == _skuID) {
				return _i;
			}
		}

		return CreateItem(_skuID);
	}

	// creates inventory item for given SKU
	// loads info from DB
	private InventoryItem_AS3 CreateItem(int _skuID) {
		// build command, get reader
		string _query = string.Format("SELECT * FROM Inventory WHERE (SKU = {0});", _skuID);
		OleDbCommand _cmd = new OleDbCommand(_query, connection);
		OleDbDataReader _reader = _cmd.ExecuteReader();

		if (!_reader.Read()) {
			Trace.Warn("shop error", string.Format("InventoryItem, no record for SKU#{0}", _skuID));
			return null;
		}

		// parse info from reader
		// SKU, Description, QOH, UnitWeight, UnitPrice
		int _sku = (int)_reader["SKU"];
		string _description = (string)_reader["Description"];
		int _qoh = (int)_reader["QOH"];
		double _weight = (double)_reader["UnitWeight"];
		double _price = (double)_reader["UnitPrice"];

		// get list of suppliers
		List<Supplier_AS3> _suppliers = GetCreateSuppliers(_sku);

		InventoryItem_AS3 _item = new InventoryItem_AS3(_sku, _suppliers, _qoh, _description, _weight, _price);
		inventories.Add(_item);

		return _item;
	}

	// gets/creates a list of suppliers that provide the given SKU
	private List<Supplier_AS3> GetCreateSuppliers(int _sku) {
		// build command, get reader
		string _query = string.Format("SELECT * FROM Supplied WHERE (SKU = {0});", _sku);
		OleDbCommand _cmd = new OleDbCommand(_query, connection);
		OleDbDataReader _reader = _cmd.ExecuteReader();

		// Parse records, build lines list
		List<Supplier_AS3> _suppliers = new List<Supplier_AS3>();

		while (_reader.Read()) {
			// parse out the SupplierID
			int _supplierID = (int)_reader["SupplierID"];

			Supplier_AS3 _supplier = GetCreateSupplier(_supplierID);
			if (_supplier == null) {
				Trace.Warn("shop errors", "failed to parse line record");
			} else {
				_suppliers.Add(_supplier);
			}
		}

		return _suppliers;
	}

	// gets supplier from cache or DB
	private Supplier_AS3 GetCreateSupplier(int _id) {
		// check cache
		foreach(Supplier_AS3 _s in suppliers) {
			if (_s.ID == _id) {
				return _s;
			}
		}

		// create
		return CreateSupplier(_id);
	}

	// grabs supplier from DB, builds obj
	private Supplier_AS3 CreateSupplier(int _id) {
		// build command, get reader
		string _query = string.Format("SELECT * FROM Supplier WHERE (SupplierID = {0});", _id);
		OleDbCommand _cmd = new OleDbCommand(_query, connection);
		OleDbDataReader _reader = _cmd.ExecuteReader();

		if (!_reader.Read()) {
			Trace.Warn("shop errors", string.Format("No supplier record for id={0}", _id));
			return null;
		}

		// parse information
		// SupplierID, Company, Contact, Phone, AreaCode
		int _supplierID = (int)_reader["SupplierID"];
		string _company = (string)_reader["Company"];
		string _contact = (string)_reader["Contact"];
		string _phone = (string)_reader["Phone"];
		string _area = (string)_reader["AreaCode"];

		// build obj, add to list
		Supplier_AS3 _supplier = new Supplier_AS3(_supplierID, _company, _contact, _phone, _area);
		suppliers.Add(_supplier);

		return _supplier;
	}

	// builds connection string to use
	private string BuildConnectionString() {
		// check if on local machine
		if (Request.UserHostAddress.ToString().Equals("::1")) {
			// local machine

			// Provider=Microsoft.Jet.OLEDB.4.0;
			// Data Source=E:\Documents\My Documents\Visual Studio 2015\WebSites\cscd379as3\shop.mdb;
			// Persist Security Info=False;
			return @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=E:\Documents\My Documents\Visual Studio 2015\WebSites\cscd379as3\shop.mdb;Persist Security Info=False;";
		} else {
			// provider (azure)

			// Provider=Microsoft.Jet.OLEDB.4.0;
			// Data Source=D:\home\site\wwwroot\as3\shop.mdb;
			// Persist Security Info=False;
			return @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\home\site\wwwroot\as3\shop.mdb;Persist Security Info=False;";
		}
	}
}