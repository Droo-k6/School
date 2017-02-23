package actbs;

interface SpotLayoutClass {
	String toString();
	String toFileString();
	// should be overriden
	static String getTypeName() {return "SpotLayout";};
	static SpotLayoutClass fromString(String _name) {return null;};
	static String fromFileString(String _name) {return null;};
}
