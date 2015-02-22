using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Project_WB.Framework.Gui.Controls {
	class Label : Control {
		#region Fields
		public string Text = string.Empty;
		public Color TextTint = Color.White;
		public Color BackgroundTint = new Color(50, 50, 50, 50);
		#endregion

		public Label(int x, int y, string text) {
			this.Bounds.X = x;
			this.Bounds.Y = y;
			this.Text = text;
		}

		#region Methods
		protected internal override void Initialize() {
			if (Bounds.Width == 0) {
				Bounds.Width = (int)GuiManager.font.MeasureString(Text).X + GuiManager.Padding * 2;
			}
			if (Bounds.Height == 0) {
				Bounds.Height = (int)GuiManager.font.MeasureString(Text).Y + GuiManager.Padding * 2;
			}
			
			base.Initialize();
		}

		public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, GameStateManagement.ScreenManager screenManager) {
			Vector2 textCenter = GuiManager.font.MeasureString(Text) * GuiManager.TextScale / 2;
			Vector2 center = GlobalPosition +
								new Vector2(Bounds.Width / 2, Bounds.Height / 2) -
								textCenter;

			screenManager.SpriteBatch.Draw(screenManager.BlankTexture, GlobalBounds, BackgroundTint);

			screenManager.SpriteBatch.DrawString(GuiManager.font, Text, center, TextTint);
			
			base.Draw(gameTime, screenManager);
		}
		#endregion
	}
}
