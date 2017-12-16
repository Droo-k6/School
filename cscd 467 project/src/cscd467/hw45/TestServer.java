/**
 * Wrapper for command server for tests
 * @author Mathew McCain
 */

package cscd467.hw45;

import java.io.IOException;

class TestServer extends Thread {
	// member variables
	private final int _slowdown;
	private boolean _running = false;
	
	/**
	 * Constructor
	 * @param title, title for server thread
	 * @param slowdown, parameteter for server if worker threads should have an artifical slowdown
	 */
	public TestServer(String title, int slowdown) {
		super(title);
		_slowdown = slowdown;
	}
	
	/**
	 * run method, will run server until interrupt and cleanup
	 * similar to CommandServer.run()
	 */
	public void run() {
		Log.logf("Setting up test server(\"%s\") on port %d, %d second worker delay%n", Thread.currentThread().getName(), Parameters.PORT, _slowdown);
		
		synchronized(this) {
			_running = true;
		}
		
		CommandServer server;
		
		try {
			server = new CommandServer(Parameters.PORT, Parameters.MAXCONNECTIONS, Parameters.CONNECTION_MANAGER_INTERVAL, Parameters.CONNECTION_MANAGER_READMAX, Parameters.MAXJOBS, Parameters.INITIALWORKERS, Parameters.MAXWORKERS, Parameters.THRESHHOLD1, Parameters.THRESHHOLD2, Parameters.POOL_MANAGER_INTERVAL, _slowdown);
		} catch (IOException e) {
			Log.elogf("Failed to create server socket(\"%s\"): \"%s\"%n", e.getClass(), e.getMessage());
			return;
		}
		
		server.start();
		server.close();
		
		synchronized(this) {
			_running = false;
		}
	}
	
	public synchronized boolean isRunning() {
		return _running;
	}
}
