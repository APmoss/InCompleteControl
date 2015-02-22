using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace GameStateManagement {
	/// <summary>
	/// Screen that appears when a fatal error has occurred and must quit.
	/// </summary>
	class ErrorScreen : GameScreen {
		// The error message
		string message = string.Empty;

		public ErrorScreen(string message) {
			this.message = message;

			using (TextWriter writer = new StreamWriter(new FileStream("errors.txt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite))) {
				writer.WriteLine(string.Format("{0} - {1} - {2}", DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString(), message));
			}
		}

		public override void HandleInput(GameTime gameTime, InputState input) {
			PlayerIndex p = PlayerIndex.One;

			// Escape to exit
			if (input.IsNewKeyPress(Keys.Escape, null, out p)) {
				ScreenManager.Game.Exit();
			}

			base.HandleInput(gameTime, input);
		}

		public override void Draw(GameTime gameTime) {
			ScreenManager.SpriteBatch.Begin();

			// Draw the error to the screen
			ScreenManager.SpriteBatch.DrawString(ScreenManager.FontLibrary.SmallSegoeUIMono,
												"An error has occurred and has been logged- \r\n-" + message + "\r\nPress escape to exit.", Vector2.Zero, Color.White);

			ScreenManager.SpriteBatch.End();
			
			base.Draw(gameTime);
		}
	}
}
