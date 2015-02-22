using System;
using System.Collections.Generic;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Project_WB.Framework.Entities;
using Nuclex.Input;
using Nuclex.Input.Devices;
using Microsoft.Xna.Framework.Graphics;

namespace Project_WB.Framework.Entities {
	class EntityManager {
		//TODO: finish documentation
		
		#region Fields
		protected readonly Rectangle haloSrcRec = new Rectangle(0, 0, 48, 48);

		List<Entity> entities = new List<Entity>();
		protected internal int tileSize = 32;
		public Texture2D EtcTextures;
		public Texture2D DefaultSpritesheet;

		Unit selectedUnit;
		#endregion

		#region Properties
		public Unit SelectedUnit {
			get { return selectedUnit; }
			set {
				foreach (var entity in entities) {
					if (entity is Unit) {
						((Unit)entity).IsSelected = false;
					}
				}

				selectedUnit = value;
			}
		}
		#endregion

		public EntityManager(Texture2D etcTextures) {
			this.EtcTextures = etcTextures;
		}
		public EntityManager(Texture2D etcTextures, Texture2D defaultSpritesheet) {
			this.EtcTextures = etcTextures;
			this.DefaultSpritesheet = defaultSpritesheet;
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
				if (SelectedUnit == entity) {
					Unit unit = (Unit)entity;
					Vector2 origin = new Vector2(haloSrcRec.Width / 2, haloSrcRec.Height / 2);
					Vector2 offset = new Vector2(unit.Bounds.Width / 2, unit.Bounds.Height / 2);
					float rotation = (float)gameTime.TotalGameTime.TotalSeconds;
					screenManager.SpriteBatch.Draw(EtcTextures, unit.Position + offset, haloSrcRec, Color.White, rotation, origin, 1, 0, 0);
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

				this.entities.Add(entity);
			}
		}
		#endregion
	}
}
