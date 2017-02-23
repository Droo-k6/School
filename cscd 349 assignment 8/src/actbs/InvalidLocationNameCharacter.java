package actbs;

public class InvalidLocationNameCharacter extends Throwable {
	public InvalidLocationNameCharacter(String _typeName, String _name, char _character) {
		super(String.format("Invalid %1$s name \"%2$s\", invalid character '%3$c' (must be alphabetic)",_typeName, _name, _character));
	}
}