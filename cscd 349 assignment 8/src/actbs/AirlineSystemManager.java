package actbs;
import java.time.LocalDate;
import java.time.YearMonth;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public class AirlineSystemManager extends SystemManager {
	// abstract methods from SystemManager
	// return name of system (Airline)
	String getSystemTypeName() {
		return "Airline";
	}
	// return name of location type (Airport)
	String getLocationTypeName() {
		return Airport.getTypeName();
	}
	// return name of provider  type (Airline)
	String getProviderTypeName() {
		return Airline.getTypeName();
	}
	String getTransportTypeName() {
		throw new UnsupportedOperationException("Airline does not support transports");
	}
	// return name of spot (seat)
	String getSpotTypeName() {
		return Seat.getTypeName();
	}
	// return name of spot class type (Seat)
	String getSpotClassTypeName() {
		return SeatClass.getTypeName();
	}
	// return name of spot layout class type (Layout)
	String getSpotLayoutClassTypeName() {
		return LayoutClass.getTypeName();
	}
	// return name of spot preference class type (Seat Preference)
	String getPreferenceClassTypeName() {
		return SeatPreference.getTypeName();
	}
	// return name of trip class type
	String getTripTypeName() {
		return Flight.getTypeName();
	}
	String getSpotClassOptions(String _prefix) {
		return SeatClass.getOptions(_prefix);
	}
	String getPreferenceOptions(String _prefix) {
		return SeatPreference.getOptions(_prefix);
	}
	String getLayoutOptions(String _prefix) {
		return LayoutClass.getOptions(_prefix);
	}
	
	// create location object (Airport)
	protected Location createLocationObject(String _name) throws InvalidLocationNameLength, InvalidLocationNameCharacter {
		return new Airport(_name);
	}
	
	// create provider object (Airline)
	protected Provider createProviderObject(String _name) throws InvalidProviderNameLength, InvalidProviderNameCharacter {
		return new Airline(_name);
	}
	
	// constructors
	public AirlineSystemManager(String _filename) {
		super(_filename);
	}
	public AirlineSystemManager() {super();}
	
	
	// convert string to proper spot class enum (SeatClass)
	protected SpotClass getSpotClass(String _name) {
		SpotClass _spotClass = SeatClass.fromString(_name);		
		return _spotClass;
	}
	
	// convert string to proper class enum (LayoutClass)
	protected SpotLayoutClass getLayoutClass(String _name) {
		SpotLayoutClass _class = LayoutClass.fromString(_name);
		return _class;
	}
	
	// unsupported
	public void createTransport(String _providerName, String _transportName) {
		throw new UnsupportedOperationException("Airline cannot create transport for airline system");
	}
	
	// convert string to proper class enum (Seat Preference)
	protected SpotPreference getPreferenceClass(String _name) {
		SpotPreference _class = SeatPreference.fromString(_name);
		return _class;
	}
	
	// unsupported
	// moved to handle at flight creation
//	public void createTrip(String _providerName, String[] _locations, int _year, int _month, int _day, String _tripID) {
//		throw new UnsupportedOperationException("Airline cannot create flight for sequence of locations");
//	}
	
	public void createTrip(String _providerName, String[] _locations, int _startYear, int _startMonth, int _startDay, int _startHour, int _startMinute, int _endYear, int _endMonth, int _endDay, int _endHour, int _endMinute, String _tripID) {
		throw new UnsupportedOperationException("Airline cannot create flight with 2 dates");
	}
	
	public void createSpotSection(String _providerName, String _tripID, String _spotClassName, int _spotPrice, int _count) {
		throw new UnsupportedOperationException("Airline flight sections must have layout class");
	}
	
	// pre findTrips, checks that origin/destination not same
	// prints title message
	protected void findTripsPre(Location _orig, Location _dest) throws InvalidTripPath {
		// check that origin and destination are not the same
		if (_orig == _dest) {
			throw new InvalidTripPath(getTripTypeName(),_orig.getName());
		}
		
		System.out.printf("Available Flights from \"%1s\" to \"%2s\": \n", _orig.getName(), _dest.getName());
	}
	protected void findTripsPre(Location _orig, Location _dest, SpotClass _spotClass, LocalDate _date) throws InvalidTripPath {
		// check that origin and destination are not the same
		if (_orig == _dest) {
			throw new InvalidTripPath(getTripTypeName(),_orig.getName());
		}
		
		System.out.printf("Available Flights (for %3$s class) from \"%1s\" to \"%2s\" on %4$s: \n", _orig.getName(), _dest.getName(), _spotClass.toString(), _date.toString());
	}
	
	// post findTrips, posts message if no trips found
	protected void findTripsPost(boolean _found) {
		if (!_found) {
			System.out.println("\tNo Flights available");
		}
	}
	
	// convert string from file for spot class to string to use for manager
	public String getSpotClassFileToString(String _str) {
		return (SeatClass.fromFileString(_str));
	}
	// convert string from file for layout class to string to use for manager
	public String getLayoutFileToString(String _str) {
		return (LayoutClass.fromFileString(_str));
	}
}