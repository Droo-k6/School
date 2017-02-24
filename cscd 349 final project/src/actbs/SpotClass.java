package actbs;

interface SpotClass {
	String toString();
	String toFileString();
	// should be implemented (must be as static)
	static String getTypeName() {return "Spot";};
	static SpotClass fromString(String _name) {return null;};
	static String fromFileString(String _name) {return null;};
}