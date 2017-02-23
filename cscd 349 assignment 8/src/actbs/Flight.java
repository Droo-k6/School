package actbs;
import java.time.LocalDate;
import java.util.ArrayList;

class Flight extends Trip {
	// abstract/overriden methods
	static String getTypeName() {return "Flight";}
	String getTypeNameNonStatic() {return getTypeName();}
	String getSpotTypeName() {return (Seat.getTypeName());}
	String getSpotSectionTypeName() {return (FlightSection.getTypeName());}
	String getTransportTypeName() {throw new UnsupportedOperationException("Flight does not supported assigned transports");}
	
	// SpotSection _section = new FlightSection(_seatClass, _seatPrice, _layout, _rows);
	SpotSection createSpotSectionObject(SpotClass _spotClass, int _spotPrice, SpotLayoutClass _layout, int _count) throws InvalidTripSectionRows, InvalidSpotPrice {
		return new FlightSection((SeatClass)_spotClass, _spotPrice, (LayoutClass) _layout, _count);
	}
	
	SpotSection createSpotSectionObject(SpotClass _spotClass, int _spotPrice, int _count) throws InvalidTripSectionRows, InvalidSpotPrice {
		throw new UnsupportedOperationException("Flights do not support spot sections without layout");
	}
	
	// constructor
	Flight(String _id, LocalDate _date, TimeOfDay _time, ArrayList<Location> _locations) throws InvalidTripIDLength, InvalidTripPath, InvalidTripIDCharacter {
		super(_id, _date, _time);
		
		// check that only 2 locations given
		if (_locations.size() > 2) {
			throw new UnsupportedOperationException("Flights do not support more than 2 locations");
		}
		
		// check that origin and destination not the same
		Airport _origAirport = (Airport) _locations.get(0);
		Airport _destAirport = (Airport) _locations.get(1);
		if (_origAirport == _destAirport) {
			throw new InvalidTripPath(getTypeName(),_origAirport.getName());
		}
		
		listLocations.addAll(_locations);
	}
}