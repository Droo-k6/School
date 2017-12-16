using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security.AntiXss;

public partial class ReflectedXSSQuery : System.Web.UI.Page {
	// page load
	protected void Page_Load(object sender, EventArgs e) {
		// get param value
		string param = Request.QueryString["param"];
		if (param == null)
			return;

		string paramDecoded = Server.UrlDecode(param);

		paramValue.Text = paramDecoded;
		paramValueText.InnerText = paramDecoded;
		paramValueHTML.InnerHtml = paramDecoded;

		string paramHTML = Server.HtmlEncode(paramDecoded);

		paramValueSafe.Text = paramHTML;
		paramValueHTMLSafe.InnerHtml = paramHTML;

		string xssEncode = AntiXssEncoder.HtmlEncode(paramDecoded, false);
		
	}

	// submit click
	protected void submitClick(object sender, EventArgs e) {
		string url = string.Format("./ReflectedXSSQuery.aspx?param={0}", Server.UrlEncode(textboxParam.Text));
		Response.Redirect(url);
	}
}