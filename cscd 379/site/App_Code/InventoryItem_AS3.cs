using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// Inventory item / SKU class
[Serializable]
public class InventoryItem_AS3 {
	// Constructor
	public InventoryItem_AS3(int _sku, List<Supplier_AS3> _suppliedby, int _qoh, string _description, double _weight, double _price) {
		SKU = _sku;
		QOH = _qoh;
		UnitWeight = _weight;
		UnitPrice = _price;
		Description = _description;
		SuppliedBy = _suppliedby;
	}

	// Fields
	public readonly int SKU, QOH;
	public readonly double UnitWeight, UnitPrice;
	public readonly string Description;
	public readonly List<Supplier_AS3> SuppliedBy;

	// to string
	public override string ToString() {
		// list of supplier IDs
		List<int> _supplierIDs = new List<int>();
		foreach(Supplier_AS3 _s in SuppliedBy) {
			_supplierIDs.Add(_s.ID);
		}

		return string.Format("({0}) {1}, {2}lb ${3} - {4}, {5}", SKU, Description, UnitWeight, UnitPrice, QOH, string.Join(",", _supplierIDs));
	}
}