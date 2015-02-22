using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameStateManagement;

namespace Project_WB.Framework.Gui.Controls {
	/// <summary>
	/// Displays text character by character with an ok button to close.
	/// </summary>
	class DialogBox : Control {
		#region Fields
		string text = string.Empty;
		public Color TextTint = Color.White;
		public Color BackgroundTint = new Color(50, 50, 50, 200);
		public TimeSpan TargetCharacterTime = new TimeSpan(0, 0, 0, 0, 50);
		TimeSpan elapsedCharacterTime = TimeSpan.Zero;

		int characterDisplays = 0;

		public bool HasButton = true;
		public Button OkButton;
		#endregion

		#region Properties
		public string Text {
			get { return text; }
			set {
				text = value;

				ReformatText();
			}
		}
		#endregion

		public DialogBox(int x, int y, int width, int height, string text) {
			this.Bounds.X = x;
			this.Bounds.Y = y;
			this.Bounds.Width = width;
			this.Bounds.Height = height;
			this.text = text;
		}

		#region Methods
		protected internal override void Initialize() {
			ReformatText();

			OkButton = new Button(GlobalBounds.Right - 110, GlobalBounds.Bottom - 50, 100, "Ok");
			OkButton.GuiManager = this.GuiManager;
			OkButton.Initialize();

			if (HasButton) {
				OkButton.LeftClicked += delegate {
					GuiManager.RemoveControl(this);
				};
			}


			base.Initialize();
		}

		public override void Update(GameTime gameTime) {
			elapsedCharacterTime += gameTime.ElapsedGameTime;
			if (elapsedCharacterTime > TargetCharacterTime) {
				elapsedCharacterTime -= TargetCharacterTime;

				if (characterDisplays < Text.Length) {
					characterDisplays++;
				}
			}

			if (HasButton) {
				OkButton.Update(gameTime);
			}			

			base.Update(gameTime);
		}

		public override void UpdateInteraction(InputState input) {
			if (HasButton) {
				OkButton.UpdateInteraction(input);
			
			}

			base.UpdateInteraction(input);
		}

		public override void Draw(GameTime gameTime, GameStateManagement.ScreenManager screenManager) {
			// Draw the dialog box background
			screenManager.SpriteBatch.Draw(screenManager.BlankTexture, GlobalBounds, BackgroundTint);
			
			// Draw the dialog box text
			screenManager.SpriteBatch.DrawString(GuiManager.font, Text.Substring(0, characterDisplays), new Vector2(GlobalBounds.X + 10, GlobalBounds.Y + 10), TextTint);

			if (HasButton) {
				//Draw the Ok button
				OkButton.Draw(gameTime, screenManager);
			}
			
			base.Draw(gameTime, screenManager);
		}

		/// <summary>
		/// Reformats the structure of the text to wrap like a dialog box.
		/// </summary>
		protected void ReformatText() {
			characterDisplays = 0;
			int position = 0;
			int count = 0;

			while (position + count < text.Length) {
				float width = GuiManager.font.MeasureString(text.Substring(position, count)).X;

				if (GuiManager.font.MeasureString(text.Substring(position, count + 1)).X > Bounds.Width - 10) {
					while (text[position + count] != ' ') {
						count--;
					}
					text = text.Remove(position + count, 1);
					text = text.Insert(position + count, "\r\n");
					position = position + count;
					count = 0;
				}
				else {
					count++;
				}
			}
		}
		#endregion
	}
}
