using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameStateManagement;

namespace Project_WB.Framework.Gui.Controls {
	class Panel : Control {
		#region Fields
		protected List<Control> Children = new List<Control>();
		public Color Tint = new Color(50, 50, 50, 50);
		#endregion

		public Panel(int x, int y, int width, int height) {
			this.Bounds.X = x;
			this.Bounds.Y = y;
			this.Bounds.Width = width;
			this.Bounds.Height = height;
		}

		#region Methods
		protected internal override void Initialize() {
			foreach (var child in Children) {
				child.GuiManager = this.GuiManager;
				child.Initialize();
			}
			
			base.Initialize();
		}

		public override void Update(GameTime gameTime) {
			foreach (var child in Children) {
				child.Update(gameTime);
			}

			base.Update(gameTime);
		}

		public override void UpdateInteraction(InputState input) {
			foreach (var child in Children) {
				child.UpdateInteraction(input);
			}
			
			base.UpdateInteraction(input);
		}

		public override void Draw(GameTime gameTime, ScreenManager screenManager) {
			screenManager.SpriteBatch.Draw(screenManager.BlankTexture, Bounds, Tint);

			foreach (var child in Children) {
				child.Draw(gameTime, screenManager);
			}
			
			base.Draw(gameTime, screenManager);
		}
		#endregion

		#region Public Methods
		public void AddChild(Control child) {
			child.GuiManager = this.GuiManager;
			child.Parent = this;

			Children.Add(child);
		}
		#endregion
	}
}
