/**
 * Server program
 * Modified from hw45 source
 */

package cscd467.hw45;

import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;
import java.net.SocketTimeoutException;

class CommandServer {
	// member variables
	private final ServerSocket _listener;
	private boolean _open = true, _running = false;
	private final ConnectionSet _connections;
	private final ThreadManager _manager;
	private final ThreadPool _pool;
	private final JobQueue _queue;
	private final int _slowdown;
	
	/**
	 * Constructor
	 * @param port, port of server
	 * @throws IOException 
	 */
	public CommandServer(int port, int maxConnections, int refreshInterval, int readMax, int maxJobs, int minWorkers, int maxWorkers, int t1, int t2, int v, int slowdown) throws IOException {
		_slowdown = slowdown;
		_listener = new ServerSocket(port);
		_listener.setSoTimeout(1);
		_queue = new JobQueue(maxJobs);
		_connections = new ConnectionSet(this, _queue, maxConnections, refreshInterval, readMax);
		_pool = new ThreadPool(_queue, minWorkers, maxWorkers, _slowdown);
		_manager = new ThreadManager(_queue, _pool, t1, t2, v);
		Log.log("Server created");
	}
	
	/**
	 * start the server
	 * @throws IOException 
	 */
	public void start() {
		if (!_open) {
			Log.elog("Server is closed");
			throw new IllegalStateException("Server is closed");
		}
		
		Log.log("Starting server loop");
		_pool.start();
		
		_running = true;
		
		// main server loop
		while (true) {
			// check for interrupt
			if (Thread.interrupted()) {
				Log.elog("Server loop interrupted");
				break;
			}
			if (!_running) {
				Log.log("Server loop stopped");
				break;
			}
			
			// check for new connections
			// new Capitalizer(listener.accept(), clientNumber++).start();
			try {
				Socket sock = _listener.accept();
				_connections.addConnection(new Connection(sock));
			}catch(SocketTimeoutException e) {
				// ignore
			}catch (IOException e) {
				Log.elogf("Exception while accepting connection(\"%s\"): \"%s\"%n", e.getClass(), e.getMessage());
			}
			
			// refresh connections
			_connections.refresh();
			
			// run manager
			_manager.run();
			
		}
		
		Log.log("Server loop stopping");
		
		// stop all threads, close connections
		_pool.stop();
		
		Log.log("Server loop ended");
	}
	
	/**
	 * Stops the server
	 */
	public void stop() {
		_running = false;
	}
	
	/**
	 * Check if server loop is running
	 * @return
	 */
	public boolean isRunning() {
		return _running;
	}
	
	/**
	 * cleans up server resources
	 */
	public void close() {
		if (!_open) {
			Log.elog("Server is closed");
			throw new IllegalStateException("Server is closed");
		}
		
		Log.log("Closing server");
		
		// cleanup
		try {
			_listener.close();
		}catch(IOException e) {
			Log.elogf("Unable to close listener socket(\"%s\": \"%s\"%n", e.getClass(), e.getMessage());
		}
		
		_open = false;
		
		Log.log("Server closed");
	}
	
	/**
	 * Setups, runs and cleans up server
	 * @param port
	 */
	public static void run() {
		Log.logf("Setting up server on port %d%n", Parameters.PORT);
		
		CommandServer server;
		try {
			server = new CommandServer(Parameters.PORT, Parameters.MAXCONNECTIONS, Parameters.CONNECTION_MANAGER_INTERVAL, Parameters.CONNECTION_MANAGER_READMAX, Parameters.MAXJOBS, Parameters.INITIALWORKERS, Parameters.MAXWORKERS, Parameters.THRESHHOLD1, Parameters.THRESHHOLD2, Parameters.POOL_MANAGER_INTERVAL, Parameters.SLOWDOWN);
		} catch (IOException e) {
			Log.elogf("Failed to create server socket(\"%s\"): \"%s\"%n", e.getClass(), e.getMessage());
			return;
		}
		
		server.start();
		server.close();
	}
}
