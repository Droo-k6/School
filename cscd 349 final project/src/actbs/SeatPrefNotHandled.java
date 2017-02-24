package actbs;

public class SeatPrefNotHandled extends Throwable {
	public SeatPrefNotHandled(String _pref) {
		super(String.format("Seating preference \"%1$s\" not handled", _pref));
	}
}