package actbs;

public class TripSectionExists extends Throwable {
	public TripSectionExists(String _typeName, String _id, String _class) {
		super(String.format("%1$s \"%2$s\" already has a %3$s class section", _typeName, _id, _class));
	}
}