using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// Address class
[Serializable]
public class Address_AS3 {
	// Constructor
	public Address_AS3(int _id, string _street, string _city, string _state, string _zip) {
		ID = _id;
		Street = _street;
		City = _city;
		State = _state;
		Zip = _zip;
	}

	// fields
	public readonly int ID;
	public readonly string Street, City, State, Zip; 

	// to string
	public override string ToString() {
		return string.Format("({0}) {1}-{2},{3} {4}", ID, Street, City, State, Zip);
	}
}