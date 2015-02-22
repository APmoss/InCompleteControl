using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project_WB.Framework.Entities.Units {
	class BakerBot : VerticalCenturion {
		Texture2D baker;

		public BakerBot(Texture2D baker) {
			Name = "BakerBot";

			this.baker = baker;
		}

		public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, GameStateManagement.ScreenManager screenManager) {
			screenManager.SpriteBatch.Draw(baker, new Rectangle((int)Position.X, (int)Position.Y - 16, 32, 32), Color.White);

			base.Draw(gameTime, screenManager);
		}
	}
}
