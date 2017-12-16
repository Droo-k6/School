<%@ Page Title="" Language="C#" MasterPageFile="./ShopMaster.master" AutoEventWireup="true" CodeFile="Confirmation.aspx.cs" Inherits="Confirmation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titleContent" Runat="Server">
	<asp:Literal ID="ContentTitleTitle" runat="server" Text=": Order Confirmation" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolderArea" Runat="Server">
	<fieldset>
		<legend>Confirmation</legend>
		Order placed sucessfully with order #<asp:Literal ID="literalOrderStr" runat="server" />
		<br />
		A confirmation email has been sent.
	</fieldset>
</asp:Content>

