using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameStateManagement;

namespace Project_WB.Framework.Particles {
	class Particle {
		//TODO: finish documentation

		#region Fields
		public Vector2 Position = Vector2.Zero;
		public Vector2 Velocity = Vector2.Zero;
		public float Scale = 1;
		public Color Tint = Color.White;
		public float RotationDegrees = 0;
		public SpriteEffects SpriteEffects = SpriteEffects.None;

		public TimeSpan LifeSpan = TimeSpan.Zero;

		Rectangle sourceRectangle = Rectangle.Empty;
		#endregion

		public Particle() { }

		public Particle(Rectangle sourceRectangle) {
			this.sourceRectangle = sourceRectangle;
		}

		#region Methods
		public virtual void Update(GameTime gameTime) {
			Position += Velocity;

			LifeSpan -= gameTime.ElapsedGameTime;
		}

		public virtual Rectangle GetNextSourceRectangle() {
			return sourceRectangle;
		}
		#endregion
	}
}
