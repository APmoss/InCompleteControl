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
			Ballistarius bal = new Ballistarius();
			Accensus acc = new Accensus();
			entityManager.AddEntities(cen, bal, acc);
			cen.Tile = new Point(4, 3);
			bal.Tile = new Point(5, 3);
			acc.Tile = new Point(6, 3);
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
