package actbs;

public class InvalidProviderNameExists extends Throwable {
	public InvalidProviderNameExists(String _typeName, String _name) {
		super(String.format("Invalid %1$s name \"%2$s\", %1$s with name already exists",_typeName,_name));
	}
}