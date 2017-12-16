using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// holds information about the shopping cart
public class ShoppingCart_S1 {
	// cart item
	public class ShoppingCartItem_S1 {
		// fields
		private readonly int itemID;
		private int quantity;

		// constructors
		public ShoppingCartItem_S1(Item_S1 _item) : this(_item.ID) {}
		public ShoppingCartItem_S1(Item_S1 _item, int _quantity) : this(_item.ID, 0) {}
		public ShoppingCartItem_S1(int _itemID) : this(_itemID, 0) {}

		public ShoppingCartItem_S1(int _itemID, int _quantity) {
			itemID = _itemID;
			Quantity = _quantity;
		}

		// properties
		public int ItemID { get { return itemID; } }
		public int Quantity {
			get { return quantity; }
			set {
				if (value < 0) {
					throw new ArgumentException("quantity must be >= 0");
				}
				quantity = value;
			}
		}
	}

	// members
	private List<ShoppingCartItem_S1> items;

	// constructor
	public ShoppingCart_S1() {
		items = new List<ShoppingCartItem_S1>();
	}

	// property to get list of items
	public IReadOnlyCollection<ShoppingCartItem_S1> Items { get { return items.AsReadOnly(); } }

	// add to cart
	public bool Add(Item_S1 _item) {
		return Add(_item.ID, 1);
	}
	public bool Add(Item_S1 _item, int _quantity) {
		return Add(_item.ID, _quantity);
	}
	public bool Add(int _itemID) {
		return Add(_itemID, 1);
	}

	public bool Add(int _itemID, int _quantity) {
		// try to find in list
		foreach (ShoppingCartItem_S1 _item in items) {
			if (_item.ItemID == _itemID) {
				_item.Quantity += _quantity;
				return true;
			}
		}

		// add to list
		items.Add(new ShoppingCartItem_S1(_itemID, _quantity));
		return true;
	}

	
	// remove from cart
	public bool Remove(Item_S1 _item) {
		return Remove(_item.ID, 1);
	}
	public bool Remove(Item_S1 _item, int _quantity) {
		return Remove(_item.ID, _quantity);
	}
	public bool Remove(int _id) {
		return Remove(_id);
	}

	public bool Remove(int _id, int _quantity) {
		// try to find in list
		foreach (ShoppingCartItem_S1 _item in items) {
			if (_item.ItemID == _id) {
				_item.Quantity -= _quantity;
				if (_item.Quantity == 0) {
					items.Remove(_item);
				}
				return true;
			}
		}

		return false;
	}

	// get quantity of item
	public int Quantity(Item_S1 _item) {
		return Quantity(_item.ID);
	}
	public int Quantity(int _id) {
		foreach (ShoppingCartItem_S1 _item in items) {
			if (_item.ItemID == _id) {
				return (_item.Quantity);
			}
		}
		return 0;
	}
}