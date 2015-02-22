using System;
using System.Collections.Generic;
using GameStateManagement;
using Project_WB.Menus;
using Microsoft.Xna.Framework;

namespace Project_WB.Framework.Gui.Controls {
	class InputBox : Button {
		bool isPasswordField = false;
		string headingText = string.Empty;
		public string internalText = string.Empty;
		ScreenManager screenManager;

		public InputBox(int x, int y, int width, int height, bool isPasswordField, string headingText, ScreenManager screenManager)
			: base(x, y, width, string.Empty) {

			this.Bounds.Height = height;
			this.isPasswordField = isPasswordField;
			this.headingText = headingText;
			this.screenManager = screenManager;
			this.Tint = new Color(20, 20, 50);
		}

		protected internal override void Initialize() {
			InputScreen inputScreen = new InputScreen(headingText, isPasswordField);
			inputScreen.Finished += delegate {
				internalText = inputScreen.Text;
				if (isPasswordField) {
					Text = inputScreen.GetPasswordText();
				}
				else {
					Text = inputScreen.Text;
				}
			};

			// A delegate inside of a lambda statement!
			LeftClicked += (s, e) => {
				screenManager.AddScreen(inputScreen, null);
			};
			
			base.Initialize();
		}
	}
}
