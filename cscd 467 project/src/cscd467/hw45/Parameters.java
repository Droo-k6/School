/**
 * Class to contains parameters for the package
 * Mostly for the server
 */

package cscd467.hw45;

class Parameters {
	// Worker pool
	public static final int INITIALWORKERS = 5;
	public static final int MAXWORKERS = 50;
	public static final int THRESHHOLD1 = 10;
	public static final int THRESHHOLD2 = 20;
	public static final int POOL_MANAGER_INTERVAL = 5;
	public static final int SLOWDOWN = 0;
	// Jobs
	public static final int MAXJOBS = 50;
	// Connections
	public static final int PORT = 9898;
	public static final int MAXCONNECTIONS = 50;
	public static final int CONNECTION_MANAGER_INTERVAL = 1;
	public static final int CONNECTION_MANAGER_READMAX = 127;
	public static final int MESSAGE_MAX = 127;
	public static final long TIMEOUT = 10;
}
