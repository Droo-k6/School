package actbs;

public class InvalidSpotColumn extends Throwable {
	public InvalidSpotColumn(String _typeName, char _col, int _totalCols) {
		super(String.format("Invalid %1$s column: %2$c (must be character from 'A' to '%3$c')",_typeName, _col, (char)(_totalCols-1+65)));
	}
}