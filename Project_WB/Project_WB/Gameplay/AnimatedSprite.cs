using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Project_WB.Gameplay {
	class AnimatedSprite : Sprite {
		protected enum AnimationState {
			Normal, MovingUp, MovingDown, MovingLeft, MovingRight, Frozen
		}

		int currentFrame = 0;
		TimeSpan elapsedAnimationTime = TimeSpan.Zero;
		public TimeSpan TargetAnimationTime = TimeSpan.FromSeconds(.25);

		AnimationState animationState = AnimationState.Normal;
		protected List<Rectangle> UpSourceRectangles = new List<Rectangle>();
		protected List<Rectangle> DownSourceRectangles = new List<Rectangle>();
		protected List<Rectangle> LeftSourceRectangles = new List<Rectangle>();
		protected List<Rectangle> RightSourceRectangles = new List<Rectangle>();

		public override void Update(GameTime gameTime) {
			elapsedAnimationTime += gameTime.ElapsedGameTime;

			if (elapsedAnimationTime > TargetAnimationTime) {
				elapsedAnimationTime -= TargetAnimationTime;

				currentFrame++;

				switch (animationState) {
					case AnimationState.Normal:
					case AnimationState.MovingDown:
						if (currentFrame >= DownSourceRectangles.Count) {
							currentFrame = 0;
						}
						break;
					case AnimationState.MovingUp:
						if (currentFrame >= UpSourceRectangles.Count) {
							currentFrame = 0;
						}
						break;
					case AnimationState.MovingLeft:
						if (currentFrame >= LeftSourceRectangles.Count) {
							currentFrame = 0;
						}
						break;
					case AnimationState.MovingRight:
						if (currentFrame >= RightSourceRectangles.Count) {
							currentFrame = 0;
						}
						break;
					case AnimationState.Frozen:
						currentFrame = 0;
						break;
				}
			}
			
			base.Update(gameTime);
		}
	}
}
