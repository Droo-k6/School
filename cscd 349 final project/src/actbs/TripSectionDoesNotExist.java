package actbs;

public class TripSectionDoesNotExist extends Throwable {
	public TripSectionDoesNotExist(String _typeName, String _id, String _spotClass) {
		super(String.format("%1$s \"%2$s\" does not have a %3$s section", _typeName, _id, _spotClass));
	}
}