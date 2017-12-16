using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

// customer class for Customer records
[Serializable]
public class CustomerAS2 : IComparable<CustomerAS2> {
	/*
	table-
	CustomerID (number)
	Name (string)
	Street (string)
	City (string)
	State (string)
	Zip (string)
	*/

	// fields
	private int customerID;
	private string name, street, city, state, zip;

	// constructor, takes in data reader to build from
	public CustomerAS2(DbDataReader _reader) {
		customerID = (int)_reader["CustomerID"];
		name = (string)_reader["Name"];
		street = (string)_reader["Street"];
		city = (string)_reader["City"];
		state = (string)_reader["State"];
		zip = (string)_reader["Zip"];
	}

	// public properties
	public int CustomerID { get { return customerID; } }
	public string Name { get { return name; } }
	public string Street { get { return street; } }
	public string City { get { return city; } }
	public string State { get { return state; } }
	public string Zip { get { return zip; } }

	// to string method
	public override string ToString() {
		return string.Format("{0}:{1} @ {2}, {3}, {4}, {5}", customerID, name, street, city, street, zip);
	}

	// IComparable, compare to
	// compares custom IDs, then customer names
	public int CompareTo(CustomerAS2 _other) {
		int _cmp = (customerID - _other.customerID);
		if (_cmp == 0) {
			_cmp = name.CompareTo(_other.name);
		}
		return _cmp;
	}
}
