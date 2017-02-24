package actbs;

public class InvalidTripSectionRows extends Throwable {
	public InvalidTripSectionRows(int _rows) {
		super(String.format("Invalid number of section rows/count: %1$d (must be 1-100 inclusive)",_rows));
	}
}