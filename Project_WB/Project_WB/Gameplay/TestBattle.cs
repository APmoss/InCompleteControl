using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Project_WB.Framework.Squared.Tiled;
using Microsoft.Xna.Framework.Input;
using Project_WB.Framework.Entities.Units;
using GameStateManagement;

namespace Project_WB.Gameplay {
	class TestBattle : Battle {
		public override void Activate(bool instancePreserved) {
			map = Map.Load(@"maps\river.tmx", ScreenManager.Game.Content);

			base.Activate(instancePreserved);

			Centurion cen = new Centurion();
			entityManager.AddEntities(cen);
			cen.Tile = new Point(3, 3);
		}

		public override void HandleInput(GameTime gameTime, InputState input) {
			PlayerIndex p = PlayerIndex.One;

			if (input.IsNewKeyPress(Keys.Escape, null, out p)) {
				ExitScreen();
			}
			
			base.HandleInput(gameTime, input);
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			

			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}
	}
}
