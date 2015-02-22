using System;
using System.Collections.Generic;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project_WB.Framework.Audio;
using MouseButton = GameStateManagement.InputState.MouseButton;
using Project_WB.Framework.Particles.Emitters;

namespace Project_WB.Framework.Entities {
	/// <summary>
	/// The class used for battle events, and an extension of the animated sprite class.
	/// Handles all important pathfinding, health, movement and others.
	/// </summary>
	class Unit : AnimatedSprite {
		// The minimal distance for the unit before moving to the next needed tile
		const int nearestDistance = 3;

		#region Fields
		// Whether the unit is the currently selected unit in the entity manager (more below)
		bool isSelected = false;
		// The current health the unit has out of the max health
		protected float health = 100;
		// The maximum amount of health the unit can achieve
		protected float maxHealth = 100;
		// The travelling speed of the unit
		public float Speed = 1;
		// The damage the unit deals to opponents
		public float Damage = 30;
		// The distance in tiles the unit can travel
		public int distance = 5;
		// The distance in tiles the unit can attack
		public int attackDistance = 1;
		// Whether the unit has been moved yet
		public bool Moved = false;
		// Whether the unit has attacked yet
		public bool HasAttacked = false;
		// Whether the unit needs to be removed from the entity manager
		public bool NeedsRemoval = false;
		// The team the unit is a part of, from 1 - 4
		public int Team = 0;
		// The collection of waypoints the unit needs to travel
		public LinkedList<Point> Waypoints = new LinkedList<Point>();

		// The names of sound files the unit speaks when selected
		protected List<string> selectCommandVoices = new List<string>();
		// The names of sound files the unit speaks when moved
		protected List<string> moveCommandVoices = new List<string>();
		#endregion

