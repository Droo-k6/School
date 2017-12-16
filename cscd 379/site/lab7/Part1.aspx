<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Part1.aspx.cs" Inherits="Part1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Part One: Data Validation Controls</title>
</head>
<body>
	<!-- javascript -->
	<script type="text/javascript">
		// validates input is even
		function ValidateEven(source, args){
			var number = args.Value;
			number = parseInt(number)
			args.IsValid = (number % 2) == 0;
		}
	</script>
	<!-- body -->
    <form id="formMain" runat="server">
    <div>

		Required Field:
		<asp:TextBox ID="txtboxValidate1" runat="server" />
		<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtboxValidate1" ErrorMessage="Required Field" />
		<br />

		Range:
		<asp:TextBox ID="txtboxValidate2" runat="server" />
		<asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtboxValidate2" ErrorMessage="Invalid range (int, 1-10)" Type="Integer" MinimumValue="1" MaximumValue="10" />
		<br />

		Custom validation:
		<asp:TextBox ID="txtboxValidate3" runat="server" />
		<asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="txtboxValidate3" ErrorMessage="Enter an even int" ValidateEmptyText="false" ClientValidationFunction="ValidateEven" />
		<br />

	</div>
    </form>
</body>
</html>
