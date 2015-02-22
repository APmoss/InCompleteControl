using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameStateManagement;
using Microsoft.Xna.Framework.Graphics;

namespace Project_WB.Framework.Entities {
	class Sprite : Entity {
		//TODO: finish documentation

		#region Fields
		public Vector2 Position = Vector2.Zero;
		public Vector2 Velocity = Vector2.Zero;
		public Color Tint = Color.White;
		public float Scale = 1;
		public float Rotation = 0;
		public SpriteEffects SpriteEffects = SpriteEffects.None;

		Rectangle sourceRectangle = Rectangle.Empty;
		protected Texture2D spriteSheet;
		#endregion

		public Sprite(Texture2D spriteSheet) {
			this.spriteSheet = spriteSheet;
		}

		public Sprite(Rectangle sourceRectangle, Texture2D spriteSheet) {
			this.sourceRectangle = sourceRectangle;
			this.spriteSheet = spriteSheet;
			this.Bounds.Width = sourceRectangle.Width;
			this.Bounds.Height = sourceRectangle.Height;
		}

		#region Methods
		public override void Update(GameTime gameTime) {
			Position += Velocity;

			base.Update(gameTime);

			Bounds.X = (int)Position.X;
			Bounds.Y = (int)Position.Y;
		}

		public override void Draw(GameTime gameTime, ScreenManager screenManager) {
			screenManager.SpriteBatch.Draw(spriteSheet, Position, sourceRectangle, Tint, Rotation, Vector2.Zero, Scale, SpriteEffects, 0);
			
			base.Draw(gameTime, screenManager);
		}
		#endregion
	}
}
