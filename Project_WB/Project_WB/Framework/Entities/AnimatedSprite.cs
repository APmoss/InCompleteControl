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

		protected bool rotationalAnimation = false;
		AnimationState animationState = AnimationState.Normal;
		protected List<Rectangle> UpSourceRectangles = new List<Rectangle>();
		protected List<Rectangle> DownSourceRectangles = new List<Rectangle>();
		protected List<Rectangle> LeftSourceRectangles = new List<Rectangle>();
		protected List<Rectangle> RightSourceRectangles = new List<Rectangle>();
		#endregion

		public AnimatedSprite() { }

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
			float angularDirection = (float)Math.Atan2(-Velocity.Y, Velocity.X);

			if (rotationalAnimation) {
				Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X) - MathHelper.PiOver2;

				if (Velocity == Vector2.Zero) {
					Rotation = 0;
				}
			}

			if (angularDirection < 3 * MathHelper.PiOver4 && angularDirection > MathHelper.PiOver4) {
				animationState = AnimationState.MovingUp;
			}
			else if (angularDirection > 3 * MathHelper.PiOver4 || angularDirection < -3 * MathHelper.PiOver4) {
				animationState = AnimationState.MovingLeft;
			}
			else if (angularDirection > -3 * MathHelper.PiOver4 && angularDirection < -MathHelper.PiOver4) {
				animationState = AnimationState.MovingDown;
			}
			else if (angularDirection < MathHelper.PiOver4 && angularDirection > -MathHelper.PiOver4) {
				animationState = AnimationState.MovingRight;
			}
			if (Velocity.X == 0 && Velocity.Y == 0) {
				animationState = AnimationState.Normal;
			}

			elapsedAnimationTime += gameTime.ElapsedGameTime;

			if (elapsedAnimationTime > TargetAnimationTime) {
				elapsedAnimationTime -= TargetAnimationTime;

				currentFrame++;

				if (rotationalAnimation) {
					if (currentFrame >= DownSourceRectangles.Count) {
						currentFrame = 0;
					}
				}
				else {
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
			}
			
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, GameStateManagement.ScreenManager screenManager) {
			screenManager.SpriteBatch.Draw(spriteSheet, Position + new Vector2(Bounds.Width / 2, Bounds.Height / 2), GetNextFrame(), Tint, Rotation, new Vector2(Bounds.Width / 2, Bounds.Height / 2), Scale, SpriteEffects, 0);
		}

		public Rectangle GetNextFrame() {
			if (rotationalAnimation) {
				return DownSourceRectangles[currentFrame];
			}
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
