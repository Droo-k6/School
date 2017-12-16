<%@ Page Language="C#" Trace="false" AutoEventWireup="true" CodeFile="QueryStringSender.aspx.cs" Inherits="QueryStringSender" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>

<script>
	<!-- javascript -->
	// function for when listbox selection changes
	// enables view button if selection made
	function listBoxSelected() {
		// get listbox
		var _listbox = document.getElementById("<%=listBox.ClientID %>");
		// check selected index
		if (_listbox.selectedIndex != -1) {
			// get button
			var _button = document.getElementById("<%=buttonMore.ClientID %>");
			_button.disabled = false;
		}
	}
</script>

<body>
    <form id="formMain" runat="server">
    <div>
		<asp:ListBox ID="listBox" runat="server" onchange="listBoxSelected(this);" ></asp:ListBox>
		<br/>
		<asp:CheckBox ID="checkBox" runat="server" Text="Display full details" />
		<br/>
		<asp:Button ID="buttonMore" runat="server" Text="View Information" OnClick="Click_Redirect" Enabled="false" />
    </div>
    </form>
</body>
</html>
