using System;
using System.Collections.Generic;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Project_WB.Menus {
	class Title : GameScreen {
		public Title() {
			TransitionOnTime = TimeSpan.FromSeconds(.3);
			TransitionOffTime = TimeSpan.FromSeconds(.3);
		}

		public override void HandleInput(GameTime gameTime, InputState input) {
			KeyboardState k = (KeyboardState)input.CurrentKeyboardStates.GetValue(0);

			if (k.GetPressedKeys().Length > 0) {
				ExitScreen();
				ScreenManager.AddScreen(new SignIn(), null);
			}

			base.HandleInput(gameTime, input);
		}

		public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}

		public override void Draw(Microsoft.Xna.Framework.GameTime gameTime) {
			//TODO: Change the drawing stuff
			ScreenManager.SpriteBatch.Begin();

			ScreenManager.SpriteBatch.DrawString(ScreenManager.FontLibrary.Centaur, "JUST PRESS A BUTTON ALREADY", new Vector2(100, 300), Color.Red * TransitionAlpha);

			ScreenManager.SpriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
