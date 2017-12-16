// Mathew McCain
// cscd371 final missile command
// contains MainWindow class, code portion
// main gui for the program

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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Diagnostics;
using System.Resources;
using System.Reflection;

namespace mccainmMissileCommand {
	// main window class
	public partial class MainWindow : Window {
		// static fields
		// header/tool tip text for controls
		private const string StrStartGame = "Start a game";
		private const string StrRestartGame = "Restart game";
		private const string StrPauseGame = "Pause game";
		private const string StrResumeGame = "Resume game";
		// button icons


		// member fields
		// handles logic side of missile command, the main object
		private GameLogic logic;
		// timer
		private DispatcherTimer timer;

		//private ResourceManager _manger;

		// constructor
		public MainWindow() {
			// create a resource manager

			// setup window
			InitializeComponent();

			// create timer, setting priority
			// will use current dispatcher
			// set the priority, the default value is too low and causes stutters
			// if inputs/lockups are an issue, may want to drop the value down from Render (7)
			// as a note, input is at 4
			timer = new DispatcherTimer(DispatcherPriority.Render);

			// construct logic
			logic = new GameLogic(CanvasGame, Refresh);

			// set timers interval and event
			// get tick rate for updates from logic
			timer.Interval = logic.TickRate;
			timer.Tick += Tick;

			// call refresh
			Refresh();

			// display the dispatcher priority
			//Dispatcher _d = Dispatcher.CurrentDispatcher;
			//DispatcherPriority _p = _d.Pro
		}

		// event handler for loaded
		// has to be used since canvas height/width isn't known at once
		private void EventLoaded(object sender, RoutedEventArgs e) {
			// tell logic form is loaded fully
			logic.Loaded();

			// open up settings menu
			SettingsWindow _window = new SettingsWindow(logic.CurrentSettings);
			_window.ShowDialog();
		}

		// Refresh event handler
		// sets enabled controls, header/tooltip text, button icons according to game state of logic
		private void Refresh() {
			// set text/icons for menu items/toolbar buttons as needed

			// check game state
			switch (logic.Status) {
				case (GameStatus.Waiting):
					// waiting state, no game in progress

					Debug.WriteLine("Refresh() Waiting");

					// set text for starting game
					MenuItemGameStartResume.Header = StrStartGame;
					ButtonStartResume.ToolTip = StrStartGame;
					ButtonStartResume.Content = FindResource("IconStart");
					// enable controls for pause/resume
					MenuItemGamePause.IsEnabled = false;
					// disable pause button, set image for play icon, set opacity to half
					ButtonPause.IsEnabled = false;
					ButtonPause.Content = FindResource("IconPlay");
					ButtonPause.Opacity = 0.5;
					// set text for pausing the game
					MenuItemGamePause.Header = StrPauseGame;
					ButtonPause.ToolTip = StrPauseGame;

					break;
				case (GameStatus.Playing):
					// playing state, game is in progress

					Debug.WriteLine("Refresh() Playing");

					// set text for restart game
					MenuItemGameStartResume.Header = StrRestartGame;
					ButtonStartResume.ToolTip = StrRestartGame;
					ButtonStartResume.Content = FindResource("IconRestart");
					// enable controls for pause/resume
					MenuItemGamePause.IsEnabled = true;
					// enable pause button, set image for pause icon, set opacity to full
					ButtonPause.IsEnabled = true;
					ButtonPause.Content = FindResource("IconPause");
					ButtonPause.Opacity = 1;
					// set text for pausing the game
					MenuItemGamePause.Header = StrPauseGame;
					ButtonPause.ToolTip = StrPauseGame;

					break;
				case (GameStatus.Paused):
					// paused state, game in progress is paused

					Debug.WriteLine("Refresh() Paused");

					// set text for restart game
					MenuItemGameStartResume.Header = StrRestartGame;
					ButtonStartResume.ToolTip = StrRestartGame;
					ButtonStartResume.Content = FindResource("IconRestart");
					// enable controls for resume game
					MenuItemGamePause.IsEnabled = true;
					// enable pause button, set image for play icon, set opacity to full
					ButtonPause.IsEnabled = true;
					ButtonPause.Content = FindResource("IconPlay");
					ButtonPause.Opacity = 1;
					// set text for resume game
					MenuItemGamePause.Header = StrResumeGame;
					ButtonPause.ToolTip = StrResumeGame;

					break;
				case (GameStatus.Ended):
					// ended state, game over - awaiting restart

					Debug.WriteLine("Refresh() Ended");

					// set text for restart game
					MenuItemGameStartResume.Header = StrRestartGame;
					ButtonPause.ToolTip = StrRestartGame;
					// disable pause/resume controls
					MenuItemGamePause.IsEnabled = false;
					ButtonPause.IsEnabled = false;
					ButtonPause.Opacity = 0.5;

					break;
			}

			// only enable timer if game is playing
			timer.IsEnabled = (logic.Status == GameStatus.Playing);
			// note: to have timer running at all times
			// change GameLogic.Start() and GameLogic constructor
			// have the currentTime value be set at construction, and not at Start()
		}

