using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// default page code behind
public partial class _Default : System.Web.UI.Page {
	// page load
	protected void Page_Load(object sender, EventArgs e) {
		// check if mobile
		if (Request.Browser.IsMobileDevice) {
			Response.Redirect("./m.Default.aspx", true);
		}


	}
}