package actbs;

class CabinSection extends SpotSection {
	static String getTypeName() {return "Cabin Section";}
	String getSpotTypeName() {
		return (Cabin.getTypeName());
	}
	
	// # of seats
	private int spots;
	// array of seats
	private Cabin[] cabins;
	
	CabinSection(SpotClass _spotClass, int _price, int _count) throws InvalidSpotPrice, InvalidTripSectionRows {
		super(_spotClass, _price, _count);
		
		// validate count
		if ((_count < 1)||(_count > 100)) {
			throw new InvalidTripSectionRows(_count);
		}

		spots = _count;
		setupCabins();
	}
	
	// sets up array of cabins
	private void setupCabins() {
		cabins = new Cabin[spots];
		availableSeats = spots;
		
		for (int _i = 0; _i < spots; ++_i) {
			cabins[_i]= new Cabin(_i+1);
		}
	}
	
	// return total # of cabins
	int getTotalSpots() {
		return spots;
	}
	
	// display details
	void displayDetails(String _prefix) {
		System.out.printf("%1$s%2$s, total cabins: %3$s, available cabins: %4$s, price: $%5$d\n", _prefix, getSpotClass().toString(), getTotalSpots(), getAvailableSpots(), getPrice());
	}
	
	// book available cabin
	String book() throws SpotIsBooked, NoSpotsAvailable {
		// find spot, book
		// loop through all seats
		for (int _i = 0; _i < spots; ++_i) {
			Cabin _cabin = cabins[_i];
			// check if seat is open
			if (!_cabin.isBooked()) {
				_cabin.book();
				--availableSeats;
				return _cabin.toString();
			}
		}
		throw new NoSpotsAvailable();
	}
	
	void book(int _row, char _col) throws SpotIsBooked, InvalidSpotRow, InvalidSpotColumn {
		throw new UnsupportedOperationException("Cabin booking does not support specific cabins");
	}
	
	String book(SpotPreference _pref) throws SpotIsBooked, SeatPrefNotHandled, InvalidLayoutColumn {
		throw new UnsupportedOperationException("Cabin booking does not support preference");
	}
}
