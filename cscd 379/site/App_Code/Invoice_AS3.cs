using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// Invoice class
[Serializable]
public class Invoice_AS3 : IComparable<Invoice_AS3> {
	// Constructor
	public Invoice_AS3(int _number, Customer_AS3 _customer, List<LineItem_AS3> _items, DateTime _order, DateTime _ship, string _status) {
		Number = _number;
		Customer = _customer;
		LineItems = _items;
		Ordered = _order;
		Shipped = _ship;
		Status = _status;

		// calculate totals
		int _itemCount = 0;
		double _cost = 0, _weight = 0;
		foreach(LineItem_AS3 _line in LineItems) {
			_itemCount += _line.Quantity;
			_cost += _line.Item.UnitPrice * _line.Quantity;
			_weight += _line.Item.UnitWeight * _line.Quantity;
		}

		TotalItems = _itemCount;
		TotalCost = _cost;
		TotalWeight = _weight;
	}

	// Fields
	public readonly int Number;
	public readonly Customer_AS3 Customer;
	public readonly List<LineItem_AS3> LineItems;
	public readonly DateTime Ordered, Shipped;
	public readonly string Status;
	public readonly int TotalItems;
	public readonly double TotalCost, TotalWeight;

	// to string
	public override string ToString() {
		return string.Format("({0}) {1} [{2},{3}], {4}", Number, Customer.Number, Ordered.ToString("yyyy/MM/dd"), Shipped.ToString("yyyy/MM/dd"), string.Join(",", LineItems));
	}

	// compare to
	public int CompareTo(Invoice_AS3 _other) {
		// compare Number
		return (Number - _other.Number);
	}
}