/**
 * Subtract Command
 * @author Mathew McCain
 */

package cscd467.hw45;

class SubtractCommand extends Command {
	// member variables
	private final int _x, _y;
	
	/**
	 * Constructor
	 * @param x, x parameter
	 * @param y, y parameter
	 */
	public SubtractCommand(int x, int y) {
		_x = x;
		_y = y;
	}
	
	/**
	 * run method for Command
	 * @param server, server object
	 * @return result of the subtraction
	 */
	@Override
	public Object run(CommandServer server) {
		return (_x-_y);
	}

}
