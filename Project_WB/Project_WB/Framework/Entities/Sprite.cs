using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project_WB.Framework.Entities {
	/// <summary>
	/// A simple sprite that has fields for position, velocity, and multiple
	/// drawing items. This sprite cannot be animated unless it is an AnimatedSprite.
	/// </summary>
	class Sprite : Entity {
		#region Fields
		/// <summary>
		/// The current world position of the sprite.
		/// </summary>
		public Vector2 Position = Vector2.Zero;
		/// <summary>
		/// The currently travelling velocity of the sprite, which
		/// is set to 0 before each update.
		/// </summary>
		public Vector2 Velocity = Vector2.Zero;
		/// <summary>
		/// The tint of the sprite, colored when drawn.
		/// </summary>
		public Color Tint = Color.White;
		/// <summary>
		/// The scale size multiple of the sprite when drawn.
		/// </summary>
		public float Scale = 1;
		/// <summary>
		/// The rotation in radians of the sprite when drawn.
		/// </summary>
		public float Rotation = 0;
		/// <summary>
		/// The sprite effects applied when drawn.
		/// </summary>
		public SpriteEffects SpriteEffects = SpriteEffects.None;

		// The single source rectangle from a spritesheet for drawing
		Rectangle sourceRectangle = Rectangle.Empty;
		// The custom sprite sheet that can be used to draw the sprite
		protected internal Texture2D spriteSheet;
		#endregion

		public Sprite() {
		}

		// Overloaded to load a custom sprite sheet instead of the default one
		public Sprite(Texture2D spriteSheet) {
			this.spriteSheet = spriteSheet;
		}

		// Overloaded to load a custom source rectangle on a custom sprite sheet
		public Sprite(Rectangle sourceRectangle, Texture2D spriteSheet) {
			this.sourceRectangle = sourceRectangle;
			this.spriteSheet = spriteSheet;
			this.Bounds.Width = sourceRectangle.Width;
			this.Bounds.Height = sourceRectangle.Height;
		}

		#region Methods
		public override void Update(GameTime gameTime) {
			// Move the sprite depending on velocity
			Position += Velocity;

			base.Update(gameTime);

			// Update the positional bounds of the sprite
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
