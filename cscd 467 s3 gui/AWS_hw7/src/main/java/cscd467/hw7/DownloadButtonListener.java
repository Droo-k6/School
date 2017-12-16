/**
 * Action listener for download button
 * @author Mathew McCain
 */

package cscd467.hw7;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

class DownloadButtonListener implements ActionListener {
	// member variables
	private final GUI _gui;
	
	/**
	 * Constructor
	 * @param gui
	 */
	public DownloadButtonListener(GUI gui) {
		_gui = gui;
	}
	
	@Override
	public void actionPerformed(ActionEvent e) {
		// request download
		if (_gui.downloadAvailable()) {
			_gui.download();
		}
	}

}
