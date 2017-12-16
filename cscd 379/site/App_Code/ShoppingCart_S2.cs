using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// holds information about the shopping cart
public class ShoppingCart_S2 {
	// cart item
	public class ShoppingCartItem_S2 {
		// fields
		private readonly int itemID;
		private int quantity;

		// constructors
		public ShoppingCartItem_S2(Item_S2 _item) : this(_item.ID) {}
		public ShoppingCartItem_S2(Item_S2 _item, int _quantity) : this(_item.ID, 0) {}
		public ShoppingCartItem_S2(int _itemID) : this(_itemID, 0) {}

		public ShoppingCartItem_S2(int _itemID, int _quantity) {
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
	private List<ShoppingCartItem_S2> items;

	// constructor
	public ShoppingCart_S2() {
		items = new List<ShoppingCartItem_S2>();
	}

	// property to get list of items
	public IReadOnlyCollection<ShoppingCartItem_S2> Items { get { return items.AsReadOnly(); } }

	// property to get specific item by id
	public ShoppingCartItem_S2 this[int id] {
		get {
			foreach(ShoppingCartItem_S2 _item in items) {
				if (_item.ItemID == id) {
					return _item;
				}
			}
			return null;
		}
	}

	// add to cart
	public bool Add(Item_S2 _item) {
		return Add(_item.ID, 1);
	}
	public bool Add(Item_S2 _item, int _quantity) {
		return Add(_item.ID, _quantity);
	}
	public bool Add(int _itemID) {
		return Add(_itemID, 1);
	}

	public bool Add(int _itemID, int _quantity) {
		// try to find in list
		foreach (ShoppingCartItem_S2 _item in items) {
			if (_item.ItemID == _itemID) {
				_item.Quantity += _quantity;
				return true;
			}
		}

		// add to list
		items.Add(new ShoppingCartItem_S2(_itemID, _quantity));
		return true;
	}

	
	// remove from cart
	public bool Remove(Item_S2 _item) {
		return Update(_item.ID, 0);
	}
	public bool Remove(int _id) {
		return Update(_id, 0);
	}
	public bool Remove(Item_S2 _item, int _quantity) {
		return Remove(_item.ID, _quantity);
	}
	public bool Remove(int _id, int _quantity) {
		// try to find in list
		foreach (ShoppingCartItem_S2 _item in items) {
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

	// update cart
	public bool Update(Item_S2 _item, int _quantity) {
		return Update(_item.ID, _quantity);
	}
	public bool Update(int _id, int _quantity) {
		// try to find in list
		foreach (ShoppingCartItem_S2 _item in items) {
			if (_item.ItemID == _id) {
				_item.Quantity = _quantity;
				if (_item.Quantity == 0) {
					items.Remove(_item);
				}
				return true;
			}
		}

		return false;
	}

	// get quantity of item
	public int Quantity(Item_S2 _item) {
		return Quantity(_item.ID);
	}
	public int Quantity(int _id) {
		foreach (ShoppingCartItem_S2 _item in items) {
			if (_item.ItemID == _id) {
				return (_item.Quantity);
			}
		}
		return 0;
	}
}