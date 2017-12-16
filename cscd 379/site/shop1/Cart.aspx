<%@ Page Title="" Language="C#" MasterPageFile="./ShopMaster.master" AutoEventWireup="true" CodeFile="Cart.aspx.cs" Inherits="Cart" %>

<asp:Content ID="contentCart" ContentPlaceHolderID="contentPlaceHolderArea" Runat="Server">
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
	</style>
	
	<fieldset>
		<legend>Current Cart</legend>
		<asp:Label ID="labelCart" runat="server" Text=""></asp:Label>
		<div id="tableCartContainer" class="tableCartContainer" runat="server" >
			<asp:Table ID="tableCart" runat="server" CssClass="CartTable" CellSpacing="10" CellPadding="10" />
		</div>
	</fieldset>

</asp:Content>
