package actbs;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Iterator;
import java.util.List;

abstract class Provider {
	// name of provider
	protected String name;
	
	// set of associated trips
	protected ArrayList<Trip> listTrips = new ArrayList<Trip>();
	// set of associated transports
	protected ArrayList<Transport> listTransports = new ArrayList<Transport>();
	
	// constructor
	protected Provider(String _name) throws InvalidProviderNameLength, InvalidProviderNameCharacter {
		// convert name to uppercase
		_name = _name.toUpperCase();
		
		// check name validity
		checkName(_name);
		
		this.name = _name;
	}
	
	// checks if given name is valid
	protected void checkName(String _name) throws InvalidProviderNameLength, InvalidProviderNameCharacter {
		// check length
		if (_name.length() >= 6) {
			throw new InvalidProviderNameLength(getTypeNameNonStatic(),_name);
		}
		// check for invalid characters
		for (char _c : (_name.toCharArray())) {
			if (!(Character.isAlphabetic(_c)||(Character.isDigit(_c)))) {
				throw new InvalidProviderNameCharacter(getTypeNameNonStatic(),_name,_c);
			}
		}
	}
	
	// get associated trip for identifier
	protected Trip getTrip(String _tripID) throws TripDoesNotExist {
		for (Trip _trip : listTrips) {
			if (_trip.getID().equalsIgnoreCase(_tripID)) {
				return _trip;
			}
		}
		throw new TripDoesNotExist(getTripTypeName(),_tripID,getTypeNameNonStatic(),name);
	}
	
	// get transport by name
	protected Transport getTransport(String _name) throws TransportDoesNotExist {
		for (Transport _transport : listTransports) {
			if (_transport.getName().equalsIgnoreCase(_name)) {
				return _transport;
			}
		}
		throw new TransportDoesNotExist(getTransportTypeName(),_name,getTypeNameNonStatic(),name);
	}
	
	// create transport
	void createTransport(String _transportName) throws InvalidTransportNameCharacter, InvalidTransportNameLength, InvalidTransportNameExists {
		// ensure name is unique
		try {
			Transport _transport = getTransport(_transportName);
		}catch(TransportDoesNotExist _e) {
			// create trip
			Transport _transport = createTransportObject(_transportName);
			
			// add to flightlist
			listTransports.add(_transport);
			
			return;
		}
		throw new InvalidTransportNameExists(getTypeNameNonStatic(), name, getTripTypeName(), _transportName);
	}
	
	// create associated flight (2 locations)
	void createTrip(String _identifier, LocalDate _date, TimeOfDay _time, Location _orig, Location _dest) throws InvalidTripIDLength, InvalidTripIDExists, InvalidTripPath, InvalidTripIDCharacter, NoTransportAvailable {
		ArrayList<Location> _locations = new ArrayList<Location>();
		_locations.add(_orig);
		_locations.add(_dest);
		createTrip(_identifier, _date, _time, _locations);
	}
	
	// create trip (list of locations)
	void createTrip(String _identifier, LocalDate _date, TimeOfDay _time, ArrayList<Location> _locations) throws InvalidTripIDLength, InvalidTripIDExists, InvalidTripPath, InvalidTripIDCharacter, NoTransportAvailable {
		try {
			Trip _trip = getTrip(_identifier);
		}catch(TripDoesNotExist _e) {
			// create trip
			Trip _trip = createTripObject(_identifier,_date,_time,_locations);
			
			// add to available transport
			findTransportAvailable(_trip);
			
			// add to flightlist
			listTrips.add(_trip);
			
			return;
		}
		throw new InvalidTripIDExists(getTypeNameNonStatic(), name, getTripTypeName(), _identifier);
	}
	
	// create trip (2 dates)
	void createTrip(String _identifier, LocalDate _date, TimeOfDay _time, LocalDate _endDate, TimeOfDay _endTime, ArrayList<Location> _locations) throws InvalidTripIDLength, InvalidTripIDExists, InvalidTripPath, InvalidTripIDCharacter, NoTransportAvailable, InvalidTripDates {
		try {
			Trip _trip = getTrip(_identifier);
		}catch(TripDoesNotExist _e) {
			// create trip
			Trip _trip = createTripObject(_identifier,_date,_time,_endDate,_endTime,_locations);
			
			// add to available transport
			findTransportAvailable(_trip);
			
			// add to flightlist
			listTrips.add(_trip);
			
			// to avoid exception throw
			return;
		}
		throw new InvalidTripIDExists(getTypeNameNonStatic(), name, getTripTypeName(), _identifier);
	}
	
	// book spot with just spot class
	String bookSpot(String _tripID, SpotClass _spotClass) throws NoSpotsAvailable, SpotIsBooked, TripSectionDoesNotExist, TripDoesNotExist {
		// find flight
		Trip _trip = getTrip(_tripID);
		
		// book seat
		String _seatName = _trip.bookSpot(_spotClass);
		return _seatName;
	}
	
	protected void findTransportAvailable(Trip _trip) throws NoTransportAvailable {
		// messy method to determine if a transport is available/added
		// but works
		for (Transport _transport : listTransports) {
			boolean added = true;
			try {
				_transport.addTrip(_trip);
			}catch(TransportNotAvailable _e) {
				added = false;
			}
			if (added) {
				return;
			}
		}
		throw new NoTransportAvailable(getTypeNameNonStatic(),name,getTransportTypeName(),getTripTypeName(),_trip.getID());
	}
	
