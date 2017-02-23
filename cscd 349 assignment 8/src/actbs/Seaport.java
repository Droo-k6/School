package actbs;

class Seaport extends Location {
	// abstract methods/other overrides
	static String getTypeName() {return "Seaport";}
	String getTypeNameNonStatic() {return getTypeName();}
	
	// constructor
	protected Seaport(String _name) throws InvalidLocationNameLength, InvalidLocationNameCharacter {
		super(_name);
	}
}
