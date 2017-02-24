package actbs;

import java.time.LocalDate;
import java.time.YearMonth;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Iterator;

public abstract class SystemManager {
	// array of locations
	protected ArrayList<Location> listLocations = new ArrayList<Location>();
	// array of providers
	protected ArrayList<Provider> listProviders = new ArrayList<Provider>();
	
	// constructor to create with input file
	protected SystemManager(String _filename) {
		parseFile(_filename);
	}
	protected SystemManager() {}
	
	// create location
	public void createLocation(String _name) {
		try{
			// check if airport w/ name exists
			if (locationExists(_name)) {
				throw new InvalidLocationNameExists(getLocationTypeName(), _name);
			}
			
			// create location
			Location _location = createLocationObject(_name);
			
			// add airport to list
			listLocations.add(_location);
		}catch(Throwable _e) {
			System.out.println("createLocation() error: " + _e.getMessage());
		}
	}
	
	// creates provider (airline/cruise) of given name
	public void createProvider(String _name) {
		try{
			// check if airline w/ name exists
			if (providerExists(_name)) {
				throw new InvalidProviderNameExists(getProviderTypeName(),_name);
			}
			
			// create airline
			Provider _provider = createProviderObject(_name);
			
			// add airline to list
			listProviders.add(_provider);
		}catch(Throwable _e) {
			System.out.println("createProvider() error: " + _e.getMessage());
		}
	}
	
	// create transport vehicle for provider
	public void createTransport(String _providerName, String _transportName) {
		try {
			// find provider
			Provider _provider = findProvider(_providerName);
			
			// request to create transport
			_provider.createTransport(_transportName);
		}catch(Throwable _e) {
			System.out.println("createTransport() error: " + _e.getMessage());
		}
	}
	
	// creates trip from given info (origin and destination location)
	public void createTrip(String _providerName, String _originLocationName, String _destinationLocationName, int _year, int _month, int _day, int _hour, int _minute, String _tripID) {
		try {
			// find provider
			Provider _provider = findProvider(_providerName);
			
			// find origin location
			Location _orig = findLocation(_originLocationName,"Origin");
			
			// find destination location
			Location _dest = findLocation(_destinationLocationName,"Destination");
			
			// create date object from date info
			LocalDate _date = createDate(_year,_month,_day);
			
			// create object for time info
			TimeOfDay _time = new TimeOfDay(_hour,_minute);
			
			// create flight for airline
			_provider.createTrip(_tripID,_date,_time,_orig,_dest);
		}catch(Throwable _e) {
			System.out.println("createTrip() error: " + _e.getMessage());
		}
	}
	
	// creates trip from given info (list of locations)
	public void createTrip(String _providerName, String[] _locations, int _year, int _month, int _day, int _hour, int _minute, String _tripID) {
		try {
			// find airline
			Provider _provider = findProvider(_providerName);
			
			// grab locations, put into ArrayList
			ArrayList<Location> _listLocations = new ArrayList<Location>();
			for (String _locStr : _locations) {
				Location _loc = findLocation(_locStr);
				_listLocations.add(_loc);
			}
			
			// create date object from date info
			LocalDate _date = createDate(_year,_month,_day);
			
			// create object for time info
			TimeOfDay _time = new TimeOfDay(_hour,_minute);
			
			// create flight for airline
			_provider.createTrip(_tripID,_date,_time,_listLocations);
		}catch(Throwable _e) {
			System.out.println("createTrip() error: " + _e.getMessage());
		}
	}
	
