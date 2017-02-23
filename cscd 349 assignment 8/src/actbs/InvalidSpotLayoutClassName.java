package actbs;

public class InvalidSpotLayoutClassName extends Throwable {
	InvalidSpotLayoutClassName(String _typeName, String _name) {
		super(String.format("%1$s class \"%2$s\" invalid", _typeName, _name));
	}
}
