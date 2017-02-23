package actbs;

class InvalidTimeHour extends Throwable {
	InvalidTimeHour(int _hour) {
		super(String.format("Invalid time of day hour: %1$d (must be 0-23)",_hour));
	}
}
