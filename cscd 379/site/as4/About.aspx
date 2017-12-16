<%@ Page Title="" Language="C#" MasterPageFile="./MasterPage.master" AutoEventWireup="true" CodeFile="About.aspx.cs" Inherits="About" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titleContent" Runat="Server">
	<asp:Literal runat="server" Text=": About" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyContent" Runat="Server">
	<h1>About</h1>

	<div class="row marketing">
		<div class="col-lg-6">
			<h4>Bootstrap</h4>
			<p>Bootstrap master page to handle changes in sizes.</p>
			<img class="img-responsive" src="./assets/img/components.png" alt="Chania" />

			<h4>ASP</h4>
			<p>Codebehind to detect mobile browser and serve different pages.</p>
		</div>

		<div class="col-lg-6">
			<h4>Full page</h4>
			<p>This is a page for a desktop browser.</p>
		</div>
	</div>
	
</asp:Content>

