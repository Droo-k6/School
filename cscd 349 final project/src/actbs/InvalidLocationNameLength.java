package actbs;

public class InvalidLocationNameLength extends Throwable {
	public InvalidLocationNameLength(String _typeName, String _name) {
		super(String.format("Invalid %1$s name \"%2$s\", invalid length: %3$d (must be 3 characters)",_typeName,_name,_name.length()));
	}
}