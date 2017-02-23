package actbs;

public class InvalidTripIDExists extends Throwable {
	public InvalidTripIDExists(String _typeNameProvider, String _providerName, String _typeNameTrip, String _id) {
		super(String.format("%1$s \"%2$s\" already has %3$s with identifier: \"%4s\"", _typeNameProvider, _providerName, _typeNameTrip, _id));
	}
}