	// creates trip from given info (list of locations) (and 2 dates)
	public void createTrip(String _providerName, String[] _locations, int _startYear, int _startMonth, int _startDay, int _startHour, int _startMinute, int _endYear, int _endMonth, int _endDay, int _endHour, int _endMinute, String _tripID) {
		try {
			// find airline
			Provider _provider = findProvider(_providerName);
			
			// grab locations, put into ArrayList
			ArrayList<Location> _listLocations = new ArrayList<Location>();
			for (String _locStr : _locations) {
				Location _loc = findLocation(_locStr);
				_listLocations.add(_loc);
			}
			
			// create date object from date info
			LocalDate _startDate = createDate(_startYear,_startMonth,_startDay);
			LocalDate _endDate = createDate(_endYear,_endMonth,_endDay);
			
			// time objects
			TimeOfDay _startTime = new TimeOfDay(_startHour,_startMinute);
			TimeOfDay _endTime = new TimeOfDay(_endHour,_endMinute);
			
			// create flight for airline
			_provider.createTrip(_tripID,_startDate,_startTime,_endDate,_endTime,_listLocations);
		}catch(Throwable _e) {
			System.out.println("createTrip() error: " + _e.getMessage());
		}
	}
	
	// create spot section for given provider/trip ID
	public void createSpotSection(String _providerName, String _tripID, String _spotClassName, int _spotPrice, String _layoutClassName, int _count) {
		try {
			// find provider
			Provider _provider = findProvider(_providerName);
			
			// convert spot string to enum value
			SpotClass _spotClass = getSpotClass(_spotClassName);
			if (_spotClass == null) {
				throw new InvalidSpotClassName(getSpotClassTypeName(), _spotClassName);
			}
			
			// convert layout string to enum value
			SpotLayoutClass _spotLayoutClass = getLayoutClass(_layoutClassName);
			if (_spotLayoutClass == null) {
				throw new InvalidSpotLayoutClassName(getSpotLayoutClassTypeName(), _layoutClassName);
			}
			
			// request provider to create section
			_provider.createSection(_tripID, _spotClass, _spotPrice, _spotLayoutClass, _count);
		}catch(Throwable _e) {
			System.out.println("createSpotSection() error: " + _e.getMessage());
		}
	}
	
	// create spot section for given provider/trip ID
	// no layout
	public void createSpotSection(String _providerName, String _tripID, String _spotClassName, int _spotPrice, int _count) {
		try {
			// find provider
			Provider _provider = findProvider(_providerName);
			
			// convert spot string to enum value
			SpotClass _spotClass = getSpotClass(_spotClassName);
			if (_spotClass == null) {
				throw new InvalidSpotClassName(getSpotClassTypeName(), _spotClassName);
			}
			
			// request provider to create section
			_provider.createSection(_tripID, _spotClass, _spotPrice, _count);
		}catch(Throwable _e) {
			System.out.println("createSpotSection() error: " + _e.getMessage());
		}
	}
	
	// book given seat for airline/flight
	public void bookSpot(String _providerName, String _tripID, String _spotClassName, int _row, char _col) {
		try {
			// find airline
			Provider _provider = findProvider(_providerName);
			
			// convert spot string to enum value
			SpotClass _spotClass = getSpotClass(_spotClassName);
			if (_spotClass == null) {
				throw new InvalidSpotClassName(getSpotClassTypeName(), _spotClassName);
			}

			// request to book seat for flight
			_provider.bookSpot(_tripID,_spotClass,_row,_col);
			
			System.out.println("Seat " + _row + _col + " booked");
		}catch(Throwable _e) {
			System.out.println("bookSpot() error: " + _e.getMessage());
		}
	}
	// book seat with preference
	public void bookSpot(String _providerName, String _tripID, String _spotClassName, String _preferenceClassName) {
		try {
			// find airline
			Provider _provider = findProvider(_providerName);
			
			// convert spot string to enum value
			SpotClass _spotClass = getSpotClass(_spotClassName);
			if (_spotClass == null) {
				throw new InvalidSpotClassName(getSpotClassTypeName(), _spotClassName);
			}
			
			// convert preference string to enum value
			SpotPreference _spotPref = getPreferenceClass(_preferenceClassName);
			if (_spotPref == null) {
				throw new InvalidSpotClassName(getPreferenceClassTypeName(), _preferenceClassName);
			}
			
			// request to book seat for flight
			// preference returns String of seat name booked
			String _seatName = _provider.bookSpot(_tripID,_spotClass,_spotPref);
			
			System.out.println("Seat " + _seatName + " booked");
		}catch(Throwable _e) {
			System.out.println("bookSpot() error: " + _e.getMessage());
		}
	}
	// book spot with only class
	public void bookSpot(String _providerName, String _tripID, String _spotClassName) {
		try {
			// find provider
			Provider _provider = findProvider(_providerName);
			
			// convert spot string to enum value
			SpotClass _spotClass = getSpotClass(_spotClassName);
			if (_spotClass == null) {
				throw new InvalidSpotClassName(getSpotClassTypeName(), _spotClassName);
			}
			
			// request to book seat for flight
			// preference returns String of seat name booked
			String _seatName = _provider.bookSpot(_tripID,_spotClass);
			
			System.out.println("Seat " + _seatName + " booked");
		}catch(Throwable _e) {
			System.out.println("bookSpot() error: " + _e.getMessage());
		}
	}
	
