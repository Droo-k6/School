using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class QueryStringSender : System.Web.UI.Page {

	// page_load event handler
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			//buttonMore.Enabled = false;

			// add values to listbox
			listBox.Items.Add("Econo Rocka");
			listBox.Items.Add("Supreme Leather Rock");
			listBox.Items.Add("Threadbare Rock");
			listBox.Items.Add("Antique Rock");
			listBox.Items.Add("Retro-Finish Rock");
		}
	}

	// redirect button click
	protected void Click_Redirect(object sender, EventArgs e) {
		// get selected item text
		string _item = listBox.SelectedItem.Text;
		// build URL
		string _url = string.Format("QueryStringRecipient.aspx?Item={0}&Mode={1}", _item, checkBox.Checked);

		// redirect
		Response.Redirect(_url);
	}
}