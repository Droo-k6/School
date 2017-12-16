using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;

public partial class _Default : System.Web.UI.Page {

	// page load
	protected void Page_Load(object sender, EventArgs e) {
		// check for cookie
		HttpCookie _cookie = Request.Cookies["Preferences"];

		// set welcome page
		if (_cookie == null) {
			labelWelcome.Text = "<br/>Welcome new Customer<br/>";
		}else{
			labelWelcome.Text = string.Format("<br/>Cookie Found<br/>Welcome back, {0}<br/>", _cookie["Name"]);
		}
	}

	// click button, create cookier
	protected void ClickCreate(object sender, EventArgs e) {
		// get cookie, create if does not exist
		HttpCookie _cookie = Request.Cookies["Preferences"];
		if (_cookie == null) {
			_cookie = new HttpCookie("Preferences");
		}

		// set name entered
		_cookie["Name"] = textboxName.Text;
		// setup cookie
		_cookie.Expires = DateTime.Now.AddYears(1);
		Response.Cookies.Add(_cookie);

		// reset welcome
		labelWelcome.Text = string.Format("<br/>Cookie created<br/>New Customer, {0}<br/>", _cookie["Name"]);
	}
}