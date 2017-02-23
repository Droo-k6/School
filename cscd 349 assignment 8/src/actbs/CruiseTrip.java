package actbs;

import java.time.Duration;
import java.time.LocalDate;
import java.time.temporal.ChronoUnit;
import java.util.ArrayList;

class CruiseTrip extends Trip {	
	static String getTypeName() {return "Trip";}
	String getTypeNameNonStatic() {return getTypeName();}
	String getSpotTypeName() {return (Cabin.getTypeName());}
	String getSpotSectionTypeName() {return CabinSection.getTypeName();}
	String getTransportTypeName() {return (Ship.getTypeName());}

	// constructor
	CruiseTrip(String _id, LocalDate _startDate, TimeOfDay _startTime, LocalDate _endDate, TimeOfDay _endTime, ArrayList<Location> _locations) throws InvalidTripIDLength, InvalidTripIDCharacter, InvalidTripDates {
		super(_id, _startDate, _startTime, _endDate, _endTime);
		
		validateDates(_startDate,_endDate);
		
		// no location validation
		
		listLocations.addAll(_locations);
	}
	
	// validates dates
	void validateDates(LocalDate _start, LocalDate _end) throws InvalidTripDates {
		// check that start date is before end date
		if (_start.compareTo(_end) >= 0) {
			throw new InvalidTripDates(_start, _end);
		}
		
		// validate different is <= 21 days
		// validate difference is <= 21 days
		long _daysDifference = ChronoUnit.DAYS.between(_start, _end);
		if (_daysDifference > 21) {
			throw new InvalidTripDates(_start,_end,(int) _daysDifference);
		}
	}

	SpotSection createSpotSectionObject(SpotClass _spotClass, int _spotPrice, int _count) throws InvalidTripSectionRows, InvalidSpotPrice {
		return new CabinSection(_spotClass, _spotPrice, _count);
	}
	
	SpotSection createSpotSectionObject(SpotClass _spotClass, int _spotPrice, SpotLayoutClass _layout, int _count) throws InvalidTripSectionRows, InvalidSpotPrice {
		throw new UnsupportedOperationException("Cabin sections do not support layouts");
	}
	
	
}
