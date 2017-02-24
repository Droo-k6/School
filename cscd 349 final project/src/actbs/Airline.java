package actbs;
import java.time.LocalDate;
import java.util.ArrayList;

class Airline extends Provider {
	// abstract methods required by Provider
	// return name of the type (airline)
	static String getTypeName() {return "Airline";};
	String getTypeNameNonStatic() {return getTypeName();}
	String getTripTypeName() {return Flight.getTypeName();}
	String getTransportTypeName() {throw new UnsupportedOperationException("Airline does not support transports");}
	
	// create trip object (Flight)
	protected Trip createTripObject(String _id, LocalDate _date, TimeOfDay _time, ArrayList<Location> _locations) throws InvalidTripIDLength, InvalidTripPath, InvalidTripIDCharacter {
		Trip _trip = new Flight(_id, _date, _time, _locations);
		return _trip;
	}
	protected Trip createTripObject(String _id, LocalDate _startDate, TimeOfDay _startTime, LocalDate _endDate, TimeOfDay _endTime, ArrayList<Location> _locations) throws InvalidTripIDLength, InvalidTripPath {
		throw new UnsupportedOperationException("Airline does not support trips with start/end date");
	}
	
	protected Transport createTransportObject(String _name) {
		throw new UnsupportedOperationException("Airline does not support transports");
	}
	
	// overriding, since no transport are used (no exceptions thrown either)
	protected void findTransportAvailable(Trip _trip) throws NoTransportAvailable {
		// do nothing
		return;
	}
	
	// constructor
	// user super
	Airline(String _name) throws InvalidProviderNameLength, InvalidProviderNameCharacter {
		super(_name);
	}
	
	// toString, returns name
	public String toString() {
		String _str = String.format("\"%1$s\"", name);
		return _str;
	}
	
	// create section for flight
	void createSection(String _tripID, SpotClass _spotClass, int _spotPrice, SpotLayoutClass _spotLayoutClass, int _rows) throws TripDoesNotExist, TripSectionExists, InvalidTripSectionRows, InvalidSpotPrice {
		// find flight
		Flight _flight = (Flight) getTrip(_tripID);
		
		SeatClass _seatClass = (SeatClass) _spotClass;
		LayoutClass _layoutClass = (LayoutClass) _spotLayoutClass;
		
		// validate price
		validatePrice(_flight, _seatClass, _spotPrice);
		
		// create section
		_flight.addSection(_seatClass, _spotPrice, _layoutClass, _rows);
	}
	
	
	void createSection(String _flightID, SpotClass _spotClass, int _seatPrice, int _count) {
		throw new UnsupportedOperationException("Airline does not support sections without layouts");
	}
	
	// validates price for seat class/path follows previous declarations
	protected void validatePrice(Trip _trip, SpotClass _class, int _price) throws InvalidSpotPrice {
		// get price of previous section that match the flights path
		int _sectionPrice = getFlightSectionPrice((Flight)_trip, (SeatClass)_class);
		
		// check if section w/ class was found
		if (_sectionPrice != -1) {
			// check that price matches
			if (_price != _sectionPrice) {
				throw new InvalidSpotPrice(name, _trip.getID(), _class.toString(), _trip.getOriginLocation().getName(), _trip.getDestinationLocation().getName(), _sectionPrice, _price);
			}
		}
	}
	
	// book specific seat
	void bookSpot(String _flightID, SpotClass _spotClass, int _row, char _col) throws TripDoesNotExist, TripSectionDoesNotExist, SpotIsBooked, InvalidSpotRow, InvalidSpotColumn {
		// find flight
		Flight _flight = (Flight) getTrip(_flightID);
		
		SeatClass _class = (SeatClass) _spotClass;
		
		// book seat
		_flight.bookSpot(_class, _row, _col);
	}
	
	// book seat with preference
	String bookSpot(String _tripID, SpotClass _spotClass, SpotPreference _spotPref) throws TripDoesNotExist, TripSectionDoesNotExist, SpotIsBooked, SeatPrefNotHandled, InvalidLayoutColumn, NoSpotsAvailable {
		// find flight
		Flight _flight = (Flight) getTrip(_tripID);
		
		SeatClass _class = (SeatClass) _spotClass;
		SeatPreference _pref = (SeatPreference) _spotPref;
		
		// book seat
		String _seatName = _flight.bookSpot(_class, _pref);
		return _seatName;
	}
	
	// get price of seat for seatclass for path that matches given flight
	// returns -1 if no matching section found
	private int getFlightSectionPrice(Flight _flight, SeatClass _class) {
		// get list of flights that match path
		ArrayList<Trip> _matchedTrips = getTrips(_flight.getOriginLocation(), _flight.getDestinationLocation());
		
		// loop through, find section that matches class and return the price
		for (Trip _trip : _matchedTrips) {
			Flight _checkFlight = (Flight) _trip;
			// ignore if is flight being matched
			if (_checkFlight == _flight) {
				continue;
			}
			// get price of section
			int _sectionPrice = _checkFlight.getSectionPrice(_class);
			// return if price valid
			if (_sectionPrice >= 0) {
				return _sectionPrice;
			}
		}
		
		// no matching section, return -1
		return -1;
	}
	
	// change price of trip section - override for airline specifics about location to location
	void changePrice(String _tripID, SpotClass _spotClass, int _price) throws TripDoesNotExist, TripSectionDoesNotExist, InvalidSpotPrice {
		// find trip
		Trip _tripFind = getTrip(_tripID);
		// check if trip has section
		if (_tripFind.getSectionPrice(_spotClass) == -1) {
			throw new TripSectionDoesNotExist(getTripTypeName(), _tripID, _spotClass.toString());
		}
		
		// find all trips with matching path
		ArrayList<Trip> _trips = getTrips(_tripFind.getOriginLocation(), _tripFind.getDestinationLocation());
		
		for (Trip _trip : _trips) {
			try {
				// request price change
				_trip.changePrice(_spotClass,_price);
			}catch(TripSectionDoesNotExist _e) {
				// do nothing..
			}
		}
	}
}