package actbs;

public class InvalidTripSectionCols extends Throwable {
	public InvalidTripSectionCols(int _cols) {
		super(String.format("Invalid number of section columns: %1$d (must be 1-10 inclusive)",_cols));
	}
}