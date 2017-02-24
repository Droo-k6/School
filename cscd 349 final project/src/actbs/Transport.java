package actbs;

import java.time.LocalDate;
import java.util.ArrayList;

abstract class Transport {
	// name of transport
	private String name;
	
	// associated trips
	protected ArrayList<Trip> listTrips = new ArrayList<Trip>();
	
	// constructor
	protected Transport(String _name) throws InvalidTransportNameCharacter, InvalidTransportNameLength {
		// validate name
		validateName(_name);
		// convert to uppercase
		_name = _name.toUpperCase();
		name = _name;
	}
	
	// validate name
	// throws exception if issue
	private void validateName(String _name) throws InvalidTransportNameCharacter, InvalidTransportNameLength {
		// check if length valid (> 0)
		if (_name.length() <= 0) {
			throw new InvalidTransportNameLength(getTypeNameNonStatic());
		}
		// check that all characters are alphanumberic
		// check for invalid characters
		for (char _c : (_name.toCharArray())) {
			if (!(Character.isAlphabetic(_c)||Character.isDigit(_c))) {
				throw new InvalidTransportNameCharacter(getTypeNameNonStatic(),_name,_c);
			}
		}
	}
	
	// get name
	String getName() {
		return name;
	}
	
	// add trip
	void addTrip(Trip _trip) throws TransportNotAvailable {
		// check that trip is valid to be added
		validateTrip(_trip);
		// add trip
		listTrips.add(_trip);
		_trip.setTransport(this);
	}
	
	// check if transport is available at date (does not check endDate)
	boolean available(LocalDate _date) {
		for (Trip _trip : listTrips) {
			if (_date.compareTo(_trip.getDate()) == 0) {
				return false;
			}
		}
		return true;
	}
	// check if transport is available between 2 dates
	boolean available(LocalDate _startDate, LocalDate _endDate) {
		for (Trip _trip : listTrips) {
			// if start start date after end, continue
			int _startCmpEnd = _trip.getDate().compareTo(_endDate);
			if (_startCmpEnd > 0) {
				continue;
			}
			// if end date before start date, continue
			int _endCmpStart = _trip.getEndDate().compareTo(_startDate);
			if (_endCmpStart < 0) {
				continue;
			}
			// date is not available
			return false;
		}
		return true;
	}
	
	// typename, should be overwriten
	static String getTypeName() {return "Transport";}
	abstract String getTypeNameNonStatic();
	abstract String getTripTypeName();
	abstract void validateTrip(Trip _trip) throws TransportNotAvailable;
}