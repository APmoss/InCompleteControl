// Made by Alex Pabst
// Publicly available

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
		// The amount of which the linear interpolation travels (.1 = 10%).
		public float TransitionStrength = .1f;
		// A local copy of the game's viewport.
		public Viewport Viewport;

		// The actual position of the camera
		Vector2 lastPosition = Vector2.Zero;
		Vector2 position = Vector2.Zero;
		// The actual scale of the camera
		float lastScale = 1f;
		float scale = 1f;
		// The actual rotations of the camera
		float lastZRotation = 0f;
		float zRotation = 0f;
		float lastXRotation = 0f;
		float xRotation = 0f;
		float lastYRotation = 0f;
		float yRotation = 0f;

		/// <summary>
		/// The position that the camera will transition towards.
		/// </summary>
		public Vector2 DestPosition = Vector2.Zero;
		/// <summary>
		/// The scale that the camera will transition towards.
		/// </summary>
		public float DestScale = 1f;
		/// <summary>
		/// The rotation in radians that the camera will transition towards.
		/// Z rotation is like turning your computer screen into portrait mode.
		/// </summary>
		public float DestZRotation = 0f;
		/// <summary>
		/// The rotation in radians that the camera will transition towards.
		/// X rotation is like looking at the ceiling.
		/// </summary>
		public float DestXRotation = 0f;
		/// <summary>
		/// The rotation in radians that the camera will transition towards.
		/// Y rotation is like looking at the stalker behind you.
		/// </summary>
		public float DestYRotation = 0f;
		#endregion

		public Camera2D(Viewport viewport) {
			this.Viewport = viewport;
		}

		#region Methods
		public void Update() {
			// Update the old transformations
			lastPosition = position;
			lastScale = scale;
			lastZRotation = zRotation;
			lastXRotation = xRotation;
			lastYRotation = yRotation;

			// Perform a linear interpolation from the current camera position to the destination
			position = Vector2.Lerp(position, DestPosition, TransitionStrength);

			// Make sure that the destination scale is actually between the minimum and maximum amounts
			DestScale = MathHelper.Clamp(DestScale, MIN_SCALE, MAX_SCALE);
			// Interpolate from the current scale to the destination scale
			scale = MathHelper.Lerp(scale, DestScale, TransitionStrength);

			// Interpolate from the current rotation in radians to the destination rotation
			zRotation = MathHelper.Lerp(zRotation, DestZRotation, TransitionStrength);
			xRotation = MathHelper.Lerp(xRotation, DestXRotation, TransitionStrength);
			yRotation = MathHelper.Lerp(yRotation, DestYRotation, TransitionStrength);
		}

		#region Matrix Transformations
		/// <summary>
		/// Returns a matrix that applies the translations, the scale, and the rotation.
		/// </summary>
		/// <returns></returns>
		public Matrix GetMatrixTransformation() {
			return
				// Translate to the position
				Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
				// Rotate in radians
				Matrix.CreateRotationX(xRotation) *
				Matrix.CreateRotationY(yRotation) *
				// We must flatten the 3 dimensional transformations because of the spritebatch viewing depth limitations.
				//Matrix.CreateScale(1, 1, 0) *
				Matrix.CreateRotationZ(zRotation) *
				// Apply scale
				Matrix.CreateScale(scale) *
				// Translate to the center of the screen (all camera modifcations are relative to the center)
				Matrix.CreateTranslation(new Vector3(Viewport.Width / 2, Viewport.Height / 2, 0));
		}

		/// <summary>
		/// Returns a matrix that applies the OLD translations, the scale, and the rotation.
		/// Used when considering old mouse positions, compared to new mouse positions.
		/// </summary>
		/// <returns></returns>
		protected Matrix GetOldMatrixTransformation() {
			return
				// Translate to the old position
				Matrix.CreateTranslation(new Vector3(-lastPosition.X, -lastPosition.Y, 0)) *
				// Rotate in old radians
				Matrix.CreateRotationX(lastXRotation) *
				Matrix.CreateRotationY(lastYRotation) *
				// We must flatten the 3 dimensional transformations because of the spritebatch viewing depth limitations.
				//Matrix.CreateScale(1, 1, 0) *
				Matrix.CreateRotationZ(lastZRotation) *
				// Apply old scale
				Matrix.CreateScale(lastScale) *
				// Translate to the center of the screen (all camera modifcations are relative to the center)
				Matrix.CreateTranslation(new Vector3(Viewport.Width / 2, Viewport.Height / 2, 0));
		}
		#endregion

		/// <summary>
		/// Returns a rectangle that covers the current viewing area.
		/// Since rectangles are axis-aligned, it will stretch to the furthest coordinates.
		/// </summary>
		/// <returns></returns>
		public Rectangle GetViewingRectangle() {
			// Retrieve all coordinate extremes for each corner
			Vector2 topLeft = ToRelativePosition(0, 0);
			Vector2 topRight = ToRelativePosition(Viewport.Width, 0);
			Vector2 bottomLeft = ToRelativePosition(0, Viewport.Height);
			Vector2 bottomRight = ToRelativePosition(Viewport.Width, Viewport.Height);

			// Get the extreme boundaries for each side
			int leftBound = (int)MathHelper.Min(MathHelper.Min(topLeft.X, topRight.X), MathHelper.Min(bottomLeft.X, bottomRight.X));
			int rightBound = (int)MathHelper.Max(MathHelper.Max(topLeft.X, topRight.X), MathHelper.Max(bottomLeft.X, bottomRight.X));
			int topBound = (int)MathHelper.Min(MathHelper.Min(topLeft.Y, topRight.Y), MathHelper.Min(bottomLeft.Y, bottomRight.Y));
			int bottomBound = (int)MathHelper.Max(MathHelper.Max(topLeft.Y, topRight.Y), MathHelper.Max(bottomLeft.Y, bottomRight.Y));

			// Test stuff
			// return new Rectangle(leftBound + 20, topBound + 20, rightBound - leftBound - 20, bottomBound - topBound - 20);

			// Return a new rectangle containing the viewing rectangle of the visible area
			return new Rectangle(leftBound, topBound, rightBound - leftBound, bottomBound - topBound);
		}

		/// <summary>
		/// Returns the current velocity of the camera as a vector.
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

		#region Position Transformations
		/// <summary>
		/// Converts a screen position into a position relative to the modifications of the camera.
		/// For example, a mouse position will be the same if you move around in the world,
		/// but applying this will convert the mouse's static position to "follow" with the world.
		/// </summary>
		/// <param name="screenPosition"></param>
		/// <returns></returns>
		public Vector2 ToRelativePosition(Vector2 screenPosition) {
			// Transform with the inverse of the transformation matrix.
			// Without inverse, you would convert relative position to screen position.

			//Vector3 thing = Vector3.Transform(new Vector3(screenPosition, 0), Matrix.Invert(GetMatrixTransformation()));
			//return new Vector2(thing.X, thing.Y);
			return Vector2.Transform(screenPosition, Matrix.Invert(GetMatrixTransformation()));
		}
		// Overloaded with X and Y coordinates
		public Vector2 ToRelativePosition(int x, int y) {
			return ToRelativePosition(new Vector2(x, y));
		}

		/// <summary>
		/// The exact same mothod as ToRelativePosition, but using old camera transformations.
		/// Use this when comparing old mouse positions to new mouse positions.
		/// </summary>
		/// <param name="screenPosition"></param>
		/// <returns></returns>
		public Vector2 ToOldRelativePosition(Vector2 screenPosition) {
			return Vector2.Transform(screenPosition, Matrix.Invert(GetOldMatrixTransformation()));
		}
		// Overloaded using X and Y coordinates
		public Vector2 ToOldRelativePosition(int x, int y) {
			return ToOldRelativePosition(new Vector2(x, y));
		}

		/// <summary>
		/// Returns an object's position on the screen, particularly the window.
		/// Similar to ToRelativePosition, but transforms a world position into the point
		/// in line with the top left of the window.
		/// </summary>
		/// <param name="relativePosition"></param>
		/// <returns></returns>
		public Vector2 ToScreenPosition(Vector2 relativePosition) {
			// Transform WITHOUT the inverse of the transformation matrix.
			// With inverse, you would convert screen position to relative position.
			return Vector2.Transform(relativePosition, GetMatrixTransformation());
		}
		// Overloaded with X and Y coordinates
		public Vector2 ToScreenPosition(int x, int y) {
			return ToScreenPosition(new Vector2(x, y));
		}
		#endregion
		#endregion
	}
}
