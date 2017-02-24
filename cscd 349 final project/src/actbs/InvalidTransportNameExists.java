package actbs;

public class InvalidTransportNameExists extends Throwable {
	public InvalidTransportNameExists(String _typeNameProvider, String _providerName, String _typeNameTrip, String _id) {
		super(String.format("%1$s \"%2$s\" already has %3$s with name: \"%2s\"", _typeNameProvider, _providerName, _typeNameTrip, _id));
	}
}