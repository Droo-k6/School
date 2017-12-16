// Mathew McCain
// cscd371 final missile command
// contains SettingsWindow class, allows editing of specific settings for game

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace mccainmMissileCommand {
	// Interaction logic for SettingsWindow.xaml
	partial class SettingsWindow : Window {
		// constructor
		public SettingsWindow(GameSettings _settings) {
			InitializeComponent();

			settings = _settings;
		}

		// settings object to use
		private GameSettings settings;

		// when ok button is pressed
		// validates/sets settings entries
		private void ClickOk(object sender, RoutedEventArgs e) {
			// pull strings from textboxes
			string _missilesStr = TextboxMissiles.Text;
			string _citiesStr = TextboxCities.Text;

			int _missiles = -1, _cities = -1;

			// parse missile amount, validate
			if (Int32.TryParse(_missilesStr, out _missiles)) {
				if (_missiles <= 0) {
					MessageBox.Show("Invalid missiles per battery value, failed to parse");
					return;
				}
			}else{
				MessageBox.Show("Invalid missiles per battery value, must be greater than zero");
				return;
			}

			// parse initial city amount, validate
			if (Int32.TryParse(_citiesStr, out _cities)) {
				if (_cities <= 0) {
					MessageBox.Show("Invalid initial cities value, failed to parse");
					return;
				}
			}else{
				MessageBox.Show("Invalid initial cities value, must be greater than zero");
				return;
			}
			
			settings.MissilesPerBattery = _missiles;
			settings.InitialCities = _cities;
			settings.MissileSpeedUp = CheckBoxSpeed.IsEnabled;

			// exit the window
			Close();
		}
	}
}
