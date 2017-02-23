package actbs;

abstract class SpotSection {
	// members
	// price of seats
	private int spotPrice;
	// class of layout
	private SpotLayoutClass layoutClass;
	// class of section
	private SpotClass spotClass;
	// # of available seats
	protected int availableSeats = 0;
	// count of some kind (rows, or just # of seats of no 2d)
	protected int spotCount;
	
	// constructor
	// supports layout
	protected SpotSection(SpotClass _spotClass, SpotLayoutClass _layoutClass, int _price, int _count) throws InvalidSpotPrice {
		// validate price
		// for airline pricing, has to be done at airline level
		if (_price < 0) {
			throw new InvalidSpotPrice(getSpotTypeName(),_price);
		}

		spotPrice = _price;
		spotClass = _spotClass;
		layoutClass = _layoutClass;
		spotCount = _count;
	}
	// no layout
	protected SpotSection(SpotClass _spotClass, int _price, int _count) throws InvalidSpotPrice {
		// validate price
		// for airline pricing, has to be done at airline level
		if (_price < 0) {
			throw new InvalidSpotPrice(getSpotTypeName(),_price);
		}

		spotPrice = _price;
		spotClass = _spotClass;
		spotCount = _count;
	}
	
	// get section class
	SpotClass getSpotClass() {
		return spotClass;
	}
	
	// get layout class
	SpotLayoutClass getLayoutClass() {
		return layoutClass;
	}
	
	// get avilable seats
	int getAvailableSpots() {
		return (availableSeats);
	}
	
	// get price of section
	int getPrice() {
		return spotPrice;
	}
	
	void changePrice(int _price) throws InvalidSpotPrice {
		// validate
		if (_price < 0) {
			throw new InvalidSpotPrice(getSpotTypeName(),_price);
		}
		
		spotPrice = _price;
	}
	
	// to file contents
	String toFileContents() {
		String _contents = "";
		
		// spot class
		_contents += spotClass.toFileString();
		// price
		_contents += ":" + spotPrice;
		// spot layout
		if (layoutClass == null) {
			_contents += ":N";
		}else{
			_contents += ":" + layoutClass.toFileString();
		}
		// count
		_contents += ":" + spotCount;
		
		return _contents;
	}
	
	static String getTypeName() {return "Spot Section";}
	abstract String getSpotTypeName();
	// abstract methods
	abstract void displayDetails(String _prefix);
	abstract int getTotalSpots();
	abstract String book() throws SpotIsBooked, NoSpotsAvailable;
	abstract void book(int _row, char _col) throws SpotIsBooked, InvalidSpotRow, InvalidSpotColumn;
	abstract String book(SpotPreference _pref) throws SpotIsBooked, SeatPrefNotHandled, InvalidLayoutColumn;
}