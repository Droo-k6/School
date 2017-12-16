/**
 * Divide Command
 * @author Mathew McCain
 */

package cscd467.hw45;

class DivideCommand extends Command {
	// member variables
	private final double _x, _y;
	
	/**
	 * Constructor
	 * @param x, x parameter
	 * @param y, y parameter
	 */
	public DivideCommand(int x, int y) {
		_x = (double) x;
		_y = (double) y;
	}
	
	/**
	 * run method for Command
	 * @param server, server object
	 * @return result of the division
	 */
	@Override
	public Object run(CommandServer server) {
		return (_x / _y);
	}

}
