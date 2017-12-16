/**
 * Client to be used programmatically
 * @author Mathew McCain
 */

package cscd467.hw45;

import java.io.IOException;

import cscd467.hw45.Connection.ReceiveResultType;

class Clientless {
	// member variables
	private Connection _connection;
	
	/**
	 * Constructor
	 */
	public Clientless() {}
	
	/**
	 * Open a connection
	 * @throws IOException 
	 */
	public void open(String address) throws IOException {
		if (isOpen()) {
			throw new IllegalStateException("Connection already open");
		}
		
		_connection = new Connection(address, Parameters.PORT);
	}
	
	/**
	 * Close connection
	 */
	public void close() {
		if (!isOpen()) {
			throw new IllegalStateException("Connection not open");
		}
		
		_connection.close();
	}
	
	/**
	 * Check if connection is open
	 * @return
	 */
	public boolean isOpen() {
		return (_connection != null) && (_connection.isOpen());
	}
	
	/**
	 * Attempt to send a message
	 * @param msg
	 * @return if the message was sent
	 */
	public boolean send(String msg) {
		if (!isOpen()) {
			throw new IllegalStateException("Connection not open");
		}
		
		try {
			return _connection.send(msg);
		}catch(IllegalArgumentException e) {
			Log.elogf("Clientless send exception(\"%s\"): \"%s\"%n", e.getClass(), e.getMessage());
			return false;
		}
	}
	
	/**
	 * Block until next message received
	 * Will return empty on error
	 * @return
	 */
	public String receive() {
		if (!isOpen()) {
			throw new IllegalStateException("Connection not open");
		}
		
		return _connection.receive();
	}
	
	/**
	 * Non-blocking receive limit
	 * @param maxToRead, max bytes to read
	 * @return the result of the limit
	 */
	public ReceiveResultType receiveLimit(int maxToRead) {
		return _connection.receiveLimit(maxToRead);
	}
	
	/**
	 * Get received message for receiveLimit()
	 * @return
	 */
	public String getReceivedMessage() {
		return _connection.getReceivedMessage();
	}
	
}
