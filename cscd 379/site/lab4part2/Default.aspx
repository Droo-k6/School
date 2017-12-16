<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="formMain" runat="server">
    <div>
		<asp:Label ID="labelWelcome" runat="server" Text=""></asp:Label>
		<br/>
		<p>
			Name: 
			<asp:TextBox ID="textboxName" runat="server"></asp:TextBox>
			<asp:Button ID="buttonCreate" runat="server" Text="Create Cookie" OnClick="ClickCreate" Height="22px" Width="108px" />
		</p>
    </div>
    </form>
</body>
</html>
