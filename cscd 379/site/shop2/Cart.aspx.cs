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

	// adds checkout button
	private void AddCheckoutButton() {
		Button _button = new Button();
		_button.ID = "buttonCheckout";
		_button.CssClass = "Button";
		_button.Text = "To Checkout";
		_button.PostBackUrl = "./Checkout.aspx";

	}

	// builds cart table
	private void BuildCartTable() {
		// get cart from master page
		ShoppingCart_S2 _cart = (ShoppingCart_S2) ((ShopMaster)Master).GetCart();

		// get items
		IReadOnlyCollection<ShoppingCart_S2.ShoppingCartItem_S2> _items = _cart.Items;

		// check if empty
		if(_items.Count > 0) {
			PopulateTable(_items);
		}else{
			DisplayEmptyCart();
		}
	}

	// fills cart table
	private void PopulateTable(IReadOnlyCollection<ShoppingCart_S2.ShoppingCartItem_S2> _items) {
		TableHeaderRow _rowh;
		TableRow _row;
		TableHeaderCell _cellh;
		TableCell _cell;

		// build title row
		_rowh = new TableHeaderRow();
		_rowh.CssClass = "TitleRow";

		// id
		_cellh = new TableHeaderCell();
		_cellh.CssClass = "TitleCell";
		_cellh.Text = "ID";
		_rowh.Cells.Add(_cellh);

		// name
		_cellh = new TableHeaderCell();
		_cellh.CssClass = "TitleCell";
		_cellh.Text = "Name";
		_rowh.Cells.Add(_cellh);

		// price
		_cellh = new TableHeaderCell();
		_cellh.CssClass = "TitleCell";
		_cellh.Text = "Unit Price";
		_rowh.Cells.Add(_cellh);

		// weight
		_cellh = new TableHeaderCell();
		_cellh.CssClass = "TitleCell";
		_cellh.Text = "Unit Weight";
		_rowh.Cells.Add(_cellh);

		// quantity
		_cellh = new TableHeaderCell();
		_cellh.CssClass = "TitleCell";
		_cellh.Text = "Quantity";
		_rowh.Cells.Add(_cellh);

		// ext price
		_cellh = new TableHeaderCell();
		_cellh.CssClass = "TitleCell";
		_cellh.Text = "Ext. Price";
		_rowh.Cells.Add(_cellh);

		// ext weight
		_cellh = new TableHeaderCell();
		_cellh.CssClass = "TitleCell";
		_cellh.Text = "Ext. Weight";
		_rowh.Cells.Add(_cellh);

		// button remove
		_cellh = new TableHeaderCell();
		_cellh.CssClass = "TitleCell";
		_cellh.Text = " ";
		_rowh.Cells.Add(_cellh);

		// button update
		_cellh = new TableHeaderCell();
		_cellh.CssClass = "TitleCell";
		_cellh.Text = " ";
		_rowh.Cells.Add(_cellh);

		tableCart.Rows.Add(_rowh);


		// populate table rows
		foreach (ShoppingCart_S2.ShoppingCartItem_S2 _cartItem in _items) {
			// get item
			Item_S2 _item = (Item_S2)((ShopMaster)Master).GetItem(_cartItem.ItemID);

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

			// weight
			_cell = new TableCell();
			_cell.CssClass = "BasicCell";
			_cell.Text = _item.Weight.ToString("f2");
			_row.Cells.Add(_cell);

			// quantity
			_cell = new TableCell();
			_cell.CssClass = "BasicCell";
			//_cell.Text = _cartItem.Quantity.ToString();
			TextBox _textbox = new TextBox();
			_textbox.ID = string.Format("quantity{0}", _item.ID);
			_textbox.Text = _cartItem.Quantity.ToString();
			_textbox.Attributes.Add("onchange", string.Format("changedQuantity({0})", _item.ID));
			_cell.Controls.Add(_textbox);
			// add validators
			// required field
			RequiredFieldValidator _reqValidator = new RequiredFieldValidator();
			_reqValidator.ErrorMessage = "Missing quantity";
			_reqValidator.ID = string.Format("quantityReqValidator{0}", _item.ID);
			_reqValidator.ControlToValidate = _textbox.ID;
			_reqValidator.ValidationGroup = string.Format("quantityGroup{0}", _item.ID);
			//_reqValidator.Display = ValidatorDisplay.None;
			_cell.Controls.Add(new Literal() { Text = "<br/>" });
			_cell.Controls.Add(_reqValidator);
			// range validator
			RangeValidator _rangeValidator = new RangeValidator();
			_rangeValidator.ErrorMessage = "Invalid quantity range";
			_rangeValidator.ID = string.Format("quantityRangeValidator{0}", _item.ID);
			_rangeValidator.ControlToValidate = _textbox.ID;
			_rangeValidator.ValidationGroup = string.Format("quantityGroup{0}", _item.ID);
			_rangeValidator.Type = ValidationDataType.Integer;
			_rangeValidator.MinimumValue = "0";
			_rangeValidator.MaximumValue = "100";
			_cell.Controls.Add(new Literal() { Text = "<br/>" });
			_cell.Controls.Add(_rangeValidator);

			_row.Cells.Add(_cell);

			// ext price
			_cell = new TableCell();
			_cell.CssClass = "BasicCell";
			_cell.Text = (_item.Price * _cartItem.Quantity).ToString("C2");
			_row.Cells.Add(_cell);

			// ext weight
			_cell = new TableCell();
			_cell.CssClass = "BasicCell";
			_cell.Text = (_item.Weight * _cartItem.Quantity).ToString("f2");
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

			// button for update
			_cell = new TableCell();
			_cell.CssClass = "ButtonCell";
			_button = new Button();
			_button.ID = string.Format("buttonUpdate{0}", _item.ID);
			_button.CssClass = "Button";
			_button.Text = "Update";
			_button.OnClientClick = string.Format("return updateCheck({0});", _item.ID);
			_button.Command += CommandUpdate;
			_button.CommandName = "ItemID";
			_button.CommandArgument = _item.ID.ToString();
			// do not use Enabled, removes on client click
			//_button.Enabled = false;
			_button.Attributes.Add("disabled", "disabled");

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

			ShoppingCart_S2 _cart = (ShoppingCart_S2)((ShopMaster)Master).GetCart();
			if (_cart.Remove(_id)) {
				Trace.Warn("cart", string.Format("removed id:{0}", _id));
			}else{
				Trace.Warn("cart", string.Format("failed to remove id:{0}", _id));
			}
			
		}
	}

	// update button click
	private void CommandUpdate(object _sender, CommandEventArgs _e) {
		Trace.Warn("cart", string.Format("CommandUpdate(\"{0}\":{1})", _e.CommandName, _e.CommandArgument));

		if (_e.CommandName.Equals("ItemID", StringComparison.OrdinalIgnoreCase)) {
			int _id = Int32.Parse((string)_e.CommandArgument);

			Trace.Write("Shop", string.Format("Updating id {0}", _id));

			// find row w/ item
			TableRowCollection _rows = tableCart.Rows;
			TableRow _row = null;
			int i = 1;
			while (i < _rows.Count) {
				_row = _rows[i];
				// get ID for row
				HyperLink _link = (HyperLink)_row.Cells[0].Controls[0];
				int _rowID = Int32.Parse(_link.Text);

				if (_id == _rowID) {
					break;
				}
				++i;
			}
			if (i == _rows.Count) {
				Trace.Warn("Shop Update", string.Format("Failed to find row w/ ID: {0}", _id));
				return;
			}

			// pull value from textbox
			int _quantity = Int32.Parse((((TextBox)(_row.Cells[4].Controls[0])).Text));
			Trace.Write("Cart", string.Format("Quantity read: {0}", _quantity));

			ShoppingCart_S2 _cart = (ShoppingCart_S2)((ShopMaster)Master).GetCart();
			if (_cart.Update(_id, _quantity)) {
				Trace.Write("cart", string.Format("updated id:{0}", _id));
			} else {
				Trace.Warn("cart", string.Format("failed to update id:{0}", _id));
			}

		}
	}

	// updates table
	private void RefreshCartTable() {
		// get cart
		ShoppingCart_S2 _cart = (ShoppingCart_S2)((ShopMaster)Master).GetCart();

		// cycle through each row, except the first one
		// find items w/ quantity of zero, remove row
		TableRowCollection _rows = tableCart.Rows;
		TableRow _row = null;
		int i = 1;
		while (i < _rows.Count) {
			_row = _rows[i];

			// get ID for row
			HyperLink _link = (HyperLink) _row.Cells[0].Controls[0];
			int _id = Int32.Parse(_link.Text);

			Trace.Write("Cart", string.Format("Refreshing row id: {0}", _id));

			int _actualQuantity = _cart.Quantity(_id);

			// check quantity
			if (_actualQuantity <= 0) {
				_row.Cells.Clear();
				_rows.Remove(_row);
				Trace.Warn("cart", string.Format("removed row id: {0}", _id));
			}else{
				// always refresh
				int _quantity = Int32.Parse((((TextBox)(_row.Cells[4].Controls[0])).Text));
				Trace.Write("Cart", string.Format("Quantity read: {0}", _quantity));

				// get item info
				Item_S2 _item = (Item_S2)((ShopMaster)Master).GetItem(_id);

				// update quantity and ext price/weight
				//_row.Cells[4].Text = _actualQuantity.ToString();
				_row.Cells[5].Text = (_item.Price * _actualQuantity).ToString("C2");
				_row.Cells[6].Text = (_item.Weight * _actualQuantity).ToString("f2");

				++i;
			}
		}

		// check row count
		if (tableCart.Rows.Count <= 1) {
			DisplayEmptyCart();
			return;
		}

		// calculate totals
		double _totalPrice = 0, _totalWeight = 0;
		foreach(ShoppingCart_S2.ShoppingCartItem_S2 _cartItem in _cart.Items) {
			// get item
			Item_S2 _item = (Item_S2)((ShopMaster)Master).GetItem(_cartItem.ItemID);

			_totalPrice += _item.Price * _cartItem.Quantity;
			_totalWeight += _item.Weight * _cartItem.Quantity;
		}

		// add totals row
		_row = new TableRow();
		_row.CssClass = "TotalRow";
		// populate empty cells
		TableCell _cell = null;
		for (i = 0; i < 5; ++i) {
			_cell = new TableCell();
			_cell.CssClass = "EmptyCell";
			_cell.Text = " ";
			_row.Cells.Add(_cell);
		}
		// total ext price
		_cell = new TableCell();
		_cell.CssClass = "EmptyCell";
		_cell.Text = _totalPrice.ToString("C2");
		_row.Cells.Add(_cell);

		// total ext weight
		_cell = new TableCell();
		_cell.CssClass = "EmptyCell";
		_cell.Text = _totalWeight.ToString("f2");
		_row.Cells.Add(_cell);

		tableCart.Rows.Add(_row);
	}

	// setup for empty cart
	private void DisplayEmptyCart() {
		// empty table
		tableCart.Rows.Clear();

		labelCart.Text = "Cart is empty";
		labelCart.ForeColor = System.Drawing.Color.OrangeRed;
		tableCartContainer.Visible = false;
		buttonCheckout.Visible = false;
	}

}