/**
 * Test that will setup a server and lots of clients
 * For part 6, 7 of hw45
 * @author Mathew McCain
 */

package cscd467.hw45;

import java.io.IOException;
import java.util.ArrayList;
import java.util.Random;

import cscd467.hw45.Connection.ReceiveResultType;

public class TestClientLoad {
	// vars for test
	// test this many clients at once
	private static final int MAX_CLIENTS = 50;
	// test this many clients total
	private static final int TOTAL_MESSAGES = 500;
	private static final Random rand = new Random();
	private static final String[] commands = new String[] { "add", "sub", "mul", "div" };

	/**
	 * Main method for test
	 * @param args
	 */
	public static void main(String[] args) {
		Log.log("Starting TestClientLoad");

		// start server thread
		TestServer server = new TestServer("TestClientLoad", 1);
		server.start();

		Log.log("Waiting a bit for server startup");
		try {
			Thread.sleep(5000);
		} catch (InterruptedException e1) {
			Log.elog("Interrupted while waiting for server startup");
			return;
		}
		
		Log.logf("Starting job build up, max clients(%d) total messages(%d)", MAX_CLIENTS, TOTAL_MESSAGES );
		
		// build/connect clients
		ArrayList<TempClient> clients = new ArrayList<TempClient>();
		ArrayList<TempClient> toRemove = new ArrayList<TempClient>();
		int commandsSent = 0;
		while (commandsSent < TOTAL_MESSAGES) {
			// create more clients
			if (clients.size() < MAX_CLIENTS) {
				TempClient client = new TempClient();
				try {
					client.client.open("127.0.0.1");
				} catch (IOException e) {
					Log.elogf("Failed to open temp client(\"%s\"): \"%s\"%n", e.getClass(), e.getMessage());
					break;
				}
				client.state = TempClientState.IDLE;
				clients.add(client);
			}
			// check clients
			for(TempClient tc : clients) {
				switch(tc.state) {
				case IDLE:
					// send command
					if (!tc.client.send(generateCmd())) {
						Log.elog("Failed to send message, stopping test");
						break;
					}
					tc.state = TempClientState.WAITING;
					break;
				case WAITING:
					// check for received message
					ReceiveResultType result = tc.client.receiveLimit(Parameters.MESSAGE_MAX);
					switch(result) {
					case NOTHING:
						break;
					case ERROR:
						toRemove.add(tc);
						break;
					case READ:
						break;
					case COMPLETE:
						// get received message, do nothing with
						tc.client.getReceivedMessage();
						tc.state = TempClientState.IDLE;
						commandsSent++;
						break;
					}
					break;
				}
			}
			// remove any clients that require it
			for(TempClient tc : toRemove) {
				tc.client.close();
			}
			toRemove.clear();
		}

		// disconnect clients
		Log.log("Cleaning up clients");
		for(TempClient tc : clients) {
			tc.client.close();
		}
		clients.clear();
		
		Log.log("Waiting for connections to timeout on server, and for jobs to die down");
		try {
			Thread.sleep(30 * 1000);
			Log.log("Server should be cleaned up connections and back to minimum workers");
		} catch (InterruptedException e1) {
			Log.elog("Interrupted while waiting on server");
		}

		Log.log("Interrupting server");
		// stop server
		server.interrupt();
		try {
			Log.log("Joining server");
			server.join();
		} catch (InterruptedException e) {
			Log.elog("TestServer join interrupted");
		}

		Log.log("Ending TestClientLoad");
	}

	/**
	 * Generate a random cmd string
	 * @return
	 */
	private static String generateCmd() {
		// generate random command
		String cmd = commands[rand.nextInt(commands.length)];
		// generate random integers
		int x = rand.nextInt(100);
		int y = rand.nextInt(100);

		return String.format("%s,%d,%d", cmd, x, y);
	}

	// wrapper class for Clientless, track expected result, sent message

	/**
	 * Wrapper class for temp clients
	 */
	private static class TempClient {
		// member variables
		public TempClientState state = TempClientState.IDLE;
		public Clientless client = new Clientless();

		public TempClient() {
		};
	}

	/**
	 * State of temp client
	 * IDLE - waiting to send a message
	 * WAITING - waiting to receive a message
	 */
	private static enum TempClientState {
		IDLE, WAITING;
	}
}