	// change price of section
	public void changePrice(String _providerName, String _tripID, String _spotClassName, int _price) {
		try {
			// find provider
			Provider _provider = findProvider(_providerName);
			
			// convert spot string to enum value
			SpotClass _spotClass = getSpotClass(_spotClassName);
			if (_spotClass == null) {
				throw new InvalidSpotClassName(getSpotClassTypeName(), _spotClassName);
			}
			
			// request price change
			_provider.changePrice(_tripID,_spotClass,_price);
		}catch(Throwable _e) {
			System.out.println("changePrice() error: " + _e.getMessage());
		}
	}
	// change price of section (given location+destination)
	public void changePrice(String _providerName, String _originName, String _destName, String _spotClassName, int _price) {
		try {
			// find provider
			Provider _provider = findProvider(_providerName);
			
			// find origin
			Location _orig = findLocation(_originName);
			// find destination
			Location _dest = findLocation(_destName);
			
			// convert spot string to enum value
			SpotClass _spotClass = getSpotClass(_spotClassName);
			if (_spotClass == null) {
				throw new InvalidSpotClassName(getSpotClassTypeName(), _spotClassName);
			}
			
			// request price change
			_provider.changePrice(_orig,_dest,_spotClass,_price);
		}catch(Throwable _e) {
			System.out.println("changePrice() error: " + _e.getMessage());
		}
	}
	
	// find available flights from origin airport to destination airport
	// results printed to terminal
	public void findAvailableTrips(String _origLocationName, String _destLocationName) {
		try {
			// find origin location
			Location _origLocation = findLocation(_origLocationName,"Origin");
			
			// find destination airport
			Location _destLocation = findLocation(_destLocationName,"Destination");
			
			// find/print a list of airlines/flights
			findTrips(_origLocation,_destLocation);
		}catch(Throwable _e) {
			System.out.println("findAvailableTrips() error: " + _e.getMessage());
		}
	}
	
	// find avilable trips from origin location to destination location
	// finds with available classes/on start date
	public void findAvailableTrips(String _origLocationName, String _destLocationName, String _spotClassName, int _dateYear, int _dateMonth, int _dateDay) {
		try {
			// find origin location
			Location _origLocation = findLocation(_origLocationName,"Origin");
			
			// find destination airport
			Location _destLocation = findLocation(_destLocationName,"Destination");
			
			// convert spot string to enum value
			SpotClass _spotClass = getSpotClass(_spotClassName);
			if (_spotClass == null) {
				throw new InvalidSpotClassName(getSpotClassTypeName(), _spotClassName);
			}
			
			// create date object from date info
			LocalDate _date = createDate(_dateYear,_dateMonth,_dateDay);
			
			// find/print a list of airlines/flights
			findTrips(_origLocation,_destLocation,_spotClass,_date);
		}catch(Throwable _e) {
			System.out.println("findAvailableTrips() error: " + _e.getMessage());
		}
	}
	
