/**
 * Tree selection listener for GUI
 * @author Mathew McCain
 */

package cscd467.hw7;

import javax.swing.event.TreeSelectionEvent;
import javax.swing.event.TreeSelectionListener;

class DragDropTreeSelectionListener implements TreeSelectionListener {
	// member variables
	private final GUI _gui;
	
	/**
	 * Constructor
	 * @param gui, gui to attach to
	 */
	public DragDropTreeSelectionListener(GUI gui) {
		_gui = gui;
	}
	
	/**
	 * method for interface TreeSelectionListener
	 * Event handler for when a user selects a node in the tree
	 */
	public void valueChanged(TreeSelectionEvent e) {
		// tell GUI to set new path (will check if valid)
		_gui.setDownloadPath(e.getNewLeadSelectionPath());
	}

}
