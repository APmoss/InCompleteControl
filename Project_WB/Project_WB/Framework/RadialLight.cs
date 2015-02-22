using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Project_WB.Framework {
	class RadialLight {
		public Vector2 Position = Vector2.Zero;
		public Vector2 Scale =  Vector2.Zero;
		public Color Tint = Color.White;

		public RadialLight(Vector2 position, Vector2 scale, Color tint) {
			this.Position = position;
			this.Scale = scale;
			this.Tint = tint;
		}
	}
}
