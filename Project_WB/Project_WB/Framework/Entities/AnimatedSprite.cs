using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project_WB.Framework.Entities {
	/// <summary>
	/// An animated sprite that extends the sprite class. Contains directional
	/// and rotational animation, variable animation frames, and variable frame positions.
	/// </summary>
	class AnimatedSprite : Sprite {
		/// <summary>
		/// The animation that should currently be playing for the sprite.
		/// </summary>
		protected enum AnimationState {
			Normal, MovingUp, MovingDown, MovingLeft, MovingRight, Frozen
		}

		#region Fields
		// The current frame for drawing
		int currentFrame = 0;
		// The current count toward the target animation time
		TimeSpan elapsedAnimationTime = TimeSpan.Zero;
		// The target amount of time for moving to the next frame of animation
		public TimeSpan TargetAnimationTime = TimeSpan.FromSeconds(.1);

		/// <summary>
		/// Decides whether to rotate the sprite for drawing or the have separate frames for each direction.
		/// </summary>
		protected bool rotationalAnimation = false;
		// The sprite's animation state (more above)
		AnimationState animationState = AnimationState.Normal;
		// The source regions that we are drawing from
		protected List<Rectangle> UpSourceRectangles = new List<Rectangle>();
		protected List<Rectangle> DownSourceRectangles = new List<Rectangle>();
		protected List<Rectangle> LeftSourceRectangles = new List<Rectangle>();
		protected List<Rectangle> RightSourceRectangles = new List<Rectangle>();
		#endregion
		// Base constructor using the default sprite sheet
		public AnimatedSprite() { }

		// Overloaded with a custom, non default sprite sheet
		public AnimatedSprite(Texture2D spriteSheet) : base(spriteSheet) { }

		// Overloaded with a custom sprite sheet and initial source rectangles
		public AnimatedSprite(Texture2D spriteSheet, List<Rectangle> upSourceRectangles, List<Rectangle> downSourceRectangles,
								List<Rectangle> leftSourceRectangles, List<Rectangle> rightSourceRectangles) : base(spriteSheet) {

			SetSourceRectangles(upSourceRectangles, downSourceRectangles, leftSourceRectangles, rightSourceRectangles);
		}

		#region Methods
		public override void Update(GameTime gameTime) {
			// If the sprite is moving
			if (Velocity != Vector2.Zero) {
				// Calculate the angular direction based on the velocity
				float angularDirection = (float)Math.Atan2(-Velocity.Y, Velocity.X);

				// Rotate the sprite itself if we are using rotational animation
				if (rotationalAnimation) {
					Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X) - MathHelper.PiOver2;
				}

				// Calculate what animation state to use depending on the direction we are travelling
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
			}

			elapsedAnimationTime += gameTime.ElapsedGameTime;

			// Check if we need to change frames
			if (elapsedAnimationTime > TargetAnimationTime) {
				elapsedAnimationTime -= TargetAnimationTime;

				// Add to the current frame to move forward
				currentFrame++;

				// Since we only use down sprites for rotational animation, just check that for rolling over current frame
				if (rotationalAnimation) {
					if (currentFrame >= DownSourceRectangles.Count) {
						currentFrame = 0;
					}
				}
				else {
					// Check for other animation states in frame animation to see if we need to roll current framer over
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
			// Draw the sprite itself with the frame needed
			screenManager.SpriteBatch.Draw(spriteSheet, Position + new Vector2(Bounds.Width / 2, Bounds.Height / 2), GetNextFrame(), Tint, Rotation, new Vector2(Bounds.Width / 2, Bounds.Height / 2), Scale, SpriteEffects, 0);
		}

		/// <summary>
		/// Returns the next needed frame of animation depending on the animation state and the current animation frame.
		/// </summary>
		/// <returns></returns>
		public Rectangle GetNextFrame() {
			// Just return the bottom rectangles if we are using rotational animation
			if (rotationalAnimation) {
				return DownSourceRectangles[currentFrame];
			}
			// Otherwise return the frame needed for the animation we are using
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

		/// <summary>
		/// Sets the sprite's frames of animation for each direction, if needed.
		/// </summary>
		/// <param name="upSourceRectangles"></param>
		/// <param name="downSourceRectangles"></param>
		/// <param name="leftSourceRectangles"></param>
		/// <param name="rightSourceRectangles"></param>
		public void SetSourceRectangles(List<Rectangle> upSourceRectangles, List<Rectangle> downSourceRectangles,
										List<Rectangle> leftSourceRectangles, List<Rectangle> rightSourceRectangles) {

			// Check if the parameters are valid. If good, set our values, otherwise just use an empty rectangle.
			this.UpSourceRectangles = upSourceRectangles.Count > 0 ? upSourceRectangles : new List<Rectangle>();
			this.DownSourceRectangles = downSourceRectangles.Count > 0 ? downSourceRectangles : new List<Rectangle>();
			this.LeftSourceRectangles = leftSourceRectangles.Count > 0 ? leftSourceRectangles : new List<Rectangle>();
			this.RightSourceRectangles = rightSourceRectangles.Count > 0 ? rightSourceRectangles : new List<Rectangle>();

			// Set up bounds depending on rectangle dimensions
			if (downSourceRectangles.Count > 0) {
				this.Bounds.Width = downSourceRectangles[0].Width;
				this.Bounds.Height = downSourceRectangles[0].Height;
			}
		}
		#endregion
	}
}