		// timer tick event
		private void Tick(object sender, EventArgs e) {
			// call tick on logic
			logic.Tick();
		}

		// game canvas event, on mouse button down
		private void CanvasGame_MouseDown(object sender, MouseButtonEventArgs e) {
			// no need to check if valid button was pressed, logic handles

			// get cordinates of mouse at press, relative to element (CanvasGame)
			Point clickPoint = e.GetPosition((IInputElement)sender);
			// call press on game logic
			logic.MousePress(e.ChangedButton, clickPoint);

			e.Handled = true;
		}

		// game canvas event, on mouse move
		private void CanvasGame_MouseMove(object sender, MouseEventArgs e) {
			// get mouse cordinates relative to element (CanvasGame)
			//Point mousePoint = e.GetPosition((IInputElement)sender);
			
			// not used atm, cursor updates handled by GameDraw.Draw()

			e.Handled = true;
		}

		// execute events for commands
		// start/restart game menu item/button
		private void CmdStartResumeExec(object sender, ExecutedRoutedEventArgs e) {
			// call StartRestart on logic
			logic.StartRestart();
		}

		// pause/resume menu item/button
		private void CmdPauseExec(object sender, ExecutedRoutedEventArgs e) {
			// call pause resume on logic
			logic.PauseResume();
		}

		// Highscores messagebox
		private void CmdScoresExec(object sender, ExecutedRoutedEventArgs e) {
			// call pause on logic
			logic.PauseIfNot();

			// get highscores text from logic, display in messagebox
			MessageBox.Show(string.Format(logic.HighscoreText()), "Highscores");
		}

		// Settings menu
		private void CmdSettingsExec(object sender, ExecutedRoutedEventArgs e) {
			// call pause on logic
			logic.PauseIfNot();

			// open up settings menu
			// CurrentSettings

			// create settings window
			SettingsWindow _window = new SettingsWindow(logic.CurrentSettings);
			_window.ShowDialog();
		}

		// Exit the app
		private void CmdExitExec(object sender, ExecutedRoutedEventArgs e) {
			// call close
			Close();
		}

		// About messagebox
		private void CmdAboutExec(object sender, ExecutedRoutedEventArgs e) {
			// call pause on logic
			logic.PauseIfNot();

			// get .NET version
			string netVersion = Environment.Version.ToString();
			// determine if process is x64, assumes x32 otherwise
			string processBitType = Environment.Is64BitProcess ? ("64") : ("32");
			// get assembly version
			string asmVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

			// Display
			MessageBox.Show(string.Format("Missile Command Final\nAuthor: {0}\nVersion: {1}\n.NET Version: {2}\nx{3} bit", "Mathew McCain", asmVersion, netVersion, processBitType),
				"Info",
				MessageBoxButton.OK,
				MessageBoxImage.Information);
		}

		// Help messagebox
		private void CmdHelpExec(object sender, ExecutedRoutedEventArgs e) {
			// call pause on logic
			logic.PauseIfNot();

			MessageBox.Show("Missile Command\nPress 'GO' to start a game, the game can be restarted by pressing the restart icon.\nThe game can be paused at any time by pressing the pause icon or (ctrl+P) - the game can be resumed in the same manner.\n\nEach level will step up the amount of incoming missiles and their speed. All friendly missile batteries are repaired/replenished between levels.\nGame ends when no cities (including bonus cities) are alive. Bonus cities earned every 4,000 points.\n\nGame settings available on program startup and under the 'Game' menu options.", "Help", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		// static commands for binding
		// Start/Restart
		public static readonly RoutedUICommand CmdStartResume = new RoutedUICommand("", "CmdStartResume", typeof(MainWindow), new InputGestureCollection() { new KeyGesture(Key.R, ModifierKeys.Control) });
		// Pause/Resume
		public static readonly RoutedUICommand CmdPause = new RoutedUICommand("", "CmdPause", typeof(MainWindow), new InputGestureCollection() { new KeyGesture(Key.P, ModifierKeys.Control) });
		// Highscores
		public static readonly RoutedUICommand CmdScores = new RoutedUICommand("", "CmdScores", typeof(MainWindow), new InputGestureCollection() { new KeyGesture(Key.H, ModifierKeys.Control) });
		// Settings
		public static readonly RoutedUICommand CmdSettings = new RoutedUICommand("", "CmdSettings", typeof(MainWindow), new InputGestureCollection() { new KeyGesture(Key.S, ModifierKeys.Control) });
		// Exit
		public static readonly RoutedUICommand CmdExit = new RoutedUICommand("", "CmdExit", typeof(MainWindow), new InputGestureCollection() { new KeyGesture(Key.F4, ModifierKeys.Alt) });
		// About
		public static readonly RoutedUICommand CmdAbout = new RoutedUICommand("", "CmdAbout", typeof(MainWindow), new InputGestureCollection() { new KeyGesture(Key.F1, ModifierKeys.None) });
		// Help
		public static readonly RoutedUICommand CmdHelp = new RoutedUICommand("", "CmdHelp", typeof(MainWindow), new InputGestureCollection() { new KeyGesture(Key.F2, ModifierKeys.None) });
	}
}
