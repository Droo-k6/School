using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mccainmmidterm {
	// Monitor Form class, primary form
	partial class FormMonitor : Form {
		// Buffer of WatcherEvents to be written to db if available
		private WatcherEventBuffer eventBuffer;

		// To maintain SQL side of things
		private EventDatabase eventDB;

		// max amount of listings in extension dropdown
		private int extHistoryMax = 8;


		// Form constructor
		public FormMonitor() {
			// Build GUI
			InitializeComponent();

			// setup event between Watcher and Form
			Watcher.WatcherEvent = new WatcherEventHandler(EH_MonitorEvent);

			// Create buffer
			eventBuffer = new WatcherEventBuffer();

			// create DB obj (not connected to anything)
			eventDB = new EventDatabase();
		}


		// methods

		// enables monitor start/stop controls
		private void EnableControlsMonitorStart(bool enabled) {
			// Menustrip
			MenuStripDropdownMonitorStart.Enabled = enabled;
			MenuStripDropdownMonitorStop.Enabled = !enabled;
			// Toolstrip
			ToolstripStartMonitor.Enabled = enabled;
			ToolstripStopMonitor.Enabled = !enabled;
			// Browse button
			ButtonMonitorBrowse.Enabled = enabled;
			// Monitor dir textbox
			TextBoxMonitorDir.Enabled = enabled;
			// Monitor ext combobox
			ComboBoxMonitorExt.Enabled = enabled;

			// Database textbox
			TextBoxDatabase.Enabled = enabled;

			// For allowing to set
			if (eventDB.Active) {
				// can uncomment so db write/query can't be done during monitoring
				/*
				// Menustrip
				MenuStripDropdownDatabaseWrite.Enabled = enabled;
				MenuStripDropdownDatabaseQuery.Enabled = enabled;
				// Toolstrip
				ToolstripWriteToDB.Enabled = enabled;
				ToolstripQueryDB.Enabled = enabled;
				*/
			}
		}

		// setting monitor directory
		private void SetMonitorDir(string dir) {
			// Check if dir is empty
			if (dir.Length <= 0) {
				// check if current watcher path is empty
				if (Watcher.Path.Length > 0) {
					// reset textbox and return
					TextBoxMonitorDir.Text = Watcher.Path;
				}
				return;
			}
			// Check if path is changing
			string oldPath = Watcher.Path;
			if (oldPath == dir) {
				return;
			}

			// TextBoxMonitorDir
			try {
				Watcher.Path = dir;
				// appropriate controls to be active
				// allow opening of monitored directory
				MenuStripDropdownMonitorOpen.Enabled = true;
				ToolstripOpenMonitorDir.Enabled = true;
				// Enable start
				EnableControlsMonitorStart(true);
			} catch (ArgumentException e) {
				MessageBox.Show(e.Message);
			}
			// Only change displayed path if chosen path is valid
			// Not done in try catch so path is reverted to last valid
			TextBoxMonitorDir.Text = Watcher.Path;
		}

		// Open dialog to prompt user for directory to monitor
		private void ChooseMonitorDir() {
			// open dialog to choose dialog
			DialogResult result = BrowseFolderMonitorDialog.ShowDialog();
			if (result == DialogResult.OK) {
				string dir = BrowseFolderMonitorDialog.SelectedPath;
				SetMonitorDir(dir);
			}
		}

		// Set extension filters from str
		private void SetMonitorExt(string extStr) {
			// convert str to appropriate list
			List<string> filters = MainClass.ExtStrToList(extStr);

			// Set filter
			Watcher.Filter = filters;

			// join list to 1 string, add to ext dropdown
			MonitorExtDropdownInsert(string.Join(",", filters));
		}

		// Add string to ComboBox dropdown at end
		// loops back to top if list grows past max size (extHistoryMax)
		private void MonitorExtDropdownAdd(string str) {
			// check that string isn't empty
			if (str.Length <= 0)
				return;

			// convert str to lowercase
			str = str.ToLower();

			// pull Items from control
			ComboBox.ObjectCollection extComboList = ComboBoxMonitorExt.Items;

			// check that string is not equal to current value at end
			// check if list contains anything
			// could use IndexOf(str), but don't want to deal with if it starts at the end or beginning of the collection
			if (extComboList.Count > 0) {
				// check if equals, return
				if (str == (string) extComboList[extComboList.Count - 1]) {
					return;
				}
			}

			// if size is at max, remove first entry
			if (extComboList.Count == extHistoryMax) {
				extComboList.RemoveAt(0);
			}

			// add to items
			extComboList.Add(str);
		}

		// add given string to ComboBox at beginning
		private void MonitorExtDropdownInsert(string str) {
			// check that string isn't empty
			if (str.Length <= 0)
				return;

			// convert str to lowercase
			str = str.ToLower();

			// pull Items from control
			ComboBox.ObjectCollection extComboList = ComboBoxMonitorExt.Items;

			// check that string is not equal to current value at beginning
			// check if list contains anything
			// could use IndexOf(str), but don't want to deal with if it starts at the end or beginning of the collection
			if (extComboList.Count > 0) {
				// check if equals, return
				if (str == (string)extComboList[0]) {
					return;
				}
			}

			// if size is at max, remove last entry
			if (extComboList.Count == extHistoryMax) {
				extComboList.RemoveAt(extComboList.Count - 1);
			}

			// insert at beginning
			extComboList.Insert(0, str);
		}

		// setting database file
		private void SetDatabaseFile(string name) {
			// check if name is empty
			if (name.Length <= 0)
				return;

			// check if name is actually changing
			if (name == eventDB.Name)
				return;

			// prompt to write current buffer to db
			if (!eventBuffer.Empty && eventDB.Active) {
				DialogResult result = MessageBox.Show("Events currently waiting to be written to current database, write to database?", "Write to database?",
						MessageBoxButtons.YesNo,
						MessageBoxIcon.None,
						MessageBoxDefaultButton.Button1,
						0);
				if (result == DialogResult.Yes) {
					WriteToDatabase();
				}
			}

			// give name to eventDB to use
			if (eventDB.Use(name)) {
				// use db successful

				// enable write/query controls
				// Menustrip
				MenuStripDropdownDatabaseWrite.Enabled = true;
				MenuStripDropdownDatabaseQuery.Enabled = true;
				// Toolstrip
				ToolstripWriteToDB.Enabled = true;
				ToolstripQueryDB.Enabled = true;
			}
			// set name (if set failed, returned name is currently working one)
			TextBoxDatabase.Text = eventDB.Name;
		}

		// Start monitoring
		private void StartMonitor() {
			// set appropriate buttons to be enabled/disabled
			EnableControlsMonitorStart(false);
			// turn on watcher
			Watcher.Watching = true;
		}

		// Stop monitoring
		private void StopMonitor() {
			// turn off watcher
			Watcher.Watching = false;
			// set appropriate buttons to be enabled/disabled
			EnableControlsMonitorStart(true);
		}

		// Write to DB
		private void WriteToDatabase() {
			// write current buffer to db
			eventDB.Write(eventBuffer);
		}

		// Query DB
		private void QueryDatabase() {
			// prompt to write current buffer to db
			if (!eventBuffer.Empty && eventDB.Active) {
				DialogResult result = MessageBox.Show("Events currently waiting to be written to database, write to database?", "Write to database?",
						MessageBoxButtons.YesNo,
						MessageBoxIcon.None,
						MessageBoxDefaultButton.Button1,
						0);
				if (result == DialogResult.Yes) {
					WriteToDatabase();
				}
			}

			// request db to query
			eventDB.Query();
		}


		// Events

		// Form load
		private void EH_Load(object sender, EventArgs e) {
			// Disable all options relevant to Monitoring
			// Menustrip
			MenuStripDropdownMonitorOpen.Enabled = false;
			MenuStripDropdownMonitorSet.Enabled = false;
			MenuStripDropdownMonitorStart.Enabled = false;
			MenuStripDropdownMonitorStop.Enabled = false;
			// Toolstrip
			ToolstripOpenMonitorDir.Enabled = false;
			ToolstripStartMonitor.Enabled = false;
			ToolstripStopMonitor.Enabled = false;

			// Disable all options to database (write/query)
			// Menustrip
			MenuStripDropdownDatabaseWrite.Enabled = false;
			MenuStripDropdownDatabaseQuery.Enabled = false;
			// Toolstrip
			ToolstripWriteToDB.Enabled = false;
			ToolstripQueryDB.Enabled = false;

			// populate list of common extension types
			MonitorExtDropdownAdd("txt");
			MonitorExtDropdownAdd("xml");
			MonitorExtDropdownAdd("doc");
			MonitorExtDropdownAdd("docx");
			MonitorExtDropdownAdd("cfg");
			MonitorExtDropdownAdd("exe");
		}

		// Form closing
		private void EH_Closing(object sender, FormClosingEventArgs e) {
			// Check closing reason
			// if no reason or is user choice, provide appropriate prompt
			// other reasons, just let cancel go through
			if (e.CloseReason == CloseReason.None || e.CloseReason == CloseReason.UserClosing) {
				// if values on buffer, and db selected
				// prompt user to write buffer to db
				if (!eventBuffer.Empty && eventDB.Active) {
					DialogResult result = MessageBox.Show("Events are waiting to be written to the database, write to before closing?",
						"Write to database?",
						MessageBoxButtons.YesNoCancel,
						MessageBoxIcon.Warning,
						MessageBoxDefaultButton.Button3,
						0);
					if (result == DialogResult.Yes) {
						// Write to database
						WriteToDatabase();
					}else if (result == DialogResult.Cancel) {
						// Cancel closing
						e.Cancel = true;
					}
				}else{
					// exit confirmation
					DialogResult result = MessageBox.Show("Are you sure you wish to exit?", "Exit?",
						MessageBoxButtons.YesNo,
						MessageBoxIcon.None,
						MessageBoxDefaultButton.Button1,
						0);
					if (result == DialogResult.No) {
						// Cancel closing
						e.Cancel = true;
					}
				}
			}
		}

		// Form closed
		private void EH_Closed(object sender, FormClosedEventArgs e) {
			// Turn off Watcher
			Watcher.Watching = false;
			// unassign event
			Watcher.WatcherEvent = null;

			// don't need to close up database
			// destructor handles
		}

		// Close
		private void EH_Close(object sender, EventArgs e) {
			// Call close
			this.Close();
			// UserClosing is reason given in Closing/Closed events
		}

		// On click browse for monitor directory
		// Opens dialog to select directory
		private void ClickMonitorBrowse(object sender, EventArgs e) {
			ChooseMonitorDir();
		}

		// When focus is left from monitor directory textbox
		private void LeaveFocusMonitorDir(object sender, EventArgs e) {
			// check if control enabled, etc
			// check if watcher running
			string dir = TextBoxMonitorDir.Text;
			SetMonitorDir(dir);
		}

		// When focus is left from monitor extensions textbox
		private void LeaveFocusMonitorExt(object sender, EventArgs e) {
			string extStr = ComboBoxMonitorExt.Text;
			SetMonitorExt(extStr);
		}

		// When focus is left from database textbox
		private void LeaveFocusDatabase(object sender, EventArgs e) {
			string dbName = TextBoxDatabase.Text;
			SetDatabaseFile(dbName);
		}

		// On key press on monitor directory textbox
		private void KeyDownMonitorDir(object sender, KeyEventArgs e) {
			// if enter key is pressed, switch focus to ext textbox
			// forcese focus leave event
			if (e.KeyCode == Keys.Enter) {
				ComboBoxMonitorExt.Focus();
			}
		}

		// On key press on monitor extension textbox
		private void KeyDownMonitorExt(object sender, KeyEventArgs e) {
			// if enter key is pressed, switch focus to database textbox
			// forces focus leave event
			if (e.KeyCode == Keys.Enter) {
				TextBoxDatabase.Focus();
			}
		}

		// On key press on database textbox
		private void KeyDownDatabase(object sender, KeyEventArgs e) {
			// if enter key is pressed, switch focus to data view
			// forces focus leave event
			if (e.KeyCode == Keys.Enter) {
				DataGridViewEvents.Focus();
			}
		}

		// when combobox dropdown selected
		private void EH_ComboboxSelect(object sender, EventArgs e) {
			// force leave focus event
			DataGridViewEvents.Focus();
		}

		// Request to open Monitored directory
		private void EH_MonitorOpenDir(object sender, EventArgs e) {
			// Startup explorer for current Watcher directory
			System.Diagnostics.Process.Start("explorer.exe", Watcher.Path);
		}

		// Request to set Monitor directory
		private void EH_MonitorSet(object sender, EventArgs e) {
			ChooseMonitorDir();
		}

		// Start the monitor
		private void EH_MonitorStart(object sender, EventArgs e) {
			StartMonitor();
		}

		// Stop the monitor
		private void EH_MonitorStop(object sender, EventArgs e) {
			StopMonitor();
		}

		// Watcher event
		public void EH_MonitorEvent(WatcherEventArgs args) {
			if (DataGridViewEvents.InvokeRequired) {
				// Need to invoke so is on main thread
				WatcherEventHandler handler = EH_MonitorEvent;
				Invoke(handler, args);
			} else {
				// add to data grid
				DataGridViewEvents.Rows.Add(args.ItemName, args.ItemType, args.Path, args.EventName, args.EventDateTime);
				// only push to buffer if db is active
				if (eventDB.Active) {
					// push to buffer
					eventBuffer.Enqueue(args);
				}
			}
		}

		// Write event buffer to database
		private void EH_DatabaseWrite(object sender, EventArgs e) {
			WriteToDatabase();
		}

		// Query current database/table
		private void EH_DatabaseQuery(object sender, EventArgs e) {
			QueryDatabase();
		}

		// Displays dialog with how to use program
		private void EH_HelpUsage(object sender, EventArgs e) {
			MessageBox.Show("Usage\n\nIn monitor directory type in directory to monitor and press enter,\nor use the browse button to select a directory.\nIn monitor extensions, type in the extension names to watch for (without the '.', seperate multiple by commas), or leave blank to watch all extension types.\n\nOnce the Monitor directory is set, the monitor can be started (green arrow icon) and stopped (red sign) through the icons at the top.\nNote: the monitored directory and extensions cannot be changed during monitoring.\nThe currently monitored directory can be opened through the folder icon.\n\nAn SQLite database can be chosen by typing in the name of an existing database to add a table to, or to create a new one.\nThe database will only be written to when requested through the disk icon above.\nQueries can be ran on the current database through the query icon above (disk stack with magnifying glass).\nNote: the current database cannot be changed while file monitoring is running.");
		}

		// Display dialog with details about program
		private void EH_HelpAbout(object sender, EventArgs e) {
			MessageBox.Show("File Directory Monitor\nAuthor: Mathew McCain\nVersion: 1.1");
		}
	}
}
