using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;

public partial class _Default : System.Web.UI.Page {
	// lists of customers/invoice line items
	private List<CustomerAS2> customers;
	private List<InvoiceLineItemAS2> invoiceLineItems;

	// page load event
	protected void Page_Load(object sender, EventArgs e) {
		if (!Page.IsPostBack) {
			// disable checkout button
			ButtonToCheckout.Enabled = false;

			// load tables from DB
			loadDB();
		} else {
			// load table values from view state
			loadViewState();
		}
	}

	// pre-render event
	protected void Page_PreRender(object sender, EventArgs e) {
		// nothing
	}

	// load tables from DB to member fields and into view state
	protected void loadDB() {
		// create connection string
		string _connectionString = "";

		// check if on local machine
		if (Request.UserHostAddress.ToString().Equals("::1")) {
			// Microsoft.ACE.OLEDB.12.0
			// E:\Documents\My Documents\Visual Studio 2015\WebSites\cscd379as2\Shop.accdb
			_connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=E:\Documents\My Documents\Visual Studio 2015\WebSites\cscd379as2\shop.mdb;Persist Security Info=False;";
		} else {
			// provider (azure)

			// Microsoft.Jet.OLEDB.4.0
			// ~\site\wwwroot\as2\Shop.accdb
			// D:\home\site\wwwroot\as2\Shop.accdb
			// |DataDirectory|\Shop.accdb	cannot go backwards through \..\

			// Provider=Microsoft.Jet.OLEDB.4.0;
			// Data Source=D:\home\site\wwwroot\as2\Shop.accdb;
			// Data Source = D:\home\site\wwwroot\as2\Shop.mdb;
			// Persist Security Info=False;
			_connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\home\site\wwwroot\as2\shop.mdb;Persist Security Info=False;";
		}

		// create connection
		OleDbConnection _con = new OleDbConnection();
		_con.ConnectionString = _connectionString;

		// attempt to open connection
		try {
			_con.Open();
		} catch (Exception _e) {
			Trace.Warn("Shop messages", "failed to open db connection", _e);
			return;
		}

		// load relevant tables to lists
		List<CustomerAS2> _customers;
		List<InvoiceLineItemAS2> _invoiceLineItems;
		try {
			// grab data from customers table
			_customers = loadRecords<CustomerAS2>(_con, "Customer");

			// grab data from invoice line items table
			_invoiceLineItems = loadRecords<InvoiceLineItemAS2>(_con, "InvoiceLineItem");
		} catch (Exception _e) {
			Trace.Warn("Shop messages", "failed to read a table", _e);

			_con.Close();
			return;
		}

		// close connection
		_con.Close();

		// set member fields
		customers = _customers;
		invoiceLineItems = _invoiceLineItems;

		// set view state
		setViewState();
	}

	// grab all records for given table
	protected List<T> loadRecords<T>(OleDbConnection _con, string _tableName) {
		// create query string
		string _query = string.Format("SELECT * FROM {0};", _tableName);

		// create command obj
		OleDbCommand _cmd = new OleDbCommand(_query, _con);

		// execute command
		OleDbDataReader _reader = _cmd.ExecuteReader();

		// build return list
		List<T> _list = new List<T>();
		while (_reader.Read()) {
			// create instance of T through activator
			T _obj = (T)Activator.CreateInstance(typeof(T), _reader);
			_list.Add(_obj);
		}

		_reader.Close();

		return _list;
	}

	// sets tables to view state
	protected void setViewState() {
		ViewState["customers"] = customers;
		ViewState["invoicelineitems"] = invoiceLineItems;
	}

	// load tables from view state, store to member fields
	protected void loadViewState() {
		try {
			customers = (List<CustomerAS2>)ViewState["customers"];
			invoiceLineItems = (List<InvoiceLineItemAS2>)ViewState["invoicelineitems"];
		} catch (Exception _e) {
			Trace.Warn("Shop messages", "failed to read a table from viewstate", _e);
		}
	}

	// build display table from member lists
	protected void buildTables() {
		// ensure lists exist
		if (customers == null) {
			Trace.Warn("Build tables", "customers null");
			return;
		}
		if (invoiceLineItems == null) {
			Trace.Warn("Build tables", "invoices null");
			return;
		}

		// get appropriate controls
		// customers
		Label _labelCustomers = getCreateLabel("LabelCustomerList", string.Format("Customers ({0})", customers.Count), 100, 30);
		ListBox _listCustomers = getCreateListBox("ListBoxCustomers", 500, 100);
		// invoice line items
		Label _labelInvoices = getCreateLabel("LabelInvoiceList", string.Format("Invoice line items ({0})", invoiceLineItems.Count), 140, 30);
		ListBox _listInvoices = getCreateListBox("ListBoxInvoices", 300, 100);

		// wipe lists
		_listCustomers.Items.Clear();
		_listInvoices.Items.Clear();

		// populate lists
		foreach (CustomerAS2 _c in customers) {
			_listCustomers.Items.Add(_c.ToString());
		}

		foreach (InvoiceLineItemAS2 _ilt in invoiceLineItems) {
			_listInvoices.Items.Add(_ilt.ToString());
		}
	}

	// get label, create if does not exist
	protected Label getCreateLabel(string _id, string _text, Unit _width, Unit _height) {
		// check if exists
		Label _label = (Label)Page.FindControl(_id);
		if (_label == null) {
			// create
			_label = new Label();

			// add to bottom form
			divBottom.Controls.Add(_label);
			// add linebreak
			//divBottom.Controls.Add(new Literal() { Text = "<hr />" });
			divBottom.Controls.Add(new Literal() { Text = "<br />" });
		}

		// set properties
		_label.ID = _id;
		_label.Text = _text;
		_label.Width = _width;
		_label.Height = _height;

		return _label;
	}

	// get list box, create if does not exist
	protected ListBox getCreateListBox(string _id, Unit _width, Unit _height) {
		// check if exists
		ListBox _list = (ListBox)Page.FindControl(_id);
		if (_list == null) {
			// create
			_list = new ListBox();

			// add to bottom form
			divBottom.Controls.Add(_list);
			// add linebreak
			//divBottom.Controls.Add(new Literal() { Text = "<hr />" });
			divBottom.Controls.Add(new Literal() { Text = "<br />" });
		}

		// set properties
		_list.ID = _id;
		_list.Width = _width;
		_list.Height = _height;

		return _list;
	}

	// ButtonLoadDB
	// ButtonToCheckout

	// click event for "Load DB" button
	protected void ClickLoad(object sender, EventArgs e) {
		// enable to checkout button
		ButtonToCheckout.Enabled = true;

		// build controls to display values
		buildTables();
	}

	// click event for "To Checkout" button
	protected void ClickCheckout(object sender, EventArgs e) {
		// transfer to checkout page
		Server.Transfer("Shop.aspx", true);
	}

	// returns internal list of customers
	public List<CustomerAS2> getCustomers() {
		return customers;
	}

	// returns internal list of invoice line items
	public List<InvoiceLineItemAS2> getInvoices() {
		return invoiceLineItems;
	}
}