// Mathew McCain
// cscd371 final missile command
// contains GameDraw class
// handles drawing for the game

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.IO;

namespace mccainmMissileCommand {
	// Handles drawing for missile command
	class GameDraw {
		// constructor
		// takes canvas to use for drawing
		public GameDraw(GameSettings _settings, Canvas _canvas, GameLogic _logic, GameState _state) {
			// set settings
			settings = _settings;
			// set canvas
			gameCanvas = _canvas;
			// set state
			state = _state;
			logic = _logic;

			// set state events
			// these are useless now that GameState holds a reference to the canvas...
			state.addShapes += AddShapes;
			state.removeShapes += RemoveShapes;

			// don't setup canvas until loaded event received
		}

		// member fields
		// settings of game
		private GameSettings settings;
		// canvas to use
		private Canvas gameCanvas;
		// game logic, careful with using - as logic primarily calls on GameDraw
		private GameLogic logic;
		// state of game
		private GameState state;

		// terrain shape (polygon)
		private Polygon terrainShape;
		// cursor shape
		private Rectangle cursorShape;
		// cursor image
		private BitmapImage cursorImage;
		// text block shapes, info bar
		private TextBlock TBCheats, TBLevel, TBLevelValue, TBScore, TBHighscore;
		// text block shapes, below batteries
		private TextBlock TBLeftBattery, TBCenterBattery, TBRightBattery;
		// text blocks for in between level display
		private TextBlock TBLevelStatus, TBLevelBonusCities, TBLevelMissiles, TBLevelCities;
		// textblock for highscores
		private TextBlock TBHighscores;

		// update canvas children with given shapes
		public void AddShapes(List<Shape> _shapes) {
			foreach(Shape _s in _shapes) {
				gameCanvas.Children.Add(_s);
			}
		}
		// remove given shapes from canvas children
		public void RemoveShapes(List<Shape> _shapes) {
			foreach (Shape _s in _shapes) {
				gameCanvas.Children.Remove(_s);
			}
		}

		// draws the current game state
		// gets list of projectiles/explosions to draw from GameState
		public void Draw() {
			// set projectile positions
			foreach(Missile _p in state.ProjectilesList) {
				Canvas.SetLeft(_p.ProjectileShape, _p.ProjectileDisplayPoint.X);
				Canvas.SetTop(_p.ProjectileShape, _p.ProjectileDisplayPoint.Y);
			}
			// set explosion positions
			foreach(Explosion _e in state.ExplosionsList) {
				Canvas.SetLeft(_e.ExplosionShape, _e.Position.X);
				Canvas.SetTop(_e.ExplosionShape, _e.Position.Y);
			}
			// set target positions (this is pointless to do every draw, but workaround work take more work)
			// todo: change to have a set position call
			// then just have state set visiblity of shapes
			foreach (ITarget _t in state.Targets) {
				Shape _shape = _t.DisplayShape;
				Point _point = _t.DisplayPoint;
				Canvas.SetLeft(_shape, _point.X);
				Canvas.SetTop(_shape, _point.Y);
			}
			// set cursor position
			SetCursorShape();
			// update info bar
			UpdateInfoBar();
			// update battery info
			UpdateBatteryInfo();
			// update level info
			UpdateLevelInfo();
			// update highscores block
			UpdateHighscoresInfo();
		}

		// set positions of given targets
		// not done in Draw, since these do not shift positions
		// assumes all shapes have been handled via AddShapes()/RemoveShapes()
		public void DrawTargets(List<ITarget> _targets) {
			foreach(ITarget _t in _targets) {
				Canvas.SetLeft(_t.DisplayShape, _t.DisplayPoint.X);
				Canvas.SetTop(_t.DisplayShape, _t.DisplayPoint.Y);
			}
		}

		// show/hide cursor
		public void ShowCursor(bool _show) {
			// validate shape exists
			if (cursorShape == null)
				return;

			// change visibility
			if (_show) {
				cursorShape.Visibility = Visibility.Visible;
			}else{
				cursorShape.Visibility = Visibility.Hidden;
			}
		}

		// check if given point is within fire boundaries
		public bool WithinBoundaries(Point _point) {
			// check x cordinates are within canvas width
			// check if y cordinates within BoundaryBottom/Top of canvas height
			return ((_point.X >= 0) && (_point.X <= (gameCanvas.ActualWidth))
				&& (_point.Y > settings.BoundaryTop) && (_point.Y < (gameCanvas.ActualHeight - settings.BoundaryBottom))
				);
		}

