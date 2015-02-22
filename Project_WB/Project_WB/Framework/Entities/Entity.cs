using System;
using System.Collections.Generic;
using GameStateManagement;
using Microsoft.Xna.Framework;
using MouseButton = GameStateManagement.InputState.MouseButton;

namespace Project_WB.Framework.Entities {
	/// <summary>
	/// An entity is the base fore all units, sprites, etc. It contains data shared by all
	/// extensions, and is extended by the sprite class directly.
	/// </summary>
	abstract class Entity {
		#region Fields
		/// <summary>
		/// The name the entity represents.
		/// </summary>
		public string Name = string.Empty;
		// An internal reference to the entity manager that contains this entity
		EntityManager entityManager;
		// The previous boundaries the entity resided in before the latest movement
		protected Rectangle lastBounds = Rectangle.Empty;
		// The current boundaries the entoty exists in
		public Rectangle Bounds = Rectangle.Empty;
		// A collection of any data associated with this entity
		public Dictionary<string, object> EntityData = new Dictionary<string, object>();
		#endregion

		#region Properties
		/// <summary>
		/// An internal reference to the entity manager that contains this entity
		/// </summary>
		public EntityManager EntityManager {
			get { return entityManager; }
			internal set { entityManager = value; }
		}
		// Returns whether the mouse is contained within the bounds of the entity
		public bool ContainsMouse {
			get; protected set;
		}
		#endregion

		#region Events
		// The mouse has entered the bounds of the entity
		public event EventHandler<EntityInputEventArgs> MouseEntered;
		// The mouse has exited the bounds of the entity
		public event EventHandler<EntityInputEventArgs> MouseExited;
		// The entity has been left clicked
		public event EventHandler<EntityInputEventArgs> LeftClicked;
		// The entity has been middle clicked
		public event EventHandler<EntityInputEventArgs> MiddleClicked;
		// The entity has been right clicked
		public event EventHandler<EntityInputEventArgs> RightClicked;

		// The entity has been created and initialized
		public event EventHandler<EventArgs> Created;
		#endregion

		#region Methods
		public virtual void Update(GameTime gameTime) {
			// Update the previous bounds before updating to the new bounds
			lastBounds = Bounds;
		}

		public virtual void UpdateInteraction(GameTime gameTime, InputState input, Camera2D camera) {
			// If the entity has boundaries
			if (Bounds != Rectangle.Empty) {
				// Calculate the relative mouse positions to check for interaction
				var lastRelativeMouseVector = camera.ToOldRelativePosition(input.LastMouseState.X, input.LastMouseState.Y);
				var lrmPoint = new Point((int)lastRelativeMouseVector.X, (int)lastRelativeMouseVector.Y);
				var currentRelativeMouseVector = camera.ToRelativePosition(input.CurrentMouseState.X, input.CurrentMouseState.Y);
				var crmPoint = new Point((int)currentRelativeMouseVector.X, (int)currentRelativeMouseVector.Y);

				// Check for the mouse entering or exiting the bounds
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

				// Check for any clicking within the bounds
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

		/// <summary>
		/// Invokes the Created event withing the entity.
		/// </summary>
		public void InvokeCreated() {
			if (Created != null) {
				Created.Invoke(this, EventArgs.Empty);
			}
		}
		#endregion
	}
}
