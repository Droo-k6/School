package actbs;

class InvalidTransportNameCharacter extends Throwable {
	InvalidTransportNameCharacter(String _typeName, String _name, char _c) {
		super(String.format("Invalid character for %1$s name: \"%2$s\": %3$c (must be letter or number)", _typeName, _name, _c));
	}
}