		// sets cursor shape to current cursor position on canvas
		private void SetCursorShape() {
			// check that shape exists
			if (cursorShape == null) {
				return;
			}

			// get position of cursor relative to canvas
			Point _p = Mouse.GetPosition(gameCanvas);

			// check if within boundaries
			if (!WithinBoundaries(_p))
				return;

			// set cursor shape to point
			Canvas.SetLeft(cursorShape, _p.X - cursorShape.ActualWidth / 2);
			Canvas.SetTop(cursorShape, _p.Y - cursorShape.ActualHeight / 2);
		}

		// hard reset, everything/including shapes
		private void ResetHard() {
			// null GameDraw specific shapes
			cursorShape = null;
			terrainShape = null;
			TBCheats = null;
			TBLevel = null;
			TBLevelValue = null;
			TBScore = null;
			TBHighscore = null;
			TBLeftBattery = null;
			TBCenterBattery = null;
			TBRightBattery = null;
			TBLevelStatus = null;
			TBLevelBonusCities = null;
			TBLevelMissiles = null;
			TBLevelCities = null;
			TBHighscores = null;

			// clear children
			gameCanvas.Children.Clear();
		}

		// soft reset, just shapes/colors that change as game progresses
		// assumes GameState.Reset() was called and wiped the shapes
		public void ResetSoft() {
			// update info bar
			UpdateInfoBar();

			UpdateBatteryInfo();

			UpdateLevelInfo();

			// reset colors
			UpdateColors();
		}

		// updates colors according to state/settings values
		public void UpdateColors() {
			// background color
			gameCanvas.Background = settings.BrushBackground;

			// hard reset cursor
			if (cursorShape != null) {
				gameCanvas.Children.Remove(cursorShape);
			}
			CreateCursor();

			// terrain colors
			terrainShape.Stroke = settings.BrushStrokeTerrain;
			terrainShape.Fill = settings.BrushFillTerrain;

			// textblock colors
			// background
			TBCheats.Background = settings.InfoBrushBackground;
			TBLevel.Background = settings.InfoBrushBackground;
			TBLevelValue.Background = settings.InfoBrushBackground;
			TBScore.Background = settings.InfoBrushBackground;
			TBHighscore.Background = settings.InfoBrushBackground;
			TBLeftBattery.Background = Brushes.Transparent;
			TBCenterBattery.Background = Brushes.Transparent;
			TBRightBattery.Background = Brushes.Transparent;
			TBLevelStatus.Background = Brushes.Transparent;
			TBLevelBonusCities.Background = Brushes.Transparent;
			TBLevelMissiles.Background = Brushes.Transparent;
			TBLevelCities.Background = Brushes.Transparent;
			TBHighscores.Background = Brushes.Transparent;
			// foreground
			TBCheats.Foreground = settings.InfoCheatBrushForeground;
			TBLevel.Foreground = settings.InfoBrushForeground;
			TBLevelValue.Foreground = settings.InfoBrushForeground;
			TBScore.Foreground = settings.InfoBrushForeground;
			TBHighscore.Foreground = settings.InfoBrushForeground;
			TBLeftBattery.Foreground = settings.InfoBrushForeground;
			TBCenterBattery.Foreground = settings.InfoBrushForeground;
			TBRightBattery.Foreground = settings.InfoBrushForeground;
			TBLevelStatus.Foreground = settings.InfoBrushForeground;
			TBLevelBonusCities.Foreground = settings.InfoBrushForeground;
			TBLevelMissiles.Foreground = settings.InfoBrushForeground;
			TBLevelCities.Foreground = settings.InfoBrushForeground;
			TBHighscores.Foreground = settings.InfoBrushForeground;
		}

		// sets up canvas, should be called once window is loaded fully
		// should make it so this method can only be called once
		public void Setup() {
			// in case recalled, reset the canvas
			ResetHard();

			// create information bar at top
			CreateInfoBar();

			// create battery textblocks
			CreateBatteryInfo();

			// create inbetween level textblocks
			CreateLevelInfo();

			// textblocks displaying highscores on screen
			CreateHighscoreInfo();

			// create terrain 
			CreateTerrain();

			// create/setup cursor shape
			CreateCursor();

			// hide the cursor shape
			ShowCursor(false);

			// set colors
			UpdateColors();
		}

