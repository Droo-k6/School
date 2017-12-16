/**
 * Used to parse information from a treepath for S3Interaction
 */

package cscd467.hw7;

import java.util.StringJoiner;

import javax.swing.tree.DefaultMutableTreeNode;
import javax.swing.tree.TreePath;

class PathInfo {
	// member variables
	private final String _bucketName;
	private final String _key;
	private final String _itemName;
	
	/**
	 * Constructor for a path and a filename
	 * @param path
	 * @param itemName
	 */
	public PathInfo(TreePath path, String itemName) {
		// parse out information
		StringJoiner joiner = new StringJoiner("/");
		
		String[] pathNames = getPathNames(path);
		_bucketName = pathNames[1];
		for(int i = 2; i < pathNames.length; ++i) {
			joiner.add(pathNames[i]);
		}
		
		joiner.add(itemName);
		_key = joiner.toString();
		_itemName = itemName;
	}
	
	/**
	 * Constructor for only a path
	 * @param path
	 */
	public PathInfo(TreePath path) {
		// parse out information
		StringJoiner joiner = new StringJoiner("/");
		
		String[] pathNames = getPathNames(path);
		_bucketName = pathNames[1];
		for(int i = 2; i < pathNames.length; ++i) {
			joiner.add(pathNames[i]);
		}
		_key = joiner.toString();
		_itemName = pathNames[pathNames.length - 1];
	}
	
	/**
	 * Convert path of tree nodes to array of strings
	 * @param path
	 * @return
	 */
	private static String[] getPathNames(TreePath path) {
		// build array
		Object[] pathNodes = path.getPath();
		String[] names = new String[pathNodes.length];
		// popualte
		for (int i = 0; i < names.length; ++i) {
			DefaultMutableTreeNode node = (DefaultMutableTreeNode) pathNodes[i];
			names[i] = (String) node.getUserObject();
		}
		return names;
	}
	
	/**
	 * Get bucket name
	 * @return
	 */
	public String getBucketName() {
		return _bucketName;
	}
	
	/**
	 * Get key
	 * @return
	 */
	public String getKey() {
		return _key;
	}
	
	/**
	 * Get item name
	 * @return
	 */
	public String getName() {
		return _itemName;
	}
}
