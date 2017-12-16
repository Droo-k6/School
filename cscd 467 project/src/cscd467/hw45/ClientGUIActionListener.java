/**
 * Action listener for the Client GUI
 * For when the enter key is pressed
 */

package cscd467.hw45;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

class ClientGUIActionListener implements ActionListener {
	// member variables
	private final ClientGUI _gui;
	
	/**
	 * Constructor
	 * @param gui, gui the listener is attached to
	 */
	public ClientGUIActionListener(ClientGUI gui) {
		_gui = gui;
	}
	
	/**
	 * method for the ActionListener interface
	 */
	@Override
	public void actionPerformed(ActionEvent e) {
		// send string in the text field
		if(!_gui.sendField()) {
			return;
		}
		
		Log.log("Waiting for server response");
		
		// wait for server response
		String response = _gui.receiveMessage();
		if (response.equals("")) {
			Log.log("Close from server received");
			_gui.writeMessage("Closing connection");
			_gui.close();
		}else{
			_gui.writeMessage(response);
		}
	}

}
