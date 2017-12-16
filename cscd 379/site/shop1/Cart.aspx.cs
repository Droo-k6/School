using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// cart page
public partial class Cart : System.Web.UI.Page {

	// load
	protected void Page_Load(object sender, EventArgs e) {
		Trace.Warn("cart", "page_load");

		// build table
		BuildCartTable();
	}

	// pre-render
	// use instead of load so button events fire before
	protected void Page_PreRender(object _sender, EventArgs _e) {
		Trace.Warn("cart", "Page_PreRender");

		// refresh table
		RefreshCartTable();
	}

	// builds cart table
	private void BuildCartTable() {
		// get cart from master page
		ShoppingCart_S1 _cart = (ShoppingCart_S1) ((ShopMaster)Master).GetCart();

		// get items
		IReadOnlyCollection<ShoppingCart_S1.ShoppingCartItem_S1> _items = _cart.Items;

		// check if empty
		if(_items.Count > 0) {
			PopulateTable(_items);
		}else{
			// build message
			labelCart.Text = "Cart is empty";
			labelCart.ForeColor = System.Drawing.Color.OrangeRed;
			tableCartContainer.Visible = false;
		}
	}

	// fills cart table
	private void PopulateTable(IReadOnlyCollection<ShoppingCart_S1.ShoppingCartItem_S1> _items) {
		TableHeaderRow _rowh;
		TableRow _row;
		TableHeaderCell _cellh;
		TableCell _cell;

		// build title row
		_rowh = new TableHeaderRow();
		_rowh.CssClass = "TitleRow";

		_cellh = new TableHeaderCell();
		_cellh.CssClass = "TitleCell";
		_cellh.Text = "ID";
		_rowh.Cells.Add(_cellh);

		_cellh = new TableHeaderCell();
		_cellh.CssClass = "TitleCell";
		_cellh.Text = "Name";
		_rowh.Cells.Add(_cellh);

		_cellh = new TableHeaderCell();
		_cellh.CssClass = "TitleCell";
		_cellh.Text = "Price";
		_rowh.Cells.Add(_cellh);

		_cellh = new TableHeaderCell();
		_cellh.CssClass = "TitleCell";
		_cellh.Text = "Quantity";
		_rowh.Cells.Add(_cellh);

		_cellh = new TableHeaderCell();
		_cellh.CssClass = "TitleCell";
		_cellh.Text = " ";
		_rowh.Cells.Add(_cellh);

		tableCart.Rows.Add(_rowh);


		// populate table rows
		foreach (ShoppingCart_S1.ShoppingCartItem_S1 _cartItem in _items) {
			// get item
			Item_S1 _item = (Item_S1)((ShopMaster)Master).GetItem(_cartItem.ItemID);

			Trace.Warn("cart", string.Format("row for {0}", _cartItem.ItemID));

			_row = new TableRow();
			_row.CssClass = "ItemRow";

			// ID
			_cell = new TableCell();
			_cell.CssClass = "BasicCell";
			HyperLink _link = new HyperLink();
			_link.Text = _item.ID.ToString();
			_link.NavigateUrl = string.Format("Item.aspx?id={0}", _item.ID);
			_cell.Controls.Add(_link);
			_row.Cells.Add(_cell);

			// name
			_cell = new TableCell();
			_cell.CssClass = "BasicCell";
			_cell.Text = _item.Name;
			_row.Cells.Add(_cell);

			// price
			_cell = new TableCell();
			_cell.CssClass = "BasicCell";
			_cell.Text = _item.Price.ToString("C2");
			_row.Cells.Add(_cell);

			// quantity
			_cell = new TableCell();
			_cell.CssClass = "BasicCell";
			_cell.Text = _cartItem.Quantity.ToString();
			_row.Cells.Add(_cell);

			// button for remove
			_cell = new TableCell();
			_cell.CssClass = "ButtonCell";
			Button _button = new Button();
			_button.ID = string.Format("buttonRemove{0}", _item.ID);
			_button.CssClass = "Button";
			_button.Text = "Remove";

			_button.Command += CommandRemove;
			_button.CommandName = "ItemID";
			_button.CommandArgument = _item.ID.ToString();

			_cell.Controls.Add(_button);
			_row.Cells.Add(_cell);

			tableCart.Rows.Add(_row);
		}
	}

	// remove button click
	private void CommandRemove(object _sender, CommandEventArgs _e) {

		Trace.Warn("cart", string.Format("CommandRemove(\"{0}\":{1})", _e.CommandName, _e.CommandArgument));

		if (_e.CommandName.Equals("ItemID", StringComparison.OrdinalIgnoreCase)) {
			int _id = Int32.Parse((string)_e.CommandArgument);

			ShoppingCart_S1 _cart = (ShoppingCart_S1)((ShopMaster)Master).GetCart();
			if (_cart.Remove(_id, 1)) {
				Trace.Warn("cart", string.Format("removed id:{0}", _id));
			}else{
				Trace.Warn("cart", string.Format("failed to remove id:{0}", _id));
			}
			
		}
	}

	// updates table
	private void RefreshCartTable() {
		// get cart
		ShoppingCart_S1 _cart = (ShoppingCart_S1)((ShopMaster)Master).GetCart();

		// cycle through each row, except the first one
		// find items w/ quantity of zero, remove row
		TableRowCollection _rows = tableCart.Rows;
		int i = 1;
		while (i < _rows.Count) {
			TableRow _row = _rows[i];

			// get ID for row
			HyperLink _link = (HyperLink) _row.Cells[0].Controls[0];
			int _id = Int32.Parse(_link.Text);

			int _actualQuantity = _cart.Quantity(_id);

			// check quantity
			if (_actualQuantity <= 0) {
				_row.Cells.Clear();
				_rows.Remove(_row);
				Trace.Warn("cart", string.Format("removed row id: {0}", _id));
			}else{
				// check if quantity changed
				int _quantity = Int32.Parse((_row.Cells[3].Text));
				if (_actualQuantity != _quantity) {
					_row.Cells[3].Text = _actualQuantity.ToString();
				}

				++i;
			}
		}

		// check row count
		if (tableCart.Rows.Count <= 1) {
			// empty table
			tableCart.Rows.Clear();

			labelCart.Text = "Cart is empty";
			labelCart.ForeColor = System.Drawing.Color.OrangeRed;
			tableCartContainer.Visible = false;
		}
	}

}