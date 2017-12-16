using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// Customer class
[Serializable]
public class Customer_AS3 {
	// Constructor
	public Customer_AS3(int _number, Address_AS3 _ship, Address_AS3 _bill, string _company, string _contact, string _phone) {
		Number = _number;
		AddrShip = _ship;
		AddrBill = _bill;
		Company = _company;
		Contact = _contact;
		Phone = _phone;
	}

	// fields
	public readonly int Number;
	public readonly Address_AS3 AddrShip, AddrBill;
	public readonly string Company, Contact, Phone;

	// to string
	public override string ToString() {
		return string.Format("({0}) {1}-{2},{3} @[{4},{5}]", Number, Company, Contact, Phone, AddrBill.ID, AddrShip.ID);
	}
}