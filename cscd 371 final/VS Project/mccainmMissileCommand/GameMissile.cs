// Mathew McCain
// cscd371 final missile command
// contains classes used as projectiles/missiles

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
	// enum for reason of destruction
	// None - just destroy missile
	// ReachedTarget - missile has reached its target
	// Collided - missile was hit by an explosion
	enum MissileDestroyedType { None, ReachedTarget, Collided}

	// base class for missiles/projectiles
	abstract class Missile {
		// settings object
		protected GameSettings settings;

		protected Point startPos;
		protected Point targetPos;
		// current center positon of projectile
		protected Point curPos;
		protected double speed;
		// if current projectile has been destroyed
		protected bool destroyed;
		// reason for destroyed
		protected MissileDestroyedType destroyedType;
		// if projectile is able to be collided with
		protected bool collidable;

		// associated shapes
		protected List<Shape> shapes;
		// projectile shape
		protected Rectangle headShape;
		// missile trail shape
		protected Line trailShape;
		// explosion object
		protected Explosion explosion;

		// constructor
		protected Missile(GameSettings _settings, Point _initialPos, double _speed, Point _targetPos) {
			settings = _settings;
			startPos = _initialPos;
			targetPos = _targetPos;
			speed = _speed;

			curPos = startPos;
			destroyed = false;
			destroyedType = MissileDestroyedType.None;
			collidable = true;

			// create trail
			trailShape = CreateTrailShape(_settings, _initialPos);

			// create missile head
			headShape = CreateHeadShape(_settings);

			// update colors of shapes
			UpdateColors();

			// shapes collection
			shapes = new List<Shape>();

			shapes.Add(trailShape);
			shapes.Add(headShape);
		}

		// get list of associated shapes
		public List<Shape> Shapes {
			get {
				return shapes;
			}
		}

		// get projectile shape
		public Shape ProjectileShape {
			get {
				return headShape;
			}
		}

		// get current projectile position
		public Point ProjectilePoint {
			get {
				return curPos;
			}
		}

		// get projectile display position
		public Point ProjectileDisplayPoint {
			get {
				Point _visual = new Point(curPos.X, curPos.Y);
				_visual.X -= headShape.Width / 2;
				_visual.Y -= headShape.Height / 2;
				return _visual;
			}
		}

		// get if collidable
		public bool Collidable {
			get {
				return collidable;
			}
		}

		// get if destroyed
		public bool Destroyed {
			get {
				return destroyed;
			}
		}

		// get destroyed reason
		public MissileDestroyedType DestroyedType {
			get {
				return destroyedType;
			}
		}

		// get explosion object
		// has to be a cleaner way to do this
		public Explosion ExplosionObj {
			get {
				return explosion;
			}
		}

		// check if projectile collides with given explosion
		// if true, sets projectile to destroyed
		public bool Collided(Explosion _e) {
			// check if missile is destroyed
			if (destroyed) {
				// throw an exception to test for sync issues
				throw new InvalidOperationException("Missile is destroyed already");
			}

			// check if missile is collidable
			if (!collidable)
				return false;

			// check if explosion is an in-air
			if (_e.Type != ExplosionType.InAir)
				return false;

			// have explosion check if position is within collision
			if (_e.Collides(curPos)) {
				// destroy projectile
				SetDestroyed(MissileDestroyedType.Collided);
				return true;
			}
			return false;
		}

		// destroy projectile at current position
		public void SetDestroyed(MissileDestroyedType _reason) {
			// check if explosion obj is already available
			if (explosion != null) {
				// throw an exception to test for sync issues
				throw new InvalidOperationException("Missile is destroyed already");
			}

			// set destroyed
			destroyed = true;

			destroyedType = _reason;

			// call WasDestroyed with reason
			// will generate explosion obj accordingly
			WasDestroyed(_reason);
		}

		// abstract method for when the missile is destroyed
		// should handle whatever it needs and create the explosion object
		abstract protected void WasDestroyed(MissileDestroyedType _reason);

		// advance missile position
		public void Advance(double dt) {
			// check if missile destroyed
			if (destroyed) {
				// throw an exception to test for sync issues
				throw new InvalidOperationException("Missile is destroyed already");
			}

			// get delta of points
			// don't need to determine angle then use sin/cos
			double dX = targetPos.X - curPos.X;
			double dY = targetPos.Y - curPos.Y;
			double vectorLength = Math.Sqrt(dX * dX + dY * dY);

			//Debug.WriteLine(string.Format("{0} dx: {1}, dy: {2}", this, dX, dY));

			// determine change in distance
			double nDistance = vectorLength - speed * dt;

			// check if reaches target during travel
			if (nDistance <= 0) {
				// if distance goes negative, reached/passed target

				// determine remaining dt
				dt -= vectorLength / speed;

				// set projectile position at target
				curPos.X = targetPos.X;
				curPos.Y = targetPos.Y;

				// set destroyed
				SetDestroyed(MissileDestroyedType.ReachedTarget);
				// advance explosion by remaining dt
				explosion.Advance(dt);
			} else {
				// determine correct angle to travel
				// don't need to determine angle, then sin/cos
				dX /= vectorLength;
				dY /= vectorLength;

				// determine new/current position of projectile
				curPos.X += speed * dt * dX;
				curPos.Y += speed * dt * dY;

				// advance line/trail
				trailShape.X2 = curPos.X;
				trailShape.Y2 = curPos.Y;
			}
		}

		// create trails line
		public static Line CreateTrailShape(GameSettings _settings, Point _startPos) {
			// create trail (line)
			Line _trailLine = new Line();
			_trailLine.Stroke = _settings.BrushABMissileTail;
			_trailLine.StrokeThickness = _settings.BrushThicknessMissileTrail;

			// set line to start position
			_trailLine.X1 = _startPos.X;
			_trailLine.Y1 = _startPos.Y;
			_trailLine.X2 = _startPos.X;
			_trailLine.Y2 = _startPos.Y;

			return _trailLine;
		}

		// create head rectangle
		public static Rectangle CreateHeadShape(GameSettings _settings) {
			Rectangle _rect = new Rectangle();
			_rect.Stroke = _settings.BrushABMissileHead;
			_rect.Fill = _settings.BrushABMissileHead;
			_rect.Height = 1;
			_rect.Width = 1;

			return _rect;
		}

		// for setting colors of missile head/trail
		abstract protected void UpdateColors();
	}

	// Anti-Ballistic Missile
	// friendly missiles, explode in-air
	// noncollidable
	class ABMissile : Missile {
		// constructor
		public ABMissile(GameSettings _settings, Point _initialPos, double _speed, Point _targetPos) : base(_settings, _initialPos, _speed, _targetPos) {
			// turn off explosion collision
			collidable = false;
		}

		// method for handling missile being destroyed
		override protected void WasDestroyed(MissileDestroyedType _reason) {
			// check reason
			// only generate an explosion on reaching target position
			if (_reason == MissileDestroyedType.ReachedTarget) {
				// create in air explosion at current position
				explosion = new Explosion(settings, ExplosionType.InAir, curPos);
			}
		}

		// set colors of missile head/trail
		override protected void UpdateColors() {
			headShape.Fill = settings.BrushABMissileHead;
			trailShape.Stroke = settings.BrushABMissileTail;
		}
	}

	// Inter-Planetary-Ballistic Missile
	// enemy missiles, target batteries/cities
	class IPBMissle : Missile {
		// current target object
		private ITarget target;

		// constructor 
		public IPBMissle(GameSettings _settings, Point _initialPos, double _speed, ITarget _target) : base(_settings, _initialPos, _speed, _target.CenterPoint) {
			target = _target;
		}

		// method for handling missile being destroyed
		override protected void WasDestroyed(MissileDestroyedType _reason) {
			// check reason
			// if none, do nothing
			// if collided
			if (_reason == MissileDestroyedType.Collided) {
				// generate in-air explosion at current position
				explosion = new Explosion(settings, ExplosionType.InAir, curPos);
			} else if (_reason == MissileDestroyedType.ReachedTarget) {
				// tell target it has been impacted
				target.Impacted(this);
				// generate impact explosion at target position
				explosion = new Explosion(settings, ExplosionType.Impact, targetPos);
			}
		}

		// set colors of missile head/trail
		override protected void UpdateColors() {
			headShape.Fill = settings.BrushIPBMissileHead;
			trailShape.Stroke = settings.BrushIPBMissileTail;
		}
	}
}
