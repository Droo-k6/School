/**
 * Class for simplifying connection details for client/server
 * Mainly a wrapper for the TCP stream to handle messages, since messages will have the first byte be the length
 */

package cscd467.hw45;

import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.Socket;

class Connection {
	// member variables
	private final Socket _socket;
	private final InputStreamReader _in;
	private final OutputStream _out;
	private ConnectionState _state;
	// for the message buffer
	private final char[] _messageBuffer = new char[Parameters.MESSAGE_MAX];
	private int _messageBufferLength = -1, _messageBufferOffset = -1;
	private MessageBufferState _bufferState = MessageBufferState.EMPTY;
	private long _lastActivity = System.currentTimeMillis();
	
	/**
	 * Constructor
	 * @param address, address of server
	 * @param port, port of server
	 * @throws IOException 
	 */
	public Connection(String address, int port) throws IOException {
		this(new Socket(address, port));
	}
	
	/**
	 * Constructor
	 * @param socket, socket for connector
	 * @throws IOException 
	 */
	public Connection(Socket socket) throws IOException {
		_socket = socket;
		_in = new InputStreamReader(_socket.getInputStream());
		_out = _socket.getOutputStream();
		
		_state = ConnectionState.READING;
	}
	
	/**
	 * Gets if the connection is open
	 * @return
	 */
	public boolean isOpen() {
		return (getState() != ConnectionState.CLOSED);
	}
	
	/**
	 * Get time of last send/receive
	 */
	public long getLastTime() {
		return _lastActivity;
	}
	
	/**
	 * Close connection
	 */
	public void close() {
		if (!isOpen()) {
			Log.elog("Connection closed already");
			throw new IllegalStateException("Connection closed already");
		}
		/*
		_out.close();
		try {
			_in.close();
		} catch (IOException e) {
			Log.elogf("Exception closing input stream(\"%s\"): \"%s\"%n", e.getClass(), e.getMessage());
		}
		*/
		
		try {
			_socket.close();
		} catch (IOException e) {
			Log.elogf("Unable to close socket(\"%s\"): \"%s\"%n", e.getClass(), e.getMessage());
		}
		
		Log.log("Connection closed");
		
		setState(ConnectionState.CLOSED);
	}
	
	/**
	 * Send given message across socket
	 * Wraps in messge format
	 * Blocks until fully sent
	 * Returns if message was sent, or the connection was closed
	 * @param msg, string to send
	 * @return if message was sent or connection closed
	 */
	public synchronized boolean send(String msg) {
		if (!isOpen()) {
			Log.elog("Connection not open");
			return false;
		}
		
		//Log.log("Sending message");
		
		// convert to desired format
		msg = msg.trim();
		int length = msg.length();
		if (length <= 0) {
			String errStr = String.format("Message length too short(%d)", length);
			Log.elog(errStr);
			throw new IllegalArgumentException(errStr);
		}
		if (length > Parameters.MESSAGE_MAX) {
			String errStr = String.format("Message length too long(%d)", length);
			Log.elog(errStr);
			throw new IllegalArgumentException(errStr);
		}
		// build message w/ length prefixed on first byte
		byte[] bytes = new byte[1+length];
		bytes[0] = (byte) length;
		byte[] msgBytes = msg.getBytes();
		for(int i = 1; i < length+1; ++i) {
			bytes[i] = msgBytes[i-1];
		}
		
		// write to stream
		try {
			_out.write(bytes);
		} catch (IOException e) {
			Log.elogf("Failed to write to output stream(\"%s\"): \"%s\"%n", e.getClass(), e.getMessage());
			close();
			return false;
		}
		
		_lastActivity = System.currentTimeMillis();
		
		return true;
	}
	
	/**
	 * Receives full message on socket
	 * Blocks until fully received
	 * If connection closes or an error occurred, returns empty string
	 * @return the message
	 */
	public synchronized String receive() {
		if (!isOpen()) {
			Log.elog("Connection not open");
			throw new IllegalStateException("Connection not open");
		}
		
		//Log.log("Waiting for full message");
		
		// grab first byte, length of message
		int length;
		while(true) {
			try {
				length = _in.read();
			} catch (IOException e) {
				Log.elogf("Failed to read in length for message(\"%s\"): \"%s\"%n", e.getClass(), e.getMessage());
				close();
				return "";
			}
			if (length < 0) {
				Log.elog("Stream was closed during read");
				close();
				return "";
			}else{
				break;
			}
		}
		
		// get rest of message
		char[] msg = new char[length];
		int offset = 0;
		while(true) {
			int read;
			try {
				read = _in.read(msg, offset, length - offset);
			} catch (IOException e) {
				Log.elogf("Failed to read message contents(\"%s\"): \"%s\"%n", e.getClass(), e.getMessage());
				close();
				return "";
			}
			if (read < 0) {
				Log.elog("Stream was closed during read");
				close();
				return "";
			}
			offset += read;
			if (offset >= length) {
				break;
			}
		}
		
		//Log.log("Message read");
		_lastActivity = System.currentTimeMillis();
		
		return new String(msg);
	}
	
