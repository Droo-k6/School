/**
 * Class for holding all connections on the server and providing access to
 * Acts as a manager over the connections
 */

package cscd467.hw45;

import java.util.ArrayList;

import cscd467.hw45.Connection.ConnectionState;

class ConnectionSet {
	// member variables
	private final ArrayList<Connection> _connections;
	private final CommandServer _server;
	private final JobQueue _queue;
	private final int _maxConnections, _readMax, _interval;
	private double _lastCheck;
	
	/**
	 * Constructor
	 * @param maxConnections, max amount of connections that can be held
	 * @param interval, time in seconds for running the manager
	 * @param readmax, max amount of bytes for the manager to read from a single connection for a single message
	 */
	public ConnectionSet(CommandServer server, JobQueue queue, int maxConnections, int interval, int readmax) {
		_server = server;
		_queue = queue;
		_maxConnections = maxConnections;
		_readMax = readmax;
		_interval = interval;
		_connections = new ArrayList<Connection>();
		_lastCheck = System.currentTimeMillis();
	}
	
	/**
	 * Add new connection
	 * @param connection
	 */
	public void addConnection(Connection connection) {
		Log.log("Adding new connection");
		if (size() >= _maxConnections) {
			Log.elog("Connection set full");
			connection.sendFullConnections();
		}else{
			_connections.add(connection);
		}
	}
	
	/**
	 * Close all connections
	 */
	public void close() {
		for(Connection c : _connections) {
			c.close();
		}
		_connections.clear();
	}
	
	/**
	 * Get number of connections in total
	 * @return
	 */
	public int size() {
		return _connections.size();
	}
	
	/**
	 * refreshes the connection lists
	 * For connections on the reading list, they will all be set to non-blocking read a certain amount of bytes
	 * If a message is finished, then it will be set to a job and moved to the waiting list
	 * For connections on the waiting list, they will be checked and removed if they are closed
	 * They will be moved back to the reading state once their job is processed
	 */
	public void refresh() {
		// check if reached interval
		long currentTime = System.currentTimeMillis();
		if (currentTime < (_lastCheck + _interval)) {
			return;
		}
		_lastCheck = currentTime;
		
		// go through each connection, check state
		ArrayList<Connection> toRemove = new ArrayList<Connection>();
		for(Connection c : _connections) {
			switch(c.getState()) {
			case CLOSED:
				toRemove.add(c);
				break;
			case READING:
				switch(c.receiveLimit(_readMax)) {
				case NOTHING:
					// check timeout
					if ((c.getLastTime() + Parameters.TIMEOUT * 1000) <= System.currentTimeMillis()) {
						Log.log("Removing connection due to timeout");
						c.sendTimeout();
						toRemove.add(c);
					}
					break;
				case READ:
					// do nothing
					break;
				case ERROR:
					// close connection
					c.close();
					toRemove.add(c);
					break;
				case COMPLETE:
					// move connection to waiting state, build job
					String msg = c.getReceivedMessage();
					if (msg.equals("")) {
						c.sendError();
						toRemove.add(c);
						continue;
					}
					
					Job job = new Job(_server, c, msg);
					
					if (!_queue.enqueue(job)) {
						c.sendFullJobs();
					}else{
						c.setState(ConnectionState.WAITING);
					}
					break;
				}
				break;
			case WAITING:
				// do nothing
				break;
			}
		}
		
		// remove any connections
		for(Connection c : toRemove) {
			_connections.remove(c);
			Log.log("Removing connection");
		}
	}
	
}
