<%@ Page Title="" Language="C#" MasterPageFile="./ShopMaster.master" AutoEventWireup="true" CodeFile="Browse.aspx.cs" Inherits="Browse" %>

<asp:Content ID="contentBrowse" ContentPlaceHolderID="contentPlaceHolderArea" Runat="Server">
	<style type="text/css">
		.ItemCell {
			border:outset;
			border-width:thin;
			border-color:dimgrey;
			border-radius:11px;
		}
		.CellText {
			display:block;
			text-align:right;
			text-wrap:normal;
			font-family:'Times New Roman';
			font-size:11px;
		}
		.CellImage {
			
			float:left;
		}
	</style>

	<fieldset>
			<legend>Browse</legend>
			<asp:Table ID="tableItems" runat="server" CellSpacing="20" CellPadding="5" />
	</fieldset>
	
</asp:Content>

