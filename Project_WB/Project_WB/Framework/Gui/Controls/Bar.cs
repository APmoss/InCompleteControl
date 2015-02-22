using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameStateManagement;

namespace Project_WB.Framework.Gui.Controls {
	/// <summary>
	/// A control that draws a simple bar in the bounds
	/// </summary>
	class Bar : Control {
		#region Fields
		public Color Tint = Color.White;
		#endregion

		public Bar(int x, int y, int width, int height, Color tint) {
			this.Bounds.X = x;
			this.Bounds.Y = y;
			this.Bounds.Width = width;
			this.Bounds.Height = height;
			this.Tint = tint;
		}

		#region Methods
		public override void Draw(GameTime gameTime, ScreenManager screenManager) {
			screenManager.SpriteBatch.Draw(screenManager.BlankTexture, GlobalBounds, Tint);			
			
			base.Draw(gameTime, screenManager);
		}
		#endregion
	}
}
