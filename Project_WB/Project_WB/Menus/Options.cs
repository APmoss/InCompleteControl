using System;
using System.Collections.Generic;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Project_WB.Framework.Gui;
using Project_WB.Framework.Gui.Controls;
using Project_WB.Framework.IO;

namespace Project_WB.Menus {
	class Options : GameScreen {
		#region Fields
		GuiManager gui;
		SettingsFile settings;
		#endregion

		public Options() {
			IsPopup = true;
		}

		#region Methods
		public override void Activate(bool instancePreserved) {
			settings = IOManager.LoadSettings();

			SetGui();
			
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
			ScreenManager.FadeBackBuffer(TransitionAlpha * .5f, Color.Black);

			ScreenManager.SpriteBatch.Begin();

			gui.Draw(gameTime, ScreenManager);

			ScreenManager.SpriteBatch.End();

			base.Draw(gameTime);
		}
		#endregion

		#region Methods
		protected void SaveAndClose() {
			settings.MusicVolume = musicVolumeSlider.Value;
			settings.InterfaceVolume = interfaceVolumeSlider.Value;
			settings.EnvironmentVolume = environmentVolumeSlider.Value;
			settings.VoiceVolume = voiceVolumeSlider.Value;
			IOManager.SaveSettings(settings);
			ExitScreen();
		}
		#endregion

		#region SetGui
		Label headerLabel;
		Button xButton;
		Label musicVolumeLabel;
		Slider musicVolumeSlider;
		Label interfaceVolumeLabel;
		Slider interfaceVolumeSlider;
		Label environmentVolumeLabel;
		Slider environmentVolumeSlider;
		Label voiceVolumeLabel;
		Slider voiceVolumeSlider;
		Button okButton;
		Panel optionsPanel;

		private void SetGui() {
			gui = new GuiManager(ScreenManager.FontLibrary.SmallSegoeUIMono);

			headerLabel = new Label(10, 10, "Options");

			xButton = new Button(755, 10, 35, "X");
			xButton.Tint = new Color(235, 0, 0, 50);
			xButton.LeftClicked += delegate {
				ExitScreen();
			};

			musicVolumeLabel = new Label(10, 100, "Music Volume");

			musicVolumeSlider = new Slider(410, 100, 380, 40, settings.MusicVolume);

			interfaceVolumeLabel = new Label(10, 150, "Interface Volume");

			interfaceVolumeSlider = new Slider(410, 150, 380, 40, settings.InterfaceVolume);

			environmentVolumeLabel = new Label(10, 200, "Environment Volume");

			environmentVolumeSlider = new Slider(410, 200, 380, 40, settings.EnvironmentVolume);

			voiceVolumeLabel = new Label(10, 250, "Voice Volume");

			voiceVolumeSlider = new Slider(410, 250, 380, 40, settings.VoiceVolume);

			okButton = new Button(690, 560, 100, "Ok");
			okButton.LeftClicked += delegate {
				SaveAndClose();
			};

			optionsPanel = new Panel(Stcs.XRes / 2 - 400, 50, 800, 600);
			optionsPanel.Tint = new Color(50, 50, 50, 200);
			optionsPanel.AddChild(headerLabel);
			optionsPanel.AddChild(xButton);
			optionsPanel.AddChild(musicVolumeLabel);
			optionsPanel.AddChild(musicVolumeSlider);
			optionsPanel.AddChild(interfaceVolumeLabel);
			optionsPanel.AddChild(interfaceVolumeSlider);
			optionsPanel.AddChild(environmentVolumeLabel);
			optionsPanel.AddChild(environmentVolumeSlider);
			optionsPanel.AddChild(voiceVolumeLabel);
			optionsPanel.AddChild(voiceVolumeSlider);
			optionsPanel.AddChild(okButton);

			gui.AddControl(optionsPanel);
		}
		#endregion
	}
}
