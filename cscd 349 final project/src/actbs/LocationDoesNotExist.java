package actbs;

public class LocationDoesNotExist extends Throwable {
	public LocationDoesNotExist(String _typeName, String _name) {
		super(String.format("%1$s \"%2$s\" does not exist",_typeName,_name));
	}
	public LocationDoesNotExist(String _typeName, String _name, String _prefix) {
		super(String.format("%1$s %2$s \"%3$s\" does not exist",_prefix,_typeName,_name));
	}
}