/**
 * Starts server for program
 */
package cscd467.hw45;

import java.io.IOException;

public class Server {
	
	/**
	 * main method, calls server
	 * @param args
	 * @throws IOException 
	 */
	public static void main(String[] args) throws IOException {
		CommandServer.run();
		Log.log("Ending program");
	}

}
