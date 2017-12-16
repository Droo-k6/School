<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="./MasterPage.master" CodeFile="m.Default.aspx.cs" Inherits="m_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titleContent" Runat="Server">
	<asp:Literal runat="server" Text=": Mobile Home" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyContent" Runat="Server">

	<div class="jumbotron">
		<h4>Mobile homepage</h4>
		<p class="lead">Homepage for mobile browser.</p>
	</div>

	<div class="row marketing">
		<div class="col-lg-6">
			<h4>Bootstrap</h4>
			<p>Utilizing bootstrap for page design.</p>

			<h4>ASP</h4>
			<p>ASP code behind to check for mobile browser and redirect.</p>

			<h4>Mobile</h4>
			<p>This page is for mobile browsers.</p>

			<h3>Navigation</h3>
			<p>About page works correctly as well.</p>
		</div>
	</div>

</asp:Content>
