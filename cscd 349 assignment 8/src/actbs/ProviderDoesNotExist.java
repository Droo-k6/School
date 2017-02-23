package actbs;

public class ProviderDoesNotExist extends Throwable {
	public ProviderDoesNotExist(String _providerTypeName, String _providerName) {
		super(String.format("%1$s \"%2$s\" does not exist", _providerTypeName, _providerName));
	}
}