/**
 * Command to represent an invalid command
 * @author Mathew McCain
 */

package cscd467.hw45;

class InvalidCommand extends Command {
	
	@Override
	public Object run(CommandServer server) {
		throw new UnsupportedOperationException();
	}

}
