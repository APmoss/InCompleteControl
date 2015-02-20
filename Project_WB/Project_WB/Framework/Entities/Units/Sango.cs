using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Project_WB.Framework.Entities.Units {
	class Sango : Unit {
		public Sango(Texture2D spriteSheet) : base(spriteSheet) {
			#region SetRectangles
			var usr = new List<Rectangle>() {
				new Rectangle(0, 96, 32, 32),
				new Rectangle(32, 96, 32, 32),
				new Rectangle(64, 96, 32, 32),
				new Rectangle(96, 96, 32, 32),
			};
			var dsr = new List<Rectangle>() {
				new Rectangle(0, 0, 32, 32),
				new Rectangle(32, 0, 32, 32),
				new Rectangle(64, 0, 32, 32),
				new Rectangle(96, 0, 32, 32),
			};
			var lsr = new List<Rectangle>() {
				new Rectangle(0, 32, 32, 32),
				new Rectangle(32, 32, 32, 32),
				new Rectangle(64, 32, 32, 32),
				new Rectangle(96, 32, 32, 32),
			};
			var rsr = new List<Rectangle>() {
				new Rectangle(0, 64, 32, 32),
				new Rectangle(32, 64, 32, 32),
				new Rectangle(64, 64, 32, 32),
				new Rectangle(96, 64, 32, 32),
			};

			SetSourceRectangles(usr, dsr, lsr, rsr);
			#endregion
		}
	}
}
