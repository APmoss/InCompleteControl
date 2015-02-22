using System;
using System.Collections.Generic;
using GameStateManagement;
using Project_WB.Framework.Gui;
using Project_WB.Framework.Gui.Controls;
using Project_WB.Framework.IO;
using Microsoft.Xna.Framework;
using Project_WB.Gameplay;

namespace Project_WB.Menus {
	class MainMenu : GameScreen {
		#region Fields
		GuiManager gui;
		#endregion

		public MainMenu() {
			TransitionOnTime = TimeSpan.FromSeconds(.5);
			TransitionOffTime = TimeSpan.FromSeconds(.5);
		}

		#region Methods
		public override void Activate(bool instancePreserved) {
			SetGui();

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

			gui.Draw(gameTime, ScreenManager);

			ScreenManager.SpriteBatch.End();
			
			base.Draw(gameTime);
		}
		#endregion

		#region SetGui
		Label welcomeLabel;
		Label timeLabel;
		Button newButton;
		Button multiButton;

		private void SetGui() {
			if (!Session.OfflineMode) {
				welcomeLabel = new Label(10, 10, string.Format("Welcome {0} {1}!", Session.FirstName, Session.LastName));
			}
			else {
				welcomeLabel = new Label(10, 10, "Welcome [OFFLINE MODE]!");
			}

			timeLabel = new Label(Stcs.XRes - 210, 10, DateTime.Now.ToShortTimeString());
			timeLabel.Bounds.Width = 200;

			newButton = new Button(100, 100, 300, "New");
			newButton.LeftClicked += delegate {
				ExitScreen();
				ScreenManager.AddScreen(new Tutorial(), null);
			};

			multiButton = new Button(100, 160, 300, "Multiplayer- 1v1");
			//multiButton.LeftClicked

			gui = new GuiManager(ScreenManager.FontLibrary.SmallSegoeUIMono);
			gui.AddControl(welcomeLabel);
			gui.AddControl(timeLabel);
			gui.AddControl(newButton);
			gui.AddControl(multiButton);
		}
		#endregion
	}
}
