using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

// page for viewing item information
public partial class Item : System.Web.UI.Page {

	// field for item being used
	private Item_S2 item;

	// page_load
	protected void Page_Load(object sender, EventArgs e) {
		// pull query string
		string _itemIDstr = Request.QueryString["id"];

		// check if a string was returned
		if (_itemIDstr == null) {
			Trace.Warn("Shop", "No query for Item ID");
			Response.Redirect("Browse.aspx", true);
			//return;
		}

		// try to parse to an int
		int _itemID;
		if (!Int32.TryParse(_itemIDstr, out _itemID)) {
			Trace.Warn("Shop", string.Format("Failed to parse item ID=\"{0}\"", _itemIDstr));
			Response.Redirect("Browse.aspx", true);
		}

		// attempt to get the item info
		try {
			item = (Item_S2)((ShopMaster)Master).GetItem(_itemID);
			ContentTitleTitle.Text = string.Format(": {0}", item.Name);
			BuildDisplay();
		} catch (Exception _e) {
			Trace.Warn("Shop", string.Format("Failed to fetch item ID={0}", _itemID), _e);
			Response.Redirect("Browse.aspx", true);
		}
	}

	// builds page for given item ID
	private void BuildDisplay() {
		Item_S2 _item = item;

		// image
		Image _image = ((ShopMaster)Master).GetItemImage(_item);
		_image.CssClass = "ItemImage";
		contentItemArea.Controls.Add(_image);

		// item info
		// table
		Table _table = new Table();
		_table.CssClass = "ItemInfo";
		_table.CellPadding = 5;
		_table.CellSpacing = 15;

		TableRow _row;
		TableCell _cell;

		// name
		_row = new TableRow();
		_cell = new TableCell();
		_cell.Text = _item.Name;

		_row.Cells.Add(_cell);
		_table.Rows.Add(_row);

		// weight
		_row = new TableRow();
		_cell = new TableCell();
		_cell.Text = string.Format("{0:f2}oz ({1:f2}lb)", _item.Weight, _item.Weight / 16);

		_row.Cells.Add(_cell);
		_table.Rows.Add(_row);

		// price
		_row = new TableRow();
		_cell = new TableCell();
		_cell.Text = string.Format("{0:c2}", _item.Price);

		_row.Cells.Add(_cell);
		_table.Rows.Add(_row);

		// buttons

		// add to cart (button)
		Button _toCart = new Button();
		_toCart.CssClass = "ItemButton";
		_toCart.Text = "Add to cart";
		_toCart.Click += ClickAddToCart;

		// view cart (button)
		Button _viewCart = new Button();
		_viewCart.CssClass = "ItemButton";
		_viewCart.Text = "View cart";
		_viewCart.Click += ClickToCart;

		// label for if add to cart response
		Label _labelMsg = new Label();
		_labelMsg.ID = "LabelCartAdded";

		_row = new TableRow();
		_cell = new TableCell();
		_cell.Controls.Add(_labelMsg);
		_cell.Controls.Add(_toCart);
		_cell.Controls.Add(_viewCart);

		_row.Cells.Add(_cell);
		_table.Rows.Add(_row);

		contentItemArea.Controls.Add(_table);
	}

	// on click for goto cart
	private void ClickToCart(object _sender, EventArgs _e) {
		Response.Redirect("Cart.aspx");
	}

	// click add to cart
	private void ClickAddToCart(object _sender, EventArgs _e) {
		// get cart
		ShoppingCart_S2 _cart = (ShoppingCart_S2) ((ShopMaster)Master).GetCart();

		// get label for response
		Label _response = (Label)contentItemArea.FindControl("LabelCartAdded");
		if (_response == null) {
			Trace.Warn("Shop", "Failed to find label for cart response");
			return;
		}

		// add item
		if (_cart.Add(item.ID)) {
			_response.Text = string.Format("Added to cart");
			_response.ForeColor = System.Drawing.Color.YellowGreen;
			Response.Redirect("Cart.aspx", true);
		}else{
			_response.Text = string.Format("Failed to add to cart");
			_response.ForeColor = System.Drawing.Color.Red;
		}
	}
}