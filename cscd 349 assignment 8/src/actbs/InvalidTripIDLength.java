package actbs;

public class InvalidTripIDLength extends Throwable {
	public InvalidTripIDLength(String _typeName) {
		super(String.format("Invalid %1$s identifier (cannot be empty)", _typeName));
	}
}