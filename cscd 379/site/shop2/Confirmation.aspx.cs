using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// confirmation page
public partial class Confirmation : System.Web.UI.Page {
	// page load
	protected void Page_Load(object sender, EventArgs e) {
		if (IsPostBack) {
			return;
		}

		// pull order string
		string orderStr = (string)Session["OrderStr"];
		if (orderStr == null) {
			Response.Redirect("./Browse.aspx", true);
		}

		// set text
		literalOrderStr.Text = orderStr;
	}
}