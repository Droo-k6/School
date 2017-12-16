// Mathew McCain
// cscd371 final missile command
// contains misc/smaller classes for the game

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Diagnostics;

namespace mccainmMissileCommand {
	// target interface
	// used for batteries and cities
	interface ITarget {
		// get center point
		Point CenterPoint { get; }
		// get display point
		Point DisplayPoint { get; }
		// get display shape
		Shape DisplayShape { get; }
		// set/get destroyed
		bool Destroyed { get; }
		// alert has been impacted by projectile
		void Impacted(Missile _p);
		// reset state
		void Reset();
	}

	// enum for battery type
	enum BatteryType { Center, Side }

	// class for a missile battery
	class Battery : ITarget {
		// constructor
		// takes settings obj to use, type of battery and location
		public Battery(GameSettings _settings, BatteryType _type, Point _point) {
			settings = _settings;
			position = _point;
			type = _type;

			// check type to determine missile speed
			if (type == BatteryType.Center) {
				baseSpeed = settings.CenterBatterySpeed;
			}else{
				baseSpeed = settings.BaseBatterySpeed;
			}

			// setup initial values
			//missileCap = settings.BaseBatteryCapacity;
			missileCap = settings.MissilesPerBattery;
			missilesAvailable = missileCap;
			destroyed = false;

			batteryShape = CreateBatteryShape();
			UpdateBatteryShape(batteryShape);

			// create shapes list
			//shapes = new List<Shape>();

			/*
			// populate with max amount of missiles to display
			for (int i = 0; i < settings.MaxMissileDisplay; ++i) {
				// create shape
				Shape _shape = CreateBatteryShape();
				// edit shape to match settings
				UpdateBatteryShape(_shape);

				shapes.Add(_shape);
			}
			*/
		}

		// member fields
		// settings obj
		private GameSettings settings;
		// location of battery
		private Point position;
		// battery shape
		private Shape batteryShape;
		// associated shapes (not really used atm)
		//private List<Shape> shapes;
		// battery type
		private BatteryType type;
		// number of available missiles
		private int missilesAvailable;
		// limit of missiles to hold
		private int missileCap;
		// base speed of missiles
		private double baseSpeed;
		// if battery is destroyed
		private bool destroyed;

		// properties
		// return if battery is destroyed
		public bool Destroyed {
			get {
				return destroyed;
			}
		}
		// return if battery is empty
		public bool Empty {
			get {
				return (missilesAvailable <= 0);
			}
		}
		// return number of missiles available
		public int MissileAvailable {
			get {
				return missilesAvailable;
			}
		}
		// get list of shapes
		// not used atm, only 1 shape
		/*public List<Shape> Shapes {
			get {
				return shapes;
			}
		}*/
		// get display shape
		public Shape DisplayShape {
			get {
				return batteryShape;
			}
		}
		// get display point
		public Point DisplayPoint {
			get {
				// use center position as base point
				Point _display = new Point(position.X, position.Y);
				_display.X -= batteryShape.ActualWidth / 2;
				//_display.Y -= batteryShape.ActualHeight / 2;

				return _display;
			}
		}
		// get center point
		public Point CenterPoint {
			get {
				return position;
			}
		}

		// tell battery it has been impacted by projectile
		// doesn't do much besides set the battery destroyed atm
		// but this could be used to simulate resitance to certain types of projectiles/# of projectiles/etc
		public void Impacted(Missile _p) {
			// check if battery is destroyed
			if (destroyed)
				return;

			// nothing to check in projectile atm
			// just set destroyed
			destroyed = true;
		}

		// fire a missile at given point, but with speed multiplier for missile
		// returns fired projectile
		public Missile Fire(Point _tar, double _multiplier) {
			// check if battery is destroyed or empty
			if (Destroyed || Empty) {
				return null;
			}

			// decrement missile count
			--missilesAvailable;

			// create/return projectile
			Missile p = new ABMissile(settings, position, baseSpeed*_multiplier, _tar);
			return p;
		}

		// fire a missile at given point
		public Missile Fire(Point _tar) {
			return Fire(_tar, 1);
		}