		// creates information bar at top
		// does not check if textblocks exist already
		private void CreateInfoBar() {

			// cheat indicator block
			TBCheats = new TextBlock();
			//TBCheats.Background = settings.InfoBrushBackground;
			//TBCheats.Foreground = settings.InfoCheatBrushForeground;
			TBCheats.MaxWidth = 15;
			TBCheats.MaxHeight = settings.BoundaryTop;
			TBCheats.TextTrimming = TextTrimming.CharacterEllipsis;
			TBCheats.FontSize = settings.InfoTextSize;
			TBCheats.Text = "*";

			gameCanvas.Children.Add(TBCheats);
			Canvas.SetLeft(TBCheats, gameCanvas.ActualWidth * settings.InfoTitleCheatMulti);
			Canvas.SetTop(TBCheats, 0);

			// level text block
			TBLevel = new TextBlock();
			//TBLevel.Background = settings.InfoBrushBackground;
			//TBLevel.Foreground = settings.InfoBrushForeground;
			TBLevel.MaxWidth = 50;
			TBLevel.MaxHeight = settings.BoundaryTop;
			TBLevel.TextTrimming = TextTrimming.CharacterEllipsis;
			TBLevel.FontSize = settings.InfoTextSize;
			TBLevel.Text = "Level: ";

			gameCanvas.Children.Add(TBLevel);
			Canvas.SetLeft(TBLevel, gameCanvas.ActualWidth * settings.InfoTitleLevelMulti);
			Canvas.SetTop(TBLevel, 0);

			// level value block
			TBLevelValue = new TextBlock();
			//TBLevelValue.Background = settings.InfoBrushBackground;
			//TBLevelValue.Foreground = settings.InfoBrushForeground;
			TBLevelValue.MaxWidth = settings.InfoLevelWidth;
			TBLevelValue.MaxHeight = settings.BoundaryTop;
			TBLevelValue.TextTrimming = TextTrimming.CharacterEllipsis;
			TBLevelValue.FontSize = settings.InfoTextSize;
			TBLevelValue.TextAlignment = TextAlignment.Right;
			TBLevelValue.Text = "***";

			gameCanvas.Children.Add(TBLevelValue);
			Canvas.SetLeft(TBLevelValue, gameCanvas.ActualWidth * settings.InfoTextLevelMulti);
			Canvas.SetTop(TBLevelValue, 0);

			// current score value block
			TBScore = new TextBlock();
			//TBScore.Background = settings.InfoBrushBackground;
			//TBScore.Foreground = settings.InfoBrushForeground;
			TBScore.MaxWidth = settings.InfoScoreWidth;
			TBScore.MaxHeight = settings.BoundaryTop;
			TBScore.TextTrimming = TextTrimming.CharacterEllipsis;
			TBScore.FontSize = settings.InfoTextSize;
			TBScore.TextAlignment = TextAlignment.Left;
			TBScore.Text = "************";

			gameCanvas.Children.Add(TBScore);
			Canvas.SetLeft(TBScore, gameCanvas.ActualWidth * settings.InfoTextScoreMulti);
			Canvas.SetTop(TBScore, 0);

			// highscore value block
			TBHighscore = new TextBlock();
			//TBHighscore.Background = settings.InfoBrushBackground;
			//TBHighscore.Foreground = settings.InfoBrushForeground;
			TBHighscore.MaxWidth = settings.InfoScoreWidth;
			TBHighscore.MaxHeight = settings.BoundaryTop;
			TBHighscore.TextTrimming = TextTrimming.CharacterEllipsis;
			TBHighscore.FontSize = settings.InfoTextSize;
			TBHighscore.TextAlignment = TextAlignment.Right;
			TBHighscore.Text = "************";

			gameCanvas.Children.Add(TBHighscore);
			Canvas.SetLeft(TBHighscore, gameCanvas.ActualWidth * settings.InfoTextHighScoreMulti);
			Canvas.SetTop(TBHighscore, 0);
		}

