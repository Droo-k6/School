package actbs;

// should just be a nested class for Flight
class FlightSection extends SpotSection {
	// abstract/overriden methods
	static String getTypeName() {return "Flight Section";}
	String getSpotTypeName() {
		return Seat.getTypeName();
	}
	
	// number of rows & columns
	private int rows, columns;
	
	// array of seats
	private Seat[][] seats;
	
	// constructor
	FlightSection(SeatClass _seatClass, int _price, LayoutClass _layout, int _rows) throws InvalidTripSectionRows, InvalidSpotPrice {
		super(_seatClass, _layout, _price, _rows);
		
		// validate rows
		if ((_rows < 1)||(_rows > 100)) {
			throw new InvalidTripSectionRows(_rows);
		}
		
		columns = _layout.getColumns();
		rows = _rows;
		
		setupSeats();
	}
	
	// sets up array(2D) of seats
	private void setupSeats() {
		seats = new Seat[rows][columns];
		availableSeats = (rows * columns);
		
		for (int _i = 0; _i < rows; ++_i) {
			for (int _j = 0; _j < columns; ++_j) {
				char _col = (char) (_j + 65);
				seats[_i][_j] = new Seat(_i+1,_col);
			}
		}
	}
	
	// return total # of seats
	int getTotalSpots() {
		return (rows * columns);
	}
	
	// display details on flight section
	void displayDetails(String _prefix) {
		System.out.printf("%1$s%2$s class, %3$s layout (%4$d columns wide, '%5$c'-'%6$c'), %7$d rows, total seats: %8$s, available seats: %9$s, price: $%10$d\n", _prefix, getSpotClass().toString(), getLayoutClass().toString(), columns, 'A', (char)(columns+64),rows, getTotalSpots(), getAvailableSpots(), getPrice());
	}
	
	// book any available spot
	String book() throws SpotIsBooked, NoSpotsAvailable {
		// loop through all seats
		for (int _i = 0; _i < rows; ++_i) {
			for (int _j = 0; _j < columns; ++_j) {
				Seat _seat = seats[_i][_j];
				// check if seat is open
				if (!_seat.isBooked()) {
					_seat.book();
					--availableSeats;
					return _seat.toString();
				}
			}
		}
		// shouldn't be possible through checks of available seats dopne in previous level
		throw new NoSpotsAvailable();
	}
	
	// attempt to book given seat
	void book(int _row, char _col) throws SpotIsBooked, InvalidSpotRow, InvalidSpotColumn {
		// validate row
		// subtract 1 from row to work as index
		--_row;
		if (_row < 0) {
			throw new InvalidSpotRow(_row+1);
		}
		if (_row >= rows) {
			throw new InvalidSpotRow(_row+1, rows);
		}
		
		// validate column
		// check that column is character
		if (!Character.isAlphabetic(_col)) {
			throw new InvalidSpotColumn(getSpotTypeName(),_col, columns);
		}
		// convert character to uppercase
		_col = Character.toUpperCase(_col);
		// convert character to int (using ascii decimal)
		int _column = ((int) _col) - 65;
		// now validate column is correct
		if (_column < 0) {
			throw new InvalidSpotColumn(getSpotTypeName(),_col, columns);
		}
		if (_column >= columns) {
			throw new InvalidSpotColumn(getSpotTypeName(),_col, columns);
		}
		
		// grab seat
		Seat _seat = seats[_row][_column];
		
		// attempt to book seat
		_seat.book();
		
		// adjust available count
		--availableSeats;
	}
	
	// attempt to book a seat with a preference
	String book(SpotPreference _pref) throws SpotIsBooked, SeatPrefNotHandled, InvalidLayoutColumn {
		// find seat that matches preference, otherwise first available seat
		Seat _seat = getSeatPreference(_pref);
		
		// attempt to book seat
		_seat.book();
		
		// adjust available count
		--availableSeats;
		
		String _seatName = _seat.toString();
		return _seatName;
	}
	
	// find available seat, using preference
	Seat getSeatPreference(SpotPreference _pref) throws SeatPrefNotHandled, InvalidLayoutColumn {
		// to keep track of first empty seat found to return instead
		Seat _emptySeat = null;
		// loop through all seats
		for (int _i = 0; _i < rows; ++_i) {
			for (int _j = 0; _j < columns; ++_j) {
				Seat _seat = seats[_i][_j];
				// check if seat is open
				if (!_seat.isBooked()) {
					// check if matches preference
					if (spotIsPreference(_pref,_j)) {
						return _seat;
					}else if (_emptySeat == null) {
						_emptySeat = _seat;
					}
				}
			}
		}
		// no check for null emptySeat
		// shouldn't be possible through checks of available seats
		return _emptySeat;
	}
	// check if seat matches preference
	private boolean spotIsPreference(SpotPreference _pref, int _col) throws SeatPrefNotHandled, InvalidLayoutColumn {
		return _pref.matchesPref((LayoutClass) getLayoutClass(), _col);
	}
	
	// toString method
	// prints class, total # of seats, rows, columsn, # of available seats
	public String toString() {
		String _str = String.format("%1$s class, %2$s layout, columns: 'A'-'%3$c', %4$d rows, total seats: %5$d, available seats: %6$d, price: $%7$d", getSpotClass().toString(), getLayoutClass().toString(), (char)(columns+65), rows, rows*columns, availableSeats, getPrice());
		return _str;
	}
}