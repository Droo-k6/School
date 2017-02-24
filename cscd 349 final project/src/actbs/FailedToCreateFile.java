package actbs;

public class FailedToCreateFile extends Throwable {
	FailedToCreateFile(String _filename) {
		super(String.format("failed to create file: %1$s", _filename));
	}
}