	/**
	 * Receive but non blocking, 
	 * reading up to the given amount of bytes or until message fully read
	 * @param maxToRead, max amount of bytes to read
	 */
	public synchronized ReceiveResultType receiveLimit(int maxToRead) {
		if (_bufferState == MessageBufferState.READY) {
			Log.elogf("Attempt to read message while message is ready on the buffer");
			return ReceiveResultType.COMPLETE;
		}
		
		// check if bytes available on the buffer
		try {
			if (!_in.ready()) {
				return ReceiveResultType.NOTHING;
			}
		} catch (IOException e) {
			Log.elogf("Exception while checking if input stream ready(\"%s\"): \"%s\"%n", e.getClass(), e.getMessage());
			close();
			return ReceiveResultType.ERROR;
		}
		
		// limit amount to read to the size of the message
		int toRead = Math.min(Parameters.MESSAGE_MAX, maxToRead);
		
		// check if message reading already started, if not - get the length
		if (_bufferState == MessageBufferState.EMPTY) {
			try {
				_messageBufferLength = _in.read();
				//Log.log("Beginning read of next message");
				if (_messageBufferLength < 0) {
					Log.elog("Input buffer ended during length read");
					close();
					return ReceiveResultType.ERROR;
				}else if (_messageBufferLength == 0) {
					// ignore empty messages
					return ReceiveResultType.NOTHING;
				}
				
				_messageBufferOffset = 0;
				--toRead;
				
				_bufferState = MessageBufferState.READING;
			} catch (IOException e) {
				Log.elogf("Exception while reading message length from input stream(\"%s\"): \"%s\"%n", e.getClass(), e.getMessage());
				close();
				return ReceiveResultType.ERROR;
			}
		}
		
		// read in message contents
		toRead = Math.min(toRead, _messageBufferLength - _messageBufferOffset);
		
		try {
			int read = _in.read(_messageBuffer, _messageBufferOffset, toRead);
			if (read == -1) {
				Log.elog("Input buffer ended during message read");
				close();
				return ReceiveResultType.ERROR;
			}
			_messageBufferOffset += read;
			
			// check if full message read
			if (_messageBufferOffset >= _messageBufferLength) {
				//Log.log("Full message received");
				_bufferState = MessageBufferState.READY;
				_messageBuffer[_messageBufferOffset] = 0;
				_lastActivity = System.currentTimeMillis();
				return ReceiveResultType.COMPLETE;
			}
			
		} catch (IOException e) {
			Log.elogf("Exception while reading message length from input stream(\"%s\"): \"%s\"%n", e.getClass(), e.getMessage());
			close();
			return ReceiveResultType.ERROR;
		}
		
		_lastActivity = System.currentTimeMillis();
		return ReceiveResultType.READ;
	}
	
	/**
	 * To be used after receiveLimit() returns a complete result
	 * Returns empty string if error
	 * @return
	 */
	public synchronized String getReceivedMessage() {
		if (_bufferState != MessageBufferState.READY) {
			Log.elog("Attempt to get a receive message when non available");
			return "";
		}
		String msg = new String(_messageBuffer, 0, _messageBufferLength);
		_bufferState = MessageBufferState.EMPTY;
		_messageBufferLength = -1;
		_messageBufferOffset = -1;
		
		return msg;
	}
	
	/**
	 * Set connection to given state
	 * @param state
	 */
	public synchronized void setState(ConnectionState state) {
		_state = state;
	}
	
	/**
	 * Get state of connection
	 * @return
	 */
	public synchronized ConnectionState getState() {
		return _state;
	}
	
	/*
	 * The following methods are only used by the server
	 */
	
	/**
	 * Send a message that the connection is full
	 */
	public synchronized void sendFullJobs() {
		send("The server is busy, try again later");
	}
	
	/**
	 * Send message that command was invalid
	 */
	public synchronized void sendInvalid() {
		send("Invalid command");
	}
	
	/**
	 * Send message that connections are full
	 */
	public synchronized void sendFullConnections() {
		send("The server is full, try again later");
		try {
			close();
		}catch(IllegalStateException e) {
			// do nothing...
		}
	}
	
	/**
	 * Send a message that an error occurred, close connection
	 */
	public synchronized void sendError() {
		send("An error occurred");
		try {
			close();
		}catch(IllegalStateException e) {
			// do nothing...
		}
	}
	
	/**
	 * Send message that job had no result, close connection
	 */
	public synchronized void sendNothing() {
		send("Command completed, no result");
		try {
			close();
		}catch(IllegalStateException e) {
			// do nothing...
		}
	}
	
	/**
	 * Send message that server is shutting down, close connection
	 */
	public synchronized void sendClosing() {
		send("Kill command received, server shutting down");
		try {
			close();
		}catch(IllegalStateException e) {
			// do nothing...
		}
	}
	
	/**
	 * Send message that connection is terminated due to timeout, close connection
	 */
	public synchronized void sendTimeout() {
		send("Connection timed out, closing");
		try {
			close();
		}catch(IllegalStateException e) {
			// do nothing...
		}
	}
	
	/**
	 * Enum for the result of the receiveLimit() method
	 * NOTHING - nothing was read
	 * ERROR - an exception or error occurred
	 * READ - contents were read in
	 * COMPLETE - a complete message was read in/finished
	 */
	static enum ReceiveResultType {
		NOTHING, ERROR, READ, COMPLETE;
	}
	
	/**
	 * Enum for the state of the enum
	 * EMPTY - nothing on the buffer, nothing expected
	 * READING - waiting for a complete message
	 * READY - a complete message is on the buffer
	 */
	static enum MessageBufferState {
		EMPTY, READING, READY;
	}
	
	/**
	 * Enum for state of the connection
	 * CLOSED - connection is closed
	 * READING - connection is awaiting a full message
	 * WAITING - connection is waiting for message to be processed
	 */
	static enum ConnectionState {
		CLOSED, READING, WAITING;
	}
}
