package actbs;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.Iterator;

abstract class Trip {
	// ID of trip
	private String identifier;
	protected LocalDate date, endDate;
	protected TimeOfDay time, endTime;
	// list of locations
	protected ArrayList<Location> listLocations = new ArrayList<Location>();
	// list of spot sections
	protected ArrayList<SpotSection> listSpotSections = new ArrayList<SpotSection>();
	// associated transport
	private Transport transport;
	
	// constructor
	protected Trip(String _id, LocalDate _date, TimeOfDay _time) throws InvalidTripIDLength, InvalidTripIDCharacter {
		// validate ID
		validateID(_id);
		
		// convert identifier to uppercase
		_id = _id.toUpperCase();
		
		identifier = _id;
		date = _date;
		time = _time;
		endDate = null;
		endTime = null;
	}
	// allows an ending date to be set
	protected Trip(String _id, LocalDate _date, TimeOfDay _startTime, LocalDate _endDate, TimeOfDay _endTime) throws InvalidTripIDLength, InvalidTripIDCharacter {
		this(_id, _date, _startTime);
		endDate = _endDate;
		endTime = _endTime;
	}
	
	// validates identifier
	private void validateID(String _id) throws InvalidTripIDLength, InvalidTripIDCharacter {
		// check if length valid (> 0)
		if (_id.length() <= 0) {
			throw new InvalidTripIDLength(getTypeNameNonStatic());
		}
		// check that all characters are alphanumberic
		// check for invalid characters
		for (char _c : (_id.toCharArray())) {
			if (!(Character.isAlphabetic(_c)||Character.isDigit(_c))) {
				throw new InvalidTripIDCharacter(getTypeNameNonStatic(),_id,_c);
			}
		}
	}
	
	// sets transport for this trip
	void setTransport(Transport _transport) {
		transport = _transport;
	}
	// return transport for trip
	Transport getTransport() {
		return transport;
	}
	
	// get date
	LocalDate getDate() {
		return date;
	}
	// get end date
	LocalDate getEndDate() {
		return endDate;
	}
	
	// get trip identifier
	String getID() {
		return (this.identifier);
	}
	
	// grab section of given class
	protected SpotSection getSection(SpotClass _class) throws TripSectionDoesNotExist {
		for (SpotSection _section : listSpotSections) {
			if (_section.getSpotClass() == _class) {
				return _section;
			}
		}
		throw new TripSectionDoesNotExist(getTypeNameNonStatic(),getID(),_class.toString());
	}
	
	// create section for trip
	void addSection(SpotClass _spotClass, int _seatPrice, LayoutClass _layout, int _rows) throws TripSectionExists, InvalidTripSectionRows, InvalidSpotPrice {
		// check if section of given class already exists
		try {
			SpotSection _section = getSection(_spotClass);
			throw new TripSectionExists(getTypeNameNonStatic(), getID(), _spotClass.toString());
		}catch(TripSectionDoesNotExist _e) {
			// create section
			SpotSection _section = createSpotSectionObject(_spotClass,_seatPrice,_layout,_rows);
			
			// add to list
			listSpotSections.add(_section);
		}
	}
	
	// create section for trip
	void addSection(SpotClass _spotClass, int _seatPrice, int _count) throws TripSectionExists, InvalidTripSectionRows, InvalidSpotPrice {
		// check if section of given class already exists
		try {
			SpotSection _section = getSection(_spotClass);
			throw new TripSectionExists(getTypeNameNonStatic(), getID(), _spotClass.toString());
		}catch(TripSectionDoesNotExist _e) {
			// create section
			SpotSection _section = createSpotSectionObject(_spotClass,_seatPrice,_count);
			
			// add to list
			listSpotSections.add(_section);
		}
	}
	
	// get price of section of given class
	// returns -1 if section does not exist
	int getSectionPrice(SpotClass _class) {
		try {
			SpotSection _section = getSection(_class);
			return _section.getPrice();
		}catch(TripSectionDoesNotExist _e) {
			return -1;
		}
	}
	
	// books requested class, no specific spot/preference
	String bookSpot(SpotClass _class) throws NoSpotsAvailable, SpotIsBooked, TripSectionDoesNotExist {
		// find section
		SpotSection _section = getSection(_class);
		
		// check if space is available
		int _avail = _section.getAvailableSpots();
		if (_avail <= 0) {
			throw new NoSpotsAvailable(getTypeNameNonStatic(), getID(), _section.getSpotClass().toString());
		}
		
		// attempt to book
		String _seatName = _section.book();
		return _seatName;
	}
	
	// books requested class/specific spot
	void bookSpot(SpotClass _class, int _row, char _col) throws TripSectionDoesNotExist, SpotIsBooked, InvalidSpotRow, InvalidSpotColumn {
		// find section
		SpotSection _section = getSection(_class);
		
		// attempt to book
		_section.book(_row,_col);
	}
	
	// books spot in section with a preference
	String bookSpot(SpotClass _class, SpotPreference _pref) throws TripSectionDoesNotExist, SpotIsBooked, SeatPrefNotHandled, InvalidLayoutColumn, NoSpotsAvailable {
		// find section
		SpotSection _section = getSection(_class);
		
		// check if space is available
		int _avail = _section.getAvailableSpots();
		if (_avail <= 0) {
			throw new NoSpotsAvailable(getTypeNameNonStatic(), getID(), _section.getSpotClass().toString());
		}
		
		// attempt to book
		String _seatName = _section.book(_pref);
		return _seatName;
	}
	
