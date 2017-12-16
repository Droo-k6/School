using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class QueryStringRecipient : System.Web.UI.Page {

	// page load event
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			// build text for label
			labelItemInfo.Text = string.Format("Item: {0}<br/>Full Info: {1}", Request.QueryString["Item"], Request.QueryString["Mode"]);
		}
	}

	// click trace button
	protected void ClickTrace(object sender, EventArgs e) {
		Trace.IsEnabled = true;
	}
}