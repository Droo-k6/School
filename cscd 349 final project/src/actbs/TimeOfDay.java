package actbs;

// for holding time of day (24 hour format)
class TimeOfDay {
	// member values
	private int hour,minute;
	// constructor
	TimeOfDay(int _hour, int _minute) throws InvalidTimeHour, InvalidtimeMinute {
		// verify hour
		if ((_hour < 0)||(_hour > 23)) {
			throw new InvalidTimeHour(_hour);
		}
		// verify minute
		if ((_minute < 0)||(_minute > 59)) {
			throw new InvalidtimeMinute(_minute);
		}
		
		hour = _hour;
		minute = _minute;
	}
	// get hour
	int getHour() {
		return hour;
	}
	// get minute
	int getMinute() {
		return minute;
	}
	// to string
	public String toString() {
		return String.format("%1$d:%2$d",hour,minute);
	}
}
