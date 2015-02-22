using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Project_WB.Framework.Entities.Units {
	class BakerBot : Centurion {
		Texture2D baker;

		public BakerBot(Texture2D spriteSheet, Texture2D baker) : base(spriteSheet) {
			Name = "BakerBot";

			this.baker = baker;
		}

		public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, GameStateManagement.ScreenManager screenManager) {
			screenManager.SpriteBatch.Draw(baker, new Rectangle((int)Position.X, (int)Position.Y - 16, 32, 32), Color.White);
			
			base.Draw(gameTime, screenManager);
		}
	}
}