		// creates textblocks below each battery to use
		private void CreateBatteryInfo() {
			// left battery textblock
			TBLeftBattery = new TextBlock();
			TBLeftBattery.MaxWidth = 35;
			TBLeftBattery.MaxHeight = settings.BoundaryTop;
			TBLeftBattery.FontSize = 12;
			TBLeftBattery.Text = "*****";

			gameCanvas.Children.Add(TBLeftBattery);
			Canvas.SetLeft(TBLeftBattery, gameCanvas.ActualWidth * settings.BatteryLeftMultiplier - TBLeftBattery.MaxWidth / 2);
			Canvas.SetTop(TBLeftBattery, gameCanvas.ActualHeight - settings.BatteryInfoHeight);
			// bring to front
			Canvas.SetZIndex(TBLeftBattery, 1);

			// right battery textblock
			TBCenterBattery = new TextBlock();
			TBCenterBattery.MaxWidth = 30;
			TBCenterBattery.MaxHeight = settings.BoundaryTop;
			TBCenterBattery.FontSize = 12;
			TBCenterBattery.Text = "*****";

			gameCanvas.Children.Add(TBCenterBattery);
			Canvas.SetLeft(TBCenterBattery, gameCanvas.ActualWidth * settings.BatteryCenterMultiplier - TBCenterBattery.MaxWidth / 2);
			Canvas.SetTop(TBCenterBattery, gameCanvas.ActualHeight - settings.BatteryInfoHeight);
			// bring to front
			Canvas.SetZIndex(TBCenterBattery, 1);

			// center battery textblock
			TBRightBattery = new TextBlock();
			TBRightBattery.MaxWidth = 35;
			TBRightBattery.MaxHeight = settings.BoundaryTop;
			TBRightBattery.FontSize = 12;
			TBRightBattery.Text = "*****";

			gameCanvas.Children.Add(TBRightBattery);
			Canvas.SetLeft(TBRightBattery, gameCanvas.ActualWidth * settings.BatteryRightMultiplier - TBRightBattery.MaxWidth / 2);
			Canvas.SetTop(TBRightBattery, gameCanvas.ActualHeight - settings.BatteryInfoHeight);
			// bring to front
			Canvas.SetZIndex(TBRightBattery, 1);
		}

		// creates textblocks for inbetween levels information
		private void CreateLevelInfo() {
			// Level status
			TBLevelStatus = new TextBlock();
			TBLevelStatus.MaxWidth = 200;
			TBLevelStatus.MaxHeight = settings.BoundaryTop;
			TBLevelStatus.FontSize = 12;
			TBLevelStatus.Text = "*****";

			gameCanvas.Children.Add(TBLevelStatus);
			Canvas.SetLeft(TBLevelStatus, gameCanvas.ActualWidth * 0.5 - TBLevelStatus.MaxWidth / 2);
			Canvas.SetTop(TBLevelStatus, gameCanvas.ActualHeight * 0.3);

			// amount of bonus cities
			TBLevelBonusCities = new TextBlock();
			TBLevelBonusCities.MaxWidth = 200;
			TBLevelBonusCities.MaxHeight = settings.BoundaryTop;
			TBLevelBonusCities.FontSize = 12;
			TBLevelBonusCities.Text = "*****";

			gameCanvas.Children.Add(TBLevelBonusCities);
			Canvas.SetLeft(TBLevelBonusCities, gameCanvas.ActualWidth * 0.5 - TBLevelBonusCities.MaxWidth / 2);
			Canvas.SetTop(TBLevelBonusCities, gameCanvas.ActualHeight * 0.4);

			// amount of missiles remaining
			TBLevelMissiles = new TextBlock();
			TBLevelMissiles.MaxWidth = 200;
			TBLevelMissiles.MaxHeight = settings.BoundaryTop;
			TBLevelMissiles.FontSize = 12;
			TBLevelMissiles.Text = "*****";

			gameCanvas.Children.Add(TBLevelMissiles);
			Canvas.SetLeft(TBLevelMissiles, gameCanvas.ActualWidth * 0.5 - TBLevelMissiles.MaxWidth / 2);
			Canvas.SetTop(TBLevelMissiles, gameCanvas.ActualHeight * 0.5);

			// amount of cities remaining
			TBLevelCities = new TextBlock();
			TBLevelCities.MaxWidth = 200;
			TBLevelCities.MaxHeight = settings.BoundaryTop;
			TBLevelCities.FontSize = 12;
			TBLevelCities.Text = "*****";

			gameCanvas.Children.Add(TBLevelCities);
			Canvas.SetLeft(TBLevelCities, gameCanvas.ActualWidth * 0.5 - TBLevelCities.MaxWidth / 2);
			Canvas.SetTop(TBLevelCities, gameCanvas.ActualHeight * 0.6);
		}

