package actbs;

class InvalidCharacterRead extends Throwable {
	InvalidCharacterRead(char _c) {
		super(String.format("Invalid character read '%1$c'",_c));
	}
	InvalidCharacterRead(char _c, String _str) {
		super(String.format("Invalid character read '%1$c': expected %2$s",_c,_str));
	}
}
