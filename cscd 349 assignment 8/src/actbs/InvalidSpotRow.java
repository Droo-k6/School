package actbs;

public class InvalidSpotRow extends Throwable {
	public InvalidSpotRow(int _row) {
		super(String.format("Invalid row number: %1$d (must be 1 or greater)",_row));
	}
	public InvalidSpotRow(int _row, int _totalRows) {
		super(String.format("Invalid row number: %1$d (higher than total number of rows: %2$d)",_row,_totalRows));
	}
}