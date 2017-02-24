package actbs;

import java.time.LocalDate;

class InvalidTripDates extends Throwable {
	InvalidTripDates(LocalDate _start, LocalDate _end) {
		super(String.format("Invalid dates, start: %1$s, end: %2$s, (start date must be before end date)", _start.toString(), _end.toString()));
	}
	InvalidTripDates(LocalDate _start, LocalDate _end, int _dif) {
		super(String.format("Invalid dates, start: %1$s, end: %2$s, (too long: %3$d, must be <= 21)", _start.toString(), _end.toString(), _dif));
	}
}
