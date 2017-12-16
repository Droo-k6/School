using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;


// class for InvoiceLineItem records
[Serializable]
public class InvoiceLineItemAS2 : IComparable<InvoiceLineItemAS2> {
	/*
	table-
	ID (number)
	CustomerID (number)
	OrderNumber (number)
	SequenceNumber (number)
	ItemSKU (number)
	quantity (number)
	price (number)
	weight (number)
	*/

	// fields
	private int customerID, orderNumber, sequenceNumber;
	private int itemSKU, quantity, price, weight;
	private int shipping;

	// constant value for shipping rate
	// price / weight
	private readonly double SHIPPING_RATE = 0.05;

	// constructor, takes in data reader to build from
	public InvoiceLineItemAS2(DbDataReader _reader) {
		// grab fields from reader
		customerID = (int)_reader["CustomerID"];
		orderNumber = (int)_reader["OrderNumber"];
		sequenceNumber = (int)_reader["SequenceNumber"];
		itemSKU = (int)_reader["ItemSKU"];
		quantity = (int)_reader["quantity"];
		price = (int)_reader["price"];
		weight = (int)_reader["weight"];

		// calculate shipping
		shipping = (int)Math.Ceiling(SHIPPING_RATE * quantity * weight);

	}

	// public properties
	public int CustomerID { get { return customerID; } }
	public int OrderNumber { get { return orderNumber; } }
	public int SequenceNumber { get { return sequenceNumber; } }
	public int ItemSKU { get { return itemSKU; } }
	public int Quantity { get { return quantity; } }
	public int Price { get { return price; } }
	public int Weight { get { return weight; } }
	public int Shipping { get { return shipping; } }

	// to string method
	public override string ToString() {
		return string.Format("{0},{1}-{2}: {3}*{4} @ {5}, {6}", customerID, orderNumber, sequenceNumber, itemSKU, quantity, price, weight);
	}

	// IComparable compare to method
	// compares customer id, order number, sequence number
	public int CompareTo(InvoiceLineItemAS2 _other) {
		int _cmp = (customerID - _other.customerID);
		if (_cmp != 0)
			return _cmp;

		_cmp = (orderNumber - _other.orderNumber);
		if (_cmp != 0)
			return _cmp;

		_cmp = (sequenceNumber - _other.sequenceNumber);
		return _cmp;
	}
}