	// find print list of providers/trips
	protected void findTrips(Location _orig, Location _dest, SpotClass _spotClass, LocalDate _date) throws InvalidTripPath {
		findTripsPre(_orig, _dest,_spotClass,_date);
		
		// boolean to say if any flight was found
		// so message can be displayed
		boolean _foundTrip = false;
		
		// loop through, find matching flights
		for (Provider _provider : listProviders) {
			ArrayList<Trip> _trips = _provider.getTrips(_orig,_dest,_spotClass,_date);
			if (_trips.size() > 0) {
				_foundTrip = true;
				
				// print airline info
				System.out.printf("\t%1$s %2$s\n", getProviderTypeName(), _provider.getName());
				
				// print matching flights
				for (Trip _trip : _trips) {
					_trip.displayDetails("\t\t");
				}
			}
		}
		
		findTripsPost(_foundTrip);
	}
	
	// find/print a list of airlines and flights
	// whose path match the 2 given airports
	// just 1 simple loop done instead of gathering a list and printing that
	protected void findTrips(Location _orig, Location _dest) throws InvalidTripPath {
		findTripsPre(_orig, _dest);
		
		// boolean to say if any flight was found
		// so message can be displayed
		boolean _foundTrip = false;
		
		// loop through, find matching flights
		for (Provider _provider : listProviders) {
			ArrayList<Trip> _trips = _provider.getTrips(_orig,_dest);
			if (_trips.size() > 0) {
				_foundTrip = true;
				
				// print airline info
				System.out.printf("\t%1$s %2$s\n", getProviderTypeName(), _provider.getName());
				
				// print matching flights
				for (Trip _trip : _trips) {
					_trip.displayDetails("\t\t");
				}
			}
		}
		
		findTripsPost(_foundTrip);
	}
	
	// prints complete details of system
	final public void displaySystemDetails() {
		System.out.println(getSystemTypeName() + " System Details:");
		displaySystemDetailsLocations();
		displaySystemDetailsProviders();
	}
	// prints details on locations in system
	private void displaySystemDetailsLocations() {
		System.out.println("\t" + getLocationTypeName() + "s(" + listLocations.size() + ")");
		for (Location _location : listLocations) {
			_location.displayDetails("\t\t");
		}
	}
	// prints details on providers in system
	private void displaySystemDetailsProviders() {
		System.out.println("\t" + getProviderTypeName() + "s(" + listProviders.size() + ")");
		for (Provider _provider : listProviders) {
			_provider.displayDetails("\t\t");
		}
	}
	
	// creates date object from given info
	// verifies that info is valid
	static protected LocalDate createDate(int _year, int _month, int _day) throws InvalidDay, InvalidMonth, InvalidYear {
		// validate values first
		// validate year
		if (_year < 0) {
			throw new InvalidYear(_year);
		}
		// validate month
		if ((_month < 1) || (_month > 12)) {
			throw new InvalidMonth(_month);
		}
		
		// find max # of days for given month/year
		YearMonth yearmonth = YearMonth.of(_year, _month);
		int _maxDays = yearmonth.lengthOfMonth();
		// validate day
		if ((_day < 1)||(_day > _maxDays)) {
			throw new InvalidDay(_day,_maxDays);
		}
		
		// create date object
		LocalDate _date = LocalDate.of(_year,_month,_day);
		
		return _date;
	}
	
	// returns if location exists
	// uses findLocation and catches the exception
	final protected boolean locationExists(String _name) {
		try {
			Location _location = findLocation(_name);
		}catch(LocationDoesNotExist _e) {
			return false;
		}
		return true;
	}
	
	// returns if provider exists
	// uses findProvider and catches the exception
	final protected boolean providerExists(String _name) {
		try {
			Provider _provider = findProvider(_name);
		}catch(ProviderDoesNotExist _e) {
			return false;
		}
		return true;
	}
	
	// find provider w/ given name from local list
	final protected Location findLocation(String _name) throws LocationDoesNotExist {
		for (Location _location : listLocations) {
			if (_location.nameEquals(_name)) {
				return _location;
			}
		}
		throw new LocationDoesNotExist(getLocationTypeName(), _name);
	}
	// takes prefix to be included in possible exception message
	final protected Location findLocation(String _name, String _prefix) throws LocationDoesNotExist {
		for (Location _location : listLocations) {
			if (_location.nameEquals(_name)) {
				return _location;
			}
		}
		throw new LocationDoesNotExist(getLocationTypeName(), _name, _prefix);
	}
	
