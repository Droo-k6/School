package actbs;

class Cabin extends Spot {
	static String getTypeName() {return "Cabin";}
	String getTypeNameNonStatic() {return getTypeName();}
	
	// constructor
	Cabin(int _number) {
		super(_number);
	}
}
