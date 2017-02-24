package actbs;

public class InvalidSpotClassName extends Throwable {
	InvalidSpotClassName(String _typeName, String _name) {
		super(String.format("%1$s class \"%2$s\" invalid", _typeName, _name));
	}
}
