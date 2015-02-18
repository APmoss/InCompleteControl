using System;
using System.Collections.Generic;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;
using Microsoft.Xna.Framework.Input;

namespace Project_WB {
	/// <summary>
	/// An extension of InputControl that allows password input using the specified filler character.
	/// </summary>
	class PasswordInputControl : InputControl {
		#region Fields
		// The filler character that will be used instead showing the password
		char fillerCharacter = '*';
		// The actual password entered, NOT shown in the box.
		// Access using GetInternalText.
		string internalText = string.Empty;
		#endregion

		public PasswordInputControl(char fillerCharacter) {
			this.fillerCharacter = fillerCharacter;
		}

		#region Overridden Methods
		// Override when a character has been entered
		protected override void OnCharacterEntered(char character) {
			// Change the box text to the real password (still NOT shown)
			Text = internalText;
			
			// Change the box with the character entered.
			base.OnCharacterEntered(character);

			// Reformat the text in the box
			ReformatText();
		}

		// Override when a key has been pressed
		protected override bool OnKeyPressed(Keys keyCode) {
			// Change the box text to the real password (still NOT shown)
			Text = internalText;

			// Apply the text change and get the return bool
			bool ret = base.OnKeyPressed(keyCode);

			// Reformat the text
			ReformatText();

			// Return using the return bool
			return ret;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Reformats the text box, setting the real password into the filler characters
		/// and moving the caret into the correct position.
		/// </summary>
		protected void ReformatText() {
			// Update the real password that was just entered
			internalText = Text;

			// Create a copy of the caret position
			int caretTemp = CaretPosition;

			// Reset the text
			Text = "";

			// Fill in spots with filler characters
			foreach (var letter in internalText) {
				Text += fillerCharacter;
			}

			// Set the caret position back to what it was
			CaretPosition = caretTemp;
		}

		/// <summary>
		/// Returns the actual password that was entered into the box.
		/// </summary>
		/// <returns></returns>
		public string GetInternalText() {
			return internalText;
		}
		#endregion
	}
}
