<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QueryStringRecipient.aspx.cs" Inherits="QueryStringRecipient" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="formMain" runat="server">
    <div>
		QueryStringRecipient page-
		<br/>
		<br/>
		<asp:Label ID="labelItemInfo" runat="server" Text=""></asp:Label>
		<br/>
		<br/>
		<asp:Button ID="buttonTrace" runat="server" Text="Get trace" OnClick="ClickTrace" />
    </div>
    </form>
</body>
</html>
