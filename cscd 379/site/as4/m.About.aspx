<%@ Page Title="" Language="C#" MasterPageFile="./MasterPage.master" AutoEventWireup="true" CodeFile="m.About.aspx.cs" Inherits="m_About" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titleContent" Runat="Server">
	<asp:Literal runat="server" Text=": Mobile About" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyContent" Runat="Server">
	<h3>Mobile About</h3>

	<div class="row marketing">
		<div class="col-lg-6">
			<h4>Using Bootstrap</h4>
			<p>Bootstrap masterpage to auto configure pages.</p>

			<h4>Mobile</h4>
			<p>This is a page for a mobile browser.</p>
			<img class="img-responsive" src="./assets/img/devices.png" alt="Chania" />

			<h4>Contact us!</h4>
			<a href="tel:+18888218218">1-888-821-8218</a>
		</div>
	</div>
</asp:Content>

