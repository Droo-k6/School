package actbs;

public class SpotIsBooked extends Throwable {
	public SpotIsBooked(String _typeName, int _count) {
		super(String.format("%1$s %2$d already booked",_typeName,_count));
	}
	public SpotIsBooked(String _typeName, int _row, char _col) {
		super(String.format("%1$s %2$d%3$c is already booked",_typeName,_row,_col));
	}
}