		// creates highscore textblock used
		private void CreateHighscoreInfo() {
			// TBHighscores
			TBHighscores = new TextBlock();
			TBHighscores.MaxWidth = 200;
			TBHighscores.MaxHeight = 300;
			TBHighscores.FontSize = 12;
			TBHighscores.Text = "*****";

			gameCanvas.Children.Add(TBHighscores);
			Canvas.SetLeft(TBHighscores, gameCanvas.ActualWidth * 0.1);
			Canvas.SetTop(TBHighscores, settings.BoundaryTop);
		}

		// updates info bar
		private void UpdateInfoBar() {
			// if level == 0, only display highscore block
			/*if (state.Level == 0) {
				TBCheats.Visibility = Visibility.Hidden;
				TBLevel.Visibility = Visibility.Hidden;
				TBLevelValue.Visibility = Visibility.Hidden;
				TBScore.Visibility = Visibility.Hidden;
			} else{*/
				// set cheats enabled block visibilty to if cheats are enabled
				TBCheats.Visibility = (state.CheatsEnabled) ? Visibility.Visible : Visibility.Hidden;
				// set other blocks to be visible
				/*TBLevel.Visibility = Visibility.Visible;
				TBLevelValue.Visibility = Visibility.Visible;
				TBScore.Visibility = Visibility.Visible;*/
				// set level/score/highscore values
				TBLevelValue.Text = string.Format("{0}", state.Level);
				TBScore.Text = string.Format("{0:0#,#}", state.Score);
				TBHighscore.Text = string.Format("{0:0#,#}", state.Highscore);
			//}
		}

		// updates textblocks below each battery
		// check if battery is destroyed/empty, then sets # of missiles available
		private void UpdateBatteryInfo() {
			// private TextBlock TBLeftBattery, TBCenterBattery, TBRightBattery;

			// left battery
			if (state.LeftBattery.Destroyed) {
				TBLeftBattery.Visibility = Visibility.Hidden;
			}else{
				TBLeftBattery.Visibility = Visibility.Visible;
				if (state.LeftBattery.Empty) {
					TBLeftBattery.Text = "OUT";
				}else{
					TBLeftBattery.Text = state.LeftBattery.MissileAvailable.ToString();
				}
			}
			// center battery
			if (state.CenterBattery.Destroyed) {
				TBCenterBattery.Visibility = Visibility.Hidden;
			} else {
				TBCenterBattery.Visibility = Visibility.Visible;
				if (state.CenterBattery.Empty) {
					TBCenterBattery.Text = "OUT";
				} else {
					TBCenterBattery.Text = state.CenterBattery.MissileAvailable.ToString();
				}
			}
			// right battery
			if (state.RightBattery.Destroyed) {
				TBRightBattery.Visibility = Visibility.Hidden;
			} else {
				TBRightBattery.Visibility = Visibility.Visible;
				if (state.RightBattery.Empty) {
					TBRightBattery.Text = "OUT";
				} else {
					TBRightBattery.Text = state.RightBattery.MissileAvailable.ToString();
				}
			}
		}

		// update level info textblocks
		private void UpdateLevelInfo() {
			// if level status not in between levels, hide blocks
			if (state.Status != GameLevelStatus.Between && state.Status != GameLevelStatus.Over) {
				TBLevelStatus.Visibility = Visibility.Hidden;
				TBLevelBonusCities.Visibility = Visibility.Hidden;
				TBLevelMissiles.Visibility = Visibility.Hidden;
				TBLevelCities.Visibility = Visibility.Hidden;
				return;
			}

			// make blocks visible
			TBLevelStatus.Visibility = Visibility.Visible;
			TBLevelBonusCities.Visibility = Visibility.Visible;
			TBLevelMissiles.Visibility = Visibility.Visible;
			TBLevelCities.Visibility = Visibility.Visible;

			// set block text
			if (state.Status == GameLevelStatus.Over) {
				TBLevelStatus.Text = "Game over";
			} else{
				TBLevelStatus.Text = "";
			}
			TBLevelBonusCities.Text = string.Format("Bonus Cities: {0}", state.BonusCities);
			TBLevelMissiles.Text = string.Format("Missiles: {0}", state.RemainingMissiles);
			TBLevelCities.Text = string.Format("Cities: {0}", state.RemainingCities);
		}

