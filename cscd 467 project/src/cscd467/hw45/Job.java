/**
 * Base class for server jobs
 * @author Mathew McCain
 */

package cscd467.hw45;

import cscd467.hw45.Connection.ConnectionState;

class Job {
	// member variables
	private final CommandServer _server;
	private final Connection _connection;
	private final String _message;
	
	/**
	 * Constructor
	 * @param connection, connection associated w/ the job
	 * @param msg, message to process
	 */
	public Job(CommandServer server, Connection connection, String msg) {
		_server = server;
		_connection = connection;
		_message = msg;
	}
	
	/**
	 * to process the job
	 */
	public final void run() {
		// process message to a command		
		Command cmd = Command.parseCommand(_message);
		// check if cmd valid
		if (cmd instanceof InvalidCommand) {
			_connection.sendInvalid();
		}else{
			// run command, send response
			Object result = cmd.run(_server);
			_connection.send(result.toString());
		}
		
		_connection.setState(ConnectionState.READING);
	}
	
	/**
	 * Get connection for the job
	 * @return
	 */
	public Connection getConnection() {
		return _connection;
	}
	
}
