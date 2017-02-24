package actbs;

import java.time.LocalDate;

public class CruiseSystemManager extends SystemManager {
	// abstract methods from SystemManager
	String getSystemTypeName() {return "Cruise";}
	// return name of location type (Seaport)
	String getLocationTypeName() {
		return (Seaport.getTypeName());
	}
	// return name of provider type (Cruise)
	String getProviderTypeName() {
		return (Cruise.getTypeName());
	}
	// return name of spot (cabin)
	String getSpotTypeName() {
		return Cabin.getTypeName();
	}
	String getTransportTypeName() {
		return (Ship.getTypeName());
	}
	String getSpotClassTypeName() {
		return (CabinClass.getTypeName());
	}
	String getSpotLayoutClassTypeName() {
		throw new UnsupportedOperationException("Cruise does not support cabin layouts");
	}
	String getPreferenceClassTypeName() {
		throw new UnsupportedOperationException("Cruise does not support cabin preferences");
	}
	String getLayoutOptions(String _prefix) {
		throw new UnsupportedOperationException("Cruise does not support cabin layouts");
	}
	String getTripTypeName() {
		return (Cruise.getTypeName());
	}
	String getSpotClassOptions(String _prefix) {
		String _str = CabinClass.getOptions(_prefix);
		return _str;
	}
	String getPreferenceOptions(String _prefix) {
		throw new UnsupportedOperationException("Cruise does not support cabin preferences");
	}
	
	// constructors
	public CruiseSystemManager(String _filename) {
		super(_filename);
	}
	public CruiseSystemManager() {super();}
	
	protected Location createLocationObject(String _name)throws InvalidLocationNameLength, InvalidLocationNameCharacter {
		return new Seaport(_name);
	}
	
	protected Provider createProviderObject(String _name)throws InvalidProviderNameLength, InvalidProviderNameCharacter {
		return new Cruise(_name);
	}
	
	public void createTrip(String _providerName, String[] _locations, int _year, int _month, int _day, int _hour, int _minute, String _tripID) {
		throw new UnsupportedOperationException("Cruise does not support trips with 1 date");
	}
	
	protected SpotClass getSpotClass(String _name) {return CabinClass.fromString(_name);}
	protected SpotLayoutClass getLayoutClass(String _name) {
		throw new UnsupportedOperationException("Cruise does not support cabin layouts");
	}
	protected SpotPreference getPreferenceClass(String _name) {
		throw new UnsupportedOperationException("Cruise does not support cabin preferences");
	}
	
	protected void findTripsPre(Location _orig, Location _dest) throws InvalidTripPath {
		System.out.printf("Available Trips from \"%1s\" to \"%2s\": \n", _orig.getName(), _dest.getName());
	}
	protected void findTripsPre(Location _orig, Location _dest, SpotClass _spotClass, LocalDate _date) throws InvalidTripPath {		
		System.out.printf("Available Trips (for %3$s class) from \"%1s\" to \"%2s\" on %4$s: \n", _orig.getName(), _dest.getName(), _spotClass.toString(), _date.toString());
	}
	protected void findTripsPost(boolean _found) {
		if (!_found) {
			System.out.println("\tNo Trips available");
		}
	}
	
	// convert string from file for spot class to string to use for manager
	public String getSpotClassFileToString(String _str) {
		return (CabinClass.fromFileString(_str));
	}
	// convert string from file for layout class to string to use for manager
	public String getLayoutFileToString(String _str) {
		return null;
	}
}
