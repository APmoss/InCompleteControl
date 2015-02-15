using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameStateManagement;
using Microsoft.Xna.Framework.Graphics;

namespace Project_WB.Gameplay {
	class Sprite : Entity {
		public Vector2 Position = Vector2.Zero;
		public Color Tint = Color.White;
		public float Scale = 1;
		public float RotationDegrees = 0;
		public SpriteEffects SpriteEffects = SpriteEffects.None;

		Rectangle sourceRectangle = Rectangle.Empty;
		Texture2D spriteSheet;

		public Sprite(Rectangle sourceRectangle, Texture2D spriteSheet) {
			this.sourceRectangle = sourceRectangle;
			this.spriteSheet = spriteSheet;
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, ScreenManager screenManager) {
			screenManager.SpriteBatch.Draw(spriteSheet, Position, sourceRectangle, Tint, RotationDegrees, Vector2.Zero, Scale, SpriteEffects, 0);
			
			base.Draw(gameTime, screenManager);
		}
	}
}
