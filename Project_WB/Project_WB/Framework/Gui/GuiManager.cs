using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Project_WB.Framework.Gui.Controls;
using GameStateManagement;
using Microsoft.Xna.Framework.Graphics;

namespace Project_WB.Framework.Gui {
	class GuiManager {
		#region Fields
		protected List<Control> controls = new List<Control>();

		protected internal SpriteFont font;
		public Color TextTint = Color.White;
		public float TextScale = 1f;

		public int Padding = 5;
		#endregion

		public GuiManager(SpriteFont font) {
			this.font = font;
		}

		public void Update(GameTime gameTime) {
			for (int i = 0; i < controls.Count; i++) {
				if (controls[i].needsRemoval) {
					controls.RemoveAt(i);
					i--;
					continue;
				}

				controls[i].Update(gameTime);
			}
			foreach (var control in controls) {
				control.Update(gameTime);
			}
		}

		public void UpdateInteraction(InputState input) {
			foreach (var control in controls) {
				control.UpdateInteraction(input);
			}
		}

		public void Draw(GameTime gameTime, ScreenManager screenManager) {
			foreach (var control in controls) {
				control.Draw(gameTime, screenManager);
			}
		}

		#region Public Methods
		public void AddControl(Control control) {
			control.GuiManager = this;
			control.Initialize();

			controls.Add(control);
		}

		public void RemoveControl(Control control) {
			control.needsRemoval = true;
		}
		#endregion
	}
}
