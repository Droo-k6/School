<%@ Page Language="C#" Trace="false" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Assignment 2: Database & Dynamic Controls</title>
	<style type="text/css">
		.auto-style1 {
			width: 417px;
			height: 70px;
		}
		#ButtonToCheckout {
			width: 150px;
		}
		#ButtonLoadDB {
			width: 100px;
		}
	</style>
</head>
<body>
    <form id="formTop" runat="server">
		<div>
    		<img class="auto-style1" src="./badbanner.png" /><br />
			Load tables, then proceed to checkout<br />
			<br />
			<asp:button runat="server" id="ButtonLoadDB" OnClick="ClickLoad" Text="Load tables" /><br />
			<br />
			<!-- PostBackUrl="./Shop.aspx" -->
			<asp:button runat="server" id="ButtonToCheckout" onClick="ClickCheckout" Text="To Checkout" />
			<br />
			<br />
		</div>
		<div id="divBottom" runat="server">

		</div>
    </form>
</body>
</html>
