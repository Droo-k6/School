package actbs;

public class InvalidProviderNameLength extends Throwable {
	public InvalidProviderNameLength(String _typeName, String _name) {
		super(String.format("Invalid %1$s name \"%2$s\", invalid length: %3$d (must be less than 6 characters)",_typeName,_name,_name.length()));
	}
}