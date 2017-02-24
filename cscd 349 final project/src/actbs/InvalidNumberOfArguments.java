package actbs;

class InvalidNumberOfArguments extends Throwable {
	InvalidNumberOfArguments(int _required, int _num) {
		super(String.format("Invalid number of arguments: %1$d (atleast %2$d required)", _num, _required));
	}
}
