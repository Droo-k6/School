package actbs;

public class InvalidtimeMinute extends Throwable {
	InvalidtimeMinute(int _minute) {
		super(String.format("Invalid time of day minute: %1$d (must be 0-59)",_minute));
	}
}
