using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameStateManagement;
using MouseButton = GameStateManagement.InputState.MouseButton;

namespace Project_WB.Framework.Gui.Controls {
	class Control {
		#region Fields
		protected internal GuiManager GuiManager;
		protected internal Control Parent;

		protected Rectangle lastBounds = Rectangle.Empty;
		public Rectangle Bounds = Rectangle.Empty;

		public bool Enabled = true;
		#endregion

		#region Properties
		public Vector2 GlobalPosition {
			get {
				if (Parent != null) {
					return new Vector2(Bounds.X, Bounds.Y) +
							Parent.GlobalPosition;
				}
				else {
					return new Vector2(Bounds.X, Bounds.Y); 
				}
			}
		}
		public bool ContainsMouse {
			get; protected set;
		}
		#endregion

		#region Events
		public event EventHandler<EventArgs> MouseEntered;
		public event EventHandler<EventArgs> MouseExited;
		public event EventHandler<EventArgs> LeftClicked;
		public event EventHandler<EventArgs> MiddleClicked;
		public event EventHandler<EventArgs> RightClicked;
		#endregion

		protected Control() {
			
		}

		protected internal virtual void Initialize() {

		}

		public virtual void Update(GameTime gameTime) {
			lastBounds = Bounds;
		}

		public virtual void UpdateInteraction(InputState input) {
			if (Bounds != Rectangle.Empty) {
				// Last mouse point
				var lmPoint = new Point(input.LastMouseState.X, input.LastMouseState.Y);
				// Current mouse point
				var cmPoint = new Point(input.CurrentMouseState.X, input.CurrentMouseState.Y);

				if (!lastBounds.Contains(lmPoint) && Bounds.Contains(cmPoint)) {
					ContainsMouse = true;
					if (MouseEntered != null)
						MouseEntered.Invoke(this, EventArgs.Empty);
				}
				else if (lastBounds.Contains(lmPoint) && !Bounds.Contains(cmPoint)) {
					ContainsMouse = false;
					if (MouseExited != null)
						MouseExited.Invoke(this, EventArgs.Empty);
				}

				if (input.IsNewMousePress(MouseButton.Left) && ContainsMouse) {
					if (LeftClicked != null)
						LeftClicked.Invoke(this, EventArgs.Empty);
				}
				if (input.IsNewMousePress(MouseButton.Middle) && ContainsMouse) {
					if (MiddleClicked != null)
						MiddleClicked.Invoke(this, EventArgs.Empty);
				}
				if (input.IsNewMousePress(MouseButton.Right) && ContainsMouse) {
					if (RightClicked != null)
						RightClicked.Invoke(this, EventArgs.Empty);
				}
			}
		}

		public virtual void Draw(GameTime gameTime, ScreenManager screenManager) {

		}
	}
}
