using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MouseButton = GameStateManagement.InputState.MouseButton;


using GameStateManagement;namespace Project_WB.Framework.Gui.Controls {
	/// <summary>
	/// A control that has a slider value that can change by dragging the notch left or right.
	/// </summary>
	class Slider : Control {
		#region Fields
		public bool DrawValue = true;
		float value = 0;
		float caretX = 0;
		#endregion

		#region Properties
		public float Value {
			get { return value; }
			set {
				if (this.value != value) {
					if (ValueChanged != null) {
						ValueChanged.Invoke(this, EventArgs.Empty);
					}
				}

				this.value = value;
				caretX = (float)(GlobalBounds.X + GuiManager.Padding + (GlobalBounds.Width - GuiManager.Padding * 2) * this.value);
			}
		}
		#endregion

		#region Events
		public event EventHandler<EventArgs> ValueChanged;
		#endregion

		public Slider(int x, int y, int width, int height) : this(x, y, width, height, .5f){
		}
		public Slider(int x, int y, int width, int height, float startingValue) {
			Bounds.X = x;
			Bounds.Y = y;
			Bounds.Width = width;
			Bounds.Height = height;
			value = startingValue;
		}

		#region Methods
		public override void Update(Microsoft.Xna.Framework.GameTime gameTime) {
			caretX = (float)(GlobalBounds.X + GuiManager.Padding + (GlobalBounds.Width - GuiManager.Padding * 2) * value);

			base.Update(gameTime);
		}

		public override void UpdateInteraction(InputState input) {
			// Update the caret position depending on the mouse position on the control
			if (ContainsMouse && input.IsMousePressed(MouseButton.Left)) {
				caretX = input.CurrentMouseState.X;
				caretX = MathHelper.Clamp(caretX, GlobalBounds.X + GuiManager.Padding, GlobalBounds.Right - GuiManager.Padding);
				double fraction = (caretX - (GlobalBounds.X + GuiManager.Padding)) / (GlobalBounds.Width - GuiManager.Padding * 2);
				value = (float)Math.Round(fraction, 2);
			}

			base.UpdateInteraction(input);
		}

		public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, GameStateManagement.ScreenManager screenManager) {
			// Calculate center of the text
			Vector2 textCenter = GuiManager.font.MeasureString(value.ToString()) * GuiManager.TextScale / 2;
			Vector2 center = GlobalPosition +
								new Vector2(Bounds.Width / 2, Bounds.Height / 2) -
								textCenter;

			// Draw the background
			screenManager.SpriteBatch.Draw(screenManager.BlankTexture, GlobalBounds, new Color(50, 50, 50, 50));
			// Draw the center line
			Rectangle centerLine = new Rectangle(GlobalBounds.X, (int)(GlobalBounds.Y + GlobalBounds.Height / 2 - .5), GlobalBounds.Width, 1);
			screenManager.SpriteBatch.Draw(screenManager.BlankTexture, centerLine, new Color(150, 0, 0, 50));
			// Draw the ticks
			double tickValue = 0f;
			for (int i = 0; i <= 100; i++) {
				Rectangle tick = new Rectangle((int)(GlobalBounds.X + GuiManager.Padding + (GlobalBounds.Width - GuiManager.Padding * 2) * tickValue), (int)(centerLine.Y - 1.5), 1, 3);
				screenManager.SpriteBatch.Draw(screenManager.BlankTexture, tick, new Color(150, 0, 0, 50));
				tickValue += .01;
			}
			// Draw the caret
			Rectangle caret = new Rectangle((int)caretX, GlobalBounds.Y + GuiManager.Padding, 1, GlobalBounds.Height - GuiManager.Padding * 2);
			screenManager.SpriteBatch.Draw(screenManager.BlankTexture, caret, new Color(0, 150, 0, 50));

			if (DrawValue) {
				//Draw the value text
				screenManager.SpriteBatch.DrawString(GuiManager.font, value.ToString(), center, Color.White, 0, Vector2.Zero, GuiManager.TextScale, 0, 0);
			}

			base.Draw(gameTime, screenManager);
		}
		#endregion
	}
}
