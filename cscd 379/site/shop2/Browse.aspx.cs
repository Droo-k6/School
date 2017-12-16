using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


// Browse page
public partial class Browse : System.Web.UI.Page {

	// page load
	protected void Page_Load(object sender, EventArgs e) {
		// build items
		BuildItemsTable();
	}

	// builds items table
	private void BuildItemsTable() {
		// get items list
		List<object> _items = ((ShopMaster)Master).GetItems();

		// current row
		TableRow _row = new TableRow();
		tableItems.Rows.Add(_row);

		foreach (object _o in _items) {
			Item_S2 _item = (Item_S2)_o;

			TableCell _cell = CreateCell(_item);

			// check cell count of row
			if (_row.Cells.Count >= 5) {
				// check row count
				if (tableItems.Rows.Count >= 5) {
					break;
				}
				_row = new TableRow();
				tableItems.Rows.Add(_row);
			}

			// add cell
			_row.Cells.Add(_cell);
		}
	}

	// creates table cell
	private TableCell CreateCell(Item_S2 _item) {
		// ItemCell, CellImage, CellText

		TableCell _cell = new TableCell();
		_cell.CssClass = "ItemCell";
		_cell.Width = 340;

		// thumbnail
		Image _image = ((ShopMaster)Master).GetItemImageThumb(_item);
		_image.CssClass = "CellImage";

		_cell.Controls.Add(_image);

		// textlink
		HyperLink _link = new HyperLink();
		_link.CssClass = "CellText";
		_link.Text = string.Format("{0}<br/>{1:c2}", _item.Name, _item.Price);
		_link.NavigateUrl = string.Format("Item.aspx?id={0}", _item.ID);

		_cell.Controls.Add(_link);

		return _cell;
	}

}