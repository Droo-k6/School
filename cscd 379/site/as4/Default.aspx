<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="./MasterPage.master" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titleContent" Runat="Server">
	<asp:Literal runat="server" Text=": Home" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyContent" Runat="Server">

	<div class="jumbotron">
		<h1>Desktop homepage</h1>
		<p class="lead">Homepage for desktop browser.</p>
	</div>

	<div class="row marketing">
		<div class="col-lg-6">
			<h4>Bootstrap</h4>
			<p>Utilizing bootstrap for page design.</p>

			<h4>ASP</h4>
			<p>ASP code behind to check for mobile browser and redirect.</p>

			<h4>Desktop</h4>
			<p>This page is for desktop browsers.</p>

			<h3>Navigation</h3>
			<p>About page works correctly as well.</p>
		</div>

		<div class="col-lg-6">
			<h3>Some images</h3>
			<img class="img-responsive img-circle" src="./assets/img/expo-vogue.jpg" alt="Chania"/> 
			<img class="img-responsive img-thumbnail" src="./assets/img/sass-less.png" alt="Chania"/> 

			<h3>Eat space</h3>
			<p>About page works correctly as well.</p>
		</div>
	</div>

</asp:Content>
