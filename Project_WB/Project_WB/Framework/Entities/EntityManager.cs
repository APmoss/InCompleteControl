using System;
using System.Collections.Generic;
using System.Linq;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project_WB.Framework.Audio;
using Project_WB.Framework.Particles;
using Project_WB.Framework.Pathfinding;
using Project_WB.Framework.Squared.Tiled;

namespace Project_WB.Framework.Entities {
	/// <summary>
	/// Contains the data needed to contain a collection of entities, with
	/// complete drawing, updating, and interacting with each entity contained.
	/// </summary>
	class EntityManager {
		#region Fields
		// The source rectangle that contains the halo texture
		protected readonly Rectangle haloSrcRec = new Rectangle(0, 0, 48, 48);
		// The source rectangle that contains the hovering surrounding texture
		protected readonly Rectangle hoverSrcRec = new Rectangle(0, 48, 48, 48);
		
		// The mainn collection of entities, all entity actions are made with this
		List<Entity> entities = new List<Entity>();
		// The size of the tiles from the currently loaded map
		protected internal int tileSize = 32;
		/// <summary>
		/// A sheet for other textures, such as the halo and hover images
		/// </summary>
		public Texture2D EtcTextures;
		/// <summary>
		/// A default sprite sheet that will be used if no specific sheet is
		/// specified when adding a new entity to the collection.
		/// </summary>
		public Texture2D DefaultSpritesheet;

		/// <summary>
		/// The team that the player controls in battle.
		/// </summary>
		protected internal int controllingTeam = 1;
		// A central random variable
		protected internal Random r = new Random();

		// An internal reference to the audio manager for unit sounds
		protected internal AudioManager audioManager;
		// An internal reference to the sound library for creating sounds for the audio manager
		protected internal SoundLibrary soundLibrary;
		// An internal reference to the particle manager for creating various particles from the entities
		protected internal ParticleManager particleManager;

		/// <summary>
		/// The path finding class that centrally handles all pathfinding for the entities.
		/// </summary>
		protected PathFinder pathFinder = new PathFinder();
		// The initial data for the current map
		public MapData mapData;

		// The currently selected unit in the entity manager
		Unit selectedUnit;
		#endregion

		#region Properties
		/// <summary>
		/// The currently selected unit in the entity manager.
		/// Returns null if no unit is selected.
		/// </summary>
		public Unit SelectedUnit {
			get { return selectedUnit; }
			set {
				if (selectedUnit != value) {
					if (NewSelectedUnit != null) {
						// Invoke if selected unit is new
						NewSelectedUnit.Invoke(this, EventArgs.Empty);
					}

					foreach (var entity in entities) {
						if (entity is Unit) {
							// Tell all the units that they are not selected
							((Unit)entity).IsSelected = false;
						}
					}

					selectedUnit = value;
				}
			}
		}
		#endregion

		#region Events
		// Invoked when a new unit has been selected
		public event EventHandler<EventArgs> NewSelectedUnit;
		#endregion

		public EntityManager(Texture2D etcTextures, Texture2D defaultSpritesheet, AudioManager audioManager, SoundLibrary soundLibrary, ParticleManager particleManager) {
			this.EtcTextures = etcTextures;
			this.DefaultSpritesheet = defaultSpritesheet;
			this.audioManager = audioManager;
			this.soundLibrary = soundLibrary;
			this.particleManager = particleManager;
		}

		#region Methods
		public void Update(GameTime gameTime) {
			for (int i = 0; i < entities.Count; i++) {
				entities[i].Update(gameTime);

				if (entities[i] is Unit) {
					if (((Unit)entities[i]).NeedsRemoval) {
						entities.RemoveAt(i);
						i--;
					}
				}
			}
		}

		public void UpdateInteration(GameTime gameTime, InputState input, Camera2D camera) {
			foreach (var entity in entities) {
				entity.UpdateInteraction(gameTime, input, camera);
			}
		}

