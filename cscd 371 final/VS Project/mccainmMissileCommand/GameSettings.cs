// Mathew McCain
// cscd371 final missile command
// class for holding game settings
// just used for data storage/retrieval between several game classes
// mainly used to avoid having to go between a bunch of files to edit trivial values

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace mccainmMissileCommand {
	// settings class (could be a struct)
	// holds various values/settings for the game
	// used by both GameLogic and GameDraw
	// used as central point so modifying trivial values doesn't mean going to a bunch of different files
	public class GameSettings {
		// constructor, does nothing
		public GameSettings() { }

		// constants
		// game refresh rate in Hz
		// used for logical and display frames
		public readonly int TickRate = 60;

		// boundary at top, reserved for displaying information
		public readonly double BoundaryTop = 30;
		// boundary at bottom, contains terrain and lowest point of firing anti-ballistic missiles
		public readonly double BoundaryBottom = 50;

		// info bar values
		// x cordinate multipliers
		// cheat indicator textblock
		public readonly double InfoTitleCheatMulti = 0;
		// level textblock
		public readonly double InfoTitleLevelMulti = 0.05;
		// level value textblock
		public readonly double InfoTextLevelMulti = 0.17;
		// current score
		public readonly double InfoTextScoreMulti = 0.4;
		// highscore
		public readonly double InfoTextHighScoreMulti = 0.7;
		// max pixel width of textblocks
		// level value
		public readonly int InfoLevelWidth = 40;
		// score/highscore values
		public readonly int InfoScoreWidth = 120;
		// font size
		public readonly int InfoTextSize = 20;
		// height of battery info text blocks
		public readonly double BatteryInfoHeight = 20;

		// battery x cordinate multipliers (against canvas width)
		// 1/10th of canvas width
		public readonly double BatteryLeftMultiplier = 1.0 / 10.0;
		// 1/2 of canvas width
		public readonly double BatteryCenterMultiplier = 0.5;
		// 9/10th of canvas width
		public readonly double BatteryRightMultiplier = 9.0 / 10.0;

		// city x cordinate multipliers
		public readonly double City1Multiplier = 2.0 / 10.0;
		public readonly double City2Multiplier = 3.0 / 10.0;
		public readonly double City3Multiplier = 4.0 / 10.0;
		public readonly double City4Multiplier = 6.0 / 10.0;
		public readonly double City5Multiplier = 7.0 / 10.0;
		public readonly double City6Multiplier = 8.0 / 10.0;

		// height for battery placement
		public readonly double BatteryHeight = 30;
		// height of terrain
		public readonly double TerrainHeight = 20;
		// width of battery terrain
		public readonly double BatteryWidth = 25;
		// distance to start ramping up for battery terrain
		public readonly double BatteryWidthBase = 40;
		// height of battery indicator text
		public readonly double BatteryTextHeight = 20;

		// base missile battery capacity
		public readonly int BaseBatteryCapacity = 10;
		// base missile battery speed
		public readonly double BaseBatterySpeed = 100.0;
		// center missile battery speed
		public readonly double CenterBatterySpeed = 180.0;
		// max amount of missiles to display
		public readonly int MaxMissileDisplay = 10;

		// brush stroke thickness for terrain
		public readonly double BrushStrokeThickness = 1.0;

		// brush thickness for missile trails
		public readonly double BrushThicknessMissileTrail = 0.5;

		// in air explosion characteristics
		public readonly double AirExplosionInitialSize = 5;
		public readonly double AirExplosionMaxSize = 30;
		public readonly double AirExplosionSpeed = 50;
		public readonly double AirExplosionDuration = 0.2;
		public readonly SolidColorBrush BrushStrokeAirExplosion = Brushes.DeepSkyBlue;
		public readonly SolidColorBrush BrushFillAirExplosion = Brushes.Cyan;
		// impact explosion characteristics
		public readonly double ImpactExplosionInitialSize = 5;
		public readonly double ImpacExplosionMaxSize = 55;
		public readonly double ImpacExplosionSpeed = 90;
		public readonly double ImpacExplosionDuration = 1.2;
		public readonly SolidColorBrush BrushStrokeImpactExplosion = Brushes.PaleVioletRed;
		public readonly SolidColorBrush BrushFillImpactExplosion = Brushes.DarkOrange;

		// scores
		// enemy missile
		public readonly int ScoreEnemyMissile = 25;
		// unused missiles
		public readonly int ScoreUnusedMissile = 5;
		// remaining cities
		public readonly int ScoreSavedCities = 100;
		// how many levels to pass to increase score multiplier
		public readonly int ScoreMultiplierLevelStep = 2;
		// score multiplier max
		public readonly int ScoreMultiplierMax = 6;
		// how many points to earn to earn a bonus city
		public readonly int ScoreBonusCity = 4000;


		// level/missile values

		// max level to reach, resets to level 1 afterwards
		public readonly int MaxLevel = 99;

		// time interval to check level state (fractions of a second)
		// if too low, spawning can get crazy
		public readonly double LevelCheckTimeInterval = 2.0;
		// time between levels (to display scores) (fractions of a second)
		public readonly double BetweenLevelTime = 4.0;

		// waves (seperate from levels, the "volleys" of missiles that appear
		// initial missile count for waves
		public readonly int WaveMissileCountInitial = 4;
		// max amount of missiles for a wave
		public readonly int WaveMissileCountMax = 6;
		// wave missile count stepping (per level) (incremental)
		public readonly int WaveMissileCountStep = 1;

		// initial missile count
		public readonly int IncomingMissileCountInitial = 10;
		// max missile count
		public readonly int IncomingMissileCountMax = 25;
		// missile count increase per level
		public readonly int IncomingMissileStep = 1;

		// spawn height of incoming issiles
		public readonly double IncomingMissileSpawnHeight = 35;
		// initial enemy missile speed
		public readonly double IncomingMissileSpeedInitial = 10;
		// max enemy missile speed
		public readonly double IncomingMissileSpeedMax = 60;
		// enemy missile speed stepping (incremental increase)
		public readonly double IncomingMissileSpeedStep = 3;
		// incoming missile speed if no speed stepping
		public readonly double IncomingMissileSpeedStatic = 25;

		// missile splitting (MIRV) values
		// note: missile splitting not implemented
		// distance a missile must travel to roll for split chance
		public readonly double MissileSplitCheckDistance = 20;
		// lowest height that a missile can split from
		public readonly double MissileSplitCheckLowestHeight = 90;
		// initial chance of a missile split
		public readonly double MissileSplitChanceInitial = 0.05;
		// max chance of a missile split
		public readonly double MissileSplitChanceMax = 0.1;
		// how much to increase chance by per level
		public readonly double MissileSplitChanceStep = 0.01;


		// highscores file name
		public readonly string FilenameHighscores = "highscores.txt";



		// properties for modifiable fields
		// used for change in color palette between sets of levels
		// note: not implemented

		// color of canvas (background)
		public SolidColorBrush BrushBackground { set; get; } = Brushes.Black;
		// color of cursor (non white pixels in image)
		public SolidColorBrush BrushCursor { set; get; } = Brushes.Red;

		// stroke color for terrain (outline)
		public SolidColorBrush BrushStrokeTerrain { set; get; } = Brushes.LightGoldenrodYellow;
		// fill color for terrain
		public SolidColorBrush BrushFillTerrain { set; get; } = Brushes.SandyBrown;

		// fill color for city
		public SolidColorBrush BrushFillCity { set; get; } = Brushes.CadetBlue;

		// missile battery, missile colors
		// fill brush
		public SolidColorBrush BrushFillBattery { set; get; } = Brushes.White;

		// background color for textblocks
		public SolidColorBrush InfoBrushBackground { set; get; } = Brushes.Transparent;
		// color of textblock text
		public SolidColorBrush InfoBrushForeground { set; get; } = Brushes.Purple;
		// color of cheat indicator text
		public SolidColorBrush InfoCheatBrushForeground { set; get; } = Brushes.PaleGoldenrod;


		// friendly missiles (AB)
		// head color
		public SolidColorBrush BrushABMissileHead { set; get; } = Brushes.White;
		// trail color
		public SolidColorBrush BrushABMissileTail { set; get; } = Brushes.OrangeRed;

		// enemy missiles (IPBM)
		// head color
		public SolidColorBrush BrushIPBMissileHead { set; get; } = Brushes.WhiteSmoke;
		// trail color
		public SolidColorBrush BrushIPBMissileTail { set; get; } = Brushes.MediumPurple;


		// for settings window
		// number of missiles/battery
		// same as BaseBatteryCapacity field
		public int MissilesPerBattery { get; set; } = 10;
		// initial number of total cities
		public int InitialCities { get; set; } = 6;
		// if missiles should speed up after each level
		public bool MissileSpeedUp { get; set; } = true;
	}
}
