<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReflectedXSSQuery.aspx.cs" Inherits="ReflectedXSSQuery" validateRequest="false" Trace="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vulnerable Reflected XSS</title>
</head>
<body>
	<!-- style, javascript -->
	<style type="text/css">
		#sectionUnsafe {
			background-color:indianred;
			border-style:solid;
			border-width:5px;
		}
		#sectionSafe {
			background-color:green;
			border-style:solid;
			border-width:5px;
		}
	</style>
	<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>
	<!-- content -->
    <form id="formMain" runat="server" >
    <div>
		Enter a param value:
		<br />
		<asp:TextBox ID="textboxParam" runat="server" />
		<br />
		<asp:Button ID="buttonSubmit" runat="server" Text="Submit" OnClick="submitClick" />
		<br />

		<br />
		<div id="sectionUnsafe">
			<h4>Unsafe</h4>
			ASP Label
			<br />
			<asp:Label ID="paramValue" runat="server" Text="" />
			<br />
			HTML Label, Inner HTML
			<br />
			<label id="paramValueHTML" runat="server" ></label>
		</div>
		<br />
		<div id="sectionSafe">
			<h4>Safe</h4>
			HTML Label, Inner Text
			<br />
			<label id="paramValueText" runat="server"></label>
			<br />
			ASP Label
			<br />
			<asp:Label ID="paramValueSafe" runat="server" Text="" />
			<br />
			HTML Label, Inner HTML
			<br />
			<label id="paramValueHTMLSafe" runat="server" ></label>
		</div>
		<br />

    </div>
    </form>
</body>
</html>
