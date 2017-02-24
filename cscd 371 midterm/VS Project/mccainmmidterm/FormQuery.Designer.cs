namespace mccainmmidterm {
	partial class FormQuery {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormQuery));
			this.DataGridViewEvents = new System.Windows.Forms.DataGridView();
			this.ColumnItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColumnType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColumnPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColumnEvent = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColumnDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.LabelDataGrid = new System.Windows.Forms.Label();
			this.LabelQuery = new System.Windows.Forms.Label();
			this.ToolStrip = new System.Windows.Forms.ToolStrip();
			this.ToolStripButtonHelp = new System.Windows.Forms.ToolStripButton();
			this.ComboBoxQuery = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.DataGridViewEvents)).BeginInit();
			this.ToolStrip.SuspendLayout();
			this.SuspendLayout();
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
			this.DataGridViewEvents.Location = new System.Drawing.Point(12, 80);
			this.DataGridViewEvents.Name = "DataGridViewEvents";
			this.DataGridViewEvents.ReadOnly = true;
			this.DataGridViewEvents.Size = new System.Drawing.Size(669, 278);
			this.DataGridViewEvents.TabIndex = 13;
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
			// LabelDataGrid
			// 
			this.LabelDataGrid.AutoSize = true;
			this.LabelDataGrid.Location = new System.Drawing.Point(9, 64);
			this.LabelDataGrid.Name = "LabelDataGrid";
			this.LabelDataGrid.Size = new System.Drawing.Size(101, 13);
			this.LabelDataGrid.TabIndex = 14;
			this.LabelDataGrid.Text = "Query event results:";
			// 
			// LabelQuery
			// 
			this.LabelQuery.AutoSize = true;
			this.LabelQuery.Location = new System.Drawing.Point(9, 25);
			this.LabelQuery.Name = "LabelQuery";
			this.LabelQuery.Size = new System.Drawing.Size(119, 13);
			this.LabelQuery.TabIndex = 15;
			this.LabelQuery.Text = "Extensions to Query for:";
			// 
			// ToolStrip
			// 
			this.ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripButtonHelp});
			this.ToolStrip.Location = new System.Drawing.Point(0, 0);
			this.ToolStrip.Name = "ToolStrip";
			this.ToolStrip.Size = new System.Drawing.Size(693, 25);
			this.ToolStrip.TabIndex = 17;
			// 
			// ToolStripButtonHelp
			// 
			this.ToolStripButtonHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ToolStripButtonHelp.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripButtonHelp.Image")));
			this.ToolStripButtonHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ToolStripButtonHelp.Name = "ToolStripButtonHelp";
			this.ToolStripButtonHelp.Size = new System.Drawing.Size(23, 22);
			this.ToolStripButtonHelp.Text = "toolStripButton1";
			this.ToolStripButtonHelp.Click += new System.EventHandler(this.EH_Help);
			// 
			// ComboBoxQuery
			// 
			this.ComboBoxQuery.FormattingEnabled = true;
			this.ComboBoxQuery.Location = new System.Drawing.Point(13, 40);
			this.ComboBoxQuery.Name = "ComboBoxQuery";
			this.ComboBoxQuery.Size = new System.Drawing.Size(252, 21);
			this.ComboBoxQuery.TabIndex = 18;
			this.ComboBoxQuery.SelectedIndexChanged += new System.EventHandler(this.EH_IndexSelect);
			this.ComboBoxQuery.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EH_KeydownQuery);
			this.ComboBoxQuery.Leave += new System.EventHandler(this.EH_LeaveFocusQuery);
			// 
			// FormQuery
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(693, 370);
			this.Controls.Add(this.ComboBoxQuery);
			this.Controls.Add(this.ToolStrip);
			this.Controls.Add(this.LabelQuery);
			this.Controls.Add(this.LabelDataGrid);
			this.Controls.Add(this.DataGridViewEvents);
			this.Name = "FormQuery";
			this.Text = "FormQuery";
			this.Load += new System.EventHandler(this.EH_Load);
			((System.ComponentModel.ISupportInitialize)(this.DataGridViewEvents)).EndInit();
			this.ToolStrip.ResumeLayout(false);
			this.ToolStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView DataGridViewEvents;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColumnItem;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColumnType;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPath;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColumnEvent;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDateTime;
		private System.Windows.Forms.Label LabelDataGrid;
		private System.Windows.Forms.Label LabelQuery;
		private System.Windows.Forms.ToolStrip ToolStrip;
		private System.Windows.Forms.ToolStripButton ToolStripButtonHelp;
		private System.Windows.Forms.ComboBox ComboBoxQuery;
	}
}