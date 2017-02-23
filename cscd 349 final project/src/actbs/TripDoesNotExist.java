package actbs;

public class TripDoesNotExist extends Throwable {
	public TripDoesNotExist(String _typeNameTrip, String _tripID, String _typeNameProvider, String _providerName) {
		super(String.format("%1$s \"%2$s\" does not exist for %3$s \"%4$s\"", _typeNameTrip, _tripID, _typeNameProvider, _providerName));
	}
}