<%@ Page Title="" Language="C#" MasterPageFile="./ShopMaster.master" AutoEventWireup="true" CodeFile="Cart.aspx.cs" Inherits="Cart" Trace="false" %>

<asp:Content ID="ContentTitle" ContentPlaceHolderID="titleContent" Runat="Server">
	<asp:Literal ID="ContentTitleTitle" runat="server" Text=": Cart" />
</asp:Content>

<asp:Content ID="contentCart" ContentPlaceHolderID="contentPlaceHolderArea" Runat="Server">

	<script type="text/javascript">
		// function for changed textbox
		function changedQuantity(id) {
			// enable update button
			var buttonName = 'buttonUpdate' + id;
			$('[id$=' + buttonName + ']').removeAttr('disabled');
		}
		// check for update press
		function updateCheck(id) {
			// check validators
			if (!Page_ClientValidate('quantityGroup' + id)) {
				alert("Please correct the quantity field");
				return false;
			}
		}
		// checks if valid to go to checkout
		function checkoutCheck() {
			// check validators
			if (!Page_ClientValidate('')) {
				alert("Please correct the necessary fields");
				return false;
			}
			return true;
		}
	</script>

	<style type="text/css">		
		.Button {
			width:auto;
			height:auto;
			border-radius:12px;
		}
		.tableCartContainer {
			
		}
		.CartTable {
			border-right:solid;
			border-left:solid;
			border-top:solid;
			border-bottom:solid;
			border-collapse:collapse;
			border-style:none;
			border-width:1px;
			border-color:black;

		}
		.TitleRow {
			border-left:none;
			border-right:none;
			border-top:none;
			border-bottom:unset;
		}
		.TitleCell {
			border-right:none;
			border-left:none;
		}
		.ItemRow {
			border:none;
		}
		.BasicCell {
			border:solid;
			border-width:thin;
		}
		.ButtonCell {
			border-top:none;
			border-bottom:none;
			border-right:none;
		}
		.ButtonCheckout {
			width:auto;
			height:auto;
			border-radius:12px;
			margin-top:7px;
		}
		.TotalRow {
			border-left:none;
			border-right:none;
			border-top:unset;
			border-bottom:unset;
		}
		.EmptyCell {
			border-left:unset;
			border-right:unset;
			border-top:unset;
			border-bottom:unset;
		}
	</style>
	
	<fieldset>
		<legend>Current Cart</legend>
		<asp:Label ID="labelCart" runat="server" Text=""></asp:Label>
		<div id="tableCartContainer" class="tableCartContainer" runat="server" >
			<asp:Table ID="tableCart" runat="server" CssClass="CartTable" CellSpacing="10" CellPadding="10" />
		</div>
		<asp:Button ID="buttonCheckout" runat="server" CssClass="ButtonCheckout" Text="Checkout" PostBackUrl="./Checkout.aspx" OnClientClick="if (!checkoutCheck()) return false;" />
	</fieldset>

</asp:Content>
