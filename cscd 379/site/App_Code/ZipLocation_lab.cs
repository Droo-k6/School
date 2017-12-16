using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Xml.Serialization;

// holds information about a zip location
[Serializable]
public class ZipLocation_lab {	

	// constructor
	private ZipLocation_lab() { }
	public ZipLocation_lab(string _zip, SqlDataReader _reader) {
		Zip = _zip;
		City = (string)_reader["City"];
		State = (string)_reader["State"];
		Class = (string)_reader["Class"];
		Valid = true;
		ValidStr = "";
	}
	public ZipLocation_lab(string _zip, string _city, string _state, string _class, bool _valid, string _validStr) {
		Zip = _zip;
		City = _city;
		State = _state;
		Class = _class;
		Valid = _valid;
		ValidStr = _validStr;
	}

	// fields
	// can't be readonly - the serializer won't work
	public string Zip, City, State, Class, ValidStr;
	public bool Valid;

	// creates invalid zip
	public static ZipLocation_lab CreateInvalid(string _zip, string _msg) {
		return new ZipLocation_lab(_zip, "", "", "", false, _msg);
	}
}