package actbs;

class NoTransportAvailable extends Throwable {
	NoTransportAvailable(String _providerTypeName, String _providerName, String _transportTypeName, String _tripTypeName, String _tripName) {
		super(String.format("%1$s \"%2$s\": no %3$s available for %4$s \"%5$s\"",_providerTypeName,_providerName,_transportTypeName,_tripTypeName, _tripName));
	}
}
