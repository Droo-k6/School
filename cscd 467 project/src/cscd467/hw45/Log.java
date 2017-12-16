/**
 * Class to log messages for package
 * Offers methods to log to stdout, stderr
 * Prepends messages w/ timestamp
 * Offers println and printf like methods
 * Synchronized to prevent messages from overlapping eachother
 * @author Mathew McCain
 */
package cscd467.hw45;

import java.io.PrintStream;
import java.time.LocalDateTime;

class Log {
	
	/**
	 * log one line of string
	 * @param str
	 */
	public static synchronized void log(String str) {
		slogf(System.out, str + "%n");
	}
	
	/**
	 * log one line of string, to stderr
	 * @param str
	 */
	public static synchronized void elog(String str) {
		slogf(System.err, str + "%n");
	}
	
	/**
	 * log string with arguments through printf
	 * @param str
	 * @param args
	 */
	public static synchronized void logf(String str, Object...args) {
		slogf(System.out, str, args);
	}
	
	/**
	 * log string with arguments through printf to stderr
	 * @param str
	 * @param args
	 */
	public static synchronized void elogf(String str, Object...args) {
		slogf(System.err, str, args);
	}
	
	/**
	 * Printf string and arguments to given stream
	 * @param stream
	 * @param str
	 * @param args
	 */
	public static synchronized void slogf(PrintStream stream, String str, Object...args) {
		// timestamp
		LocalDateTime now = LocalDateTime.now();
		stream.printf("%d-%02d-%02d %02d:%02d:%02d: ", now.getYear(), now.getMonthValue(), now.getDayOfMonth(), now.getHour(), now.getMinute(), now.getSecond());
		// printf
		stream.printf(str, args);
	}
	
}
