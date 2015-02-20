using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameStateManagement;

namespace Project_WB.Framework.Entities {
	class Unit : AnimatedSprite {
		#region Fields
		bool isSelected = false;
		LinkedList<Point> Waypoints = new LinkedList<Point>();

		//remove?
		int haloOrbPadding = 5;
		#endregion

		#region Properties
		public bool IsSelected {
			get { return isSelected; }
			set{
				// Was not previously selected, but is now (Not Selected -> Selected)
				if ((isSelected == false) && value) {
					if (Selected != null) {
						Selected.Invoke(this, EventArgs.Empty);
					}
				}
				// Was previously selected, but not anymore (Selected -> Not Selected)
				else if (isSelected && (value == false)) {
					if (Deselected != null) {
						Deselected.Invoke(this, EventArgs.Empty);
					}
				}

				isSelected = value;
			}
		}
		#endregion

		#region Events
		public event EventHandler<EventArgs> Selected;
		public event EventHandler<EventArgs> Deselected;
		public event EventHandler<HotkeyEventArgs> HotkeyPressed;
		#endregion

		public Unit(Texture2D spriteSheet) : base(spriteSheet) {
			LeftClicked += delegate {
				IsSelected = true;
			};
		}
		public Unit(Texture2D spriteSheet,
						List<Rectangle> upSourceRectangles,
						List<Rectangle> downSourceRectangles,
						List<Rectangle> leftSourceRectangles,
						List<Rectangle> rightSourceRectangles) : base(spriteSheet) {

			SetSourceRectangles(upSourceRectangles, downSourceRectangles, leftSourceRectangles, rightSourceRectangles);

			LeftClicked += delegate(object sender, EntityInputEventArgs e) {
				EntityManager.SelectedEntity = this;

				IsSelected = true;
			};
		}

		#region Methods
		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
		}

		public override void UpdateInteraction(GameTime gameTime, GameStateManagement.InputState input, Camera2D camera) {
			base.UpdateInteraction(gameTime, input, camera);
		}

		public override void Draw(GameTime gameTime, ScreenManager screenManager) {
			//remove?
			if (ContainsMouse) {
				int haloX = (int)(Position.X - haloOrbPadding + (Math.Cos(gameTime.TotalGameTime.TotalSeconds * 2) + 1) * (Bounds.Width / 2 + haloOrbPadding));
				int haloY = (int)(Position.Y - haloOrbPadding + (Math.Sin(gameTime.TotalGameTime.TotalSeconds * 2) + 1) * (Bounds.Height / 2 + haloOrbPadding));

				Rectangle rect = new Rectangle(haloX, haloY, 3, 3);

				screenManager.SpriteBatch.Draw(screenManager.BlankTexture, rect, Color.White);
			}

			base.Draw(gameTime, screenManager);
		}
		#endregion
	}
}
