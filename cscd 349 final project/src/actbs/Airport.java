package actbs;

class Airport extends Location {
	// abstract methods/other overrides
	static String getTypeName() {return "Airport";};
	String getTypeNameNonStatic() {return getTypeName();}
	
	// constructor
	Airport(String _name) throws InvalidLocationNameLength, InvalidLocationNameCharacter {
		super(_name);
	}
	
	// to string
	// just places name
	public String toString() {
		String _str = String.format("Airport \"%1$s\"", getName());
		return _str;
	}
}