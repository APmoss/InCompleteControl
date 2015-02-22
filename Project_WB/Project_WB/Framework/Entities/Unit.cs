using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameStateManagement;

namespace Project_WB.Framework.Entities {
	class Unit : AnimatedSprite {
		const int nearestDistance = 3;

		#region Fields
		bool isSelected = false;
		float health = 100;
		protected float maxHealth = 100;
		public float Speed = 1;
		public LinkedList<Point> Waypoints = new LinkedList<Point>();
		#endregion

		#region Properties
		public float Health {
			get { return health; }
			set {
				if (health != (float)MathHelper.Clamp(value, 0, maxHealth) && HealthChanged != null) {
					HealthChanged.Invoke(this, EventArgs.Empty);
				}

				health = (float)MathHelper.Clamp(value, 0, maxHealth);

				if (health == 0 && Killed != null) {
					Killed.Invoke(this, EventArgs.Empty);
				}
			}
		}

		public float MaxHealth {
			get { return maxHealth; }
		}

		public Point Tile {
			get {
				return new Point((int)Math.Round(Position.X / EntityManager.tileSize),
								(int)Math.Round(Position.Y / EntityManager.tileSize));
			}
		}

		public bool IsSelected {
			get { return isSelected; }
			set{
				// Was not previously selected, but is now (Not Selected -> Selected)
				if ((isSelected == false) && value) {
					if (Selected != null) {
						Selected.Invoke(this, EventArgs.Empty);
					}
				}
				// Was previously selected, but not anymore (Selected -> Not Selected)
				else if (isSelected && (value == false)) {
					if (Deselected != null) {
						Deselected.Invoke(this, EventArgs.Empty);
					}
				}

				isSelected = value;
			}
		}
		#endregion

		#region Events
		public event EventHandler<EventArgs> Selected;
		public event EventHandler<EventArgs> Deselected;
		public event EventHandler<HotkeyEventArgs> HotkeyPressed;

		public event EventHandler<EventArgs> HealthChanged;
		public event EventHandler<EventArgs> Killed;

		public event EventHandler<EventArgs> WaypointsChanged;
		public event EventHandler<EventArgs> DestinationAchieved;
		#endregion

		public Unit() {
			// Wooh, lambda expressions!
			LeftClicked += (s, e) => { EntityManager.SelectedUnit = this; IsSelected = true; };
		}

		public Unit(Texture2D spriteSheet) : base(spriteSheet) {
			// Wooh, lambda expressions!
			LeftClicked += (s, e) => { EntityManager.SelectedUnit = this; IsSelected = true; };
		}
		public Unit(Texture2D spriteSheet,
						List<Rectangle> upSourceRectangles,
						List<Rectangle> downSourceRectangles,
						List<Rectangle> leftSourceRectangles,
						List<Rectangle> rightSourceRectangles) : base(spriteSheet) {

			SetSourceRectangles(upSourceRectangles, downSourceRectangles, leftSourceRectangles, rightSourceRectangles);

			// Wooh, lambda expressions!
			LeftClicked += (s, e) => { EntityManager.SelectedUnit = this; IsSelected = true; };
		}

		#region Overridden Methods
		public override void Update(GameTime gameTime) {
			if (Waypoints.Count > 0) {
				var ts = EntityManager.tileSize;

				if (Vector2.Distance(Position, new Vector2(Waypoints.First.Value.X * ts, Waypoints.First.Value.Y * ts)) < nearestDistance) {
					Waypoints.RemoveFirst();
				}
				else {
					var targetVector = new Vector2(Waypoints.First.Value.X * ts - Position.X,
													Waypoints.First.Value.Y * ts - Position.Y);
					targetVector.Normalize();
					targetVector *= Speed;

					Velocity = targetVector;
				}
			}
			else {
				Velocity = Vector2.Zero;
			}

			base.Update(gameTime);
		}

		public override void UpdateInteraction(GameTime gameTime, GameStateManagement.InputState input, Camera2D camera) {
			base.UpdateInteraction(gameTime, input, camera);
		}

		public override void Draw(GameTime gameTime, ScreenManager screenManager) {
			if (IsSelected && Waypoints.Count > 0 && DownSourceRectangles.Count > 0) {
				int ts = EntityManager.tileSize;

				foreach (var waypoint in Waypoints) {
					Rectangle dotPosition = new Rectangle((int)(waypoint.X * ts + ts / 2), (int)(waypoint.Y * ts + ts / 2), 4, 4);
					screenManager.SpriteBatch.Draw(screenManager.BlankTexture, dotPosition, null, Color.DarkBlue, (float)gameTime.TotalGameTime.TotalSeconds * 4, new Vector2(.5f), 0, 0);
				}

				Vector2 ghostPosition = new Vector2(Waypoints.Last.Value.X, Waypoints.Last.Value.Y) * ts;
				screenManager.SpriteBatch.Draw(spriteSheet, ghostPosition, DownSourceRectangles[0], new Color(255, 255, 255, 100));
			}

			base.Draw(gameTime, screenManager);
		}
		#endregion

		#region Public Methods
		public Point GetTile(bool centered) {
			if (centered) {
				return new Point((int)Math.Round((Position.X + Bounds.Width / 2) / EntityManager.tileSize),
								(int)Math.Round((Position.Y + Bounds.Height / 2) / EntityManager.tileSize));
			}

			return new Point((int)Math.Round(Position.X / EntityManager.tileSize),
								(int)Math.Round(Position.Y / EntityManager.tileSize));
		}
		#endregion

		#region Invocations
		public void InvokeWaypointsChanged() {
			if (WaypointsChanged != null) {
				WaypointsChanged.Invoke(this, EventArgs.Empty);
			}
		}
		public void InvokeDestinationAchieved() {
			if (DestinationAchieved != null) {
				DestinationAchieved.Invoke(this, EventArgs.Empty);
			}
		}
		#endregion
	}
}