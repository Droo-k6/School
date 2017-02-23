package actbs;

public class InvalidLayoutColumn extends Throwable {
	public InvalidLayoutColumn(int _column, int _columnMax) {
		super(String.format("Invalid layout column: %1$d (must be 0-%2$d inclusive)", _column, _columnMax-1));
	}
}
