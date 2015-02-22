using System;
using System.Collections.Generic;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Project_WB.Framework.Entities;
using Nuclex.Input;
using Nuclex.Input.Devices;

namespace Project_WB.Framework.Entities {
	class EntityManager {
		//TODO: finish documentation
		
		#region Fields
		List<Entity> entities = new List<Entity>();
		public Entity SelectedEntity;
		#endregion

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
				entity.Draw(gameTime, screenManager);
			}
		}

		public Entity[] GetEntities() {
			return entities.ToArray();
		}

		public void AddEntities(params Entity[] entities) {
			foreach (var entity in entities) {
				entity.EntityManager = this;

				this.entities.Add(entity);
			}
		}
		#endregion
	}
}
