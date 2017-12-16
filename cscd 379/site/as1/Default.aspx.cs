using System;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page {
	// page load event handler
	protected void Page_Load(object sender, EventArgs e) {
		// set current date
		DateTime _cur = DateTime.Today;
		LabelCurrentDate.Text = DateTime.Today.ToString("dddd, MMMM dd, yyyy");

		// check if postback call
		if (Page.IsPostBack) {
			// get dates
			DateTime _birthDate = buildDate(TextBoxBirthYear, TextBoxBirthMonth, TextBoxBirthDay);
			DateTime _gradDate = buildDate(TextBoxGradYear, TextBoxGradMonth, TextBoxGradDay);

			// get difference
			int _years = (_gradDate.Year - _birthDate.Year) - 1;
			if (_gradDate.Month > _birthDate.Month) {
				_years++;
			}else if (_gradDate.Month == _birthDate.Month) {
				if (_gradDate.Day >= _birthDate.Day) {
					_years++;
				}
			}

			// set age label
			LabelAge.Text = string.Format("You will be {0} years old at graduation.", _years);
			LabelAge.Visible = true;
		}else{
			// else, hide age label
			LabelAge.Visible = false;
		}
	}

	// builds date from given textboxs
	private DateTime buildDate(TextBox _boxYear, TextBox _boxMonth, TextBox _boxDay) {
		int _year = Int32.Parse(_boxYear.Text);
		int _month = Int32.Parse(_boxMonth.Text);
		int _day = Int32.Parse(_boxDay.Text);

		return new DateTime(_year, _month, _day);
	}
}