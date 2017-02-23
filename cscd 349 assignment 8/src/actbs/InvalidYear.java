package actbs;

public class InvalidYear extends Throwable {
	public InvalidYear(int _year) {
		super(String.format("Invalid year: %1d (less than zero)", _year));
	}
}