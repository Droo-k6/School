/**
 * Kill Command
 */

package cscd467.hw45;

class KillCommand extends Command {

	/**
	 * run method for Command
	 * @param server, server object
	 * @return
	 */
	@Override
	public Object run(CommandServer server) {
		server.stop();
		return "Killing server";
	}

}
