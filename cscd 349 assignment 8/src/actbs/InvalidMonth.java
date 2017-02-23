package actbs;

public class InvalidMonth extends Throwable {
	public InvalidMonth(int _month) {
		super(String.format("Invalid month: %1d (must be between 1-12 inclusive)", _month));
	}
}