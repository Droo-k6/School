﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// mobile about code behind
public partial class m_About : System.Web.UI.Page {
	// page load
	protected void Page_Load(object sender, EventArgs e) {
		// check if mobile
		if (!Request.Browser.IsMobileDevice) {
			Response.Redirect("./About.aspx", true);
		}
	}
}