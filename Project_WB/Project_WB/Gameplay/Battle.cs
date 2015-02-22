using System;
using System.Collections.Generic;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Project_WB.Framework;
using Project_WB.Framework.Pathfinding;
using Project_WB.Framework.Squared.Tiled;
using Project_WB.Framework.Entities;
using Project_WB.Framework.Audio;
using Project_WB.Framework.Particles;

namespace Project_WB.Gameplay {
	class Battle : GameScreen {
		#region Fields
		AudioManager audioManager;
		ParticleManager particleManager;
		EntityManager entityManager;

		Camera2D camera;

		Map map;
		#endregion

		#region Overridden Methods
		public override void Activate(bool instancePreserved) {
			base.Activate(instancePreserved);
		}

		public override void HandleInput(GameTime gameTime, InputState input) {
			base.HandleInput(gameTime, input);
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}

		public override void Draw(GameTime gameTime) {
			base.Draw(gameTime);
		}
		#endregion

		#region Methods
		#endregion
	}
}
