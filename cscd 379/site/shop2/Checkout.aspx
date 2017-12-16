<%@ Page Title="" Language="C#" MasterPageFile="./ShopMaster.master" AutoEventWireup="true" CodeFile="Checkout.aspx.cs" Inherits="Checkout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titleContent" Runat="Server">
	<asp:Literal ID="ContentTitleTitle" runat="server" Text=": Checkout" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolderArea" Runat="Server">
	<style type="text/css">
		.summaryTable {
			border-collapse:collapse;
		}
		.informationTable {
			border-collapse:collapse;
		}
		.TitleCell {
			min-width:100px;
		}
		.ValueCell {
			text-align:right;
		}
		.InfoTextbox {

		}
		.ButtonSubmit {
			width:auto;
			height:auto;
			border-radius:12px;
			margin-top:14px;
		}
		.containerCell {
			border-style:inset;
			border-color:lightblue;
			border-radius:8px;
			border-width:4px;

			padding:20px;
		}
		.container {
			
		}
	</style>

	<fieldset>
		<legend>Checkout</legend>
		<table>
			<tr>
				<th><h4>Order Summary</h4></th>
				<th><h4>Order Information</h4></th>
			</tr>
			<tr>
				<td class="containerCell" >
					<!-- summary -->
					<div id="containerSummary" runat="server" class="container" >
					<asp:Table ID="tableSummary" runat="server" CssClass="summaryTable" >
						<asp:TableRow>
							<asp:TableCell Text="Items" CssClass="TitleCell" />
							<asp:TableCell ID="CellItems" runat="server" CssClass="ValueCell" />
						</asp:TableRow>
						<asp:TableRow>
							<asp:TableCell Text="Quantity" CssClass="TitleCell" />
							<asp:TableCell ID="CellQuantity" runat="server" CssClass="ValueCell" />
						</asp:TableRow>
						<asp:TableRow>
							<asp:TableCell Text="Weight" CssClass="TitleCell" />
							<asp:TableCell ID="CellWeight" runat="server" CssClass="ValueCell" />
						</asp:TableRow>
						<asp:TableRow>
							<asp:TableCell Text="Price" CssClass="TitleCell" />
							<asp:TableCell ID="CellPrice" runat="server" CssClass="ValueCell" />
						</asp:TableRow>
						<asp:TableRow>
							<asp:TableCell Text="Shipping" CssClass="TitleCell" />
							<asp:TableCell ID="CellShipping" runat="server" CssClass="ValueCell" />
						</asp:TableRow>
						<asp:TableRow>
							<asp:TableCell Text="Total" CssClass="TitleCell" />
							<asp:TableCell ID="CellTotals" runat="server" CssClass="ValueCell" />
						</asp:TableRow>
					</asp:Table>
					</div>
				</td>
				<td class="containerCell" >
					<!-- customer information -->
					<div id="containerInfo" runat="server" class="container" >
					<asp:Table ID="tableInformation" runat="server" CssClass="informationTable" >
						<asp:TableRow>
							<asp:TableCell Text="Name" CssClass="TitleCell" />
							<asp:TableCell CssClass="ValueCell" >
								<asp:TextBox ID="TextboxName" runat="server" CssClass="InfoTextbox" />
							</asp:TableCell>
						</asp:TableRow>
						<asp:TableRow>
							<asp:TableCell Text="Street" CssClass="TitleCell" />
							<asp:TableCell CssClass="ValueCell" >
								<asp:TextBox ID="TextboxStreet" runat="server" CssClass="InfoTextbox" />
							</asp:TableCell>
						</asp:TableRow>
						<asp:TableRow>
							<asp:TableCell Text="City" CssClass="TitleCell" />
							<asp:TableCell CssClass="ValueCell" >
								<asp:TextBox ID="TextboxCity" runat="server" CssClass="InfoTextbox" />
							</asp:TableCell>
						</asp:TableRow>
						<asp:TableRow>
							<asp:TableCell Text="State" CssClass="TitleCell" />
							<asp:TableCell CssClass="ValueCell" >
								<asp:TextBox ID="TextboxState" runat="server" CssClass="InfoTextbox" />
							</asp:TableCell>
						</asp:TableRow>
						<asp:TableRow>
							<asp:TableCell Text="ZIP" CssClass="TitleCell" />
							<asp:TableCell CssClass="ValueCell" >
								<asp:TextBox ID="TextboxZIP" runat="server" CssClass="InfoTextbox" />
							</asp:TableCell>
						</asp:TableRow>
						<asp:TableRow>
							<asp:TableCell Text="Email" CssClass="TitleCell" />
							<asp:TableCell CssClass="ValueCell" >
								<asp:TextBox ID="TextboxEmail" runat="server" CssClass="InfoTextbox" />
							</asp:TableCell>
						</asp:TableRow>
					</asp:Table>
					</div>
				</td>
				<td>
					<!-- validators -->
					<br />
					<asp:RequiredFieldValidator ID="reqName" runat="server" ControlToValidate="TextboxName" ErrorMessage="Name missing" EnableClientScript="false" />
					<br />
					<asp:RegularExpressionValidator ID="regName" runat="server" ControlToValidate="TextboxName" ErrorMessage="Invalid characters in name" ValidationExpression="^[a-zA-Z\s]{1,50}$" EnableClientScript="false" />
					<br />
					<asp:RequiredFieldValidator ID="reqStreet" runat="server" ControlToValidate="TextboxStreet" ErrorMessage="Street missing" EnableClientScript="false" />
					<br />
					<asp:RequiredFieldValidator ID="reqCity" runat="server" ControlToValidate="TextboxCity" ErrorMessage="City missing" EnableClientScript="false" />
					<br />
					<asp:RequiredFieldValidator ID="reqState" runat="server" ControlToValidate="TextboxState" ErrorMessage="State missing" EnableClientScript="false" />
					<br />
					<asp:RequiredFieldValidator ID="reqZip" runat="server" ControlToValidate="TextboxZIP" ErrorMessage="ZIP missing" EnableClientScript="false" />
					<br />
					<asp:RegularExpressionValidator ID="regZip" runat="server" ControlToValidate="TextboxZIP" ErrorMessage="Invalid ZIP format" ValidationExpression="^((\d{5})|(\d{5}-\d{4}))$" EnableClientScript="false" />
					<br />
					<asp:RequiredFieldValidator ID="reqEmail" runat="server" ControlToValidate="TextboxEmail" ErrorMessage="Email missing" EnableClientScript="false" />
					<br />
					<asp:RegularExpressionValidator ID="regEmail" runat="server" ControlToValidate="TextboxEmail" ErrorMessage="Invalid email format" ValidationExpression="^.+@.+$" EnableClientScript="false" />
					<br />
					<asp:CustomValidator ID="cstEmail" runat="server" ErrorMessage="Invalid Email" ControlToValidate="TextboxEmail" OnServerValidate="ValidateEmail" EnableClientScript="false" />
					<br />
				</td>
			</tr>
		</table>
		<!-- submit -->
		<asp:Button ID="buttonSubmit" runat="server" CssClass="ButtonSubmit" Text="Submit" OnClick="SubmitClick"  />
		<br/>
		<asp:Label ID="labelWarning" runat="server" Visible="false" ForeColor="#ff0000" />
	</fieldset>
</asp:Content>

