/**
 * GUI for hw7
 * Based off DragDropFiles class provided
 * 
 * @author Mathew McCain
 */

package cscd467.hw7;

import java.awt.BorderLayout;
import java.awt.Dimension;
import java.io.File;
import java.util.Enumeration;

import javax.swing.BorderFactory;
import javax.swing.Box;
import javax.swing.BoxLayout;
import javax.swing.DropMode;
import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JOptionPane;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.JTree;
import javax.swing.SwingUtilities;
import javax.swing.tree.DefaultMutableTreeNode;
import javax.swing.tree.DefaultTreeModel;
import javax.swing.tree.TreePath;
import javax.swing.tree.TreeSelectionModel;

@SuppressWarnings("serial")
class GUI extends JFrame {
	// member variables
	private final S3Interaction _s3;
	private final JTree _tree;
	private final JLabel _statusLabel;
	private TreePath _downloadPath = null;

	/**
	 * Constructor
	 */
	public GUI() {
		super("Drag and Drop File Transfers in Cloud");

		// setup logic objects
		_s3 = new S3Interaction(this);
		// TODO try catch

		// setup GUI
		
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		
		// create top panel		
		JPanel topPanel = new JPanel();
		topPanel.setLayout(new BoxLayout(topPanel, BoxLayout.X_AXIS));
		
		topPanel.add(Box.createHorizontalStrut(4));
		topPanel.add(Box.createGlue());
		
		JPanel wrap = new JPanel();
		
		_statusLabel = new JLabel();
		setStatus("Status...");
		wrap.add(_statusLabel);
		topPanel.add(wrap);
		
		topPanel.add(Box.createGlue());
		topPanel.add(Box.createHorizontalStrut(4));
		
		getContentPane().add(topPanel, BorderLayout.NORTH);
		
		// create tree panel
		// setup tree
		DefaultTreeModel model = _s3.buildTreeModel();
		_tree = new JTree(model);
		_tree.setBorder(BorderFactory.createEmptyBorder(2, 4, 2, 4));
		_tree.getSelectionModel().setSelectionMode(TreeSelectionModel.DISCONTIGUOUS_TREE_SELECTION);
		_tree.setDropMode(DropMode.ON);
		_tree.getSelectionModel().setSelectionMode(TreeSelectionModel.SINGLE_TREE_SELECTION);
		_tree.expandRow(2);
		_tree.expandRow(1);
		_tree.setRowHeight(0);

		// Set event handlers for tree and drag/drop
		_tree.addTreeSelectionListener(new DragDropTreeSelectionListener(this));
		_tree.setTransferHandler(new DragDropTransferHandler(this));
		
		getContentPane().add(new JScrollPane(_tree), BorderLayout.CENTER);
		
		// create bottom panel
		JPanel bottomPanel = new JPanel();
		bottomPanel.setLayout(new BoxLayout(bottomPanel, BoxLayout.X_AXIS));
		
		bottomPanel.add(Box.createHorizontalStrut(4));
		bottomPanel.add(Box.createGlue());
		
		JPanel buttonWrap = new JPanel();
		
		// download button
		JButton download = new JButton("Download");
		download.addActionListener(new DownloadButtonListener(this));
		
		buttonWrap.add(download);
		
		bottomPanel.add(buttonWrap);
		
		bottomPanel.add(Box.createGlue());
		bottomPanel.add(Box.createHorizontalStrut(4));
		
		getContentPane().add(bottomPanel, BorderLayout.SOUTH);

		// finalize content panel
		getContentPane().setPreferredSize(new Dimension(400, 450));
	}
	
	/**
	 * Used to determine if a download is available
	 * @return
	 */
	public synchronized boolean downloadAvailable() {
		return (_downloadPath != null);
	}
	
	/**
	 * Gets the path of the tree
	 * @return
	 */
	public synchronized TreePath getDownloadPath() {
		if (!downloadAvailable()) {
			throw new IllegalStateException("No download path available");
		}
		return _downloadPath;
	}
	
	/**
	 * Used to set the current download path
	 * @param path
	 */
	public synchronized void setDownloadPath(TreePath path) {
		_downloadPath = path;
	}
	
	/**
	 * Get model for tree
	 * @return
	 */
	public synchronized DefaultTreeModel getModel() {
		return (DefaultTreeModel) _tree.getModel();
	}
	
	/**
	 * Creates node on tree
	 * @param path
	 * @param name
	 * @param index
	 */
	@SuppressWarnings("unchecked")
	public void createNode(TreePath path, String name) {
		DefaultMutableTreeNode parentNode = (DefaultMutableTreeNode) path.getLastPathComponent();
		// index should be based on alphabetical insert, don't care
		int index = parentNode.getChildCount();
		
		// check that node w/ name does not exist
		// could determine insert index here
		for (Enumeration<DefaultMutableTreeNode> children = parentNode.children(); children.hasMoreElements();) {
			DefaultMutableTreeNode child = children.nextElement();
			String childName = (String) child.getUserObject();
			if (name.equalsIgnoreCase(childName)) {
				return;
			}
		}
		
		// insert node
		DefaultMutableTreeNode newNode = new DefaultMutableTreeNode(name, false);
		getModel().insertNodeInto(newNode, parentNode, index);
		
		// make the new node visible and scroll so that it's visible
		_tree.makeVisible(path.pathByAddingChild(newNode));
		_tree.scrollRectToVisible(_tree.getPathBounds(path.pathByAddingChild(newNode)));
	}
	
	/**
	 * Sets text for status label
	 * @param str
	 */
	public void setStatus(String str) {
		_statusLabel.setText(str);
	}
	
	/**
	 * Displays error message
	 * @param str
	 */
	public void warning(String str) {
		System.err.println(str);
		JOptionPane.showMessageDialog(this, str, "Warning", JOptionPane.ERROR_MESSAGE);
	}
	
	/**
	 * Diplay information
	 * @param str
	 */
	public void info(String str) {
		System.out.println(str);
		JOptionPane.showMessageDialog(this, str, "Info", JOptionPane.INFORMATION_MESSAGE);
	}
	
	/**
	 * Check if path is valid, and if upload will be accepted
	 * @param path
	 * @return
	 */
	public synchronized boolean canUpload(TreePath path) {
		return _s3.canUpload(path);
	}
	
	/**
	 * Upload given file
	 * @param path
	 * @param name
	 * @param file
	 * @return
	 */
	public boolean upload(TreePath path, String name, File file) {
		if (!_s3.upload(path, name, file)) {
			warning(String.format("Unable to upload \"%s\" at this time", name));
			return false;
		}
		return true;
	}
	
	/**
	 * Download current path
	 * @return
	 */
	public void download() {
		// check if download available
		if (downloadAvailable() && _s3.canDownload(_downloadPath)) {
			// download
			_s3.download(_downloadPath);
		}
	}
	
	/**
	 * Static method to run the GUI
	 */
	public static void run() {
		SwingUtilities.invokeLater(new RunGUI());
	}
	
}
