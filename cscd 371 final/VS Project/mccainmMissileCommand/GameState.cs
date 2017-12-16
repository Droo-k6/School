// Mathew McCain
// cscd371 final missile command
// GameState class, holds information about current game state

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace mccainmMissileCommand {
	// delegate for setting a new score
	// used between GameLogic and GameState
	delegate void NewScoreHandler(int _score);

	// delegate for projectile/explosion update (add/remove)
	// only updates shapes
	// useless now that state holds a reference to the canvas
	delegate void UpdateShapesHandler(List<Shape> _shapes);

	// enum for level status
	// None, no level
	// Active, level in progress
	// Between, between levels
	// Over, ended
	enum GameLevelStatus { None, Active, Between, Over }

	// Holds information about gamestate (such as current projectiles, explosions, settings)
	// used primarily between GameLogic and GameDraw
	// lots of areas to cleanup, was initially going to be a container class - then changed to advance/handle the state of the game
	// became really messy then
	// should just be merged back into GameLogic
	class GameState {
		// members
		// settings  obj
		private GameSettings settings;
		// game canvas
		private Canvas gameCanvas;
		// sound player
		private SoundPlayer soundPlayer;
		// game time (in fractions of a second)
		private double gameTime;
		// list of projectiles
		private List<Missile> projectiles;
		// list of exposions
		private List<Explosion> explosions;

		// missile batteries
		private Battery leftBattery, centerBattery, rightBattery;

		// cities
		private List<City> cities;

		// current list of targets (todo: strip out from GameState/GameDraw, useless now)
		private List<ITarget> targets;
		// current level targets
		private List<ITarget> levelTargets;
		// random object to use
		private Random rand;
		// level status
		private GameLevelStatus status;
		// current level, score
		private int level, score;
		// remaining score to next bonus city
		// should be rework to score of last bonus city, otherwise have to update remainingScore everytime score is updated
		private int remainingScore;
		// number of bonus cities
		private int bonusCities;
		// current level time, time of last check of level state
		private double levelTime, levelCheckTime;
		// current score multiplier, 
		private int scoreMultiplier;
		// levels remaining before increasing multiplier
		private int levelsRemaining;
		// total # of incoming missiles for level
		private int incomingMissilesTotal;
		// # of incoming missiles
		private int incomingMissiles;
		// # of incoming missiles visible
		private int incomingMissilesUp;
		// incoming missile speed
		private double incomingMissileSpeed;
		// chance of initial splitting of incoming missile
		private double incomingMissleSplitChance;
		// # of missiles in a wave (missile spawn/volley)
		private int incomingMissileWaveMax;

		// event for adding a new score
		public event NewScoreHandler newScore;
		// events for updating shapes (for GameDraw)
		// public for now
		// event for adding shapes
		public event UpdateShapesHandler addShapes;
		// event for removing shapes
		public event UpdateShapesHandler removeShapes;

		// constructor
		public GameState(GameSettings _settings, Canvas _canvas, SoundPlayer _soundPlayer) {
			rand = new Random();
			settings = _settings;
			gameCanvas = _canvas;
			soundPlayer = _soundPlayer;
			// init values
			gameTime = 0;
			status = GameLevelStatus.None;
			level = 0;
			score = 0;
			CheatsEnabled = false;
			Highscore = 0;

			// create lists for projectile/explosion objects
			projectiles = new List<Missile>();
			explosions = new List<Explosion>();
			// list of targets
			targets = new List<ITarget>();
			// list of targets for a level
			levelTargets = new List<ITarget>();
		}

		// sets up battery/city positions according to given values
		// does not check if recalled
		public void Setup(double _canvasHeight, double _canvasWidth) {
			// setup missile batteries

			// determine points for batteries
			Point leftPoint = new Point(_canvasWidth * settings.BatteryLeftMultiplier, _canvasHeight - settings.BatteryHeight);
			Point centerPoint = new Point(_canvasWidth * settings.BatteryCenterMultiplier, _canvasHeight - settings.BatteryHeight);
			Point rightPoint = new Point(_canvasWidth * settings.BatteryRightMultiplier, _canvasHeight - settings.BatteryHeight);

			// create the batteries
			leftBattery = new Battery(settings, BatteryType.Side, leftPoint);
			centerBattery = new Battery(settings, BatteryType.Center, centerPoint);
			rightBattery = new Battery(settings, BatteryType.Side, rightPoint);

			// determine points for cities
			Point city1Point = new Point(_canvasWidth * settings.City1Multiplier, _canvasHeight - settings.TerrainHeight);
			Point city2Point = new Point(_canvasWidth * settings.City2Multiplier, _canvasHeight - settings.TerrainHeight);
			Point city3Point = new Point(_canvasWidth * settings.City3Multiplier, _canvasHeight - settings.TerrainHeight);
			Point city4Point = new Point(_canvasWidth * settings.City4Multiplier, _canvasHeight - settings.TerrainHeight);
			Point city5Point = new Point(_canvasWidth * settings.City5Multiplier, _canvasHeight - settings.TerrainHeight);
			Point city6Point = new Point(_canvasWidth * settings.City6Multiplier, _canvasHeight - settings.TerrainHeight);

			// create cities
			List<City> _cities = new List<City>();
			_cities.Add(new City(settings, city1Point));
			_cities.Add(new City(settings, city2Point));
			_cities.Add(new City(settings, city3Point));
			_cities.Add(new City(settings, city4Point));
			_cities.Add(new City(settings, city5Point));
			_cities.Add(new City(settings, city6Point));

			cities = _cities;
		}

		// properties

		// get list of projectiles (read only)
		public IReadOnlyCollection<Missile> ProjectilesList {
			get {
				return projectiles.AsReadOnly();
			}
		}
		// get list of explosions (read only)
		public IReadOnlyCollection<Explosion> ExplosionsList {
			get {
				return explosions.AsReadOnly();
			}
		}

		// get a missile battery
		public Battery LeftBattery { get { return leftBattery; } }
		public Battery CenterBattery { get { return centerBattery; } }
		public Battery RightBattery { get { return rightBattery; } }

		// get list of targets
		public List<ITarget> Targets { get { return targets; } }

		// get status of level
		public GameLevelStatus Status { get { return status; } }
		// if cheats are enabled or not
		public bool CheatsEnabled { set; get; }
		// current highscore
		public int Highscore { set; get; }
		// current level/score, not directly modifable from outside, state determines itself
		public int Level { get { return level; } }
		public int Score { get { return score; } }
		// get remaining missiles/cities at end of level
		public int RemainingMissiles {
			get {
				int _remaining = 0;
				if (!leftBattery.Destroyed)
					_remaining += leftBattery.MissileAvailable;
				if (!centerBattery.Destroyed)
					_remaining += centerBattery.MissileAvailable;
				if (!rightBattery.Destroyed)
					_remaining += rightBattery.MissileAvailable;
				return _remaining;
			}
		}
		public int RemainingCities {
			get {
				int _remaining = 0;
				foreach (City _c in cities) {
					if (!_c.Destroyed)
						_remaining++;
				}
				return _remaining;
			}
		}
		// get # of bonus cities
		public int BonusCities { get { return bonusCities; } }

		// add a projectile
		public void AddProjectile(Missile _p) {
			projectiles.Add(_p);
			addShapes(_p.Shapes);
		}

		// remove a projectile
		/*public void RemoveProjectile(Projectile _p) {
			projectiles.Remove(_p);
			removeShapes(_p.Shapes);
		}*/

		// add an explosion
		public void AddExplosion(Explosion _e) {
			// play sound
			soundPlayer.Play();

			explosions.Add(_e);
			addShapes(_e.Shapes);
		}

		// remove an explosion
		/*public void RemoveExplosion(Explosion _e) {
			explosions.Remove(_e);
			removeShapes(_e.Shapes);
		}*/
		

		// advance game state by amount of time
		public void Advance(double dt) {
			// advance each projectile
			foreach (Missile _p in projectiles) {
				//Debug.WriteLine(string.Format("{0} advanced", _p));
				_p.Advance(dt);
			}
			// advance each explosion
			foreach (Explosion _e in explosions) {
				_e.Advance(dt);
			}

			// collision checks
			foreach (Missile _p in projectiles) {
				// only check if not destroyed
				// and if projectile is capable of collision
				if (!_p.Destroyed && _p.Collidable) {
					// check against each explosion
					foreach (Explosion _e in explosions) {
						// check if projectile collided
						if (_p.Collided(_e)) {
							// break inner loop
							// destroyed check below will handle cleanup
							break;
						}
					}
				}
			}


			// not using foreach/list.RemoveAll(_p => _p.destroyed), etc
			// because would be too inoptimal for what is needed

			// list to gather shapes to remove
			List<Shape> _toRemove = new List<Shape>();

			// check for destroyed projectiles
			// loop backwards to handle index shifting from RemoveAt
			for (int _i = projectiles.Count - 1; _i >= 0; --_i) {
				Missile _p = projectiles[_i];

				if (_p.Destroyed) {
					projectiles.RemoveAt(_i);
					_toRemove.AddRange(_p.Shapes);

					// add explosion object from projectile if exists
					Explosion _e = _p.ExplosionObj;
					if (_e != null) {
						AddExplosion(_e);
					}

					// if was an IPBM, decrement incomingMissilesUp
					if (_p is IPBMissle) {
						//--incomingMissilesUp;
						// shouldn't go negative but just in case
						incomingMissilesUp = Math.Max(incomingMissilesUp - 1, 0);
						// if destroyed reason collided, add score
						if (_p.DestroyedType == MissileDestroyedType.Collided) {
							int _scoreAdd = settings.ScoreEnemyMissile * scoreMultiplier;
							score += _scoreAdd;
							remainingScore -= _scoreAdd;
						}
					}
				}
			}

			// check for inactive explosions
			for (int _i = explosions.Count - 1; _i >= 0; --_i) {
				Explosion _e = explosions[_i];
				if (!_e.Active) {
					explosions.RemoveAt(_i);
					_toRemove.Add(_e.ExplosionShape);
				}
			}

			// check for destroyed targets
			for (int _i = targets.Count - 1; _i >= 0; --_i) {
				ITarget _t = targets[_i];
				if (_t.Destroyed) {
					targets.RemoveAt(_i);
					_toRemove.Add(_t.DisplayShape);
				}
			}

			// call remove shapes event
			removeShapes(_toRemove);
			
			// advance game time
			gameTime += dt;

			// check level
			CheckLevel(dt);
		}

		// checks state of level
		private void CheckLevel(double dt) {
			// check level sdtatus
			if (status == GameLevelStatus.Active) {
				// level in progress
				// advance level time
				levelTime += dt;

				// check if time to check level
				if ((levelTime - levelCheckTime) > settings.LevelCheckTimeInterval) {
					// set level check time
					levelCheckTime = levelTime;

					// if any of enemy missiles remaining
					bool _incomingMissilesRemain = incomingMissiles > 0;
					// if any incoming missiles are visible
					//bool _incomingMissilesVisible = incomingMissilesUp > 0;
					// if any missiles in batteries remain
					//bool _batteryMissilesRemain = (!leftBattery.Empty || !centerBattery.Empty || !rightBattery.Empty);
					// if any of the level targets remain
					bool _targetsRemain = levelTargets.Count > 0;
					// if any projectiles are flying
					bool _projectilesAlive = projectiles.Count > 0;

					Debug.WriteLine(string.Format("incoming: {0}, targets: {1}, projectiles: {2}", _incomingMissilesRemain, _targetsRemain, _projectilesAlive));

					// determine what to do
					if ((!_targetsRemain || !_incomingMissilesRemain) && !_projectilesAlive) {
						// if no targets remaining - or no missiles remaining, no projectiles flying, end level
						EndLevel();
					}else if (_targetsRemain && _incomingMissilesRemain && (incomingMissilesUp < incomingMissileWaveMax)) {
						// if targets remain and missiles available (but missiles up is under wave max), spawn a wave
						SpawnWave();
					}
				}
			} else if (status == GameLevelStatus.Between) {
				// in between levels
				// advance level time
				levelTime += dt;

				// if time between levels over, proceed to next level
				if ((levelTime - levelCheckTime) > settings.BetweenLevelTime) {
					NextLevel();
				}
			}
		}

		// spawns a wave/volley of incoming missiles
		private void SpawnWave() {
			// determine amount to spawn
			int _amount = rand.Next(incomingMissileWaveMax - incomingMissilesUp) + 1;
			// ensure amount spawned isn't larger than remaining incoming missiles
			_amount = Math.Min(_amount, incomingMissiles);

			// decrement amount from incoming missile count
			incomingMissiles -= _amount;

			// spawn
			for (int _i=0; _i < _amount; ++_i) {
				// determine spawn position
				Point _spawn = new Point(rand.NextDouble() * gameCanvas.ActualWidth, settings.IncomingMissileSpawnHeight);

				// determine target
				ITarget _target = levelTargets[rand.Next(levelTargets.Count)];

				Missile _missile = new IPBMissle(settings, _spawn, incomingMissileSpeed, _target);

				// add missile
				AddProjectile(_missile);

				++incomingMissilesUp;
			}
		}

		// transitions to next level
		// calculates bonus from remaining cities/missiles
		private void EndLevel() {
			Debug.WriteLine("EndLevel()");

			// determine remaining missiles
			int _remainingLevelMissiles = RemainingMissiles;

			// determine remaining cities
			int _remainingLevelCities = RemainingCities;

			// determine bonus scores
			int _cityBonus = _remainingLevelCities * settings.ScoreSavedCities * scoreMultiplier;
			int _missileBonus = _remainingLevelMissiles * settings.ScoreUnusedMissile * scoreMultiplier;
			// subtract from remaining score
			remainingScore -= _cityBonus + _missileBonus;
			// add to score
			score += _cityBonus + _missileBonus;

			// check for earned bonus cities
			// if remaining score <= 0, add bonus cities
			if (remainingScore <= 0) {
				int _score = Math.Abs(remainingScore) + settings.ScoreBonusCity;
				// determine bonus cities
				bonusCities += (_score - (_score % settings.ScoreBonusCity)) / settings.ScoreBonusCity;
				// determine new remaining score
				remainingScore = settings.ScoreBonusCity - (_score % settings.ScoreBonusCity);
			}

			// check if game over
			bool gameOver = true;
			// check if all cities destroyed
			foreach (City _c in cities) {
				if (!_c.Destroyed) {
					gameOver = false;
					break;
				}
			}
			if (gameOver) {
				// check if bonus cities available
				gameOver = bonusCities <= 0;
			}

			// transition to next status
			if (gameOver) {
				status = GameLevelStatus.Over;
				// call event to add new score
				newScore(score);
			}else{
				status = GameLevelStatus.Between;
			}
		}

		// Reset game state
		// assumes call is from Logic, resets state end
		// calls event on GameDraw to remove appropriate shapes
		public void Reset() {
			// reset score
			CheatsEnabled = false;
			level = 0;
			score = 0;
			//Highscore = 0;

			// reset game time
			gameTime = 0;

			status = GameLevelStatus.None;

			// shapes to remove
			List<Shape> _toRemove = new List<Shape>();

			// get projectile shapes
			foreach(Missile _p in projectiles) {
				_toRemove.AddRange(_p.Shapes);
			}
			// get explosion shapes
			foreach(Explosion _e in explosions) {
				_toRemove.AddRange(_e.Shapes);
			}
			// get battery shapes
			_toRemove.Add(leftBattery.DisplayShape);
			_toRemove.Add(centerBattery.DisplayShape);
			_toRemove.Add(rightBattery.DisplayShape);
			// get city shapes
			foreach(City _c in cities) {
				_toRemove.Add(_c.DisplayShape);
			}

			// call event to remove shapes
			removeShapes(_toRemove);

			// reset projectiles
			projectiles.Clear();

			// reset explosions
			explosions.Clear();

			// reset batteries
			leftBattery.Reset();
			centerBattery.Reset();
			rightBattery.Reset();

			// reset cities
			foreach (City _c in cities) {
				_c.Reset();
			}

			// wipe target list
			targets.Clear();
		}

		// set state to a new game
		public void Start() {
			// reset just in case
			Reset();

			// initial values
			status = GameLevelStatus.Active;
			levelTime = 0;
			levelCheckTime = 0;
			level = 1;
			score = 0;
			scoreMultiplier = 1;
			remainingScore = settings.ScoreBonusCity;
			//bonusCities = 0;
			bonusCities = (settings.InitialCities > 6) ? (settings.InitialCities - 6) : 0;
			levelsRemaining = settings.ScoreMultiplierLevelStep;
			incomingMissilesTotal = settings.IncomingMissileCountInitial;
			incomingMissiles = incomingMissilesTotal;
			incomingMissilesUp = 0;
			incomingMissileSpeed = (settings.MissileSpeedUp) ? settings.IncomingMissileSpeedInitial : settings.IncomingMissileSpeedStatic;
			incomingMissleSplitChance = settings.MissileSplitChanceInitial;
			incomingMissileWaveMax = settings.WaveMissileCountInitial;

			// if settings.InitialCities < 6, destroy some cities
			if (settings.InitialCities < 6) {
				int _toDestroy = 6 - settings.InitialCities;
				while (_toDestroy > 0) {
					// destroy a random city
					City _c = cities[rand.Next(cities.Count)];
					// very bad workaround to just building a list
					// or making a static method to grab a random index from a list
					if (_c.Destroyed)
						continue;
					_c.Impacted(null);
					_toDestroy--;
				}
			}

			// build target list
			targets.Add(leftBattery);
			targets.Add(centerBattery);
			targets.Add(rightBattery);
			foreach (City _c in cities) {
				if (!_c.Destroyed)
					targets.Add(_c);
			}

			// build level targets list
			BuildLevelTargets();

			// add target shapes
			List<Shape> _toAdd = new List<Shape>();
			foreach(ITarget _target in targets) {
				_toAdd.Add(_target.DisplayShape);
			}
			addShapes(_toAdd);

			// set color palette
		}

		// advance level to next
		private void NextLevel() {
			Debug.WriteLine("NextLevel()");

			// reset level time
			levelTime = 0;
			levelCheckTime = 0;
			status = GameLevelStatus.Active;

			// check if level is at most
			if (level >= settings.MaxLevel) {
				// reset values back to initial
				level = 1;
				scoreMultiplier = 1;
				levelsRemaining = settings.ScoreMultiplierLevelStep;
				incomingMissilesTotal = settings.IncomingMissileCountInitial;
				incomingMissiles = incomingMissilesTotal;
				incomingMissilesUp = 0;
				incomingMissileSpeed = (settings.MissileSpeedUp) ? settings.IncomingMissileSpeedInitial : settings.IncomingMissileSpeedStatic;
				incomingMissleSplitChance = settings.MissileSplitChanceInitial;
				incomingMissileWaveMax = settings.WaveMissileCountInitial;

				// set color palette

				return;
			}

			// up level
			++level;
			// check if time to increase score multiplier
			if (levelsRemaining <= 0) {
				// increase score multiplier, limit to multiplier max
				scoreMultiplier = Math.Min(scoreMultiplier+1, settings.ScoreMultiplierMax);
				// reset levels remaining
				levelsRemaining = settings.ScoreMultiplierLevelStep;
			}else{
				--levelsRemaining;
			}

			// up missile count
			incomingMissilesTotal = Math.Min(incomingMissilesTotal + settings.IncomingMissileStep, settings.IncomingMissileCountMax);
			incomingMissiles = incomingMissilesTotal;
			incomingMissilesUp = 0;

			// check if should up missile speed
			if (settings.MissileSpeedUp) {
				// up missile speed
				incomingMissileSpeed = Math.Min(incomingMissileSpeed + settings.IncomingMissileSpeedStep, settings.IncomingMissileSpeedMax);
			}

			// up missile split chance
			incomingMissleSplitChance = Math.Min(incomingMissleSplitChance + settings.MissileSplitChanceStep, settings.MissileSplitChanceMax);
			// up missile wave max
			incomingMissileWaveMax = Math.Min(incomingMissileWaveMax + settings.WaveMissileCountStep, settings.WaveMissileCountMax);

			// replenish cities if possible (from bonusCities)
			ReplenishCities();

			// readd battery shapes if destroyed
			List<Shape> _toAdd = new List<Shape>();
			if (leftBattery.Destroyed)
				_toAdd.Add(leftBattery.DisplayShape);
			if (centerBattery.Destroyed)
				_toAdd.Add(centerBattery.DisplayShape);
			if (rightBattery.Destroyed)
				_toAdd.Add(rightBattery.DisplayShape);
			addShapes(_toAdd);

			// reset batteries
			leftBattery.Reset();
			centerBattery.Reset();
			rightBattery.Reset();

			// rebuild targets list
			targets.Clear();
			targets.Add(leftBattery);
			targets.Add(centerBattery);
			targets.Add(rightBattery);
			foreach (City _c in cities) {
				if (!_c.Destroyed)
					targets.Add(_c);
			}

			// determine level targets
			BuildLevelTargets();

			// determine if time to change color pallete
			// advance color palette
		}

		// resets cities if any available from bonusCities
		private void ReplenishCities() {
			// check if bonus cities are available
			if (bonusCities <= 0)
				return;

			// determine which cities are destroyed
			List<City> _destroyedCities = new List<City>();
			foreach(City _c in cities) {
				if (_c.Destroyed)
					_destroyedCities.Add(_c);
			}

			// check if any cities are destroyed
			if (_destroyedCities.Count <= 0)
				return;

			// list of shapes to readd
			List<Shape> _toAdd = new List<Shape>();

			// randomly reset a city until bonusCities depleted or no destroyed cities
			while((_destroyedCities.Count > 0) && (bonusCities > 0)) {
				// select random city
				City _c = _destroyedCities[rand.Next(_destroyedCities.Count)];
				// call reset on city, readd shape to GameDraw
				_c.Reset();
				_toAdd.Add(_c.DisplayShape);
				// remove from temp list, decrement bonus cities count
				_destroyedCities.Remove(_c);
				--bonusCities;
			}

			// readd any shapes back to canvas
			addShapes(_toAdd);
		}

		// determines levelTargets list from targets list
		private void BuildLevelTargets() {
			// clear list
			levelTargets.Clear();

			// add all batteries
			levelTargets.Add(leftBattery);
			levelTargets.Add(centerBattery);
			levelTargets.Add(rightBattery);

			// build list of potential city targets
			List<City> _potentialCities = new List<City>();
			foreach(City _c in cities) {
				if (!_c.Destroyed)
					_potentialCities.Add(_c);
			}

			// how many cities to add at most
			int _remainingCities = 3;
			// determine which 3 of remaining cities to target
			while ((_potentialCities.Count > 0) && (_remainingCities > 0)) {
				// select a random city
				City _c = _potentialCities[rand.Next(_potentialCities.Count)];
				levelTargets.Add(_c);
				_potentialCities.Remove(_c);
				--_remainingCities;
			}
		}
	}
}
