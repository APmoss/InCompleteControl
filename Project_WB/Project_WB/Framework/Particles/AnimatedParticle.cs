using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project_WB.Framework.Particles {
	/// <summary>
	/// A particle extension that plays through an animation cycle.
	/// </summary>
	class AnimatedParticle : Particle {
		#region Fields
		int currentFrame = 0;
		TimeSpan elapsedAnimationTime = TimeSpan.Zero;
		public TimeSpan TargetAnimationTime = TimeSpan.FromSeconds(.25);

		protected List<Rectangle> sourceRectangles = new List<Rectangle>();
		#endregion

		public AnimatedParticle() { }

		public AnimatedParticle(List<Rectangle> sourceRectangles) {
			this.sourceRectangles = sourceRectangles;
		}

		#region Methods
		public override void Update(GameTime gameTime) {
			elapsedAnimationTime += gameTime.ElapsedGameTime;
			
			// Check to cycle through frames
			if (elapsedAnimationTime > TargetAnimationTime) {
				elapsedAnimationTime -= TargetAnimationTime;

				currentFrame++;

				if (currentFrame >= sourceRectangles.Count) {
					currentFrame = 0;
				}
			}
			
			base.Update(gameTime);
		}

		public override Rectangle GetNextSourceRectangle() {
			return sourceRectangles[currentFrame];
		}
		#endregion
	}
}