	// get total number of available spots
	int getAvailableSpots() {
		int _spots = 0;
		for (SpotSection _section : listSpotSections) {
			_spots += _section.getAvailableSpots();
		}
		return _spots;
	}
	
	// get total number of available spots for given class
	int getAvailableSpots(SpotClass _class) {
		try {
			SpotSection _section = getSection(_class);
			return _section.getAvailableSpots();
		}catch(TripSectionDoesNotExist _e) {
			return 0;
		}
	}
	
	// get total number of spots
	int getTotalSpots() {
		int _spots = 0;
		for (SpotSection _section : listSpotSections) {
			_spots += _section.getTotalSpots();
		}
		return _spots;
	}
	
	// get origin airport
	Location getOriginLocation() {
		return (listLocations.get(0));
	}
	// get destination airport
	Location getDestinationLocation() {
		return (listLocations.get(listLocations.size() - 1));
	}
	
	// checks if path matches given path (airport to airport)
	boolean matchPath(Location _origin, Location _destination) {
		boolean _matchOrigin = (_origin == getOriginLocation());
		boolean _matchDestination = (_destination == getDestinationLocation());
		return (_matchOrigin && _matchDestination);
	}
	
	// checks if path matches given path (airport to airport)
	boolean matchPath(Location _origin, Location _destination, LocalDate _date) {
		boolean _matchDates = (date.compareTo(_date) == 0);
		boolean _matchOrigin = (_origin == getOriginLocation());
		boolean _matchDestination = (_destination == getDestinationLocation());
		return (_matchDates && _matchOrigin && _matchDestination);
	}
	
	// checks if paths are same of path
	boolean matchPath(Trip _trip) {
		boolean _matchOrigin = (_trip.getOriginLocation() == getOriginLocation());
		boolean _matchDestination = (_trip.getDestinationLocation() == getDestinationLocation());
		return (_matchOrigin && _matchDestination);
	}
	
	void changePrice(SpotClass _spotClass, int _price) throws TripSectionDoesNotExist, InvalidSpotPrice {
		// get section
		SpotSection _section = getSection(_spotClass);
		
		// change price
		_section.changePrice(_price);
	}
	
	// display details on trip
	void displayDetails(String _prefix) {
		// display basic info
		// if uses endDate
		if (endDate == null) {
			System.out.printf("%1$s\"%2$s\", on %3$s at %7$s, total %6$ss: %4$d, available %6$ss: %5$d", _prefix, getID(), date.toString(), getTotalSpots(), getAvailableSpots(), getSpotTypeName(), time.toString());
		}else{
			System.out.printf("%1$s\"%2$s\", on %3$s at %8$s to %4$s at %9$s, total %7$ss: %5$d, available %7$ss: %6$d", _prefix, getID(), date.toString(), endDate.toString(), getTotalSpots(), getAvailableSpots(), getSpotTypeName(), time.toString(), endTime.toString());
		}
		
		// if uses transport
		if (transport == null) {
			System.out.println();
		}else{
			System.out.printf(", %1$s: %2$s\n", getTransportTypeName(), transport.getName());
		}
		
		// display path
		System.out.printf("%1$s\tPath: ", _prefix);
		for (Location _loc : listLocations) {
			System.out.print(" " + _loc.getName());
		}
		System.out.println();
		
		// display sections
		for (SpotSection _section : listSpotSections) {
			_section.displayDetails(_prefix + "\t");
		}
	}
	
	// convert trip to file contents
	String toFileContents() {
		String _contents = "";
		
		// ID + '|'
		_contents += getID() + "|";
		// start date (year/month/day)
		_contents += String.format("%1$d,%2$d,%3$d,", date.getYear(), date.getMonthValue(), date.getDayOfMonth());
		// start time (hour/minute)
		_contents += String.format("%1$d,%2$d", time.getHour(), time.getMinute());
		if (endDate != null) {
			// end date (year/month/day)
			_contents += String.format(",%1$d,%2$d,%3$d", endDate.getYear(), endDate.getMonthValue(), endDate.getDayOfMonth());
			if (endTime != null) {
				// end time (hour/minute)
				_contents += String.format(",%1$d,%2$d", endTime.getHour(), endTime.getMinute());
			}
		}
		
		// path
		_contents += pathToFileContents();
		
		// spot sections
		_contents += sectionsToFileContents();
		
		return _contents;
	}
	// locations to file contents
	String pathToFileContents() {
		String _contents = "";
		for (Location _loc : listLocations) {
			_contents += "|" + _loc.getName();
		}
		return _contents;
	}
	
	// sections to file contents
	String sectionsToFileContents() {
		String _contents = "";
		_contents += "[";
		
		Iterator<SpotSection> _it = listSpotSections.iterator();
		while (_it.hasNext()) {
			SpotSection _section = _it.next();
			_contents += _section.toFileContents();
			if (_it.hasNext()) {
				_contents += ",";
			}
		}
		
		_contents += "]";
		return _contents;
	}
	
	// abstract/overriden methods
	static String getTypeName() {return "Trip";};
	abstract String getTypeNameNonStatic();
	abstract String getSpotTypeName();
	abstract String getSpotSectionTypeName();
	abstract String getTransportTypeName();
	// if trips path matches given path
	abstract SpotSection createSpotSectionObject(SpotClass _spotClass, int _spotPrice, SpotLayoutClass _layout, int _count) throws InvalidTripSectionRows, InvalidSpotPrice;
	abstract SpotSection createSpotSectionObject(SpotClass _spotClass, int _spotPRice, int _count) throws InvalidTripSectionRows, InvalidSpotPrice;
}