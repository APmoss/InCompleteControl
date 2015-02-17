using System;
using System.Collections.Generic;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;
using Microsoft.Xna.Framework.Input;

namespace Project_WB {
	class PasswordInputControl : InputControl {
		char fillerCharacter = '*';
		string internalText = string.Empty;

		public PasswordInputControl(char fillerCharacter) {
			this.fillerCharacter = fillerCharacter;
		}

		public string GetInternalText() {
			return internalText;
		}

		protected override void OnCharacterEntered(char character) {
			Text = internalText;
			
			base.OnCharacterEntered(character);

			ReformatText();
		}

		protected override bool OnKeyPressed(Keys keyCode) {
			Text = internalText;

			bool ret = base.OnKeyPressed(keyCode);

			ReformatText();

			return ret;
		}

		protected void ReformatText() {
			internalText = Text;

			int caretTemp = CaretPosition;

			Text = "";

			foreach (var letter in internalText) {
				Text += fillerCharacter;
			}

			CaretPosition = caretTemp;
		}
	}
}
