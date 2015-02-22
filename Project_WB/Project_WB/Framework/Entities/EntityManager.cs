using System;
using System.Collections.Generic;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Project_WB.Framework.Entities;
using Microsoft.Xna.Framework.Graphics;
using Project_WB.Framework.Audio;
using Project_WB.Framework.Particles;
using Project_WB.Framework.Pathfinding;
using Project_WB.Framework.Squared.Tiled;
using System.Linq;

namespace Project_WB.Framework.Entities {
	class EntityManager {
		//TODO: finish documentation

		#region Fields
		protected readonly Rectangle haloSrcRec = new Rectangle(0, 0, 48, 48);
		protected readonly Rectangle hoverSrcRec = new Rectangle(0, 48, 48, 48);

		List<Entity> entities = new List<Entity>();
		protected internal int tileSize = 32;
		public Texture2D EtcTextures;
		public Texture2D DefaultSpritesheet;

		protected internal Random r = new Random();

		protected internal AudioManager audioManager;
		protected internal SoundLibrary soundLibrary;
		protected internal ParticleManager particleManager;

		protected PathFinder pathFinder = new PathFinder();
		protected MapData mapData;

		Unit selectedUnit;
		#endregion

		#region Properties
		public Unit SelectedUnit {
			get { return selectedUnit; }
			set {
				if (selectedUnit != value) {
					if (NewSelectedUnit != null) {
						NewSelectedUnit.Invoke(this, EventArgs.Empty);
					}

					foreach (var entity in entities) {
						if (entity is Unit) {
							((Unit)entity).IsSelected = false;
						}
					}

					selectedUnit = value;
				}
			}
		}
		#endregion

		#region Events
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
			foreach (var entity in entities) {
				entity.Update(gameTime);
			}
		}

		public void UpdateInteration(GameTime gameTime, InputState input, Camera2D camera) {
			foreach (var entity in entities) {
				entity.UpdateInteraction(gameTime, input, camera);
			}
		}

		public void Draw(GameTime gameTime, ScreenManager screenManager) {
			foreach (var entity in entities) {
				// Entity is selected
				if (SelectedUnit == entity) {
					Unit unit = (Unit)entity;
					Vector2 origin = new Vector2(haloSrcRec.Width / 2, haloSrcRec.Height / 2);
					Vector2 offset = new Vector2(unit.Bounds.Width / 2, unit.Bounds.Height / 2);
					float rotation = (float)gameTime.TotalGameTime.TotalSeconds;
					screenManager.SpriteBatch.Draw(EtcTextures, unit.Position + offset, haloSrcRec, Color.White, rotation, origin, 1, 0, 0);
				}
				// Mouse is over the entity
				if (entity is Unit && entity.ContainsMouse) {
					Unit unit = (Unit)entity;
					Vector2 origin = new Vector2(hoverSrcRec.Width / 2, hoverSrcRec.Height / 2);
					Vector2 offset = new Vector2(unit.Bounds.Width / 2, unit.Bounds.Height / 2);
					float rotation = -(float)gameTime.TotalGameTime.TotalSeconds * 2;
					screenManager.SpriteBatch.Draw(EtcTextures, unit.Position + offset, hoverSrcRec, Color.White, rotation, origin, 1, 0, 0);

					Rectangle healthBar = new Rectangle((int)unit.Position.X, (int)unit.Position.Y - 5, unit.Bounds.Width, 3);
					screenManager.SpriteBatch.Draw(screenManager.BlankTexture, healthBar, Color.Red);
					healthBar = new Rectangle((int)unit.Position.X, (int)unit.Position.Y - 5, (int)(unit.Health / unit.MaxHealth * unit.Bounds.Width), 3);
					screenManager.SpriteBatch.Draw(screenManager.BlankTexture, healthBar, Color.Green);
				}
				entity.Draw(gameTime, screenManager);
			}
		}

		public Entity[] GetEntities() {
			return entities.ToArray();
		}

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

		public bool QuickFind(Point start, Point finish, List<Point> otherBarriers, out LinkedList<Point> solution) {
			List<Point> barriers = mapData.Barriers;
			barriers.AddRange(otherBarriers);
			barriers = barriers.Distinct().ToList();

			var md = new MapData(mapData.NumberColumns, mapData.NumberRows, start, finish, barriers);

			return pathFinder.QuickFind(md, out solution);
		}
		#endregion
	}
}
