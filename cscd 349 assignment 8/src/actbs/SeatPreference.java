package actbs;

enum SeatPreference implements SpotPreference {
	// values (preference)
	window,aisle,none;
	
	// check if seat matches preference
	public boolean matchesPref(SpotLayoutClass _spotLayout, int _col) throws SeatPrefNotHandled, InvalidLayoutColumn {
		LayoutClass _layout = (LayoutClass) _spotLayout;
		if (this == window) {
			return (_layout.isWindowSeat(_col));
		}
		if (this == aisle) {
			return (_layout.isAisleSeat(_col));
		}
		if (this == none) {
			return true;
		}
		// not possible to reach here, throw exception just incase
		throw new SeatPrefNotHandled(this.toString());
	}
	
	public String toString() {
		switch (this) {
		case window: return "window";
		case aisle: return "aisle";
		case none: return "none";
		// will never reach here
		default: return "invalid";
		}
	}
	
	static String getTypeName() {return "Seat Preference";};
	
	static SpotPreference fromString(String _name) {
		_name = _name.toLowerCase();
		switch (_name) {
		case "window": return window;
		case "aisle": return aisle;
		case "none": return none;
		default: return null;
		}
	}
	
	static String getOptions(String _prefix) {
		String _str = "";
		_str += _prefix + "window" + "\n";
		_str += _prefix + "aisle" + "\n";
		return _str;
	}
}