// Mathew McCain
// cscd371 midterm project
// Contains FormQuery class, used as form for database query

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mccainmmidterm {
	// FormQuery, form for database query
	partial class FormQuery : Form {
		// EventDatabase to query on
		EventDatabase eventDB;

		// Constructor
		// takes EventDatabase to run queries on
		public FormQuery(EventDatabase db) {
			InitializeComponent();

			eventDB = db;
		}

		// on load
		private void EH_Load(object sender, EventArgs e) {
			// populate query combobox with common extension types
			ComboBox.ObjectCollection queryComboList = ComboBoxQuery.Items;
			queryComboList.Add("txt");
			queryComboList.Add("xml");
			queryComboList.Add("doc");
			queryComboList.Add("docx");
			queryComboList.Add("cfg");
			queryComboList.Add("exe");
		}

		// display help for using query form
		private void EH_Help(object sender, EventArgs e) {
			MessageBox.Show("Database query usage:\nEnter a file extension to query the database for, results display below.\nMultiple extensions can be filered for by seperating by commas, a blank line will grab all entries.\nPress enter to perform the query.");
		}

		// TextBoxQuery
		// 

		// On focus leave for query textbox, executes query
		private void EH_LeaveFocusQuery(object sender, EventArgs e) {
			// pull query text
			string extStr = ComboBoxQuery.Text;

			// convert str to appropriate list
			List<string> extList = MainClass.ExtStrToList(extStr);

			// reset textbox display
			ComboBoxQuery.Text = string.Join(",", extList);

			// clear datagrid
			DataGridViewEvents.Rows.Clear();
			DataGridViewEvents.Refresh();

			// call up on query
			// DataGridViewEvents
			List<WatcherEventArgs> listResults = eventDB.QueryFor(extList);

			// populate datagrid
			foreach (WatcherEventArgs args in listResults) {
				DataGridViewEvents.Rows.Add(args.ItemName, args.ItemType, args.Path, args.EventName, args.EventDateTime);
			}
		}

		// On keydown for query textbox
		private void EH_KeydownQuery(object sender, KeyEventArgs e) {
			// on enter press, force focus leave
			if (e.KeyCode == Keys.Enter) {
				DataGridViewEvents.Focus();
			}
		}

		// when selecting entry from combobox dropdown
		private void EH_IndexSelect(object sender, EventArgs e) {
			// force leave focus so event is fired
			DataGridViewEvents.Focus();
		}
	}
}