		// resets battery
		public void Reset() {
			destroyed = false;
			// reset missile cap
			missileCap = settings.MissilesPerBattery;
			missilesAvailable = missileCap;
		}

		// update battery missile colors to match settings
		// not used really since only 1 shape atm
		private void UpdateBatteryShape(Shape _shape) {
			_shape.Fill = settings.BrushFillBattery;
		}

		// static method to create the battery shape
		// pointless atm, only using 1 shape
		private static Shape CreateBatteryShape() {
			// create points for polygon shape
			PointCollection points = new PointCollection();

			// some shortcut way to only make half, then auto make other half?

			// bottom left
			points.Add(new Point(0, 0));
			points.Add(new Point(0, 2));
			points.Add(new Point(1, 2));
			points.Add(new Point(1, 8));
			points.Add(new Point(3, 9));
			points.Add(new Point(3, 2));
			points.Add(new Point(4, 2));
			points.Add(new Point(4, 8));
			points.Add(new Point(7, 9));
			points.Add(new Point(7, 2));
			points.Add(new Point(8, 2));
			points.Add(new Point(8, 8));
			points.Add(new Point(11, 9));
			points.Add(new Point(11, 2));
			points.Add(new Point(12, 2));
			points.Add(new Point(12, 0));
			// back to bottom left
			points.Add(new Point(0, 0));

			// create the shape
			Polygon _batteryShape = new Polygon();
			_batteryShape.Points = points;

			// have to rotate shape
			// create transform
			RotateTransform _rotate = new RotateTransform(180);
			// set center
			_rotate.CenterX = 7;
			_rotate.CenterY = 0;
			// rotate
			_batteryShape.RenderTransform = _rotate;

			return _batteryShape;
		}
	}

	// class for cities
	class City : ITarget {
		// members
		// settings obj
		private GameSettings settings;
		// center position of city
		private Point center;
		// shape for city
		private Shape cityShape;
		// if destroyed or not
		private bool destroyed;

		// constructor
		public City(GameSettings _settings, Point _position) {
			settings = _settings;
			center = _position;
			destroyed = false;

			// create shape
			cityShape = CreateShape();
			UpdateShape();
		}

		// get if city destroyed
		public bool Destroyed {
			get {
				return destroyed;
			}
		}
		// get center point of city
		public Point CenterPoint {
			get {
				return center;
			}
		}
		// get display point of city
		public Point DisplayPoint {
			get {
				// copy center point, shift according to shape dimensions
				Point _display = new Point(center.X, center.Y);
				_display.X -= cityShape.ActualWidth / 2;
				//_display.Y -= cityShape.ActualHeight / 2;
				return _display;
			}
		}
		// get shape for display
		public Shape DisplayShape {
			get {
				return cityShape;
			}
		}
		// tell city is has been impacted by projectile
		public void Impacted(Missile _p) {
			// just set destroyed
			destroyed = true;
		}

		// updates shape to match settings
		public void UpdateShape() {
			cityShape.Fill = settings.BrushFillCity;
		}

		// reset state of city
		public void Reset() {
			destroyed = false;
		}

		// static method to create a city shape
		public static Shape CreateShape() {
			// create points for polygon shape
			PointCollection points = new PointCollection();

			// bottom left
			points.Add(new Point(0, 0));
			points.Add(new Point(5, 10));
			points.Add(new Point(7, 5));
			points.Add(new Point(9, 5));
			points.Add(new Point(10, 11));
			points.Add(new Point(12, 8));
			points.Add(new Point(15, 9));
			points.Add(new Point(17, 5));
			points.Add(new Point(18, 5));
			points.Add(new Point(18, 15));
			points.Add(new Point(19, 15));
			points.Add(new Point(19, 10));
			points.Add(new Point(20, 10));
			points.Add(new Point(21, 0));
			// connect to bottom left
			points.Add(new Point(0, 0));

			// create the shape
			Polygon _cityShape = new Polygon();
			_cityShape.Points = points;

			// have to rotate shape
			// create transform
			RotateTransform _rotate = new RotateTransform(180);
			// set center
			_rotate.CenterX = 10.5;
			_rotate.CenterY = 0;
			// rotate
			_cityShape.RenderTransform = _rotate;

			return _cityShape;
		}
	}
}
