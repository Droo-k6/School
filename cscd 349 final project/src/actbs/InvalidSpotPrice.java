package actbs;

public class InvalidSpotPrice extends Throwable {
	public InvalidSpotPrice(String _typeName, int _price) {
		super(String.format("Invalid %1$s price: %2$d (must be >= 0)", _typeName, _price));
	}
	public InvalidSpotPrice(String _airlineName, String _flightID, String _class, String _origin, String _destination, int _previousPrice, int _price) {
		super(String.format("Invalid \"%1$s\" class seat price for flight \"%2$s\" on airline \"%3$s\": %4$d (previous price for \"%5$s\" to \"%6$s\": %7$d)", _class, _flightID, _airlineName, _price, _origin, _destination, _previousPrice));
	}
}