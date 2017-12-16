/**
 * Test that kill works for the server
 * For part 8 of hw45
 * @author Mathew McCain
 */

package cscd467.hw45;

import java.io.IOException;

public class TestKill {

	/**
	 * Main method for test
	 * @param args
	 */
	public static void main(String[] args) {
		Log.log("Starting TestKill");
		
		// start server thread
		TestServer server = new TestServer("TestClientLoad", 2);
		server.start();
				
		// build/connect client
		Clientless client = new Clientless();
		try {
			client.open("127.0.0.1");
		} catch (IOException e) {
			Log.elogf("Failed to connect to server(\"%s\"): \"%s\"%n", e.getClass(), e.getMessage());
			cleanup(server);
			return;
		}
		
		// send kill
		if (!client.send("KILL")) {
			Log.elogf("Failed to send kill command");
			cleanup(server);
			return;
		}
		
		// disconnect clients
		client.close();
		
		// check that server is stopped
		try {
			while(server.isRunning()) {
				Log.log("Server still running");
				Thread.sleep(1000);
			}
			Log.log("Server ended (killed most likely)");
		} catch (InterruptedException e) {
			Log.elogf("Interrupted while waiting for server to end(\"%s\"): \"%s\"%n", e.getClass(), e.getMessage());
		}
		
		cleanup(server);
		
		Log.log("Ending TestKill");
	}
	
	/**
	 * Cleanup
	 */
	private static void cleanup(TestServer server) {
		// stop server
		server.interrupt();
		try {
			server.join();
		} catch (InterruptedException e) {
			Log.elog("TestServer join interrupted");
		}
	}
	
}
