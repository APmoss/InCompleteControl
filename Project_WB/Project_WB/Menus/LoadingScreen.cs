using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;

namespace Project_WB.Menus {
	/// <summary>
	/// The loading screen is the screen we see when the game must handle content
	/// that may take longer than usual. Access it using the LoadingScreen.Load() method.
	/// </summary>
	class LoadingScreen : GameScreen {
		List<GameScreen> screensToLoad = new List<GameScreen>();

		// Protected constructor, access this using the LoadingScreen.Load() method
		protected LoadingScreen(List<GameScreen> screensToLoad) {
			this.screensToLoad = screensToLoad;

			TransitionOffTime = TimeSpan.FromSeconds(.3);
			TransitionOnTime = TimeSpan.FromSeconds(.3);
		}

		#region Overridden Methods
		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			// If there are screens still transitioning off, then we don't want to continue
			if (ScreenManager.GetScreens().Length <= 1) {
				// Exit this loading screen
				ExitScreen();

				// Add all screens to be added
				foreach (var screen in screensToLoad) {
					ScreenManager.AddScreen(screen, null);
				}

				// Reset the game's elapsed time
				ScreenManager.Game.ResetElapsedTime();
			}

			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}

		public override void Draw(GameTime gameTime) {
			ScreenManager.SpriteBatch.Begin();

			//ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture, viewport, Color.Black * TransitionAlpha);
			ScreenManager.SpriteBatch.DrawString(ScreenManager.FontLibrary.Centaur, "Loading the codes", new Vector2(10, 10), Color.White);

			ScreenManager.SpriteBatch.End();

			base.Draw(gameTime);
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Used when loading large screens to exit all screens currently open,
		/// add a loading screen, and to launch all loaded screens.
		/// </summary>
		/// <param name="screenManager"></param>
		/// <param name="screensToLoad"></param>
		public static void Load(ScreenManager screenManager, params GameScreen[] screensToLoad) {
			// Exit all screen that are open
			screenManager.ExitAllScreens();

			// Add the loading screen
			screenManager.AddScreen(new LoadingScreen(new List<GameScreen>(screensToLoad)), null);
		}
		#endregion
	}
}
