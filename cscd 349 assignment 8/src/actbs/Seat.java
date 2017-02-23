package actbs;

class Seat extends Spot {
	// constructor
	Seat(int _row, char _col) {
		super(_row,_col);
	}
	
	// static/abstract/overrides
	static String getTypeName() {return "Seat";}
	String getTypeNameNonStatic() {return getTypeName();}
}