	// find provider w/ given name from local list
	final protected Provider findProvider(String _name) throws ProviderDoesNotExist {
		for (Provider _provider : listProviders) {
			if (_provider.nameEquals(_name)) {
				return _provider;
			}
		}
		throw new ProviderDoesNotExist(getProviderTypeName(), _name);
	}
	
	
	// for parsing a file
	public void parseFile(String _filename) {
		try {
			SystemToFromFile.parseFile(this, _filename);
		}catch(Throwable _e) {
			System.out.printf("parseFile(%1$s) error: %2$s\n",_filename,_e.getMessage());
		}
	}
	
	// dump manager to file
	public void dumpToFile(String _filename) {
		try {
			SystemToFromFile.dumpFile(this, _filename);
		}catch(Throwable _e) {
			System.out.printf("dumpToFile(%1$s) error: %2$s\n",_filename,_e.getMessage());
		}
	}
	
	// convert current manager state to contents
	String toFileContents() {
		String _contents = "";
		
		// locations list
		_contents += locationsToFileContents();
		
		// providers list
		_contents += providersToFileContents();
		
	//	System.out.println("created contents:\n" + _contents);
		
		return _contents;
	}
	// convert list of locations to file contents string
	String locationsToFileContents() {
		String _contents = "";
		
		_contents += "[";
		Iterator<Location> _it = listLocations.iterator();
		while (_it.hasNext()) {
			Location _loc = _it.next();
			_contents += _loc.getName();
			if (_it.hasNext()) {
				_contents += ",";
			}
		}
		_contents += "]";
		
		return _contents;
	}
	// convert list of providers to file contents string
	String providersToFileContents() {
		String _contents = "";
		
		_contents += "{";
		Iterator<Provider> _it = listProviders.iterator();
		while (_it.hasNext()) {
			Provider _provider = _it.next();
			_contents += _provider.toFileContents();
			if (_it.hasNext()) {
				_contents += ",";
			}
		}
		_contents += "}";
		
		return _contents;
	}

	// convert string from file for spot class to string to use for manager
	abstract public String getSpotClassFileToString(String _str);
	// convert string from file for layout class to string to use for manager
	abstract public String getLayoutFileToString(String _str);
	
	
	// abstract methods that much be implemented for interface to work correctly
	// to get names of types
	abstract String getSystemTypeName();
	abstract String getLocationTypeName();
	abstract String getProviderTypeName();
	abstract String getTransportTypeName();
	abstract String getSpotTypeName();
	abstract String getSpotClassTypeName();
	abstract String getSpotLayoutClassTypeName();
	abstract String getPreferenceClassTypeName();
	abstract String getTripTypeName();
	abstract String getSpotClassOptions(String _prefix);
	abstract String getPreferenceOptions(String _prefix);
	abstract String getLayoutOptions(String _prefix);
	// public functionality	
	// create location object
	abstract protected Location createLocationObject(String _name) throws InvalidLocationNameLength, InvalidLocationNameCharacter;
	// create provider object
	abstract protected Provider createProviderObject(String _name) throws InvalidProviderNameLength, InvalidProviderNameCharacter;
	// convert spot class string to actual object
	abstract protected SpotClass getSpotClass(String _name);
	// convert spot layout class string to actual object
	abstract protected SpotLayoutClass getLayoutClass(String _name);
	// convert spot preference class string to actual object
	abstract protected SpotPreference getPreferenceClass(String _name);
	// to run before findTrips
	abstract protected void findTripsPre(Location _orig, Location _dest) throws InvalidTripPath;
	abstract protected void findTripsPre(Location _orig, Location _dest, SpotClass _spotClass, LocalDate _date) throws InvalidTripPath;
	// to ran after findTrips
	abstract protected void findTripsPost(boolean _found);
}
