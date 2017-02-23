package actbs;

import java.io.BufferedWriter;
import java.io.File;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.io.LineNumberReader;

// class for converting system manager to/from a file
// static interaction
class SystemToFromFile {
	// Buffer size to use (for reading)
	// Absolute max
	// ~500 kb
	static final private int BUF_SIZE = 100 * (1024);
	
	// static method that is called
	// takes in manager to use and filename
	static void parseFile(SystemManager _manager, String _filename) throws IOException, InvalidCharacterRead {
		// open file
		File fin = new File(_filename);
		// read all contents to one string
		String _contents = fileToString(_filename);
		// parse
		Parser.parse(_manager,_contents);
	}
	// reads file to one string
	private static String fileToString(String _filename) throws IOException {
		String _content = "";
		
		FileReader _fileReader = new FileReader(_filename);
		LineNumberReader _lineReader = new LineNumberReader(_fileReader, BUF_SIZE);
		
		// read all lines to one string
		boolean _eof = false;
		while (!_eof) {
			String _line = _lineReader.readLine();
			if (_line == null) {
				_eof = true;
			}else{
				_content += _line;
			}
		}
		
		_lineReader.close();
		_fileReader.close();
		
		return _content;
	}
	// static method called to dump to file
	static void dumpFile(SystemManager _manager, String _filename) throws FailedToCreateFile, IOException {
		// Create file
		File _fout = createFile(_filename);
		
		// get contents
		String _contents = _manager.toFileContents();
		
		// dump contents
		dumpContents(_fout,_contents);		
	}
	// create file of filename
	private static File createFile(String _filename) throws FailedToCreateFile, IOException {
		File _fout = new File(_filename);
		if (!_fout.exists()) {
			if (!_fout.createNewFile()) {
				throw new FailedToCreateFile(_filename);
			}
		}
		return _fout;
	}
	// write contents to file
	private static void dumpContents(File _fout, String _contents) throws IOException {
		FileWriter _fw = new FileWriter(_fout);
		BufferedWriter _bw = new BufferedWriter(_fw, BUF_SIZE);
		
		_fw.write(_contents);
		
		_bw.close();
		_fw.close();
	}
}
