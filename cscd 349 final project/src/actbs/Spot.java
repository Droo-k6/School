package actbs;

public abstract class Spot {
	private boolean booked = false;
	// optional for use
	// bool for if column is used (row is treated as spot#)
	private boolean useCol = false;
	private int row;
	private char col;
	
	// constructor
	// default, does nothing
	protected Spot(int _row) {
		row = _row;
	}
	// for use for spots that use row/column
	protected Spot(int _row, char _col) {
		row = _row;
		col = _col;
		useCol = true;
	}
	
	// book this spot
	void book() throws SpotIsBooked {
		if (booked) {
			if (useCol) {
				throw new SpotIsBooked(getTypeName(),row, col);
			}else{
				throw new SpotIsBooked(getTypeName(),row);
			}
		}
		booked = true;
	}
	
	// return if booked
	boolean isBooked() {
		return booked;
	}
	
	// return row/spot #
	int getRow() {
		return row;
	}
	// return column
	char getCol() {
		if (useCol) {
			return col;
		}
		throw new UnsupportedOperationException("Column is not used for spot type");
	}
	
	// return String
	public String toString() {
		if (useCol) {
			return String.format("%1$d%2$c",getRow(),getCol());
		}else{
			return String.format("%1$d",getRow());
		}
	}
	
	// static method that should be updated by children
	static String getTypeName() {return "Spot";}
	abstract String getTypeNameNonStatic();
}
