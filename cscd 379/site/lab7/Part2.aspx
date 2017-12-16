<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Part2.aspx.cs" Inherits="Part2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Part Two: Manual Data Validation</title>
</head>
<body>
    <form id="formMain" runat="server">
    <div>
		Enter value to be checked: 
		<asp:TextBox ID="txtboxValidate" runat="server" />
		<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtboxValidate" ErrorMessage="Required field" EnableClientScript="false" />
		<asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtboxValidate" ErrorMessage="Must enter a positive integer" Type="Integer" MinimumValue="0" MaximumValue="2000000000" EnableClientScript="false"  />
		<asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="txtboxValidate" ErrorMessage="Must checksum to zero" OnServerValidate="ServerValidateChecksum" EnableClientScript="false" ValidateEmptyText="false" />
		<br />
		<asp:Button ID="buttonSubmit" runat="server" Text="Submit" OnClick="SubmitClick" />
		<br />
		<asp:Label ID="lblResponse" runat="server" />
    </div>
    </form>
</body>
</html>