		// updates highscores block
		public void UpdateHighscoresInfo() {
			// if logic state is waiting, display highscores
			if (logic.Status == GameStatus.Waiting) {
				TBHighscores.Visibility = Visibility.Visible;
				// get text for highscores from logic
				TBHighscores.Text = logic.HighscoreText();
			} else{
				TBHighscores.Visibility = Visibility.Hidden;
			}
		}

		// create terrain
		// does not check if terrain shape exists
		private void CreateTerrain() {
			// create points for polygon shape
			PointCollection points = new PointCollection();

			// bottom left
			points.Add(new Point(0, gameCanvas.ActualHeight));
			// bottom right
			points.Add(new Point(gameCanvas.ActualWidth, gameCanvas.ActualHeight));
			// top right
			points.Add(new Point(gameCanvas.ActualWidth, gameCanvas.ActualHeight - settings.TerrainHeight));

			// right battery
			// right slope bottom
			points.Add(new Point(gameCanvas.ActualWidth * settings.BatteryRightMultiplier + settings.BatteryWidthBase / 2, gameCanvas.ActualHeight - settings.TerrainHeight));
			// right slope top
			points.Add(new Point(gameCanvas.ActualWidth * settings.BatteryRightMultiplier + settings.BatteryWidth / 2, gameCanvas.ActualHeight - settings.BatteryHeight));
			// left slope top
			points.Add(new Point(gameCanvas.ActualWidth * settings.BatteryRightMultiplier - settings.BatteryWidth / 2, gameCanvas.ActualHeight - settings.BatteryHeight));
			// left slop bottom
			points.Add(new Point(gameCanvas.ActualWidth * settings.BatteryRightMultiplier - settings.BatteryWidthBase / 2, gameCanvas.ActualHeight - settings.TerrainHeight));

			// center battery
			// right slope bottom
			points.Add(new Point(gameCanvas.ActualWidth * settings.BatteryCenterMultiplier + settings.BatteryWidthBase / 2, gameCanvas.ActualHeight - settings.TerrainHeight));
			// right slope top
			points.Add(new Point(gameCanvas.ActualWidth * settings.BatteryCenterMultiplier + settings.BatteryWidth / 2, gameCanvas.ActualHeight - settings.BatteryHeight));
			// left slope top
			points.Add(new Point(gameCanvas.ActualWidth * settings.BatteryCenterMultiplier - settings.BatteryWidth / 2, gameCanvas.ActualHeight - settings.BatteryHeight));
			// left slop bottom
			points.Add(new Point(gameCanvas.ActualWidth * settings.BatteryCenterMultiplier - settings.BatteryWidthBase / 2, gameCanvas.ActualHeight - settings.TerrainHeight));

			// left battery
			// right slope bottom
			points.Add(new Point(gameCanvas.ActualWidth * settings.BatteryLeftMultiplier + settings.BatteryWidthBase / 2, gameCanvas.ActualHeight - settings.TerrainHeight));
			// right slope top
			points.Add(new Point(gameCanvas.ActualWidth * settings.BatteryLeftMultiplier + settings.BatteryWidth / 2, gameCanvas.ActualHeight - settings.BatteryHeight));
			// left slope top
			points.Add(new Point(gameCanvas.ActualWidth * settings.BatteryLeftMultiplier - settings.BatteryWidth / 2, gameCanvas.ActualHeight - settings.BatteryHeight));
			// left slop bottom
			points.Add(new Point(gameCanvas.ActualWidth * settings.BatteryLeftMultiplier - settings.BatteryWidthBase / 2, gameCanvas.ActualHeight - settings.TerrainHeight));

			// top left
			points.Add(new Point(0, gameCanvas.ActualHeight - settings.TerrainHeight));
			// bottom left (to finish, may not be needed)
			points.Add(new Point(0, gameCanvas.ActualHeight));

			// create polygon for terrain
			Polygon _terrainShape = new Polygon();
			_terrainShape.Stroke = settings.BrushStrokeTerrain;
			_terrainShape.Fill = settings.BrushFillTerrain;
			_terrainShape.StrokeThickness = settings.BrushStrokeThickness;
			_terrainShape.Points = points;

			gameCanvas.Children.Add(_terrainShape);
			terrainShape = _terrainShape;
		}

