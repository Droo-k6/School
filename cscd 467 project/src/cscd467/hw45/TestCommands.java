/**
 * Test for commands
 * For part 9 of hw45
 * @author Mathew McCain
 */

package cscd467.hw45;

import java.io.IOException;

public class TestCommands {

	/**
	 * Main method for test
	 * @param args
	 */
	public static void main(String[] args) {
		Log.log("Starting TestCommands");
		
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
		
		// test commands
		testAdd(client);
		testSubtract(client);
		testMultiply(client);
		testDivide(client);
		
		// send kill
		if (!client.send("KILL")) {
			Log.elogf("Failed to send kill command");
			cleanup(server);
			return;
		}
		
		// disconnect clients
		client.close();
		
		cleanup(server);
		
		Log.log("Ending TestCommands");
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
	
	/**
	 * Test a command to the server
	 * @param client
	 * @param sendStr
	 * @param expectedRcv
	 */
	private static void testCommand(Clientless client, String sendStr, String expectedRcv) {
		client.send(sendStr);
		String result = client.receive();
		Log.logf("Command(\"%s\"), received(\"%s\"), correct result(%b)%n", sendStr, result, result.equalsIgnoreCase(expectedRcv));
	}
	
	/**
	 * Test add command
	 * @param client
	 */
	private static void testAdd(Clientless client) {
		testCommand(client, "add,1,2", "3");
		testCommand(client, "ADD,0,100", "100");
		testCommand(client, "adD,3,4", "7");
		testCommand(client, "ADD ,1,1", "Invalid command");
		testCommand(client, "ADD", "Invalid command");
		testCommand(client, "ADD,1,a1", "Invalid command");
	}
	
	/**
	 * Test subtract command
	 * @param client
	 */
	private static void testSubtract(Clientless client) {
		testCommand(client, "sub,1,2", "-1");
		testCommand(client, "SUB,23,10", "13");
		testCommand(client, "SuB,5,5", "0");
		testCommand(client, "SUB ,1,1", "Invalid command");
		testCommand(client, "SUB", "Invalid command");
		testCommand(client, "SUB,-1,a", "Invalid command");
	}
	
	/**
	 * Test multiply command
	 * @param client
	 */
	private static void testMultiply(Clientless client) {
		testCommand(client, "mul,1,2", "2");
		testCommand(client, "MUL,0,100", "0");
		testCommand(client, "mUl,3,4", "12");
		testCommand(client, "MUL ,1,1", "Invalid command");
		testCommand(client, "mul", "Invalid command");
		testCommand(client, "MUL,b,1", "Invalid command");
	}
	
	/**
	 * Test divide command
	 * @param client
	 */
	private static void testDivide(Clientless client) {
		testCommand(client, "div,4,2", "2.0");
		testCommand(client, "DIV,10,4", "2.5");
		testCommand(client, "dIv,3,3", "1.0");
		testCommand(client, "div ,1,1", "Invalid command");
		testCommand(client, "div", "Invalid command");
		testCommand(client, "div,1,a1", "Invalid command");
	}
	
}
