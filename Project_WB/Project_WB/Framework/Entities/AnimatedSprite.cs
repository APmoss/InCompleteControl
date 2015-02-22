using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project_WB.Framework.Entities {
	class AnimatedSprite : Sprite {
		//TODO: finish documentation

		protected enum AnimationState {
			Normal, MovingUp, MovingDown, MovingLeft, MovingRight, Frozen
		}

		#region Fields
		int currentFrame = 0;
		TimeSpan elapsedAnimationTime = TimeSpan.Zero;
		public TimeSpan TargetAnimationTime = TimeSpan.FromSeconds(.1);

		AnimationState animationState = AnimationState.Normal;
		protected List<Rectangle> UpSourceRectangles = new List<Rectangle>();
		protected List<Rectangle> DownSourceRectangles = new List<Rectangle>();
		protected List<Rectangle> LeftSourceRectangles = new List<Rectangle>();
		protected List<Rectangle> RightSourceRectangles = new List<Rectangle>();
		#endregion

		public AnimatedSprite(Texture2D spriteSheet) : base(spriteSheet) { }

		public AnimatedSprite(Texture2D spriteSheet,
								List<Rectangle> upSourceRectangles,
								List<Rectangle> downSourceRectangles,
								List<Rectangle> leftSourceRectangles,
								List<Rectangle> rightSourceRectangles) : base(spriteSheet) {

			SetSourceRectangles(upSourceRectangles, downSourceRectangles, leftSourceRectangles, rightSourceRectangles);
		}

		#region Methods
		public override void Update(GameTime gameTime) {
			if (Velocity.X < 0) {
				animationState = AnimationState.MovingLeft;
			}
			else if (Velocity.X > 0) {
				animationState = AnimationState.MovingRight;
			}
			if (Velocity.Y < 0) {
				animationState = AnimationState.MovingUp;
			}
			else if (Velocity.Y > 0) {
				animationState = AnimationState.MovingDown;
			}
			if (Velocity.X == 0 && Velocity.Y == 0) {
				animationState = AnimationState.Normal;
			}

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

		public override void Draw(GameTime gameTime, GameStateManagement.ScreenManager screenManager) {
			screenManager.SpriteBatch.Draw(spriteSheet, Position, GetNextFrame(), Tint, Rotation, Vector2.Zero, Scale, SpriteEffects, 0);
		}

		public Rectangle GetNextFrame() {
			switch (animationState) {
				case AnimationState.Normal:
				case AnimationState.MovingDown:
					return DownSourceRectangles[currentFrame];
				case AnimationState.MovingUp:
					return UpSourceRectangles[currentFrame];
				case AnimationState.MovingLeft:
					return LeftSourceRectangles[currentFrame];
				case AnimationState.MovingRight:
					return RightSourceRectangles[currentFrame];
				case AnimationState.Frozen:
					if (DownSourceRectangles.Count > 0) {
						return DownSourceRectangles[0];
					}
					return Rectangle.Empty;
				default:
					return Rectangle.Empty;
			}
		}

		public void SetSourceRectangles(List<Rectangle> upSourceRectangles, List<Rectangle> downSourceRectangles,
										List<Rectangle> leftSourceRectangles, List<Rectangle> rightSourceRectangles) {

			// Check if the parameters are valid. If good, set our values, otherwise just use an empty rectangle.
			this.UpSourceRectangles = upSourceRectangles.Count > 0 ? upSourceRectangles : new List<Rectangle>();
			this.DownSourceRectangles = downSourceRectangles.Count > 0 ? downSourceRectangles : new List<Rectangle>();
			this.LeftSourceRectangles = leftSourceRectangles.Count > 0 ? leftSourceRectangles : new List<Rectangle>();
			this.RightSourceRectangles = rightSourceRectangles.Count > 0 ? rightSourceRectangles : new List<Rectangle>();

			if (downSourceRectangles.Count > 0) {
				this.Bounds.Width = downSourceRectangles[0].Width;
				this.Bounds.Height = downSourceRectangles[0].Height;
			}
		}
		#endregion
	}
}
