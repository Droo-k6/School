namespace mccainmmidterm {
	partial class FormMonitor {
		// Required designer variable.
		private System.ComponentModel.IContainer components = null;

		// Clean up any resources being used.
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMonitor));
			this.MenuStrip = new System.Windows.Forms.MenuStrip();
			this.MenuStripDropdownFile = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuStripDropdownFileClose = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuStripDropdownMonitor = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuStripDropdownMonitorOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuStripDropdownMonitorSet = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuStripDropdownMonitorStart = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuStripDropdownMonitorStop = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuStripDropdownDatabase = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuStripDropdownDatabaseWrite = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuStripDropdownDatabaseQuery = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuStripDropdownHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuStripDropdownHelpUsage = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuStripDropdownHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStrip = new System.Windows.Forms.ToolStrip();
			this.ToolstripOpenMonitorDir = new System.Windows.Forms.ToolStripButton();
			this.ToolstripStartMonitor = new System.Windows.Forms.ToolStripButton();
			this.ToolstripStopMonitor = new System.Windows.Forms.ToolStripButton();
			this.ToolstripWriteToDB = new System.Windows.Forms.ToolStripButton();
			this.ToolstripQueryDB = new System.Windows.Forms.ToolStripButton();
			this.ToolstripHelp = new System.Windows.Forms.ToolStripButton();
			this.LabelMonitorDir = new System.Windows.Forms.Label();
			this.LabelMonitodExt = new System.Windows.Forms.Label();
			this.ComboBoxMonitorExt = new System.Windows.Forms.ComboBox();
			this.ButtonMonitorBrowse = new System.Windows.Forms.Button();
			this.TextBoxMonitorDir = new System.Windows.Forms.TextBox();
			this.LabelDatabase = new System.Windows.Forms.Label();
			this.TextBoxDatabase = new System.Windows.Forms.TextBox();
			this.LabelMonitorEvents = new System.Windows.Forms.Label();
			this.DataGridViewEvents = new System.Windows.Forms.DataGridView();
			this.ColumnItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColumnType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColumnPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColumnEvent = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColumnDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.BrowseFolderMonitorDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.BrowseFolderDatabaseDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.MenuStrip.SuspendLayout();
			this.ToolStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.DataGridViewEvents)).BeginInit();
			this.SuspendLayout();
			// 
			// MenuStrip
			// 
			this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuStripDropdownFile,
            this.MenuStripDropdownMonitor,
            this.MenuStripDropdownDatabase,
            this.MenuStripDropdownHelp});
			this.MenuStrip.Location = new System.Drawing.Point(0, 0);
			this.MenuStrip.Name = "MenuStrip";
			this.MenuStrip.Size = new System.Drawing.Size(700, 24);
			this.MenuStrip.TabIndex = 0;
			// 
			// MenuStripDropdownFile
			// 
			this.MenuStripDropdownFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuStripDropdownFileClose});
			this.MenuStripDropdownFile.Name = "MenuStripDropdownFile";
			this.MenuStripDropdownFile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F)));
			this.MenuStripDropdownFile.Size = new System.Drawing.Size(37, 20);
			this.MenuStripDropdownFile.Text = "File";
			// 
			// MenuStripDropdownFileClose
			// 
			this.MenuStripDropdownFileClose.Name = "MenuStripDropdownFileClose";
			this.MenuStripDropdownFileClose.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
			this.MenuStripDropdownFileClose.Size = new System.Drawing.Size(145, 22);
			this.MenuStripDropdownFileClose.Text = "Close";
			this.MenuStripDropdownFileClose.Click += new System.EventHandler(this.EH_Close);
			// 
			// MenuStripDropdownMonitor
			// 
			this.MenuStripDropdownMonitor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuStripDropdownMonitorOpen,
            this.MenuStripDropdownMonitorSet,
            this.MenuStripDropdownMonitorStart,
            this.MenuStripDropdownMonitorStop});
			this.MenuStripDropdownMonitor.Name = "MenuStripDropdownMonitor";
			this.MenuStripDropdownMonitor.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.M)));
			this.MenuStripDropdownMonitor.Size = new System.Drawing.Size(62, 20);
			this.MenuStripDropdownMonitor.Text = "Monitor";
			// 
			// MenuStripDropdownMonitorOpen
			// 
			this.MenuStripDropdownMonitorOpen.Name = "MenuStripDropdownMonitorOpen";
			this.MenuStripDropdownMonitorOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.O)));
			this.MenuStripDropdownMonitorOpen.Size = new System.Drawing.Size(251, 22);
			this.MenuStripDropdownMonitorOpen.Text = "Open monitored directory";
			this.MenuStripDropdownMonitorOpen.Click += new System.EventHandler(this.EH_MonitorOpenDir);
			// 
			// MenuStripDropdownMonitorSet
			// 
			this.MenuStripDropdownMonitorSet.Name = "MenuStripDropdownMonitorSet";
			this.MenuStripDropdownMonitorSet.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.I)));
			this.MenuStripDropdownMonitorSet.Size = new System.Drawing.Size(251, 22);
			this.MenuStripDropdownMonitorSet.Text = "Set monitored directory";
			this.MenuStripDropdownMonitorSet.Click += new System.EventHandler(this.EH_MonitorSet);
			// 
			// MenuStripDropdownMonitorStart
			// 
			this.MenuStripDropdownMonitorStart.Name = "MenuStripDropdownMonitorStart";
			this.MenuStripDropdownMonitorStart.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.A)));
			this.MenuStripDropdownMonitorStart.Size = new System.Drawing.Size(251, 22);
			this.MenuStripDropdownMonitorStart.Text = "Start monitoring";
			this.MenuStripDropdownMonitorStart.Click += new System.EventHandler(this.EH_MonitorStart);
			// 
			// MenuStripDropdownMonitorStop
			// 
			this.MenuStripDropdownMonitorStop.Name = "MenuStripDropdownMonitorStop";
			this.MenuStripDropdownMonitorStop.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.S)));
			this.MenuStripDropdownMonitorStop.Size = new System.Drawing.Size(251, 22);
			this.MenuStripDropdownMonitorStop.Text = "Stop monitoring";
			this.MenuStripDropdownMonitorStop.Click += new System.EventHandler(this.EH_MonitorStop);
			// 
			// MenuStripDropdownDatabase
			// 
			this.MenuStripDropdownDatabase.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuStripDropdownDatabaseWrite,
            this.MenuStripDropdownDatabaseQuery});
			this.MenuStripDropdownDatabase.Name = "MenuStripDropdownDatabase";
			this.MenuStripDropdownDatabase.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D)));
			this.MenuStripDropdownDatabase.Size = new System.Drawing.Size(67, 20);
			this.MenuStripDropdownDatabase.Text = "Database";
			// 
			// MenuStripDropdownDatabaseWrite
			// 
			this.MenuStripDropdownDatabaseWrite.Name = "MenuStripDropdownDatabaseWrite";
			this.MenuStripDropdownDatabaseWrite.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.MenuStripDropdownDatabaseWrite.Size = new System.Drawing.Size(243, 22);
			this.MenuStripDropdownDatabaseWrite.Text = "Write events to database";
			this.MenuStripDropdownDatabaseWrite.Click += new System.EventHandler(this.EH_DatabaseWrite);
			// 
			// MenuStripDropdownDatabaseQuery
			// 
			this.MenuStripDropdownDatabaseQuery.Name = "MenuStripDropdownDatabaseQuery";
			this.MenuStripDropdownDatabaseQuery.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Q)));
			this.MenuStripDropdownDatabaseQuery.Size = new System.Drawing.Size(243, 22);
			this.MenuStripDropdownDatabaseQuery.Text = "Query database";
			this.MenuStripDropdownDatabaseQuery.Click += new System.EventHandler(this.EH_DatabaseQuery);
			// 
			// MenuStripDropdownHelp
			// 
			this.MenuStripDropdownHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuStripDropdownHelpUsage,
            this.MenuStripDropdownHelpAbout});
			this.MenuStripDropdownHelp.Name = "MenuStripDropdownHelp";
			this.MenuStripDropdownHelp.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.H)));
			this.MenuStripDropdownHelp.Size = new System.Drawing.Size(44, 20);
			this.MenuStripDropdownHelp.Text = "Help";
			// 
			// MenuStripDropdownHelpUsage
			// 
			this.MenuStripDropdownHelpUsage.Name = "MenuStripDropdownHelpUsage";
			this.MenuStripDropdownHelpUsage.ShortcutKeys = System.Windows.Forms.Keys.F2;
			this.MenuStripDropdownHelpUsage.Size = new System.Drawing.Size(126, 22);
			this.MenuStripDropdownHelpUsage.Text = "Usage";
			this.MenuStripDropdownHelpUsage.Click += new System.EventHandler(this.EH_HelpUsage);
			// 
			// MenuStripDropdownHelpAbout
			// 
			this.MenuStripDropdownHelpAbout.Name = "MenuStripDropdownHelpAbout";
			this.MenuStripDropdownHelpAbout.ShortcutKeys = System.Windows.Forms.Keys.F1;
			this.MenuStripDropdownHelpAbout.Size = new System.Drawing.Size(126, 22);
			this.MenuStripDropdownHelpAbout.Text = "About";
			this.MenuStripDropdownHelpAbout.Click += new System.EventHandler(this.EH_HelpAbout);
			// 
			// ToolStrip
			// 
			this.ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolstripOpenMonitorDir,
            this.ToolstripStartMonitor,
            this.ToolstripStopMonitor,
            this.ToolstripWriteToDB,
            this.ToolstripQueryDB,
            this.ToolstripHelp});
			this.ToolStrip.Location = new System.Drawing.Point(0, 24);
			this.ToolStrip.Name = "ToolStrip";
			this.ToolStrip.Size = new System.Drawing.Size(700, 25);
			this.ToolStrip.TabIndex = 1;
			this.ToolStrip.Text = "toolStrip1";
			// 
			// ToolstripOpenMonitorDir
			// 
			this.ToolstripOpenMonitorDir.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ToolstripOpenMonitorDir.Image = ((System.Drawing.Image)(resources.GetObject("ToolstripOpenMonitorDir.Image")));
			this.ToolstripOpenMonitorDir.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ToolstripOpenMonitorDir.Name = "ToolstripOpenMonitorDir";
			this.ToolstripOpenMonitorDir.Size = new System.Drawing.Size(23, 22);
			this.ToolstripOpenMonitorDir.ToolTipText = "Open monitored directory";
			this.ToolstripOpenMonitorDir.Click += new System.EventHandler(this.EH_MonitorOpenDir);
			// 
			// ToolstripStartMonitor
			// 
			this.ToolstripStartMonitor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ToolstripStartMonitor.Image = ((System.Drawing.Image)(resources.GetObject("ToolstripStartMonitor.Image")));
			this.ToolstripStartMonitor.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ToolstripStartMonitor.Name = "ToolstripStartMonitor";
			this.ToolstripStartMonitor.Size = new System.Drawing.Size(23, 22);
			this.ToolstripStartMonitor.ToolTipText = "Start monitoring";
			this.ToolstripStartMonitor.Click += new System.EventHandler(this.EH_MonitorStart);
			// 
			// ToolstripStopMonitor
			// 
			this.ToolstripStopMonitor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ToolstripStopMonitor.Image = ((System.Drawing.Image)(resources.GetObject("ToolstripStopMonitor.Image")));
			this.ToolstripStopMonitor.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ToolstripStopMonitor.Name = "ToolstripStopMonitor";
			this.ToolstripStopMonitor.Size = new System.Drawing.Size(23, 22);
			this.ToolstripStopMonitor.ToolTipText = "Stop monitoring";
			this.ToolstripStopMonitor.Click += new System.EventHandler(this.EH_MonitorStop);
			// 
			// ToolstripWriteToDB
			// 
			this.ToolstripWriteToDB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ToolstripWriteToDB.Image = ((System.Drawing.Image)(resources.GetObject("ToolstripWriteToDB.Image")));
			this.ToolstripWriteToDB.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ToolstripWriteToDB.Name = "ToolstripWriteToDB";
			this.ToolstripWriteToDB.Size = new System.Drawing.Size(23, 22);
			this.ToolstripWriteToDB.ToolTipText = "Write events to chosen database";
			this.ToolstripWriteToDB.Click += new System.EventHandler(this.EH_DatabaseWrite);
			// 
			// ToolstripQueryDB
			// 
			this.ToolstripQueryDB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ToolstripQueryDB.Image = ((System.Drawing.Image)(resources.GetObject("ToolstripQueryDB.Image")));
			this.ToolstripQueryDB.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ToolstripQueryDB.Name = "ToolstripQueryDB";
			this.ToolstripQueryDB.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.ToolstripQueryDB.Size = new System.Drawing.Size(23, 22);
			this.ToolstripQueryDB.ToolTipText = "Query database";
			this.ToolstripQueryDB.Click += new System.EventHandler(this.EH_DatabaseQuery);
			// 
			// ToolstripHelp
			// 
			this.ToolstripHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ToolstripHelp.Image = ((System.Drawing.Image)(resources.GetObject("ToolstripHelp.Image")));
			this.ToolstripHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ToolstripHelp.Name = "ToolstripHelp";
			this.ToolstripHelp.Size = new System.Drawing.Size(23, 22);
			this.ToolstripHelp.ToolTipText = "Help";
			this.ToolstripHelp.Click += new System.EventHandler(this.EH_HelpUsage);
			// 
			// LabelMonitorDir
			// 
			this.LabelMonitorDir.AutoSize = true;
			this.LabelMonitorDir.Location = new System.Drawing.Point(21, 57);
			this.LabelMonitorDir.Name = "LabelMonitorDir";
			this.LabelMonitorDir.Size = new System.Drawing.Size(99, 13);
			this.LabelMonitorDir.TabIndex = 8;
			this.LabelMonitorDir.Text = "Monitored Directory";
			// 
			// LabelMonitodExt
			// 
			this.LabelMonitodExt.AutoSize = true;
			this.LabelMonitodExt.Location = new System.Drawing.Point(12, 83);
			this.LabelMonitodExt.Name = "LabelMonitodExt";
			this.LabelMonitodExt.Size = new System.Drawing.Size(108, 13);
			this.LabelMonitodExt.TabIndex = 7;
			this.LabelMonitodExt.Text = "Monitored Extensions";
			// 
			// ComboBoxMonitorExt
			// 
			this.ComboBoxMonitorExt.FormattingEnabled = true;
			this.ComboBoxMonitorExt.Location = new System.Drawing.Point(126, 83);
			this.ComboBoxMonitorExt.Name = "ComboBoxMonitorExt";
			this.ComboBoxMonitorExt.Size = new System.Drawing.Size(121, 21);
			this.ComboBoxMonitorExt.TabIndex = 4;
			this.ComboBoxMonitorExt.SelectedIndexChanged += new System.EventHandler(this.EH_ComboboxSelect);
			this.ComboBoxMonitorExt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownMonitorExt);
			this.ComboBoxMonitorExt.Leave += new System.EventHandler(this.LeaveFocusMonitorExt);
			// 
			// ButtonMonitorBrowse
			// 
			this.ButtonMonitorBrowse.Location = new System.Drawing.Point(481, 52);
			this.ButtonMonitorBrowse.Name = "ButtonMonitorBrowse";
			this.ButtonMonitorBrowse.Size = new System.Drawing.Size(57, 23);
			this.ButtonMonitorBrowse.TabIndex = 3;
			this.ButtonMonitorBrowse.Text = "Browse";
			this.ButtonMonitorBrowse.UseVisualStyleBackColor = true;
			this.ButtonMonitorBrowse.Click += new System.EventHandler(this.ClickMonitorBrowse);
			// 
			// TextBoxMonitorDir
			// 
			this.TextBoxMonitorDir.Location = new System.Drawing.Point(126, 57);
			this.TextBoxMonitorDir.Name = "TextBoxMonitorDir";
			this.TextBoxMonitorDir.Size = new System.Drawing.Size(349, 20);
			this.TextBoxMonitorDir.TabIndex = 2;
			this.TextBoxMonitorDir.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownMonitorDir);
			this.TextBoxMonitorDir.Leave += new System.EventHandler(this.LeaveFocusMonitorDir);
			// 
			// LabelDatabase
			// 
			this.LabelDatabase.AutoSize = true;
			this.LabelDatabase.Location = new System.Drawing.Point(67, 110);
			this.LabelDatabase.Name = "LabelDatabase";
			this.LabelDatabase.Size = new System.Drawing.Size(53, 13);
			this.LabelDatabase.TabIndex = 6;
			this.LabelDatabase.Text = "Database";
			// 
			// TextBoxDatabase
			// 
			this.TextBoxDatabase.Location = new System.Drawing.Point(126, 110);
			this.TextBoxDatabase.Name = "TextBoxDatabase";
			this.TextBoxDatabase.Size = new System.Drawing.Size(169, 20);
			this.TextBoxDatabase.TabIndex = 5;
			this.TextBoxDatabase.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownDatabase);
			this.TextBoxDatabase.Leave += new System.EventHandler(this.LeaveFocusDatabase);
			// 
			// LabelMonitorEvents
			// 
			this.LabelMonitorEvents.AutoSize = true;
			this.LabelMonitorEvents.Location = new System.Drawing.Point(12, 136);
			this.LabelMonitorEvents.Name = "LabelMonitorEvents";
			this.LabelMonitorEvents.Size = new System.Drawing.Size(90, 13);
			this.LabelMonitorEvents.TabIndex = 1;
			this.LabelMonitorEvents.Text = "Monitored Events";
			// 
			// DataGridViewEvents
			// 
			this.DataGridViewEvents.AllowUserToAddRows = false;
			this.DataGridViewEvents.AllowUserToDeleteRows = false;
			this.DataGridViewEvents.AllowUserToOrderColumns = true;
			this.DataGridViewEvents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DataGridViewEvents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.DataGridViewEvents.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnItem,
            this.ColumnType,
            this.ColumnPath,
            this.ColumnEvent,
            this.ColumnDateTime});
			this.DataGridViewEvents.Location = new System.Drawing.Point(12, 152);
			this.DataGridViewEvents.Name = "DataGridViewEvents";
			this.DataGridViewEvents.ReadOnly = true;
			this.DataGridViewEvents.Size = new System.Drawing.Size(673, 370);
			this.DataGridViewEvents.TabIndex = 0;
			// 
			// ColumnItem
			// 
			this.ColumnItem.HeaderText = "Item";
			this.ColumnItem.Name = "ColumnItem";
			this.ColumnItem.ReadOnly = true;
			this.ColumnItem.Width = 150;
			// 
			// ColumnType
			// 
			this.ColumnType.HeaderText = "Type";
			this.ColumnType.Name = "ColumnType";
			this.ColumnType.ReadOnly = true;
			this.ColumnType.Width = 60;
			// 
			// ColumnPath
			// 
			this.ColumnPath.HeaderText = "Path";
			this.ColumnPath.Name = "ColumnPath";
			this.ColumnPath.ReadOnly = true;
			this.ColumnPath.Width = 230;
			// 
			// ColumnEvent
			// 
			this.ColumnEvent.HeaderText = "Event";
			this.ColumnEvent.Name = "ColumnEvent";
			this.ColumnEvent.ReadOnly = true;
			this.ColumnEvent.Width = 60;
			// 
			// ColumnDateTime
			// 
			this.ColumnDateTime.HeaderText = "Date & Time";
			this.ColumnDateTime.Name = "ColumnDateTime";
			this.ColumnDateTime.ReadOnly = true;
			this.ColumnDateTime.Width = 125;
			// 
			// FormMonitor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(700, 534);
			this.Controls.Add(this.DataGridViewEvents);
			this.Controls.Add(this.LabelMonitorEvents);
			this.Controls.Add(this.TextBoxDatabase);
			this.Controls.Add(this.LabelDatabase);
			this.Controls.Add(this.ButtonMonitorBrowse);
			this.Controls.Add(this.TextBoxMonitorDir);
			this.Controls.Add(this.ComboBoxMonitorExt);
			this.Controls.Add(this.LabelMonitodExt);
			this.Controls.Add(this.LabelMonitorDir);
			this.Controls.Add(this.ToolStrip);
			this.Controls.Add(this.MenuStrip);
			this.MainMenuStrip = this.MenuStrip;
			this.Name = "FormMonitor";
			this.Text = "File Directory Monitor";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EH_Closing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EH_Closed);
			this.Load += new System.EventHandler(this.EH_Load);
			this.MenuStrip.ResumeLayout(false);
			this.MenuStrip.PerformLayout();
			this.ToolStrip.ResumeLayout(false);
			this.ToolStrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.DataGridViewEvents)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip MenuStrip;
		private System.Windows.Forms.ToolStripMenuItem MenuStripDropdownFile;
		private System.Windows.Forms.ToolStripMenuItem MenuStripDropdownFileClose;
		private System.Windows.Forms.ToolStripMenuItem MenuStripDropdownMonitor;
		private System.Windows.Forms.ToolStripMenuItem MenuStripDropdownMonitorOpen;
		private System.Windows.Forms.ToolStripMenuItem MenuStripDropdownMonitorSet;
		private System.Windows.Forms.ToolStripMenuItem MenuStripDropdownMonitorStart;
		private System.Windows.Forms.ToolStripMenuItem MenuStripDropdownMonitorStop;
		private System.Windows.Forms.ToolStripMenuItem MenuStripDropdownDatabase;
		private System.Windows.Forms.ToolStripMenuItem MenuStripDropdownDatabaseWrite;
		private System.Windows.Forms.ToolStripMenuItem MenuStripDropdownDatabaseQuery;
		private System.Windows.Forms.ToolStripMenuItem MenuStripDropdownHelp;
		private System.Windows.Forms.ToolStripMenuItem MenuStripDropdownHelpUsage;
		private System.Windows.Forms.ToolStripMenuItem MenuStripDropdownHelpAbout;
		private System.Windows.Forms.ToolStrip ToolStrip;
		private System.Windows.Forms.ToolStripButton ToolstripOpenMonitorDir;
		private System.Windows.Forms.ToolStripButton ToolstripStartMonitor;
		private System.Windows.Forms.ToolStripButton ToolstripStopMonitor;
		private System.Windows.Forms.ToolStripButton ToolstripWriteToDB;
		private System.Windows.Forms.ToolStripButton ToolstripQueryDB;
		private System.Windows.Forms.ToolStripButton ToolstripHelp;
		private System.Windows.Forms.Label LabelMonitorDir;
		private System.Windows.Forms.Label LabelMonitodExt;
		private System.Windows.Forms.ComboBox ComboBoxMonitorExt;
		private System.Windows.Forms.Button ButtonMonitorBrowse;
		private System.Windows.Forms.TextBox TextBoxMonitorDir;
		private System.Windows.Forms.Label LabelDatabase;
		private System.Windows.Forms.TextBox TextBoxDatabase;
		private System.Windows.Forms.Label LabelMonitorEvents;
		private System.Windows.Forms.DataGridView DataGridViewEvents;
		private System.Windows.Forms.FolderBrowserDialog BrowseFolderMonitorDialog;
		private System.Windows.Forms.FolderBrowserDialog BrowseFolderDatabaseDialog;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColumnItem;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColumnType;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPath;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColumnEvent;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDateTime;
	}
}

