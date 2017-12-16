using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// Invoice Line Item class
[Serializable]
public class LineItem_AS3 {
	// constructor
	public LineItem_AS3(InventoryItem_AS3 _item, int _quantity) {
		Item = _item;
		Quantity = _quantity;
	}

	// fields
	public readonly InventoryItem_AS3 Item;
	public readonly int Quantity;

	// to string
	public override string ToString() {
		return string.Format("{0}x{1}", Item.SKU, Quantity);
	}
}