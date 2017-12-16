using System;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page {
	// page load event handler
	protected void Page_Load(object sender, EventArgs e) {
		// do nothing...
	}

	// web method
	// returns date of server as a string
	[WebMethod]
	public static String GetDate() {
		// Now, Today
		return DateTime.Today.ToString("dddd, MMMM dd, yyyy");
	}
}