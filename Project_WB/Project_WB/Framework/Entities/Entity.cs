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
		EntityManager entityManager;
		protected Rectangle lastBounds = Rectangle.Empty;
		public Rectangle Bounds = Rectangle.Empty;
		public Dictionary<string, string> EntityData = new Dictionary<string, string>();
		#endregion

		#region Properties
		public EntityManager EntityManager {
			get { return entityManager; }
			internal set { entityManager = value; }
		}
		public bool ContainsMouse {
			get; protected set;
		}
		#endregion

		#region Events
		public event EventHandler<EntityInputEventArgs> MouseEntered;
		public event EventHandler<EntityInputEventArgs> MouseExited;
		public event EventHandler<EntityInputEventArgs> LeftClicked;
		public event EventHandler<EntityInputEventArgs> MiddleClicked;
		public event EventHandler<EntityInputEventArgs> RightClicked;
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
						MouseEntered.Invoke(this, new EntityInputEventArgs(input));
				}
				else if (lastBounds.Contains(lrmPoint) && !Bounds.Contains(crmPoint)) {
					ContainsMouse = false;
					if (MouseExited != null)
						MouseExited.Invoke(this, new EntityInputEventArgs(input));
				}

				if (input.IsNewMousePress(MouseButton.Left) && ContainsMouse) {
					if (LeftClicked != null)
						LeftClicked.Invoke(this, new EntityInputEventArgs(input));
				}
				if (input.IsNewMousePress(MouseButton.Middle) && ContainsMouse) {
					if (MiddleClicked != null)
						MiddleClicked.Invoke(this, new EntityInputEventArgs(input));
				}
				if (input.IsNewMousePress(MouseButton.Right) && ContainsMouse) {
					if (RightClicked != null)
						RightClicked.Invoke(this, new EntityInputEventArgs(input));
				}
			}
		}

		public virtual void Draw(GameTime gameTime, ScreenManager screenManager) {
		
		}
		#endregion
	}
}
