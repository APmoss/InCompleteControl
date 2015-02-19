using System;
using System.Collections.Generic;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Project_WB.Framework {
	/// <summary>
	/// An overlay that contains many useful debug tools, such as a framerate counter.
	/// </summary>
	class DebugOverlay : GameScreen {
		#region Fields
		// Framerate variables used to calculate framerate
		int frameRate = 0;
		int frameCounter = 0;
		TimeSpan elapsedTime = TimeSpan.Zero;

		// Static variables for displaying text or changing visibility
		public static StringBuilder DebugText = new StringBuilder();
		public static bool IsVisible = false;
		#endregion

		public DebugOverlay() {
			IsPopup = true;
			OverrideInput = true;
		}

		#region Overridden Methods
		public override void HandleInput(GameTime gameTime, InputState input) {
			// If F3 is pressed, debug visibility will be toggled.
			PlayerIndex p;
			if (input.IsNewKeyPress(Keys.F3, null, out p)) {
				IsVisible = !IsVisible;
			}

			base.HandleInput(gameTime, input);
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			// Calculate the delta time, and adjust the framerate accordingly
			elapsedTime += gameTime.ElapsedGameTime;

			if (elapsedTime.TotalSeconds > 1) {
				elapsedTime -= TimeSpan.FromSeconds(1);
				frameRate = frameCounter;
				frameCounter = 0;
			}

			base.Update(gameTime, otherScreenHasFocus, false);
		}

		public override void Draw(GameTime gameTime) {
			// Add to the total frames rendered per second
			frameCounter++;

			// If it's visible, show the debuggin text
			if (IsVisible) {
				// Compile the output cleanly
				StringBuilder output = new StringBuilder();

				output.Append("Debug-").AppendLine();
				output.Append("FPS: ").Append(frameRate).AppendLine();
				output.Append(DebugText);

				// Draw it, with two shades in case of reading difficulty
				ScreenManager.SpriteBatch.Begin();

				ScreenManager.SpriteBatch.DrawString(ScreenManager.FontLibrary.Consolas, output, Vector2.Zero, Color.DarkGray, 0, Vector2.Zero, .4f, SpriteEffects.None, 0);
				ScreenManager.SpriteBatch.DrawString(ScreenManager.FontLibrary.Consolas, output, new Vector2(1), Color.LightGray, 0, Vector2.Zero, .4f, SpriteEffects.None, 0);

				ScreenManager.SpriteBatch.End();
			}

			// Clear the debug text to allow future appends
			DebugText.Clear();

			base.Draw(gameTime);
		}
		#endregion
	}
}
