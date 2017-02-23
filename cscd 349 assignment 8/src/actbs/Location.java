package actbs;

abstract class Location {
	// name of location
	private String name;
	
	// constructor
	protected Location(String _name) throws InvalidLocationNameLength, InvalidLocationNameCharacter {
		// convert to uppercase
		_name = _name.toUpperCase();
		
		// test name is valid
		checkName(_name);
		
		this.name = _name;
	}
	
	// validate name for construction
	// checks if given name is valid
	protected void checkName(String _name) throws InvalidLocationNameLength, InvalidLocationNameCharacter {
		// check length
		if (_name.length() != 3) {
			throw new InvalidLocationNameLength(getTypeNameNonStatic(),_name);
		}
		// check for invalid characters
		for (char _c : (_name.toCharArray())) {
			if (!Character.isAlphabetic(_c)) {
				throw new InvalidLocationNameCharacter(getTypeNameNonStatic(),_name,_c);
			}
		}
	}
	
	// get name
	String getName() {
		return (this.name);
	}
	
	// check if name matches name of object
	boolean nameEquals(String _name) {
		return (name.equalsIgnoreCase(_name));
	}
	
	// display details
	void displayDetails(String _prefix) {
		System.out.println(_prefix + getName());
	}
	
	// should be overrided
	static String getTypeName() {return "Location";};
	abstract String getTypeNameNonStatic();
	// abstract methods
}