		// create cursor shape
		// does not check if cursor shape exists already
		private void CreateCursor() {
			// make cursor image usable for game
			UpdateCursorImage();

			// create image brush to use
			ImageBrush _brush = new ImageBrush(cursorImage);

			// create a rectangle
			cursorShape = new Rectangle();
			cursorShape.Height = 16;
			cursorShape.Width = 16;
			cursorShape.Fill = _brush;

			// add to canvas
			gameCanvas.Children.Add(cursorShape);
		}

		// converts colors of the cursor image to be used with the game
		// whites (255,255,255,255), converted to match background color (from settings)
		// any other color, converted to match cursor color (from settings)
		// this could be broken up into smaller methods, but no need as they arn't reusable anywhere (yet)
		private void UpdateCursorImage() {
			// don't care if cursorImage has been set, not usable
			// create base image for cursor
			BitmapImage _baseImage = new BitmapImage(new Uri("pack://application:,,,/Resources/IconCursor.png", UriKind.Absolute));

			// big help
			// https://social.msdn.microsoft.com/Forums/vstudio/en-US/948fb3a1-473e-4952-9727-b7c586d0a427/get-pixels-from-a-bitmapsource-image-using-copypixels-method?forum=csharpgeneral
			// http://stackoverflow.com/questions/14161665/how-do-i-convert-a-writeablebitmap-object-to-a-bitmapimage-object-in-wpf

			// create editor for image
			WriteableBitmap _edit = new WriteableBitmap(_baseImage);

			// calculate stride
			// bits per pixel
			int _bipp = _edit.Format.BitsPerPixel;
			// bytes per pixel (rounds up to nearest whole byte count)
			int _bypp = (_bipp + 7) / 8;
			int _stride = _edit.PixelWidth * _bypp;

			// build the array for pixel bytes
			int _len = _stride * _edit.PixelHeight;
			byte[] _pixelBytes = new Byte[_len];	

			// copy out pixels
			_edit.CopyPixels(_pixelBytes, _stride, 0);

			// modify pixels
			// white color, to identify parts that should be transparent
			Color _colorWhite = Color.FromArgb(255, 255, 255, 255);
			// color of background
			Color _colorBackground = settings.BrushBackground.Color;
			// color of cursor image
			Color _colorCursor = settings.BrushCursor.Color;

			// loop through bytes
			// each pixel contains a set number of bytes (identified by _bypp)
			for (int _i = 0; _i < _len; _i += _bypp) {
				// get color structure (RGB) (.png is in BGR)
				// only care about first 3 bytes of _bypp section (4th needed for comparison)
				Color _color = new Color();
				_color.B = _pixelBytes[_i + 0];
				_color.G = _pixelBytes[_i + 1];
				_color.R = _pixelBytes[_i + 2];
				// pointless (if using 24bitpp BGR image), but needed for color comparison
				_color.A = _pixelBytes[_i + 3];

				// determine if pixel color should be background or cursor
				// Color.Equals does not work so well...
				if (_color.Equals(_colorWhite)) {
					_color = _colorBackground;
				} else{
					_color = _colorCursor;
				}

				// rewrite color back
				// png is in BGR, so backwards
				_pixelBytes[_i + 0] = _color.B;
				_pixelBytes[_i + 1] = _color.G;
				_pixelBytes[_i + 2] = _color.R;
				_pixelBytes[_i + 3] = _color.A;
			}

			// to loop through bytes with stride in mind
			// i: 0 -> pixel height
			// j: 0 -> stride
			// index: i*stride + j

			// create a rect for writing pixels
			Int32Rect _rect = new Int32Rect(0, 0, _edit.PixelWidth, _edit.PixelHeight);
			// write pixels back
			_edit.WritePixels(_rect, _pixelBytes, _stride, 0);

			// create new bitmap image to write to
			BitmapImage _nimage = new BitmapImage();

			// encode it back
			using (MemoryStream _stream = new MemoryStream()) {
				PngBitmapEncoder _enc = new PngBitmapEncoder();
				_enc.Frames.Add(BitmapFrame.Create(_edit));
				_enc.Save(_stream);
				_nimage.BeginInit();
				_nimage.CacheOption = BitmapCacheOption.OnLoad;
				_nimage.StreamSource = _stream;
				_nimage.EndInit();
				_nimage.Freeze();
			}

			// set cursor image to new image
			cursorImage = _nimage;
		}
	}
}
