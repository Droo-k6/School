using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// part 2 code behind
public partial class Part2 : System.Web.UI.Page {
	// on submit button press
	protected void SubmitClick(object sender, EventArgs e) {
		string _responses = "<b>Responses:</b><br/>";

		// go through validators
		foreach(BaseValidator _validator in Validators) {
			if (!_validator.IsValid) {
				TextBox _textbox = (TextBox) FindControl(_validator.ControlToValidate);
				_responses += string.Format("{0}<br/>&emsp;*Input=\"{1}\"<br/>", _validator.ErrorMessage, _textbox.Text);
			}
		}

		// set responses text
		lblResponse.ForeColor = System.Drawing.Color.Red;
		lblResponse.Text = _responses;
	}

	// validates checksum of textbox
	protected void ServerValidateChecksum(object source, ServerValidateEventArgs args) {
		// get number
		int num;
		if (!Int32.TryParse(args.Value, out num)) {
			args.IsValid = false;
			return;
		}

		// checksum
		int sum = 0;
		while(num > 0) {
			sum += num % 10;
			num /= 10;
		}

		args.IsValid = ((sum % 10) == 0);
	}
}