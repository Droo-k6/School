package actbs;

class Ship extends Transport {
	static String getTypeName() {return "Ship";}
	String getTypeNameNonStatic() {
		return getTypeName();
	}
	String getTripTypeName() {
		return (CruiseTrip.getTypeName());
	}
	
	// constructor
	Ship(String _name) throws InvalidTransportNameCharacter, InvalidTransportNameLength {
		super(_name);
	}
	
	// validate trip is valid
	void validateTrip(Trip _trip) throws TransportNotAvailable {
		// check if ship is available on dates
		if (!available(_trip.getDate(), _trip.getEndDate())) {
			throw new TransportNotAvailable(getTypeName(),getName(),_trip);
		}
	}
}
