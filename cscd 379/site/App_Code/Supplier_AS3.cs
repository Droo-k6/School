using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// Supplier class
[Serializable]
public class Supplier_AS3 {
	// Constructor
	public Supplier_AS3(int _id, string _company, string _contact, string _phone, string _areacode) {
		ID = _id;
		Company = _company;
		Contact = _contact;
		Phone = _phone;
		AreaCode = _areacode;
	}

	// Fields
	public readonly int ID;
	public readonly string Company, Contact, Phone, AreaCode;

	// to string
	public override string ToString() {
		return string.Format("({0}) {1} @ {2},{3}-{4}", ID, Company, Contact, AreaCode, Phone);
	}
}