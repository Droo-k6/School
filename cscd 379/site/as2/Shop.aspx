<%@ Page trace="false" Language="C#" AutoEventWireup="true" CodeFile="Shop.aspx.cs" Inherits="Shop" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>page #2: Checkout</title>
	<style type="text/css">
		.auto-style1 {
			width: 417px;
			height: 70px;
		}
	</style>
</head>
<body>
    <form id="formMain" runat="server">
    <div>
    	<img class="auto-style1" src="./badbanner2.png" /><br />
		<asp:Label ID="LabelStatus" runat="server" Text="Status..." Width="500px"></asp:Label>
		<div id="dynamicControls" runat="server">

		</div>
    </div>
    </form>
</body>
</html>
