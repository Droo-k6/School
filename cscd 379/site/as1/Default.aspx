<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Assignment 1: Age at Grad</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    	<asp:Image ID="ImageBanner" runat="server" Height="70px" ImageUrl="./badbanner.png" Width="720px" />
		<br />
		<br />
		The current date is
		<asp:Label ID="LabelCurrentDate" runat="server" Text="..date.."></asp:Label>
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
		<br />
		<asp:Button ID="ButtonSubmit" runat="server" Height="22px" Text="Submit" Width="61px" TabIndex="3" />
		<br />
		<br />
		<asp:Label ID="LabelAge" runat="server" Text="..age.."></asp:Label>
    
    </div>
    </form>
</body>
</html>
