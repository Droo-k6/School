package actbs;

public class NoSpotsAvailable extends Throwable {
	// to be rethrown for more specific message
	public NoSpotsAvailable() {
		super("");
	}
	public NoSpotsAvailable(String _typeName, String _id, String _class) {
		super(String.format("No seats available on %1$s \"%2$s\" for %3$s class", _typeName, _id, _class));
	}
}