package actbs;

public class InvalidLocationNameExists extends Throwable {
	public InvalidLocationNameExists(String _typeName, String _name) {
		super(String.format("Invalid %1$s name \"%2$s\", %1$s with name already exists",_typeName,_name));
	}
}