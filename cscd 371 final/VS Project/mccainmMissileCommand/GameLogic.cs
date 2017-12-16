// Mathew McCain
// cscd371 final missile command
// contains GameLogic class
// handles logic side of the game

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Shapes;
using System.IO;
using System.Media;

namespace mccainmMissileCommand {
	// delegate for refresh event
	delegate void RefreshHandler();

	// enum for game state/status
	// Waiting, waiting for player to start the game
	// Playing, game is in progress
	// Paused, game is paused
	// Ended, game is over - waiting for player to restart
	enum GameStatus { Waiting, Playing, Paused, Ended }

	// Handles logic of missile command
	class GameLogic {
		// constructor
		// takes canvas used for the game, and a delegate to use for the refresh event
		public GameLogic(Canvas _canvas, RefreshHandler _delegate) {
			// create settings obj
			settings = new GameSettings();

			// set canvas to use
			gameCanvas = _canvas;

			// setup sound player
			//soundPlayer = new SoundPlayer("explosion.wav");
			//soundPlayer = new SoundPlayer("pack://application:,,,/Resources/explosion.wav");
			//soundPlayer = new SoundPlayer("/Resources/explosion.wav");
			// sound from http://soundbible.com/1234-Bomb.html
			soundPlayer = new SoundPlayer(Properties.Resources.explosion);

			// create game state
			state = new GameState(settings, gameCanvas, soundPlayer);

			// set new score handler
			state.newScore += NewScore;

			// create drawing obj
			draw = new GameDraw(settings, gameCanvas, this, state);

			// set handler for event
			refreshOccurred = _delegate;

			// set initial game state
			status = GameStatus.Waiting;

			// do not invoke refresh, will not work as the logic reference is not set yet

			// init accumulator to 0
			accumulator = 0;

			// build list of highscores
			highscores = new List<int>();
			
			// check if file exists
			if (File.Exists(settings.FilenameHighscores)) {
				// read the file contents into highscores
				using (StreamReader _reader = new StreamReader(settings.FilenameHighscores)) {
					string _line;
					while ((_line = _reader.ReadLine()) != null) {
						// parse line into an int, add as score
						int _score = -1;
						if (Int32.TryParse(_line, out _score)) {
							NewScore(_score);
						}
					}
				}
			}
		}

		// destructor
		// used to dump highscores to file
		~GameLogic() {
			// dump highscores to correct file
			using(StreamWriter _writer = new StreamWriter(settings.FilenameHighscores, false)) {
				foreach(int _i in highscores) {
					_writer.WriteLine(_i.ToString());
				}
			}
		}

		// constants

		// member fields
		// settings object
		private GameSettings settings;
		// handles drawing for game
		private GameDraw draw;
		// canvas for game
		private Canvas gameCanvas;
		// game status
		private GameStatus status;
		// game state
		private GameState state;
		// sound player
		private SoundPlayer soundPlayer;
		
		// real time of current game state (or last render update)
		private DateTime currentTime;
		// accumulator value, for the game loop in Tick()
		private double accumulator;

		// list of highscores
		// would work more optimally as a min-heap with a limited size
		private List<int> highscores;

		// refresh event
		private event RefreshHandler refreshOccurred;

		// properties
		// get current game state
		// get settings object
		public GameSettings CurrentSettings {
			get {
				return settings;
			}
		}
		public GameStatus Status {
			get {
				return status;
			}
		}
		// get tick rate, as TimeSpan
		public TimeSpan TickRate {
			get {
				// get tick rate in seconds
				// 1 / refresh rate (Hz)
				double rawTick = 1.0 / settings.TickRate;
				// conver to timespan obj
				TimeSpan tickSeconds = TimeSpan.FromSeconds(rawTick);

				//Debug.WriteLine(string.Format("tickRate: {0}, rawTick: {1}, tickSeconds: {2}", tickRate, rawTick, tickSeconds.TotalSeconds));

				return tickSeconds;
			}
		}
		

		// for when window has finished loading
		public void Loaded() {
			Debug.WriteLine("GameLogic.Loaded()");


			Debug.WriteLine("GameLogic.Loaded() GameState.Setup()");
			// tell state to generate final values
			// give canvas height/width
			state.Setup(gameCanvas.ActualHeight, gameCanvas.ActualWidth);

			Debug.WriteLine("GameLogic.Loaded() GameDraw.Setup()");

			// tell draw to setup the canvas
			draw.Setup();

			Debug.WriteLine("GameLogic.Loaded() GameLogic.Refresh()");

			// refresh
			Refresh();

			Debug.WriteLine("GameLogic.Loaded() Done");
		}

