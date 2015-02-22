using System;
using System.Collections.Generic;
using Nuclex.Input;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Project_WB.Menus {
	class InputScreen : GameScreen {
		InputManager input;

		string headingText = string.Empty;
		public string Text = string.Empty;
		bool passwordField = false;

		bool flash = false;
		TimeSpan flashElapsed = TimeSpan.Zero;
		TimeSpan flashTarget = TimeSpan.FromSeconds(.5);

		public event EventHandler<EventArgs> Finished;

		public InputScreen(string headingText, bool passwordField) {
			IsPopup = true;
			TransitionOnTime = TimeSpan.FromSeconds(.5);
			TransitionOffTime = TimeSpan.FromSeconds(.5);

			this.headingText = headingText;
			this.passwordField = passwordField;
		}

		public override void Activate(bool instancePreserved) {
			input = new InputManager(ScreenManager.Game.Window.Handle);
			var keyboard = input.GetKeyboard();

			input.GetKeyboard().CharacterEntered += new Nuclex.Input.Devices.CharacterDelegate(InputScreen_CharacterEntered);

			base.Activate(instancePreserved);
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			if (input != null) {
				input.Update();
			}

			flashElapsed += gameTime.ElapsedGameTime;

			if (flashElapsed > flashTarget) {
				flashElapsed -= flashTarget;

				flash = !flash;
			}

			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}

		public override void HandleInput(GameTime gameTime, InputState input) {
			PlayerIndex p = PlayerIndex.One;

			if (input.IsNewKeyPress(Keys.Enter, null, out p)) {
				if (Finished != null) {
					Finished.Invoke(this, EventArgs.Empty);
				}

				this.input.Dispose();
				this.input = null;
				ExitScreen();
			}
			
			base.HandleInput(gameTime, input);
		}

		public override void Draw(GameTime gameTime) {
			ScreenManager.FadeBackBuffer(TransitionAlpha * 2 / 3, Color.Black);

			ScreenManager.SpriteBatch.Begin();

			var font = ScreenManager.FontLibrary.Centaur;
			Vector2 center = new Vector2(Stcs.XRes / 2 - font.MeasureString(headingText).X / 2, Stcs.YRes / 2 - font.MeasureString(headingText).Y / 2 - 200);
			ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture,
											new Rectangle((int)center.X - 10, (int)center.Y - 10, (int)font.MeasureString(headingText).X + 20, (int)font.MeasureString(headingText).Y + 20),
											Color.White * .7f * TransitionAlpha);
			ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture,
											new Rectangle((int)center.X - 5, (int)center.Y - 5, (int)font.MeasureString(headingText).X + 10, (int)font.MeasureString(headingText).Y + 10),
											Color.Black * .5f * TransitionAlpha);
			ScreenManager.SpriteBatch.DrawString(font, headingText, center, Color.White);

			string drawText = string.Empty;
			if (passwordField) {
				drawText = GetPasswordText();
			}
			else {
				drawText = Text;
			}
			font = ScreenManager.FontLibrary.SmallSegoeUIMono;
			if (passwordField) {
				center = new Vector2(Stcs.XRes / 2 - font.MeasureString(drawText).X / 2, Stcs.YRes / 2 - font.MeasureString(drawText).Y / 2);
				ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture,
												new Rectangle((int)center.X - 10, (int)center.Y - 10, (int)font.MeasureString(drawText + "_").X + 20, (int)font.MeasureString(drawText + "_").Y + 20),
												Color.White * .7f * TransitionAlpha);
				ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture,
												new Rectangle((int)center.X - 5, (int)center.Y - 5, (int)font.MeasureString(drawText + "_").X + 10, (int)font.MeasureString(drawText + "_").Y + 10),
												Color.Black * .5f * TransitionAlpha);
			}
			else {
				center = new Vector2(Stcs.XRes / 2 - font.MeasureString(drawText).X / 2, Stcs.YRes / 2 - font.MeasureString(drawText).Y / 2);
				ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture,
												new Rectangle((int)center.X - 10, (int)center.Y - 10, (int)font.MeasureString(drawText + "_").X + 20, (int)font.MeasureString(drawText + "_").Y + 20),
												Color.White * .7f * TransitionAlpha);
				ScreenManager.SpriteBatch.Draw(ScreenManager.BlankTexture,
												new Rectangle((int)center.X - 5, (int)center.Y - 5, (int)font.MeasureString(drawText + "_").X + 10, (int)font.MeasureString(drawText + "_").Y + 10),
												Color.Black * .5f * TransitionAlpha);
			}
			
			if (flash) {
				ScreenManager.SpriteBatch.DrawString(font, drawText + "_", center, Color.White * TransitionAlpha);
			}
			else {
				ScreenManager.SpriteBatch.DrawString(font, drawText, center, Color.White * TransitionAlpha);
			}

			ScreenManager.SpriteBatch.End();
			
			base.Draw(gameTime);
		}

		void InputScreen_CharacterEntered(char character) {
			if (character >= 32 && character <= 126) {
				Text += character;
			}
			else if (character == '\b' && Text.Length > 0) {
				Text = Text.Substring(0, Text.Length - 1);
			}
		}

		public string GetPasswordText() {
			string stars = string.Empty;

			for (int i = 0; i < Text.Length; i++) {
				stars += "*";
			}

			return stars;
		}
	}
}
