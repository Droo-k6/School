<%@ Page Title="" Language="C#" Trace="false" MasterPageFile="./ShopMaster.master" AutoEventWireup="true" CodeFile="Item.aspx.cs" Inherits="Item" %>

<asp:Content ID="ContentTitle" ContentPlaceHolderID="titleContent" Runat="Server">
	<asp:Literal ID="ContentTitleTitle" runat="server" Text="" />
</asp:Content>

<asp:Content ID="contentItem" ContentPlaceHolderID="contentPlaceHolderArea" Runat="Server">
	<style type="text/css" >
		.ItemImage {
			float: left;
			margin:50px;
		}
		.ItemInfo {
			text-align:right;
			float:right;
			margin:50px;
			width:500px;
		}
		.ItemButton {
			border-radius:9px;
			width:auto;
			height:auto;
			padding:5px;
			margin:10px;
		}
		#contentItemArea{
			vertical-align:middle;
			text-align:right;
			margin:20px;
			width:1000px;
			height:600px;
		}
	</style>

	<fieldset>
			<legend>Item</legend>
			<div id="contentItemArea" runat="server"></div>
	</fieldset>
</asp:Content>

