using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameStateManagement;
using MouseButton = GameStateManagement.InputState.MouseButton;

namespace Project_WB.Framework.Gui.Controls {
	/// <summary>
	/// The base class for all gui controls. Contains essentials such as boundaries
	/// and reference to an internal gui manager.
	/// </summary>
	class Control {
		#region Fields
		protected internal GuiManager GuiManager;
		protected internal Control Parent;

		// New an old bounds for interaction
		protected Rectangle lastBounds = Rectangle.Empty;
		public Rectangle Bounds = Rectangle.Empty;

		public bool Enabled = true;

		// Used for removal from manager
		internal bool needsRemoval = false;
		#endregion

		#region Properties
		/// <summary>
		/// Gets the position in screen space of the control.
		/// </summary>
		public Vector2 GlobalPosition {
			get {
				if (Parent != null) {
					return new Vector2(Bounds.X, Bounds.Y) +
							Parent.GlobalPosition;
				}
				return new Vector2(Bounds.X, Bounds.Y); 
			}
		}
		/// <summary>
		/// Gets the inherited bounds of the control from parents if applicable.
		/// </summary>
		public Rectangle GlobalBounds {
			get {
				if (Parent != null) {
					return new Rectangle(Bounds.X + (int)Parent.GlobalPosition.X, Bounds.Y + (int)Parent.GlobalPosition.Y,
										Bounds.Width, Bounds.Height);
				}
				return new Rectangle((int)GlobalPosition.X, (int)GlobalPosition.Y,
										Bounds.Width, Bounds.Height);
			}
		}
		protected Rectangle LastGlobalBounds {
			get {
				if (Parent != null) {
					return new Rectangle(lastBounds.X + (int)Parent.GlobalPosition.X, Bounds.Y + (int)Parent.GlobalPosition.Y,
										lastBounds.Width, lastBounds.Height);
				}
				return new Rectangle((int)GlobalPosition.X, (int)GlobalPosition.Y,
										lastBounds.Width, lastBounds.Height);
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
			if (GlobalBounds != Rectangle.Empty) {
				// Last mouse point
				var lmPoint = new Point(input.LastMouseState.X, input.LastMouseState.Y);
				// Current mouse point
				var cmPoint = new Point(input.CurrentMouseState.X, input.CurrentMouseState.Y);

				if (GlobalBounds.Contains(cmPoint)) {
					ContainsMouse = true;
				}
				else {
					ContainsMouse = false;
				}
				if (!LastGlobalBounds.Contains(lmPoint) && GlobalBounds.Contains(cmPoint)) {
					if (MouseEntered != null)
						MouseEntered.Invoke(this, EventArgs.Empty);
				}
				else if (LastGlobalBounds.Contains(lmPoint) && !GlobalBounds.Contains(cmPoint)) {
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
