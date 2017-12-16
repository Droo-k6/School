/**
 * Used to run the GUI
 * @author Mathew McCain
 */

package cscd467.hw7;

import java.awt.Font;

import javax.swing.UIManager;

class RunGUI implements Runnable {

	/**
	 * Increase font of given type
	 * @param type
	 */
	private static void increaseFont(String type) {
		Font font = UIManager.getFont(type);
		font = font.deriveFont(font.getSize() + 4f);
		UIManager.put(type, font);
	}
	
	/**
	 * Run method
	 */
	@Override
	public void run() {
		try {
			UIManager.setLookAndFeel("com.sun.java.swing.plaf.windows.WindowsLookAndFeel");
			increaseFont("Tree.font");
			increaseFont("Label.font");
			increaseFont("ComboBox.font");
			increaseFont("List.font");
			
			// Turn off metal's use of bold fonts
			UIManager.put("swing.boldMetal", Boolean.FALSE);
			
			// Create and set up the window.
			GUI gui = new GUI();

			// Display the window.
			gui.pack();
			gui.setVisible(true);
		} catch (Exception e) {
			System.err.printf("run() GUI exception(\"%s\"): \"%s\"%n", e.getClass(), e.getMessage());
			e.printStackTrace();
		}
	}

}
