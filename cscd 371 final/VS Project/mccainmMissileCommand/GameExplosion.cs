// Mathew McCain
// cscd371 final missile command
// contains explosion class

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
	// enum for type of explosion
	enum ExplosionType { InAir, Impact }

	// enum for explosion state
	enum ExplosionState { Expanding, Peeked, Contracting, Finished }

	// explosion class, used by projectiles
	class Explosion {
		// members
		// settings obj
		private GameSettings settings;
		// general variables
		private ExplosionType type;
		private Point pos;
		private double size, maxSize, speed, duration;
		protected List<Shape> shapes;
		private Ellipse explosionShape;
		// how long the explosion has been at peek size
		private double peekTime;
		// state of explosion
		private ExplosionState state;

		// constructor, defining explosion characteristics
		// _pos = center point of explosion
		// size = diameter
		// _expansionSpeed = how fast explosion should reach max size
		// _duration = how long explosion should last once reached max size (before contracting)
		public Explosion(GameSettings _settings, ExplosionType _type, Point _pos) {
			settings = _settings;
			type = _type;

			state = ExplosionState.Expanding;

			// create list for shapes
			shapes = new List<Shape>();

			// create shape
			explosionShape = new Ellipse();
			shapes.Add(ExplosionShape);

			// set specifics according to explosion type
			if (type == ExplosionType.Impact) {
				// explosion properties
				size = settings.ImpactExplosionInitialSize;
				maxSize = settings.ImpacExplosionMaxSize;
				speed = settings.ImpacExplosionSpeed;
				duration = settings.ImpacExplosionDuration;
				// shape colors
				explosionShape.Stroke = settings.BrushStrokeImpactExplosion;
				explosionShape.Fill = settings.BrushFillImpactExplosion;
			} else {
				// explosion properties
				size = settings.AirExplosionInitialSize;
				maxSize = settings.AirExplosionMaxSize;
				speed = settings.AirExplosionSpeed;
				duration = settings.AirExplosionDuration;
				// shape colors
				explosionShape.Stroke = settings.BrushStrokeAirExplosion;
				explosionShape.Fill = settings.BrushFillAirExplosion;
			}

			// center point correctly (top left)
			pos = new Point(_pos.X - size / 2, _pos.Y - size / 2);

			explosionShape.Height = size;
			explosionShape.Width = size;
		}

		// property to get position
		public Point Position {
			get {
				return pos;
			}
		}

		// property to get shape
		public Shape ExplosionShape {
			get {
				return explosionShape;
			}
		}

		// get all shapes
		public List<Shape> Shapes {
			get {
				return shapes;
			}
		}

		// property to get if explosion is active
		public bool Active {
			get {
				return (state != ExplosionState.Finished);
			}
		}

		// get type of explosion
		public ExplosionType Type {
			get {
				return type;
			}
		}

		// sets shape size to this size, adjust positions
		private void SetSize(double _size) {
			// if size < 0, set state to finished
			if (_size <= 0) {
				state = ExplosionState.Finished;
				_size = 0;
			}

			// determine new cordinates
			pos.X += (size - _size) / 2;
			pos.Y += (size - _size) / 2;
			// set size
			explosionShape.Height = _size;
			explosionShape.Width = _size;
			size = _size;
		}

		// advance state of explosion by time
		public void Advance(double dt) {
			// check that explosion is active
			if (!Active)
				return;

			double nSize = 0;

			switch (state) {
				case ExplosionState.Expanding:
					// explosion is expanding

					// determine new size
					nSize = size + speed * dt;
					// check if new size is larger than max size
					if (nSize >= maxSize) {
						// set size to max
						SetSize(maxSize);

						// determine at what dt the size maxed
						dt -= (maxSize - nSize) / speed;

						// should check dt >= 0

						// set to peeked state
						state = ExplosionState.Peeked;
						peekTime = dt;

						// recursive advance to handle
						// could just use goto statements
						Advance(dt);
					} else {
						SetSize(nSize);
					}

					break;
				case ExplosionState.Contracting:
					// explosion is contracting

					// determine new size
					// SetSize() checks if it is too small
					SetSize(size - speed * dt);

					break;
				case ExplosionState.Peeked:
					// explosion is waiting at peek size

					// check if peek time exceeds duration limit
					if ((peekTime += dt) > duration) {
						// determine at what time the shape peeked
						dt -= peekTime - duration;

						// swap to contracting state
						// could just use goto statements
						state = ExplosionState.Contracting;
						Advance(dt);
					}

					break;
			}

		}

		// check if a given point collides with explosion
		public bool Collides(Point _p) {
			// get center point of explosion
			Point center = new Point(pos.X + size / 2, pos.Y + size / 2);

			// get distance between point and explosion center
			double distance = Math.Sqrt(Math.Pow(_p.X - center.X, 2) + Math.Pow(_p.Y - center.Y, 2));

			// check if distance is within explosion radius
			return (distance <= (size / 2));
		}

	}
}
