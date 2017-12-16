<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lab: Age at Grad w/ JS/jQuery</title>
	<style type="text/css">
		#ButtonSubmit {
			height: 23px;
			width: 61px;
		}
	</style>
	<!-- java script functions -->
	<script type="text/javascript" src="http://code.jquery.com/jquery-latest.min.js"></script>
	<script>
		/*
		unique IDs by ASP.NET
		https://msdn.microsoft.com/en-us/library/1d04y8ss.aspx

		http://stackoverflow.com/questions/3584145/how-to-change-the-text-of-a-label-in-jquery

		1)
		document.getElementById("labelCurrentDate").innerHTML = results.d;
		
		2)
		$("#labelCurrentDate").html(results.d);

		document.getElementById("labelCurrentDate")
		returns raw DOM object
		this is faster of course

		$("#labelCurrentDate")
		returns DOM object wrapped in jQuery object
		can get the DOM by $("#labelCurrentDate")[0]


		hiding an object
		
		visibility: hidden
		element.style.visibility = "hidden"
		makes items invisible, but space is still allocated for
		still in flow

		display: none
		element.style.display = "none"
		removes element from flow, present in DOM

		hidden, equivalent to display: none


		element.setAttribute("name",value);


		innerHTML	parsed as HTML
		innerText	parsed as plaintext

		*/

		// anonymous function passed to ready
		// event handler
		$(document).ready(function () {
			// hide labelAge
			var elem = document.getElementById("labelAge");
			elem.style.visibility = "hidden";

			//$("#labelAge").hide();

			// set server date
			getServerDate();
		});

		// function to set server date
		function getServerDate() {
			// call ajax
			// argument is an associative array
			$.ajax({
				type: "POST",
				url: 'default.aspx/GetDate',
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				// success, anonymous function to set servdate
				success: function (results) {
					document.getElementById("labelCurrentDate").innerHTML = results.d;
					//$("#labelCurrentDate").html(results.d);
				},
				// error, anonymous function to alert of error
				error: function (err) {
					alert(err.status + " - " + err.statusText);
				}
			});
		}

		// function for submit button press
		function submitClick() {
			getServerDate();

			// get birth date
			var birthDate = buildDate("TextBoxBirthYear","TextBoxBirthMonth","TextBoxBirthDay");
			// get grad date
			var gradDate = buildDate("TextBoxGradYear","TextBoxGradMonth","TextBoxGradDay");

			if ((birthDate == null) || (gradDate == null)) {
				return;
			}

			// calculate age
			var age = (gradDate.getYear() - birthDate.getYear()) - 1;
			if (gradDate.getMonth() > birthDate.getMonth()) {
				age++;
			} else if (gradDate.getMonth() == birthDate.getMonth()) {
				if (gradDate.getDay() >= birthDate.getDay()) {
					age++;
				}
			}

			// display
			var elem = document.getElementById("labelAge");
			//elem.innerHTML = age;
			elem.innerText = "You will be " + age + " years old at graduation.";
			elem.style.visibility = "visible";
			
			//$("#labelAge").show();
			//$("#labelAge").text("Hello!!");
		}

		// gets values from given boxes
		// builds Date object
		// takes control names of year/month/day
		function buildDate(ctrlYear, ctrlMonth, ctrlDay) {
			// get elements from given names
			var elemYear = document.getElementById(ctrlYear);
			var elemMonth = document.getElementById(ctrlMonth);
			var elemDay = document.getElementById(ctrlDay);

			// validate elements
			if ((elemYear == null)||(elemMonth == null)||(elemDay == null)) {
				alert("Invalid id name");
				return;
			}

			// parse information
			var year = parseInt(elemYear.value);
			var month = parseInt(elemMonth.value);
			var day = parseInt(elemDay.value);

			// validate info
			if (isNaN(year) || isNaN(month) || isNaN(day)) {
				alert("failed to parse int from field");
				return;
			}
			if (year <= 0) {
				alert("invalid year");
				return;
			}
			if ((month < 1)||(month > 12)) {
				alert("invalid month");
				return;
			}
			if ((day < 1)||(day > 31)) {
				alert("invalid day");
				return;
			}

			// build/return date
			var date = new Date(year, month - 1, day);

			// to validate month/day
			// could compare values in date object to what was passed
			// if any changes, was invalid
			// could do instead of above validation

			return date;
		}

	</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    	<asp:Image ID="ImageBanner" runat="server" Height="70px" ImageUrl="./badbanner.png" Width="720px" />
		<br />
		<br />
		The current date is
		<label id="labelCurrentDate">..date..</label>
		<br />
		How old will you be when you graduate?<br />
		<br />
		Enter your birth date and expected graduation date-<br />
		<br />
		
		<asp:Table ID="TableDates" runat="server" Height="111px" Width="243px">
			<asp:TableRow runat="server" ID="RowTitle">
				<asp:TableCell runat="server" Width="70px">Birth</asp:TableCell>
				<asp:TableCell runat="server"></asp:TableCell>
				<asp:TableCell runat="server" Width="70px">Graduation</asp:TableCell>
				<asp:TableCell runat="server"></asp:TableCell>
			</asp:TableRow>
			<asp:TableRow runat="server" ID="RowYear">
				<asp:TableCell runat="server">Year</asp:TableCell>
				<asp:TableCell runat="server">
					<asp:TextBox ID="TextBoxBirthYear" runat="server" Height="14px" Width="50px" Wrap="False" TabIndex="1"></asp:TextBox>
				</asp:TableCell>
				<asp:TableCell runat="server">Year</asp:TableCell>
				<asp:TableCell runat="server">
					<asp:TextBox ID="TextBoxGradYear" runat="server" Height="14px" Width="50px" Wrap="False" TabIndex="2"></asp:TextBox>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="RowMonth" runat="server">
				<asp:TableCell runat="server">Month</asp:TableCell>
				<asp:TableCell runat="server">
					<asp:TextBox ID="TextBoxBirthMonth" runat="server" Height="14px" Width="50px" Wrap="False" TabIndex="1"></asp:TextBox>
				</asp:TableCell>
				<asp:TableCell runat="server">Month</asp:TableCell>
				<asp:TableCell runat="server">
					<asp:TextBox ID="TextBoxGradMonth" runat="server" Height="14px" Width="50px" Wrap="False" TabIndex="2"></asp:TextBox>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="RowDay" runat="server">
				<asp:TableCell runat="server">Day</asp:TableCell>
				<asp:TableCell runat="server">
					<asp:TextBox ID="TextBoxBirthDay" runat="server" Height="14px" Width="50px" Wrap="False" TabIndex="1"></asp:TextBox>
				</asp:TableCell>
				<asp:TableCell runat="server">Day</asp:TableCell>
				<asp:TableCell runat="server">
					<asp:TextBox ID="TextBoxGradDay" runat="server" Height="14px" Width="50px" Wrap="False" TabIndex="2"></asp:TextBox>
				</asp:TableCell>
			</asp:TableRow>
		</asp:Table>
		<input id="ButtonSubmit" type="button" value="Submit" tabindex="3" onclick="submitClick()" />
		<br />
		<label id="labelAge">..age..</label>
    </div>
    </form>
</body>
</html>