	// get flights from location to location
	ArrayList<Trip> getTrips(Location _origin, Location _destination) {
		ArrayList<Trip> _trips = new ArrayList<Trip>();
		
		for (Trip _trip : listTrips) {
			// check if path matches desired path
			if (_trip.matchPath(_origin,_destination)) {
				// check that space is available
				if (_trip.getAvailableSpots() > 0) {
					_trips.add(_trip);
				}
			}
		}
		
		return _trips;
	}
	
	// get trips from location to location, with class and on date
	ArrayList<Trip> getTrips(Location _origin, Location _destination, SpotClass _spotClass, LocalDate _date) {
		ArrayList<Trip> _trips = new ArrayList<Trip>();
		
		for (Trip _trip : listTrips) {
			// check if path matches desired path
			if (_trip.matchPath(_origin,_destination,_date)) {
				// check that space is available
				if (_trip.getAvailableSpots(_spotClass) > 0) {
					_trips.add(_trip);
				}
			}
		}
		
		return _trips;
	}
	
	// change price of trip section
	void changePrice(String _tripID, SpotClass _spotClass, int _price) throws TripDoesNotExist, TripSectionDoesNotExist, InvalidSpotPrice {
		// find trip
		Trip _trip = getTrip(_tripID);
		// request price change
		_trip.changePrice(_spotClass,_price);
	}
	
	// change price of trips with matching origin/destination
	void changePrice(Location _orig, Location _dest, SpotClass _class, int _price) throws TripSectionDoesNotExist, InvalidSpotPrice {
		// find trips
		ArrayList<Trip> _trips = getTrips(_orig,_dest);
		// change price
		for (Trip _trip : _trips) {
			if (_trip.getSectionPrice(_class) != -1) {
				_trip.changePrice(_class, _price);
			}
		}
	}
	
	// get name
	String getName() {
		return (this.name);
	}
	
	// check if name matches name of object
	boolean nameEquals(String _name) {
		return (name.equalsIgnoreCase(_name));
	}
	
	// dispaly details on Airline
	void displayDetails(String _prefix) {
		System.out.println(_prefix + getName());
		System.out.println(_prefix + listTrips.size() + " " + getTripTypeName() + "s");
		for (Trip _trip : listTrips) {
			_trip.displayDetails(_prefix+"\t");
		}
	}
	
	// get as file contents
	String toFileContents() {
		String _contents = getName();
		
		// get list of transports
		_contents += transportsToFileContents();
		
		// get list of trips
		_contents += tripsToFileContents();
		
		return _contents;
	}
	// get list of transports to file contents
	private String transportsToFileContents() {
		String _contents = "";
		for (Transport _transport : listTransports) {
			_contents += "|" + _transport.getName();
		}
		return _contents;
	}
	// get list of trips to file contents
	private String tripsToFileContents() {
		String _contents = "";
		_contents += "[";
		
		Iterator<Trip> _it = listTrips.iterator();
		while (_it.hasNext()) {
			Trip _trip = _it.next();
			_contents += _trip.toFileContents();
			if (_it.hasNext()) {
				_contents += ",";
			}
		}
		
		_contents += "]";
		return _contents;
	}
	
	// should be overridded
	// returns display name of the type
	static String getTypeName() {return "Provider";};
	// just so exception messages display correctly
	abstract String getTypeNameNonStatic();
	abstract String getTripTypeName();
	abstract String getTransportTypeName();
	// abstract methods
	// create trip object
	abstract protected Trip createTripObject(String _id, LocalDate _date, TimeOfDay _time, ArrayList<Location> _locations) throws InvalidTripIDLength, InvalidTripPath, InvalidTripIDCharacter;
	abstract protected Trip createTripObject(String _id, LocalDate _startDate, TimeOfDay _time, LocalDate _endDate, TimeOfDay _endTime, ArrayList<Location> _locations) throws InvalidTripIDLength, InvalidTripPath, InvalidTripIDCharacter, InvalidTripDates;
	abstract protected Transport createTransportObject(String _name) throws InvalidTransportNameCharacter, InvalidTransportNameLength;
	// validate name for construction
	abstract void createSection(String _tripID, SpotClass _spotClass, int _spotPrice, SpotLayoutClass _spotLayoutClass, int _rows) throws TripDoesNotExist, TripSectionExists, InvalidTripSectionRows, InvalidSpotPrice;
	abstract void createSection(String _tripID, SpotClass _spotClass, int _seatPrice, int _count) throws InvalidSpotPrice, TripSectionExists, InvalidTripSectionRows, TripDoesNotExist;
	abstract void bookSpot(String _tripID, SpotClass _spotClass, int _row, char _col) throws TripDoesNotExist, TripSectionDoesNotExist, SpotIsBooked, InvalidSpotRow, InvalidSpotColumn;
	abstract String bookSpot(String _tripID, SpotClass _spotClass, SpotPreference _spotPref) throws TripDoesNotExist, TripSectionDoesNotExist, SpotIsBooked, SeatPrefNotHandled, InvalidLayoutColumn, NoSpotsAvailable;
}