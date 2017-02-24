package actbs;

class InvalidTransportNameLength extends Throwable {
	InvalidTransportNameLength(String _typeName) {
		super(String.format("Invalid %1$s name length (can't be empty)", _typeName));
	}
}
