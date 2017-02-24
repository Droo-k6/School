package actbs;

public class InvalidDay extends Throwable {
	public InvalidDay(int _day, int _maxDays) {
		super(String.format("Invalid day: %1d (must be between 1-%2$d given month/year)", _day, _maxDays));
	}
}