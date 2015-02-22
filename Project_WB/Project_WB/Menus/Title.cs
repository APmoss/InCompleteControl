using System;
using System.Collections.Generic;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Project_WB.Framework.IO;

namespace Project_WB.Menus {
	class Title : GameScreen {
		Texture2D title;
		SoundEffectInstance music;

		public Title() {
			TransitionOnTime = TimeSpan.FromSeconds(.5);
			TransitionOffTime = TimeSpan.FromSeconds(.5);
		}

		public override void Activate(bool instancePreserved) {
			title = ScreenManager.Game.Content.Load<Texture2D>("textures/title");
			music = ScreenManager.SoundLibrary.GetSound("dicksInDetention").CreateInstance();
			music.Play();
			music.Volume = IOManager.LoadSettings().MusicVolume;

			base.Activate(instancePreserved);
		}

		public override void HandleInput(GameTime gameTime, InputState input) {
			KeyboardState k = (KeyboardState)input.CurrentKeyboardStates.GetValue(0);

			if (k.GetPressedKeys().Length > 0 && ScreenState == GameStateManagement.ScreenState.Active) {
				music.Stop();
				ExitScreen();
				ScreenManager.AddScreen(new SignIn(), null);
			}

			base.HandleInput(gameTime, input);
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}

		public override void Draw(GameTime gameTime) {
			ScreenManager.SpriteBatch.Begin();

			ScreenManager.SpriteBatch.Draw(title, new Rectangle(0, 0, Stcs.XRes, Stcs.YRes), Color.White * TransitionAlpha);

			ScreenManager.SpriteBatch.DrawString(ScreenManager.FontLibrary.HighTowerText, "Press any key to continue...", new Vector2(290, Stcs.YRes - 200),
				Color.Red * (float)((Math.Sin(gameTime.TotalGameTime.TotalSeconds * 6) / 4 + .375)) * TransitionAlpha);

			ScreenManager.SpriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
