using System;
using System.Collections.Generic;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Project_WB.Framework.Gui.Controls;
using Project_WB.Framework.Gui;

namespace Project_WB.Menus {
	class MessageBox : GameScreen {
		#region Fields
		GuiManager gui;

		string header = string.Empty;
		string message = string.Empty;
		#endregion

		public MessageBox(string header, string message) {
			TransitionOnTime = TimeSpan.FromSeconds(.5);
			TransitionOffTime = TimeSpan.FromSeconds(.5);
			IsPopup = true;
			
			this.header = header;
			this.message = message;
		}

		public override void Activate(bool instancePreserved) {
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

		public override void Draw(Microsoft.Xna.Framework.GameTime gameTime) {
			ScreenManager.FadeBackBuffer(TransitionAlpha * 2 / 3, Color.Black);
			
			ScreenManager.SpriteBatch.Begin();

			gui.Draw(gameTime, ScreenManager);

			ScreenManager.SpriteBatch.End();
			
			base.Draw(gameTime);
		}

		#region SetGui
		Label headerLabel;
		DialogBox messageDialog;
		Button closeButton;
		Panel messagePanel;

		private void SetGui() {
			gui = new GuiManager(ScreenManager.FontLibrary.SmallSegoeUIMono);

			headerLabel = new Label(10, 10, header);

			messageDialog = new DialogBox(10, 60, 380, 330, message);

			closeButton = new Button(355, 10, 35, "X");
			closeButton.Tint = new Color(235, 0, 0, 50);
			closeButton.LeftClicked += delegate {
				ExitScreen();
			};

			messagePanel = new Panel(Stcs.XRes / 2 - 200, Stcs.YRes / 2 - 200, 400, 400);
			messagePanel.AddChild(headerLabel);
			messagePanel.AddChild(messageDialog);
			messagePanel.AddChild(closeButton);

			gui.AddControl(messagePanel);

			messageDialog.OkButton.LeftClicked += delegate {
				ExitScreen();
			};
		}
		#endregion
	}
}
