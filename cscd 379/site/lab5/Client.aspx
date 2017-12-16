<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Client.aspx.cs" Inherits="Client" %>

<!DOCTYPE html>

<noscript>JavaScript needs to be enabled for this page to be displayed properly</noscript>
<script src="http://code.jquery.com/jquery-latest.min.js"></script>
<script>
	// handles button click
	function getClick() {
		// get label for response
		var _mainLabel = document.getElementById("LabelResponse");
		var _classLabel = document.getElementById("LabelClass");

		// get info from textbox
		var _textbox = document.getElementById("InputZip");
		var _text = _textbox.value;

		// call web service
		WebService.GetLocation(_text,
			// anonymous function for success
			function (_result) {
				// check if result was valid
				if (_result.Valid) {
					_mainLabel.style.color = "black";
					_mainLabel.innerText = _result.City + ", " + _result.State;
					if (_result.Class) {
						_classLabel.style.visibility = "visible";
						_classLabel.innerText = _result.Class;
					}else{
						_classLabel.style.visibility = "hidden";
					}
				} else {
					_classLabel.style.visibility = "hidden";
					_mainLabel.style.color = "red";
					_mainLabel.innerText = _result.ValidStr;
				}
			},
			// anonymous function for failure
			function (_result) {
				_classLabel.style.visibility = "hidden";
				_mainLabel.style.color = "red";
				_mainLabel.innerText = "Failed to reach service";
			}
		)
	}

</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lab: WebService and Client w/ MS SQLServer</title>
</head>
<body>
	<!-- styles -->
	<style type="text/css">
		html {
			font-family:'Times New Roman';
			font-size:14px;
		}
		#container {
			width:270px;
			border-style:inset;
			border-width:3px;
			border-color:gray;
			background-color:azure;
			padding:5px;
		}
		#ButtonUse {
			margin:2px;
			width:auto;
			height:auto;
		}
		#InputZip {
			width:110px;
		}
		#LabelClass {
			visibility:hidden;
		}
	</style>
	<form runat="server">
		<!-- My container -->
		<div id="ContainerMain" runat="server" >
			<!-- script manager -->
			<asp:ScriptManager runat="server" ID="ScriptManagerMain" >
				<Services>
					<asp:ServiceReference Path="./WebService.asmx" />
				</Services>
			</asp:ScriptManager>
			<!-- controls -->
			<div id="container" runat="server">
				<fieldset>
					<legend>WebService Client</legend>
					Enter a zip code and press get info.
					<br/>
					Zip: <input type="text" id="InputZip" runat="server" />
					<input type="button" id="ButtonUse" runat="server" onclick="getClick()" value="Get location" />
					<br/>
					Location: <label id="LabelResponse" runat="server"></label>
					<br/>
					<label id="LabelClass" runat="server"></label>
				</fieldset>
				
			</div>
		</div>
	</form>
</body>
</html>
