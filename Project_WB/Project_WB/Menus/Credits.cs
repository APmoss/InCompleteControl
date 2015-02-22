using System;
using System.Collections.Generic;
using GameStateManagement;
using Project_WB.Framework.Gui;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Project_WB.Framework.Gui.Controls;
using Microsoft.Xna.Framework.Audio;
using Project_WB.Framework.IO;

namespace Project_WB.Menus {
	class Credits : GameScreen {
		#region Fields
		GuiManager gui;
		Texture2D background;
		SoundEffectInstance music;
		#endregion

		public Credits() {
			TransitionOnTime = TimeSpan.FromSeconds(.5);
			TransitionOffTime = TimeSpan.FromSeconds(.5);
		}

		#region Methods
		public override void Activate(bool instancePreserved) {
			SetGui();

			music = ScreenManager.SoundLibrary.GetSound("endTitleDream").CreateInstance();
			music.Play();
			music.Volume = IOManager.LoadSettings().MusicVolume;
			background = ScreenManager.Game.Content.Load<Texture2D>("textures/credits");

			base.Activate(instancePreserved);
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			gui.Update(gameTime);
			
			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}

		public override void HandleInput(GameTime gameTime, InputState input) {
			gui.UpdateInteraction(input);
			
			base.HandleInput(gameTime, input);
		}

		public override void Draw(GameTime gameTime) {
			ScreenManager.SpriteBatch.Begin();

			ScreenManager.SpriteBatch.Draw(background, new Rectangle(0, 0, Stcs.XRes, Stcs.YRes), Color.White * TransitionAlpha);
			gui.Draw(gameTime, ScreenManager);

			ScreenManager.SpriteBatch.End();
			
			base.Draw(gameTime);
		}
		#endregion

		#region SetGui
		Button backButton;

		private void SetGui() {
			gui = new GuiManager(ScreenManager.FontLibrary.SmallSegoeUIMono);

			backButton = new Button(Stcs.XRes - 110, Stcs.YRes - 60, 100, "Back");
			backButton.LeftClicked += delegate {
				music.Stop();
				ExitScreen();
			};

			gui.AddControl(backButton);
		}
		#endregion
	}
}
