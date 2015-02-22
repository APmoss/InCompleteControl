using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameStateManagement;
using Project_WB.Framework.Audio;
using MouseButton = GameStateManagement.InputState.MouseButton;
using Microsoft.Xna.Framework.Input;

namespace Project_WB.Framework.Entities {
	class Unit : AnimatedSprite {
		const int nearestDistance = 3;

		#region Fields
		bool isSelected = false;
		protected float health = 100;
		protected float maxHealth = 100;
		public float Speed = 1;
		public int distance = 5;
		public int attackDistance = 1;
		public bool Moved = false;
		public bool Spent = false;
		public int Team = 0;
		public LinkedList<Point> Waypoints = new LinkedList<Point>();

		protected List<string> selectCommandVoices = new List<string>();
		protected List<string> moveCommandVoices = new List<string>();
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

			set {
				Position = new Vector2(value.X * EntityManager.tileSize, value.Y * EntityManager.tileSize);
			}
		}

		public bool IsSelected {
			get { return isSelected; }
			set{
				// Was not previously selected, but is now (Not Selected -> Selected)
				if ((isSelected == false) && (value == true)) {
					if (Selected != null) {
						Selected.Invoke(this, EventArgs.Empty);
					}
				}
				// Was previously selected, but not anymore (Selected -> Not Selected)
				else if ((isSelected == true) && (value == false)) {
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
			sharedConstruct();
		}

		public Unit(Texture2D spriteSheet) : base(spriteSheet) {
			sharedConstruct();
		}
		public Unit(Texture2D spriteSheet, List<Rectangle> upSourceRectangles, List<Rectangle> downSourceRectangles,
										List<Rectangle> leftSourceRectangles, List<Rectangle> rightSourceRectangles) : base(spriteSheet) {

			SetSourceRectangles(upSourceRectangles, downSourceRectangles, leftSourceRectangles, rightSourceRectangles);

			sharedConstruct();
		}

		/// <summary>
		/// A method shared between the overloaded constructors to reduce redundancy
		/// </summary>
		void sharedConstruct() {
			// Multiple event handler initializations for every unit;
			// Wooh, lambda expressions!
			LeftClicked += (s, e) => { EntityManager.SelectedUnit = this; IsSelected = true; };

			Selected += (s, e) => {
				if (selectCommandVoices.Count > 0) {
					InterfaceSound inS = new InterfaceSound(EntityManager.soundLibrary.GetSound(selectCommandVoices[EntityManager.r.Next(selectCommandVoices.Count)]));
					EntityManager.audioManager.AddSounds(inS);
				}
			};
			WaypointsChanged += (s, e) => {
				if (moveCommandVoices.Count > 0) {
					InterfaceSound inS = new InterfaceSound(EntityManager.soundLibrary.GetSound(moveCommandVoices[EntityManager.r.Next(moveCommandVoices.Count)]));
					EntityManager.audioManager.AddSounds(inS);
				}
			};
			DestinationAchieved += (s, e) => {
				Moved = true;
			};
		}

		#region Overridden Methods
		/// <summary>
		/// Starts second-stage initialization for the unit,
		/// with access to the entity manager.
		/// </summary>
		public virtual void Initialize() {
		}

		public override void Update(GameTime gameTime) {
			if (Waypoints.Count > 0) {
				var ts = EntityManager.tileSize;

				if (Vector2.Distance(Position, new Vector2(Waypoints.First.Value.X * ts, Waypoints.First.Value.Y * ts)) < nearestDistance) {
					Waypoints.RemoveFirst();
					if (Waypoints.Count == 0) {
						InvokeDestinationAchieved();
					}
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

		public override void UpdateInteraction(GameTime gameTime, InputState input, Camera2D camera) {
			PlayerIndex p = PlayerIndex.One;
			var relativeMouse = camera.ToRelativePosition(input.CurrentMouseState.X, input.CurrentMouseState.Y);
			relativeMouse.X -= EntityManager.tileSize / 2;
			relativeMouse.Y -= EntityManager.tileSize / 2;
			var mouseTile = new Point((int)Math.Round(relativeMouse.X / 32),
								(int)Math.Round(relativeMouse.Y / 32));

			// Moving the unit with right click
			if (!Moved) {
				if (IsSelected && input.IsNewMousePress(MouseButton.Right) && GetTravelableTiles().Contains(mouseTile) && Waypoints.Count == 0) {
					var solution = new LinkedList<Point>();
					var otherBarriers = new List<Point>();
					foreach (var entity in EntityManager.GetEntities()) {
						if (entity is Unit) {
							Unit u = entity as Unit;
							otherBarriers.Add(u.Tile);

							if (u.Waypoints.Count > 0) {
								otherBarriers.Add(u.Waypoints.Last.Value);
							}
						}
					}

					// Add to the waypoint list (append move)
					if (input.IsKeyPressed(Keys.LeftShift, null, out p) && Waypoints.Count > 0) {
						if (EntityManager.QuickFind(Tile, mouseTile, otherBarriers, out solution)) {
							solution.RemoveFirst();
							foreach (var point in solution) {
								Waypoints.AddLast(point);
							}

							InvokeWaypointsChanged();
						}
					}
					// Override previous orders and head straight to location
					else {
						if (EntityManager.QuickFind(Tile, mouseTile, otherBarriers, out solution)) {
							Waypoints = solution;

							InvokeWaypointsChanged();
						}
					}
				}
			}
			// Attack with the unit with right click
			else if (Moved && !Spent) {

			}

			base.UpdateInteraction(gameTime, input, camera);
		}

		public override void Draw(GameTime gameTime, ScreenManager screenManager) {
			int ts = EntityManager.tileSize;

			if (IsSelected && Waypoints.Count > 0 && DownSourceRectangles.Count > 0) {
				foreach (var waypoint in Waypoints) {
					Rectangle dotPosition = new Rectangle((int)(waypoint.X * ts + ts / 2), (int)(waypoint.Y * ts + ts / 2), 4, 4);
					screenManager.SpriteBatch.Draw(screenManager.BlankTexture, dotPosition, null, Color.DarkBlue, (float)gameTime.TotalGameTime.TotalSeconds * 4, new Vector2(.5f), 0, 0);
				}

				Vector2 ghostPosition = new Vector2(Waypoints.Last.Value.X, Waypoints.Last.Value.Y) * ts;
				screenManager.SpriteBatch.Draw(spriteSheet, ghostPosition, DownSourceRectangles[0], new Color(255, 255, 255, 100));
			}

			base.Draw(gameTime, screenManager);

			if (Moved) {
				screenManager.SpriteBatch.Draw(screenManager.BlankTexture, Bounds, new Color(10, 10, 10, 150));
			}
			if (Spent) {
				screenManager.SpriteBatch.Draw(screenManager.BlankTexture, Bounds, new Color(10, 10, 10, 150));
			}
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

		public List<Point> GetTravelableTiles() {
			List<Point> tiles = new List<Point>();
			int ts = EntityManager.tileSize;

			for (int i = -distance; i <= distance; i++) {
				for (int j = -distance; j <= distance; j++) {
					if (Vector2.Distance(new Vector2(Tile.X, Tile.Y), new Vector2(i + Tile.X, j + Tile.Y)) <= distance) {
						tiles.Add(new Point(i + Tile.X, j + Tile.Y));
					}
				}
			}

			return tiles;
		}

		public List<Point> GetAttackableTiles() {
			List<Point> tiles = new List<Point>();
			int ts = EntityManager.tileSize;

			for (int i = -attackDistance; i <= attackDistance; i++) {
				for (int j = -attackDistance; j <= attackDistance; j++) {
					if (Vector2.Distance(new Vector2(Tile.X, Tile.Y), new Vector2(i + Tile.X, j + Tile.Y)) <= attackDistance) {
						tiles.Add(new Point(i + Tile.X, j + Tile.Y));
					}
				}
			}

			return tiles;
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