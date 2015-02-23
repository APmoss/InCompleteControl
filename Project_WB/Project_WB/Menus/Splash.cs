using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameStateManagement;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Project_WB.Menus {
	/// <summary>
	/// A simple screen to show before all others; Displays producer, developer, etc.
	/// </summary>
	class Splash : GameScreen {
		Texture2D bllLogo;
		Vector2 bllPosition = new Vector2(-200, 100);
		float presentsAlpha = 0;
		Texture2D rfLogo;
		Vector2 rfPosition = new Vector2(1400, 500);
		float aProductionAlpha = 0;
		float byAlpha = 0;

		int wave1Count = 0, wave2Count = 0, wave3Count = 0;
		Texture2D wave1, wave2, wave3;


		// Elapsed time to count length
		TimeSpan elapsed = TimeSpan.Zero;

		public Splash() {
			TransitionOnTime = TimeSpan.FromSeconds(.5);
			TransitionOffTime = TimeSpan.FromSeconds(0);
		}

		public override void Activate(bool instancePreserved) {
			bllLogo = ScreenManager.Game.Content.Load<Texture2D>("textures/bllLogo");
			rfLogo = ScreenManager.Game.Content.Load<Texture2D>("textures/rfLogo");

			wave1 = ScreenManager.Game.Content.Load<Texture2D>("textures/wave1");
			wave2 = ScreenManager.Game.Content.Load<Texture2D>("textures/wave2");
			wave3 = ScreenManager.Game.Content.Load<Texture2D>("textures/wave3");

			base.Activate(instancePreserved);
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			elapsed += gameTime.ElapsedGameTime;

			if (elapsed.TotalSeconds >= 1) {
				bllPosition.X = MathHelper.Lerp(bllPosition.X, 300, .1f);
			}
			if (elapsed.TotalSeconds >= 2 && presentsAlpha < 1) {
				presentsAlpha += .01f;
			}
			if (elapsed.TotalSeconds >= 3 && aProductionAlpha < 1) {
				aProductionAlpha += .01f;
			}
			if (elapsed.TotalSeconds >= 4 && byAlpha < 1) {
				byAlpha += .01f;
			}
			if (elapsed.TotalSeconds >= 5) {
				rfPosition.X = MathHelper.Lerp(rfPosition.X, 600, .1f);
			}
			if (elapsed.TotalSeconds >= 10) {
				ExitScreen();
				ScreenManager.AddScreen(new Title(), null);
			}

			if (wave1Count > 96) {
				wave1Count = 0;
			}
			else {
				wave1Count += 14;
			}
			if (wave2Count > 92) {
				wave2Count = 0;
			}
			else {
				wave2Count += 12;
			}
			if (wave3Count > 64) {
				wave3Count = 0;
			}
			else {
				wave3Count += 10;
			}

			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}

		public override void HandleInput(GameTime gameTime, InputState input) {
			PlayerIndex p = PlayerIndex.One;

			if (input.IsNewKeyPress(Keys.Escape, null, out p)) {
				ExitScreen();
				ScreenManager.AddScreen(new Title(), null);
			}
			
			base.HandleInput(gameTime, input);
		}

		public override void Draw(GameTime gameTime) {
			ScreenManager.SpriteBatch.Begin();

			for (int i = -96; i < Stcs.XRes + 96; i += 96) {
				ScreenManager.SpriteBatch.Draw(wave1, new Vector2(i + wave1Count, 120), Color.White * TransitionAlpha);
			}
			for (int i = -92; i < Stcs.XRes + 92; i += 92) {
				ScreenManager.SpriteBatch.Draw(wave2, new Vector2(i - wave2Count, 310), Color.White * TransitionAlpha);
			}
			for (int i = -64; i < Stcs.XRes + 64; i += 64) {
				ScreenManager.SpriteBatch.Draw(wave3, new Vector2(i + wave3Count, 520), Color.White * TransitionAlpha);
			}

			ScreenManager.SpriteBatch.Draw(bllLogo, bllPosition, Color.White * TransitionAlpha);
			ScreenManager.SpriteBatch.DrawString(ScreenManager.FontLibrary.HighTowerText, "presents...", new Vector2(700, 100), Color.DarkGray * presentsAlpha * TransitionAlpha);
			ScreenManager.SpriteBatch.DrawString(ScreenManager.FontLibrary.HighTowerText, "a                             production...", new Vector2(200, 300), Color.DarkGray * aProductionAlpha * TransitionAlpha);
			ScreenManager.SpriteBatch.DrawString(ScreenManager.FontLibrary.Centaur, "BitBlit Interactive", new Vector2(255, 300), Color.DarkRed * aProductionAlpha * TransitionAlpha);
			ScreenManager.SpriteBatch.DrawString(ScreenManager.FontLibrary.HighTowerText, "by...", new Vector2(350, 550), Color.DarkBlue * byAlpha * TransitionAlpha);
			ScreenManager.SpriteBatch.Draw(rfLogo, rfPosition, Color.White * TransitionAlpha);

			ScreenManager.SpriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