		// for setting a new score
		public void NewScore(int _score) {
			// blah
			// state.Highscore

			// the following would be far more effecient with a min-heap
			// add to score list
			highscores.Add(_score);
			// sort the list
			highscores.Sort();
			// reverse list
			highscores.Reverse();
			// would be more effecient to have sort do descending, but doesn't matter atm

			// trim down to max size
			if (highscores.Count > 10) {
				highscores.RemoveRange(10, highscores.Count - 10);
			}

			// set states highscore
			state.Highscore = highscores[0];
		}

		// generates text of highscores
		public string HighscoreText() {
			string _text = "";
			int _pos = 1;
			foreach(int _i in highscores) {
				_text += string.Format("{0}. {1}\n", _pos, _i);
				_pos++;
			}
			return _text;
		}

		// Tick, advances game state if available
		public void Tick() {
			// if game not playing, don't update
			// note: current setup, mainwindow.Refresh only enables the timer during play time
			// when game is resumed, need to update the currentTime value otherwise skips will happen
			// can be modified to support an idle screen
			if (status != GameStatus.Playing) {
				return;
			}

			// loop from
			// http://gafferongames.com/game-physics/fix-your-timestep/

			// get minimum time delta
			double dt = 1.0 / settings.TickRate;

			DateTime curTime = DateTime.Now;
			// get difference in actual time
			TimeSpan frameTime = curTime - currentTime;
			currentTime = curTime;

			// get frametime in seconds
			double frameTime_s = frameTime.TotalSeconds;

			// add to accumulator
			accumulator += frameTime_s;

			// advance game for each time delta block in accumulator
			while (accumulator >= dt) {
				state.Advance(dt);
				accumulator -= dt;
			}

			// have gamedraw draw the state
			// updates cusor
			draw.Draw();
		}

		// for mouse button press
		// given button pressed and position relative to canvas
		public void MousePress(MouseButton button, Point pos) {
			// check that game is playing
			if (status != GameStatus.Playing)
				return;

			// check that pos is within bounds
			if ((pos.Y < settings.BoundaryTop) || (pos.Y > (gameCanvas.ActualHeight - settings.BoundaryBottom)))
				return;

			// determine battery to use
			Battery _battery = null;
			if (button == MouseButton.Left) {
				_battery = state.LeftBattery;
			} else if (button == MouseButton.Middle) {
				_battery = state.CenterBattery;
			} else if (button == MouseButton.Right) {
				_battery = state.RightBattery;
			}

			// check if battery was found
			if (_battery != null) {
				// fire from battery
				Missile _proj = _battery.Fire(pos);
				// check if projectile was created
				if (_proj != null) {
					state.AddProjectile(_proj);
				}
			}
		}

		// pauses the game if not paused
		public void PauseIfNot() {
			if (status == GameStatus.Playing) {
				status = GameStatus.Paused;
				// call refresh
				Refresh();
			}
		}

		// starts the game if in waiting state, otherwise resets game to waiting state
		public void StartRestart() {
			// check if state is in waiting and start game, or just restart the game
			if (status == GameStatus.Waiting) {
				// start the game
				Start();
			}else{
				// restart the game
				Restart();
			}
		}

		// pause/resume, pauses game if playing - resumes game if paused
		public void PauseResume() {
			// check that game is not in waiting, or ended
			if (status == GameStatus.Waiting || status == GameStatus.Ended)
				return;

			// swap out states
			if (status == GameStatus.Playing) {
				status = GameStatus.Paused;
			} else{
				status = GameStatus.Playing;
			}

			// call refresh
			Refresh();
		}

		// setup for a new game
		private void Restart() {
			// reset state
			state.Reset();

			// reset draw
			draw.ResetSoft();

			// set state to waiting
			status = GameStatus.Waiting;

			// call refresh
			Refresh();
		}

		// start the game
		private void Start() {
			// set level=1
			// use Level property, to simplify multipliers and shit
			// actually, just use a NextLevel method
			// don't wanna deal with some equation bullshit to determine all that

			// call start on state
			state.Start();

			// set state to playing
			status = GameStatus.Playing;

			// call refresh
			Refresh();
		}

		// Refresh, calls refresh event and sets some values accordingly
		private void Refresh() {
			// reset current time
			currentTime = DateTime.Now;

			// show cursor depending if playing game
			draw.ShowCursor(status == GameStatus.Playing);

			// to display highscores when in waiting state
			draw.UpdateHighscoresInfo();

			// call refresh event
			refreshOccurred();
		}
	}
}
