using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

// struct for an item record
public struct Item_S1 {
	// constructor
	public Item_S1(int _id, string _name, double _price, double _weight) {
		ID = _id;
		Name = _name;
		Price = _price;
		Weight = _weight;
	}
	public Item_S1(SqlDataReader _reader) {
		int _id = (int)_reader["ItemID"];
		string _name = (string)_reader["Name"];
		double _price = Decimal.ToDouble((Decimal)_reader["Price"]);
		double _weight = Decimal.ToDouble((Decimal)_reader["Weight"]);

		ID = _id;
		Name = _name.Trim();
		Price = _price;
		Weight = _weight;
	}

	// fields
	public readonly int ID;
	public readonly string Name;
	public readonly double Price, Weight;

	// get image name
	public string ImageName {
		get {
			return string.Format("item{0}.jpg", ID);
		}
	}
	public string ImageThumbName {
		get {
			return string.Format("item{0}s.jpg", ID);
		}
	}
}
 