		public void Draw(GameTime gameTime, ScreenManager screenManager) {
			DebugOverlay.DebugText.Append((float)((Math.Sin(gameTime.TotalGameTime.TotalSeconds * 6) / 4 + .5))).AppendLine();

			if (SelectedUnit != null) {
				// If the unit hasn't moved, highlight the movable tiles
				if (!SelectedUnit.Moved) {
					foreach (var tile in SelectedUnit.GetTravelableTiles()) {
						if (!mapData.Barriers.Contains(tile) && (tile.X >= 0 && tile.X < mapData.NumberColumns) && (tile.Y >= 0 && tile.Y < mapData.NumberRows)) {
							var drawRec = new Rectangle(tile.X * tileSize, tile.Y * tileSize, tileSize - 1, tileSize - 1);
							float dist = Vector2.Distance(new Vector2(SelectedUnit.Tile.X, SelectedUnit.Tile.Y), new Vector2(tile.X, tile.Y));
							if (SelectedUnit.Team == controllingTeam) {
								screenManager.SpriteBatch.Draw(screenManager.BlankTexture, drawRec, Color.Blue * (float)((Math.Sin(-gameTime.TotalGameTime.TotalSeconds * 3 + dist) / 4 + .5)));
							}
							else {
								screenManager.SpriteBatch.Draw(screenManager.BlankTexture, drawRec, Color.DarkOrange * (float)((Math.Sin(-gameTime.TotalGameTime.TotalSeconds * 3 + dist) / 4 + .5)));
							}
						}
					}
				}
				// If the unit hasn't attacked, highlight attackable tiles
				if (!SelectedUnit.HasAttacked) {
					foreach (var tile in SelectedUnit.GetAttackableTiles()) {
						if (!mapData.Barriers.Contains(tile) && (tile.X >= 0 && tile.X < mapData.NumberColumns) && (tile.Y >= 0 && tile.Y < mapData.NumberRows)) {
							var drawRec = new Rectangle(tile.X * tileSize + 5, tile.Y * tileSize + 5, tileSize - 11, tileSize - 11);
							float dist = Vector2.Distance(new Vector2(SelectedUnit.Tile.X, SelectedUnit.Tile.Y), new Vector2(tile.X, tile.Y));
							screenManager.SpriteBatch.Draw(screenManager.BlankTexture, drawRec, Color.Red * (float)((Math.Sin(-gameTime.TotalGameTime.TotalSeconds * 3 + dist) / 4 + .5)));
						}
					}
				}
			}

			foreach (var entity in entities) {
				// If this is the selected unit, draw a halo around it, circling
				if (SelectedUnit == entity) {
					Unit unit = (Unit)entity;
					Vector2 origin = new Vector2(haloSrcRec.Width / 2, haloSrcRec.Height / 2);
					Vector2 offset = new Vector2(unit.Bounds.Width / 2, unit.Bounds.Height / 2);
					float rotation = (float)gameTime.TotalGameTime.TotalSeconds;
					screenManager.SpriteBatch.Draw(EtcTextures, unit.Position + offset, haloSrcRec, Color.White, rotation, origin, 1, 0, 0);
				}
				// If the mouse is hovering over this entity, draw a hover texture over it
				if (entity is Unit && entity.ContainsMouse) {
					Unit unit = (Unit)entity;
					Vector2 origin = new Vector2(hoverSrcRec.Width / 2, hoverSrcRec.Height / 2);
					Vector2 offset = new Vector2(unit.Bounds.Width / 2, unit.Bounds.Height / 2);
					float rotation = -(float)gameTime.TotalGameTime.TotalSeconds * 2;
					screenManager.SpriteBatch.Draw(EtcTextures, unit.Position + offset, hoverSrcRec, Color.White, rotation, origin, 1, 0, 0);

					// Also draw a small health bar above it
					Rectangle healthBar = new Rectangle((int)unit.Position.X, (int)unit.Position.Y - 5, unit.Bounds.Width, 3);
					screenManager.SpriteBatch.Draw(screenManager.BlankTexture, healthBar, Color.Red);
					healthBar = new Rectangle((int)unit.Position.X, (int)unit.Position.Y - 5, (int)(unit.Health / unit.MaxHealth * unit.Bounds.Width), 3);
					screenManager.SpriteBatch.Draw(screenManager.BlankTexture, healthBar, Color.Green);
				}
				entity.Draw(gameTime, screenManager);
			}
		}

		/// <summary>
		/// Returns a copy of the collection of entities contained by the entity manager.
		/// </summary>
		/// <returns></returns>
		public Entity[] GetEntities() {
			return entities.ToArray();
		}

		/// <summary>
		/// Adds entities to the manager and initializes them.
		/// </summary>
		/// <param name="entities"></param>
		public void AddEntities(params Entity[] entities) {
			foreach (var entity in entities) {
				entity.EntityManager = this;
				if (entity is Sprite) {
					if (((Sprite)entity).spriteSheet == null) {
						((Sprite)entity).spriteSheet = DefaultSpritesheet;
					}
				}
				if (entity is Unit) {
					((Unit)entity).Initialize();
				}
				entity.InvokeCreated();

				this.entities.Add(entity);
			}
		}

		public void RemoveEntities(params Entity[] entities) {
			foreach (var entity in entities) {
				this.entities.Remove(entity);
			}
		}

		public void ResetEntities() {
			foreach (var entity in entities) {
				if (entity is Unit) {
					Unit unit = entity as Unit;
					unit.Moved = false;
					unit.HasAttacked = false;
				}
			}
		}

		/// <summary>
		/// Loads map data for pathfinding.
		/// </summary>
		/// <param name="map"></param>
		public void LoadMap(Map map) {
			List<Point> barriers = new List<Point>();

			for (int i = 0; i < map.Layers["collision"].Width; i++) {
				for (int j = 0; j < map.Layers["collision"].Height; j++) {
					if (map.Layers["collision"].GetTile(i, j) == 256) {
						barriers.Add(new Point(i, j));
					}
				}
			}

			mapData = new MapData(map.Height, map.Width, Point.Zero, Point.Zero, barriers);
		}

		/// <summary>
		/// Quickly finds a path for entities to access, specifying the unit's position, destination, etc.
		/// </summary>
		/// <param name="start"></param>
		/// <param name="finish"></param>
		/// <param name="otherBarriers"></param>
		/// <param name="solution"></param>
		/// <returns></returns>
		public bool QuickFind(Point start, Point finish, List<Point> otherBarriers, out LinkedList<Point> solution) {
			List<Point> barriers = mapData.Barriers.ToList();
			barriers.AddRange(otherBarriers);
			barriers = barriers.Distinct().ToList();

			var md = new MapData(mapData.NumberColumns, mapData.NumberRows, start, finish, barriers);

			// Send to the pathfinder and return
			return pathFinder.QuickFind(md, out solution);
		}
		#endregion
	}
}
