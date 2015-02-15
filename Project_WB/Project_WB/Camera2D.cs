using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project_WB {
	class Camera2D {
		private const float MAX_SCALE = 500;
		private const float MIN_SCALE = .001f;

		public float TransitionStrength = .1f;
		Viewport viewport;

		Vector2 position = Vector2.Zero;
		float scale = 1f;
		float rotationDegrees = 0f;

		public Vector2 DestPosition = Vector2.Zero;
		public float DestScale = 1f;
		public float DestRotationDegrees = 0f;

		public Camera2D(Viewport viewport) {
			this.viewport = viewport;
		}

		public void Update() {
			position.X = MathHelper.Lerp(position.X, DestPosition.X, TransitionStrength);
			position.Y = MathHelper.Lerp(position.Y, DestPosition.Y, TransitionStrength);

			scale = MathHelper.Lerp(scale, DestScale, TransitionStrength);
			scale = MathHelper.Clamp(scale, MIN_SCALE, MAX_SCALE);

			rotationDegrees = MathHelper.Lerp(rotationDegrees, DestRotationDegrees, TransitionStrength);
			//rotationDegrees = MathHelper.ToDegrees(MathHelper.WrapAngle(MathHelper.ToRadians(rotationDegrees)));

			DebugOverlay.DebugText.Append("Scale: ").Append(scale).AppendLine();
			DebugOverlay.DebugText.Append("Rotation: ").Append(rotationDegrees).AppendLine();
			
		}

		public Matrix GetMatrixTransformation() {
			return
				Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
				Matrix.CreateRotationZ(MathHelper.ToRadians(rotationDegrees)) *
				Matrix.CreateScale(scale) *
				Matrix.CreateTranslation(new Vector3(viewport.Width / 2, viewport.Height / 2, 0));
		}

		public float GetScale() {
			return scale;
		}

		public Vector2 ToRelativePosition(Vector2 position) {
			return Vector2.Transform(position, Matrix.Invert(GetMatrixTransformation()));
		}
	}
}
