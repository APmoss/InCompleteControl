using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameStateManagement;
using MouseButton = GameStateManagement.InputState.MouseButton;

namespace Project_WB.Framework.Entities {
	abstract class Entity {
		//TODO: finish documentation

		#region Fields
		public string Name = string.Empty;
		protected Rectangle lastBounds = Rectangle.Empty;
		public Rectangle Bounds = Rectangle.Empty;
		#endregion

		#region Properties
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

		#region Methods
		public virtual void Update(GameTime gameTime) {
			lastBounds = Bounds;
		}

		public virtual void UpdateInteraction(GameTime gameTime, InputState input, Camera2D camera) {
			if (Bounds != Rectangle.Empty) {
				var lastRelativeMouseVector = camera.ToOldRelativePosition(input.LastMouseState.X, input.LastMouseState.Y);
				var lrmPoint = new Point((int)lastRelativeMouseVector.X, (int)lastRelativeMouseVector.Y);
				var currentRelativeMouseVector = camera.ToRelativePosition(input.CurrentMouseState.X, input.CurrentMouseState.Y);
				var crmPoint = new Point((int)currentRelativeMouseVector.X, (int)currentRelativeMouseVector.Y);
				if (!lastBounds.Contains(lrmPoint) && Bounds.Contains(crmPoint)) {
					ContainsMouse = true;
					if (MouseEntered != null)
						MouseEntered.Invoke(this, EventArgs.Empty);
				}
				else if (lastBounds.Contains(lrmPoint) && !Bounds.Contains(crmPoint)) {
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
		#endregion
	}
}
