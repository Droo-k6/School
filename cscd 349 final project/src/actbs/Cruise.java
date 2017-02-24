package actbs;

import java.time.LocalDate;
import java.util.ArrayList;

class Cruise extends Provider {
	// static/abstract methods for naming
	static String getTypeName() {return "Cruise";}
	String getTypeNameNonStatic() {return getTypeName();}
	String getTripTypeName() {return Trip.getTypeName();}
	String getTransportTypeName() {return Ship.getTypeName();}
	
	// constructor
	Cruise(String _name) throws InvalidProviderNameLength, InvalidProviderNameCharacter {
		super(_name);
	}
	
	protected Trip createTripObject(String _id, LocalDate _date, TimeOfDay _time, ArrayList<Location> _locations) throws InvalidTripIDLength, InvalidTripPath {
		throw new UnsupportedOperationException("Cruise does not support trips with one date");
	}
	
	protected Trip createTripObject(String _id, LocalDate _startDate, TimeOfDay _startTime, LocalDate _endDate, TimeOfDay _endTime, ArrayList<Location> _locations) throws InvalidTripIDLength, InvalidTripPath, InvalidTripIDCharacter, InvalidTripDates {
		return new CruiseTrip(_id, _startDate, _startTime, _endDate, _endTime, _locations);
	}
	
	protected Transport createTransportObject(String _name) throws InvalidTransportNameCharacter, InvalidTransportNameLength {
		return new Ship(_name);
	}
	
	void createSection(String _tripID, SpotClass _spotClass, int _spotPrice, SpotLayoutClass _spotLayoutClass, int _rows) throws TripDoesNotExist, TripSectionExists, InvalidTripSectionRows, InvalidSpotPrice {
		throw new UnsupportedOperationException("Cruise does not support layouts for spots");
	}
	
	void createSection(String _tripID, SpotClass _spotClass, int _spotPrice, int _count) throws InvalidSpotPrice, TripSectionExists, InvalidTripSectionRows, TripDoesNotExist {
		// find Trip
		CruiseTrip _cruise = (CruiseTrip) getTrip(_tripID);
		CabinClass _cabinClass = (CabinClass) _spotClass;
		
		// create section
		_cruise.addSection(_cabinClass, _spotPrice, _count);
	}
	
	void bookSpot(String _tripID, SpotClass _spotClass, int _row, char _col) throws TripDoesNotExist, TripSectionDoesNotExist, SpotIsBooked, InvalidSpotRow, InvalidSpotColumn {
		throw new UnsupportedOperationException("Cruise does not support columns for spots");
	}

	String bookSpot(String _tripID, SpotClass _spotClass, SpotPreference _spotPref) throws TripDoesNotExist, TripSectionDoesNotExist, SpotIsBooked, SeatPrefNotHandled, InvalidLayoutColumn, NoSpotsAvailable {
		throw new UnsupportedOperationException("Cruise does not support preferences for spots");
	}
}


