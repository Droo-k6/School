/**
 * Base class for commands
 * @author Mathew McCain
 */

package cscd467.hw45;

import java.util.Arrays;
import java.util.List;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

abstract class Command {
	
	/**
	 * Class for matching a string to a regex and building the command
	 */
	static abstract class CommandBuilder {
		// member variables
		private final Pattern _pattern;
		
		/**
		 * Build builder
		 * @param pattern, regex pattern to use
		 */
		public CommandBuilder(String pattern) {
			_pattern = Pattern.compile(pattern, Pattern.CASE_INSENSITIVE);
		}
		/**
		 * Parse matcher into a command
		 * @param matcher
		 * @return
		 */
		public Command parse(String str) {
			Matcher matcher = _pattern.matcher(str);
			if (!matcher.matches()) {
				return new InvalidCommand();
			}
			return build(matcher);
		}
		/**
		 * Build command from matcher
		 * @param matcher
		 * @return
		 */
		protected abstract Command build(Matcher matcher);
		/**
		 * Static method to pull all groups from a matcher
		 * @param matcher
		 * @return string array of groups
		 */
		protected static String[] getGroups(Matcher matcher) {
			String[] groups = new String[matcher.groupCount()];
			for(int i = 1; i <= matcher.groupCount(); ++i) {
				groups[i-1] = matcher.group(i);
			}
			return groups;
		}
	}
	
	/**
	 * Builder for add commands
	 */
	static class AddCommandBuilder extends CommandBuilder {
		// constructor
		public AddCommandBuilder() {
			super("^ADD,(\\d+),(\\d+)$");
		}
		// build
		@Override
		protected Command build(Matcher matcher) {
			// parse out arguments
			String[] groups = getGroups(matcher);
			int x = Integer.parseInt(groups[0]);
			int y = Integer.parseInt(groups[1]);
			return new AddCommand(x,y);
		}
	}
	
	/**
	 * Builder for subtract commands
	 */
	static class SubtractCommandBuilder extends CommandBuilder {
		// constructor
		public SubtractCommandBuilder() {
			super("^SUB,(\\d+),(\\d+)$");
		}
		// build
		@Override
		protected Command build(Matcher matcher) {
			// parse out arguments
			String[] groups = getGroups(matcher);
			int x = Integer.parseInt(groups[0]);
			int y = Integer.parseInt(groups[1]);
			return new SubtractCommand(x,y);
		}
	}
	
	/**
	 * Builder for multiply commands
	 */
	static class MultiplyCommandBuilder extends CommandBuilder {
		// constructor
		public MultiplyCommandBuilder() {
			super("^MUL,(\\d+),(\\d+)$");
		}
		// build
		@Override
		protected Command build(Matcher matcher) {
			// parse out arguments
			String[] groups = getGroups(matcher);
			int x = Integer.parseInt(groups[0]);
			int y = Integer.parseInt(groups[1]);
			return new MultiplyCommand(x,y);
		}
	}
	
	/**
	 * Builder for divide commands
	 */
	static class DivideCommandBuilder extends CommandBuilder {
		// constructor
		public DivideCommandBuilder() {
			super("^DIV,(\\d+),(\\d+)$");
		}
		// build
		@Override
		protected Command build(Matcher matcher) {
			// parse out arguments
			String[] groups = getGroups(matcher);
			int x = Integer.parseInt(groups[0]);
			int y = Integer.parseInt(groups[1]);
			return new DivideCommand(x,y);
		}
	}
	
	/**
	 * Builder for kill commands
	 */
	static class KillCommandBuilder extends CommandBuilder {
		// constructor
		public KillCommandBuilder() {
			super("^KILL$");
		}
		// build
		@Override
		protected Command build(Matcher matcher) {
			return new KillCommand();
		}
	}
	
	// member variables
	private static final List<CommandBuilder> _builders = Arrays.asList(
			new AddCommandBuilder(),
			new SubtractCommandBuilder(),
			new MultiplyCommandBuilder(),
			new DivideCommandBuilder(),
			new KillCommandBuilder());
	
	/**
	 * Abstract method for running the command
	 * @return
	 */
	public abstract Object run(CommandServer server);
	
	/**
	 * Parse command from string
	 * @param str
	 * @return command object
	 */
	public static Command parseCommand(String str) {
		for(CommandBuilder builder : _builders) {
			Command cmd = builder.parse(str);
			if (!(cmd instanceof InvalidCommand)) {
				return cmd;
			}
		}
		return new InvalidCommand();
	}
}
