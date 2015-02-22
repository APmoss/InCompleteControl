using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameStateManagement;

namespace Project_WB.Framework.Gui.Controls {
	class Button : Control {
		#region Fields
		public string Text = string.Empty;
		public Color Tint = new Color(10, 10, 10);
		#endregion

		public Button(int x, int y, int width, string text) {
			this.Bounds.X = x;
			this.Bounds.Y = y;
			this.Bounds.Width = width;
			this.Text = text;
		}

		#region Methods
		protected internal override void Initialize() {
			this.Bounds.Height = (int)(GuiManager.font.MeasureString(Text).Y * GuiManager.TextScale) + GuiManager.Padding * 2;
			
			base.Initialize();
		}

		public override void Draw(GameTime gameTime, ScreenManager screenManager) {
			Vector2 textCenter = GuiManager.font.MeasureString(Text) * GuiManager.TextScale / 2;
			Vector2 center = GlobalPosition +
								new Vector2(Bounds.Width / 2, Bounds.Height / 2) -
								textCenter;

			screenManager.SpriteBatch.Draw(screenManager.BlankTexture, GlobalBounds, Tint);
			if (ContainsMouse) {
				screenManager.SpriteBatch.Draw(screenManager.BlankTexture, GlobalBounds, Color.White * .2f);
			}
			screenManager.SpriteBatch.DrawString(GuiManager.font, Text, center, Color.White, 0, Vector2.Zero, GuiManager.TextScale, 0, 0);
			
			base.Draw(gameTime, screenManager);
		}
		#endregion
	}
}
