package actbs;

enum SeatClass implements SpotClass {
	first,economy,business,value;
	
	
	public String toString() {
		switch (this) {
		case first: return "first";
		case economy: return "economy";
		case business: return "business";
		case value: return "value";
		// will never reach here
		default: return "invalid";
		}
	}
	
	static public SpotClass fromString(String _name) {
		_name = _name.toLowerCase();
		switch (_name) {
		case "first": return first;
		case "economy": return economy;
		case "business": return business;
		case "value": return value;
		default: return null;
		}
	}
	
	static String fromFileString(String _name) {
		_name = _name.toUpperCase();
		switch (_name) {
		case "E": return "first";
		case "V": return "economy";
		case "F": return "business";
		case "B": return "value";
		default: return null;
		}
	}
	public String toFileString() {
		switch (this) {
		case first: return "F";
		case economy: return "E";
		case business: return "B";
		case value: return "V";
		default: return "N";
		}
	}
	
	static String getTypeName() {return "Seat";};
	static String getOptions(String _prefix) {
		String _str = "";
		_str += _prefix + economy.toString() + "\n";
		_str += _prefix + value.toString() + "\n";
		_str += _prefix + first.toString() + "\n";
		_str += _prefix + business.toString() + "\n";
		return _str;
	}
}