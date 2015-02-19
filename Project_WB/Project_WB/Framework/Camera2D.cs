using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project_WB.Framework {
	/// <summary>
	/// A 2 dimensional camera. Can be used to apply translations, scales, rotations and more to a scene.
	/// </summary>
	class Camera2D {
		#region Constants
		// Maximum and minimum scale amounts
		private const float MAX_SCALE = 100;
		private const float MIN_SCALE = .01f;
		#endregion

		#region Fields
		/// <summary>
		/// The amount of which the linear interpolation travels (.1 = 10%).
		/// </summary>
		public float TransitionStrength = .1f;
		// A local copy of the game's viewport.
		public Viewport Viewport;

		// The actual position of the camera
		Vector2 position = Vector2.Zero;
		// The actual scale of the camera
		float scale = 1f;
		// The actual rotation of the camera
		float rotationDegrees = 0f;

		/// <summary>
		/// The position that the camera will transition towards.
		/// </summary>
		public Vector2 DestPosition = Vector2.Zero;
		/// <summary>
		/// The scale that the camera will transition towards.
		/// </summary>
		public float DestScale = 1f;
		/// <summary>
		/// The rotation in degrees that the camera will transition towards.
		/// </summary>
		public float DestRotationDegrees = 0f;
		#endregion

		public Camera2D(Viewport viewport) {
			this.Viewport = viewport;
		}

		#region Methods
		public void Update() {
			// Perform a linear interpolation from the current camera position to the destination
			position = Vector2.Lerp(position, DestPosition, TransitionStrength);

			// Make sure that the destination scale is actually between the minimum and maximum amounts
			DestScale = MathHelper.Clamp(DestScale, MIN_SCALE, MAX_SCALE);
			// Interpolate from the current scale to the destination scale
			scale = MathHelper.Lerp(scale, DestScale, TransitionStrength);			

			// Interpolate from the current rotation in degrees to the destination rotation
			rotationDegrees = MathHelper.Lerp(rotationDegrees, DestRotationDegrees, TransitionStrength);			
		}

		/// <summary>
		/// Returns a matrix that applies the translations, the scale, and the rotation.
		/// </summary>
		/// <returns></returns>
		public Matrix GetMatrixTransformation() {
			return
				// Translate to the position
				Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
				// Rotate in radians
				Matrix.CreateRotationZ(MathHelper.ToRadians(rotationDegrees)) *
				// Apply scale
				Matrix.CreateScale(scale) *
				// Translate to the center of the screen (all camera modifcations are relative to the center)
				Matrix.CreateTranslation(new Vector3(Viewport.Width / 2, Viewport.Height / 2, 0));
		}

		/// <summary>
		/// Returns a rectangle that covers the current viewing area.
		/// Since rectangles are axis-aligned, it will stretch to the furthest coordinates.
		/// </summary>
		/// <returns></returns>
		public Rectangle GetViewingRectangle() {
			//TODO: Implement transformation modifications
			int x = (int)(position.X - Viewport.Width / (2 * scale)) + 10;
			int y = (int)(position.Y - Viewport.Height / (2 * scale)) + 10;
			return new Rectangle(x, y, (int)(Viewport.Width / scale) - 20, (int)(Viewport.Height / scale) - 20);
		}

		/// <summary>
		/// Returns the current velociy of the camera as a vector.
		/// </summary>
		/// <returns></returns>
		public Vector2 GetCurrentVelocity() {
			// Return where the camera would travel minus where the camera currently is
			return Vector2.Lerp(position, DestPosition, TransitionStrength) - position;
		}

		/// <summary>
		/// Returns the actual current scale of the camera.
		/// </summary>
		/// <returns></returns>
		public float GetScale() {
			return scale;
		}

		/// <summary>
		/// Converts a screen position into a position relative to the modifications of the camera.
		/// For example, a mouse position will be the same if you move around in the world,
		/// but applying this will convert the mouse's static position to "follow" with the world.
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public Vector2 ToRelativePosition(Vector2 position) {
			// Transform with the inverse of the transformation matrix.
			// Without inverse, you would convert relative position to screen position.
			return Vector2.Transform(position, Matrix.Invert(GetMatrixTransformation()));
		}
		// Overloaded with X and Y coordinates
		public Vector2 ToRelativePosition(int x, int y) {
			return ToRelativePosition(new Vector2(x, y));
		}
		#endregion
	}
}
