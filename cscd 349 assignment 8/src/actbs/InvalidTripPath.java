package actbs;

public class InvalidTripPath extends Throwable {
	public InvalidTripPath(String _typeName, String _locationName) {
		super(String.format("%1$s origin and destination are the same, \"%2$s\"", _typeName, _locationName));
	}
}