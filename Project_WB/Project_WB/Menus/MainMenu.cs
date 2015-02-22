using System;
using System.Collections.Generic;
using GameStateManagement;
using Project_WB.Framework.Gui;
using Project_WB.Framework.Gui.Controls;
using Project_WB.Framework.IO;
using Microsoft.Xna.Framework;
using Project_WB.Gameplay;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Project_WB.Menus {
	class MainMenu : GameScreen {
		#region Fields
		GuiManager gui;
		Texture2D background;
		SoundEffectInstance music;
		#endregion

		public MainMenu() {
			TransitionOnTime = TimeSpan.FromSeconds(.5);
			TransitionOffTime = TimeSpan.FromSeconds(.5);
		}

		#region Methods
		public override void Activate(bool instancePreserved) {
			SetGui();

			background = ScreenManager.Game.Content.Load<Texture2D>("textures/gameModeScreen");
			music = ScreenManager.SoundLibrary.GetSound("holySax").CreateInstance();
			music.Play();
			music.Volume = IOManager.LoadSettings().MusicVolume;

			base.Activate(instancePreserved);
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			gui.Update(gameTime);

			timeLabel.Text = DateTime.Now.ToShortTimeString();
			
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
		Label welcomeLabel;
		Label timeLabel;
		Button tutorialButton;
		Button multiButton;
		Panel modePanel;
		Button logoutButton;

		Label debugLabel;
		Button debugButton1;
		Button debugButton2;
		Panel debugPanel;

		private void SetGui() {
			if (!Session.OfflineMode) {
				welcomeLabel = new Label(10, 10, string.Format("Welcome {0} {1}!", Session.FirstName, Session.LastName));
			}
			else {
				welcomeLabel = new Label(10, 10, "Welcome [OFFLINE MODE]!");
			}

			timeLabel = new Label(Stcs.XRes - 210, 10, DateTime.Now.ToShortTimeString());
			timeLabel.Bounds.Width = 200;

			tutorialButton = new Button(10, 10, 200, "Tutorial");
			tutorialButton.Tint = new Color(30, 30, 30);
			tutorialButton.LeftClicked += delegate {
				ExitScreen();
				music.Stop();
				ScreenManager.AddScreen(new Tutorial(), null);
			};

			multiButton = new Button(10, 60, 200, "Multiplayer- 1v1");
			multiButton.Tint = new Color(30, 30, 30);
			multiButton.LeftClicked += delegate {
				ExitScreen();
				music.Stop();
				ScreenManager.AddScreen(new Multi1v1(), null);
			};

			modePanel = new Panel(Stcs.XRes / 2 - 110, 100, 220, 110);
			modePanel.AddChild(tutorialButton);
			modePanel.AddChild(multiButton);

			logoutButton = new Button(Stcs.XRes - 110, Stcs.YRes - 60, 100, "Logout");
			logoutButton.Tint = new Color(30, 30, 30);
			logoutButton.LeftClicked += delegate {
				Session.ResetSession();
				music.Stop();
				ScreenManager.AddScreen(new SignIn(), null);
				ExitScreen();
			};

			debugLabel = new Label(5, 10, "Beware: Prototype levels");
			debugLabel.TextTint = Color.Red;

			debugButton1 = new Button(10, 60, 100, "L1");
			debugButton1.LeftClicked += delegate {
				music.Stop();
				ScreenManager.AddScreen(new TestThing(), null);
			};

			debugButton2 = new Button(120, 60, 100, "M2");
			debugButton2.LeftClicked += delegate {
				music.Stop();
				ScreenManager.AddScreen(new MiniGame(), null);
			};

			debugPanel = new Panel(Stcs.XRes / 2 - 140, Stcs.YRes - 120, 280, 110);
			debugPanel.AddChild(debugLabel);
			debugPanel.AddChild(debugButton1);
			debugPanel.AddChild(debugButton2);

			gui = new GuiManager(ScreenManager.FontLibrary.SmallSegoeUIMono);
			gui.AddControl(welcomeLabel);
			gui.AddControl(timeLabel);
			gui.AddControl(modePanel);
			gui.AddControl(logoutButton);
			gui.AddControl(debugPanel);
		}
		#endregion
	}
}
