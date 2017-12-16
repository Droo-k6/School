using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

// stuct for adds
public struct Ad_S2 {
	// enum for type of add
	public enum AddType {Vertical, Horizontal, Invalid}

	// constructor
	public Ad_S2(int _id, AddType _type, string _link) {
		ID = _id;
		Type = _type;
		Link = _link;
	}
	public Ad_S2(SqlDataReader _reader) {
		int _id = (int)_reader["AdID"];
		string _typeStr = ((string)_reader["Type"]).ToLower();
		string _link = ((string)_reader["Link"]).Trim();

		AddType _type = AddType.Invalid;
		if (_typeStr.Equals("h")) {
			_type = AddType.Horizontal;
		} else if (_typeStr.Equals("v")) {
			_type = AddType.Vertical;
		}

		ID = _id;
		Type = _type;
		Link = _link;
	}

	// fields
	public readonly int ID;
	public readonly AddType Type;
	public readonly string Link;

	// get image name
	public string ImageName {
		get {
			return string.Format("ad{0}.jpg", ID);
		}
	}

	// picks random ad from given list for given type
	public static Ad_S2 GetAdd(List<Ad_S2> _ads, AddType _type) {
		bool _contains = false;
		// check that list contains type
		foreach(Ad_S2 _ad in _ads) {
			if (_ad.Type == _type) {
				_contains = true;
				break;
			}
		}

		if (!_contains) {
			throw new ArgumentException(string.Format("List of ads does not contain given type \"{0}\"", _type.ToString()));
		}

		// pick a random ad
		while(true) {
			Random _rand = new Random();
			int _index = _rand.Next(_ads.Count);

			Ad_S2 _ad = _ads[_index];
			if (_ad.Type == _type) {
				return _ad;
			}
		}
	}
}