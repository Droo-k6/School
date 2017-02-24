// Mathew McCain
// cscd371 midterm project
// FormWriteWait, form for display a please wait message while writing to the database

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mccainmmidterm {
	// FormWriteWait class, form to display to user while writing to database
	// could make use of BackgroundWorker class instead, or implementing async
	partial class FormWriteWait : Form {
		// Constructor, takes EventDatabase and Queue<WatcherEventArgs> to call the write to execute
		public FormWriteWait() {
			InitializeComponent();

			// only works if put here
			Cursor.Current = Cursors.WaitCursor;
		}

		// Shown EH
		private void EH_Shown(object sender, EventArgs e) {
			// set cursor to waiting
			//Cursor.Current = Cursors.WaitCursor;

			// Refresh so everything displays correctly
			// otherwise controls are whited out
			Refresh();
		}

		// Closed EH
		private void EH_Closed(object sender, FormClosedEventArgs e) {
			// doesn't seem to be required
			// set cursor back to default
			//Cursor.Current = Cursors.Default;
		}
	}
}
