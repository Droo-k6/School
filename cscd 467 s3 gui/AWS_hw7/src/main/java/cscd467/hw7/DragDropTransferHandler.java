/**
 * Transfer Handler for GUI
 * @author Mathew McCain
 */

package cscd467.hw7;

import java.awt.datatransfer.DataFlavor;
import java.awt.datatransfer.Transferable;
import java.awt.datatransfer.UnsupportedFlavorException;
import java.io.File;
import java.io.IOException;
import java.util.List;

import javax.swing.JTree;
import javax.swing.TransferHandler;
import javax.swing.tree.TreePath;

@SuppressWarnings("serial")
class DragDropTransferHandler extends TransferHandler {
	// member variables
	private final GUI _gui;
	
	/**
	 * Constructor
	 * @param gui, gui object
	 */
	public DragDropTransferHandler(GUI gui) {
		_gui = gui;
	}
	
	/**
	 * Determine if component will accept the transfer
	 * @param info
	 * @return
	 */
	public boolean canImport(TransferHandler.TransferSupport info) {
		
		// we'll only support drops (not clip-board paste)
		if (!info.isDrop()) {
			return false;
		}
		
		// we import Strings and files
		if (!info.isDataFlavorSupported(DataFlavor.stringFlavor) && !info.isDataFlavorSupported(DataFlavor.javaFileListFlavor)) {
			return false;
		}
		
		info.setDropAction(COPY);
		info.setShowDropLocation(true);

		// fetch the drop location
		JTree.DropLocation dl = (JTree.DropLocation) info.getDropLocation();
		TreePath path = dl.getPath();
		
		// check if gui will accept
		return _gui.canUpload(path);
	}

	/**
	 * Import from clipboard/ drag & drop operation
	 * @param info
	 * @return
	 */
	public boolean importData(TransferHandler.TransferSupport info) {
		// fetch the drop location
		JTree.DropLocation dl = (JTree.DropLocation) info.getDropLocation();

		// determine path
		TreePath path = dl.getPath();
		
		// fetch single file to upload
		File file = getFile(info);
		if (file == null) {
			return false;
		}
		
		return _gui.upload(path, file.getName(), file);
	}
	
	/**
	 * Get file from transfer info
	 * @param info
	 * @return
	 */
	@SuppressWarnings("unchecked")
	private File getFile(TransferHandler.TransferSupport info) {
		// pull list of files
		Transferable t = info.getTransferable();
		try {
			java.util.List<File> l = (List<File>) t.getTransferData(DataFlavor.javaFileListFlavor);

			for (File f : l) {
				return f;
			}
		} catch (UnsupportedFlavorException e) {
			_gui.warning(String.format("Unsupported flavor (\"%s\")", e.getMessage()));
		} catch (IOException e) {
			_gui.warning(String.format("Failed to open file (\"%s\")", e.getMessage()));
		}
		
		return null;
	}
	
}