		#region Properties
		/// <summary>
		/// The current health of the unit.
		/// </summary>
		public float Health {
			get { return health; }
			set {
				// Check to see if the health has been changed
				if (health != (float)MathHelper.Clamp(value, 0, maxHealth) && HealthChanged != null) {
					HealthChanged.Invoke(this, EventArgs.Empty);
				}

				health = (float)MathHelper.Clamp(value, 0, maxHealth);

				// Check to see if the unit has zero remaining health
				if (health == 0 && Killed != null) {
					Killed.Invoke(this, EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// The maximum amout of health the unit can have.
		/// </summary>
		public float MaxHealth {
			get { return maxHealth; }
		}

		/// <summary>
		/// Gets or sets the current tile the unit is placed on.
		/// </summary>
		public Point Tile {
			get {
				return new Point((int)Math.Round(Position.X / EntityManager.tileSize),
								(int)Math.Round(Position.Y / EntityManager.tileSize));
			}

			set {
				if (EntityManager != null) {
					Position = new Vector2(value.X * EntityManager.tileSize, value.Y * EntityManager.tileSize);
				}
				else {
					Position = new Vector2(value.X * 32, value.Y * 32);
				}
			}
		}

		/// <summary>
		/// Gets or sets whether the unit is currently selected in the entity manager.
		/// </summary>
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
		// Invoked when the unit is selected
		public event EventHandler<EventArgs> Selected;
		// Invoked when the unit is deselected
		public event EventHandler<EventArgs> Deselected;

		// Invoked when the unit's health has changed
		public event EventHandler<EventArgs> HealthChanged;
		// Invoked when the unit has been killed
		public event EventHandler<EventArgs> Killed;

		// Invoked when the unit's waypoint collection has chenged
		public event EventHandler<EventArgs> WaypointsChanged;
		// Invoked when the final waypoint has been reached
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
		/// A method shared between the overloaded constructors to reduce redundancy.
		/// </summary>
		void sharedConstruct() {
			// Multiple event handler initializations for every unit
			// Wooh, lambda expressions!
			// Become selected when left clicked
			LeftClicked += (s, e) => { EntityManager.SelectedUnit = this; IsSelected = true; };

			// Play a sound when selected
			Selected += (s, e) => {
				if (selectCommandVoices.Count > 0) {
					InterfaceSound inS = new InterfaceSound(EntityManager.soundLibrary.GetSound(selectCommandVoices[EntityManager.r.Next(selectCommandVoices.Count)]));
					EntityManager.audioManager.AddSounds(inS);
				}
			};
			// Play a sound when moved
			WaypointsChanged += (s, e) => {
				if (moveCommandVoices.Count > 0) {
					InterfaceSound inS = new InterfaceSound(EntityManager.soundLibrary.GetSound(moveCommandVoices[EntityManager.r.Next(moveCommandVoices.Count)]));
					EntityManager.audioManager.AddSounds(inS);
				}
			};
			// Moving is complete
			DestinationAchieved += (s, e) => {
				Moved = true;
			};

			Killed += (s, e) => {
				NeedsRemoval = true;
				EntityManager.particleManager.AddParticleEmitter(new Explosion(TimeSpan.Zero, TimeSpan.FromSeconds(.2), Bounds));
				EntityManager.SelectedUnit = null;
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
			// If we have a location to go to
			if (Waypoints.Count > 0) {
				var ts = EntityManager.tileSize;

				// Check if we have hit the target tile
				if (Vector2.Distance(Position, new Vector2(Waypoints.First.Value.X * ts, Waypoints.First.Value.Y * ts)) < nearestDistance) {
					Position = new Vector2(Waypoints.First.Value.X * ts, Waypoints.First.Value.Y * ts);

					Waypoints.RemoveFirst();
					if (Waypoints.Count == 0) {
						InvokeDestinationAchieved();
					}
				}
				// Start to travel to the next waypoint in the collection
				else {
					var targetVector = new Vector2(Waypoints.First.Value.X * ts - Position.X,
													Waypoints.First.Value.Y * ts - Position.Y);
					targetVector.Normalize();
					targetVector *= Speed;

					Velocity = targetVector;
				}
			}
			// Otherwise stop all motion
			else {
				Velocity = Vector2.Zero;
			}

			base.Update(gameTime);
		}

		public override void UpdateInteraction(GameTime gameTime, InputState input, Camera2D camera) {
			// The player performing the action
			PlayerIndex p = PlayerIndex.One;
			// Calculate a world mouse position
			var relativeMouse = camera.ToRelativePosition(input.CurrentMouseState.X, input.CurrentMouseState.Y);
			relativeMouse.X -= EntityManager.tileSize / 2;
			relativeMouse.Y -= EntityManager.tileSize / 2;
			// Calculate the tile the mouse is currently on
			var mouseTile = new Point((int)Math.Round(relativeMouse.X / 32),
								(int)Math.Round(relativeMouse.Y / 32));

			// Move the unit with right click
			if (!Moved) {
				if (IsSelected && input.IsNewMousePress(MouseButton.Right) && GetTravelableTiles().Contains(mouseTile) && Waypoints.Count == 0 && Team == EntityManager.controllingTeam) {
					var solution = new LinkedList<Point>();
					var otherBarriers = new List<Point>();
					// Calculate other barriers needed such as other entities and other moving units
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
			if (!HasAttacked && input.IsNewMousePress(MouseButton.Right) && Team == EntityManager.controllingTeam) {
				foreach (var entity in EntityManager.GetEntities()) {
					if (entity is Unit) {
						Unit unit = entity as Unit;
						// Check to see if the entity is within attacking range, the mouse is clicking on it, and it is not a friendly unit
						if (GetAttackableTiles().Contains(mouseTile) && mouseTile == unit.Tile && unit.Team != Team) {
							unit.Health -= Damage;
							HasAttacked = true;

							EntityManager.particleManager.AddParticleEmitter(
														new MediumBullet(TimeSpan.FromSeconds(.1), TimeSpan.FromSeconds(.5),
														this.Position + new Vector2(Bounds.Width / 2, Bounds.Height / 2), unit.Position + new Vector2(unit.Bounds.Width / 2, unit.Bounds.Height / 2)));
						}
					}
				}
			}

			base.UpdateInteraction(gameTime, input, camera);
		}

		public override void Draw(GameTime gameTime, ScreenManager screenManager) {
			int ts = EntityManager.tileSize;

			// If the unit is selected and is moving, show the pathway with blue dots
			if (IsSelected && Waypoints.Count > 0 && DownSourceRectangles.Count > 0) {
				foreach (var waypoint in Waypoints) {
					// Calculate the position of the dot
					Rectangle dotPosition = new Rectangle((int)(waypoint.X * ts + ts / 2), (int)(waypoint.Y * ts + ts / 2), 4, 4);
					screenManager.SpriteBatch.Draw(screenManager.BlankTexture, dotPosition, null, Color.DarkBlue, (float)gameTime.TotalGameTime.TotalSeconds * 4, new Vector2(.5f), 0, 0);
				}

				// Draw a little ghost sprite of where the unit will end up
				Vector2 ghostPosition = new Vector2(Waypoints.Last.Value.X, Waypoints.Last.Value.Y) * ts;
				screenManager.SpriteBatch.Draw(spriteSheet, ghostPosition, DownSourceRectangles[0], new Color(255, 255, 255, 100));
			}

			base.Draw(gameTime, screenManager);

			// If the unit has been moved, darken it
			if (Moved) {
				screenManager.SpriteBatch.Draw(screenManager.BlankTexture, Bounds, new Color(10, 10, 10, 150));
			}
			// If the unit has attacked, darken it
			if (HasAttacked) {
				screenManager.SpriteBatch.Draw(screenManager.BlankTexture, Bounds, new Color(10, 10, 10, 150));
			}
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Returns the tile the unit is currently on, either on the center or the origin.
		/// </summary>
		/// <param name="centered"></param>
		/// <returns></returns>
		public Point GetTile(bool centered) {
			if (centered) {
				return new Point((int)Math.Round((Position.X + Bounds.Width / 2) / EntityManager.tileSize),
								(int)Math.Round((Position.Y + Bounds.Height / 2) / EntityManager.tileSize));
			}

			return new Point((int)Math.Round(Position.X / EntityManager.tileSize),
								(int)Math.Round(Position.Y / EntityManager.tileSize));
		}

		/// <summary>
		/// Returns a collection of points that the unit can travel to.
		/// </summary>
		/// <returns></returns>
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

		/// <summary>
		/// Returns a collection of points the unit can attack.
		/// </summary>
		/// <returns></returns>
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
		/// <summary>
		/// Invokes the Waypoints Changed event.
		/// </summary>
		public void InvokeWaypointsChanged() {
			if (WaypointsChanged != null) {
				WaypointsChanged.Invoke(this, EventArgs.Empty);
			}
		}
		/// <summary>
		/// Invokes the Destination Achieved event.
		/// </summary>
		public void InvokeDestinationAchieved() {
			if (DestinationAchieved != null) {
				DestinationAchieved.Invoke(this, EventArgs.Empty);
			}
		}
		#endregion
	}
}