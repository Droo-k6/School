package actbs;

class TransportNotAvailable extends Throwable {
	TransportNotAvailable(String _typeName, String _name, Trip _trip) {
		super(String.format("%1$s %2$s not available for %3$s %4$s",_typeName,_name,_trip.getTypeNameNonStatic(),_trip.getID()));
	}
}
