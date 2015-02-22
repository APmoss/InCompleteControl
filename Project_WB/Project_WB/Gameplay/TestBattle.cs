using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Project_WB.Gameplay {
	class TestBattle : Battle {
		public override void HandleInput(Microsoft.Xna.Framework.GameTime gameTime, GameStateManagement.InputState input) {
			PlayerIndex p = PlayerIndex.One;

			if (input.IsNewKeyPress(Microsoft.Xna.Framework.Input.Keys.Escape, null, out p)) {
				ExitScreen();
			}
			
			base.HandleInput(gameTime, input);
		}
	}
}
