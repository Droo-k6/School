package actbs;

interface SpotPreference {
	static String getTypeName() {return "SpotPreference";};
	String toString();
	static SpotPreference fromString(String _name) {return null;};
	boolean matchesPref(SpotLayoutClass _spotLayout, int _col) throws SeatPrefNotHandled, InvalidLayoutColumn;
}
