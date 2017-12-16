/**
 * Client program
 * Modified from hw45 source
 */

package cscd467.hw45;

import java.awt.event.WindowEvent;
import java.io.IOException;
import javax.swing.JFrame;
import javax.swing.JOptionPane;
import javax.swing.JScrollPane;
import javax.swing.JTextArea;
import javax.swing.JTextField;

class ClientGUI {
	// member variables
	// GUI
	private final JFrame _frame;
	private JTextField _dataField;
	private JTextArea _messageArea;
	// Connection
	private boolean _open = true, _connectionSet = false;
	private Connection _connection;
	
	/**
	 * Constructor
	 */
	public ClientGUI() {
		
		// setup GUI
		_frame = new JFrame("Capitalize Client");
		_dataField = new JTextField(40);
		_messageArea = new JTextArea(8, 60);
		
		// Layout GUI
		_messageArea.setEditable(false);
		_frame.getContentPane().add(_dataField, "North");
		_frame.getContentPane().add(new JScrollPane(_messageArea), "Center");
		
		_dataField.addActionListener(new ClientGUIActionListener(this));
		
		_frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		_frame.pack();
		_frame.setVisible(true);
		
	}
		
	/**
	 * Begin connection to the server
	 * @return if connection was established
	 * @throws IOException
	 */
	public boolean connect() {
		if (_connection != null && _connection.isOpen()) {
			throw new IllegalStateException("Connection already open");
		}
		
		// prompt user for server address
		String serverAddress = JOptionPane.showInputDialog(
				_frame,
				"Enter IP Address of the Server:",
				"Welcome to the Command Program",
				JOptionPane.QUESTION_MESSAGE);
		
		// Make connection and initialize streams
		Log.logf("Connecting to server @ %s:%d%n", serverAddress, Parameters.PORT);
		
		try {
			_connection = new Connection(serverAddress, Parameters.PORT);
			_connectionSet = true;
			return true;
		}catch(IOException e){
			Log.elogf("Failed to open connection(\"%s\"): \"%s\"%n", e.getClass(), e.getMessage());
			return false;
		}
		
	}
	
	/**
	 * Close connection
	 */
	public void close() {
		if (!_open) {
			throw new IllegalStateException("Connection closed already");
		}
		
		if (_connectionSet) {
			_connection.close();
			_connectionSet = false;
		}
		
		_open = false;
	}
	
	/**
	 * Exit GUI
	 */
	public void exit() {
		Log.log("Exiting GUI");
		close();
		_frame.dispatchEvent(new WindowEvent(_frame, WindowEvent.WINDOW_CLOSING));
		_open = false;
	}
	
	/**
	 * Get string from textbox, send to server
	 * @return if the message was sent, false if connection closed or some other issue
	 */
	boolean sendField() {
		String msg = _dataField.getText().trim();
		if (msg.equals("exit")) {
			exit();
			return false;
		}
		
		try {
			if (!_connection.send(msg)) {
				writeMessage("The connection was closed");
				return false;
			}
			return true;
		}catch(IllegalArgumentException e) {
			writeMessage(String.format("Invalid message: \"%s\"", e.getMessage()));
			return false;
		}
		
	}
	
	/**
	 * Get message from connection, blocks
	 * @return the message
	 */
	String receiveMessage() {
		return _connection.receive();
	}
	
	/**
	 * Write message to message area
	 * @param msg
	 */
	void writeMessage(String msg) {
		_messageArea.append(msg + "\n");
		_dataField.selectAll();
	}
	
	/**
	 * Runs the application
	 */
	public static void run() {
		// connect to server
		ClientGUI client = new ClientGUI();
		if (!client.connect()) {
			Log.elog("Failed to establish connection, closing client");
			client.close();
		}
	}
	
}