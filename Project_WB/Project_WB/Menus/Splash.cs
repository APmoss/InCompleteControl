using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameStateManagement;

namespace Project_WB.Menus {
	/// <summary>
	/// A simple screen to show before all others; Displays producer, developer, etc.
	/// </summary>
	class Splash : GameScreen {
		// The amount of time to stay here
		const int SPLASH_TIME = 6;

		// Elapsed time to count length
		TimeSpan elapsedTime = TimeSpan.Zero;

		public Splash() {
			TransitionOnTime = TimeSpan.FromSeconds(.5);
			TransitionOffTime = TimeSpan.FromSeconds(.3);
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			elapsedTime += gameTime.ElapsedGameTime;

			// Check if we have been here long enough
			if (elapsedTime.TotalSeconds >= SPLASH_TIME) {
				ExitScreen();
				ScreenManager.AddScreen(new Title(), null);
			}

			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}

		public override void Draw(GameTime gameTime) {
			ScreenManager.SpriteBatch.Begin();

			//TODO:remove this
			ScreenManager.SpriteBatch.DrawString(ScreenManager.FontLibrary.Centaur, "STUFF STUFF BLAH BAKERLAND LABS", new Vector2(4), Color.DarkBlue * TransitionAlpha);
			ScreenManager.SpriteBatch.DrawString(ScreenManager.FontLibrary.HighTowerText, "DRAMATIC SPLASH SCREEEENNNNNNNN", new Vector2(4, 300), Color.DarkBlue * TransitionAlpha);

			ScreenManager.SpriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
