using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Project_WB.Framework.Entities.Units {
	class Guy : Unit {
		public Guy(Texture2D spriteSheet) : base(spriteSheet) {
			#region SetRectangles
			var usr = new List<Rectangle>() {
				new Rectangle(0, 224, 32, 32),
				new Rectangle(32, 224, 32, 32),
				new Rectangle(64, 224, 32, 32),
				new Rectangle(96, 224, 32, 32),
			};
			var dsr = new List<Rectangle>() {
				new Rectangle(0, 128, 32, 32),
				new Rectangle(32, 128, 32, 32),
				new Rectangle(64, 128, 32, 32),
				new Rectangle(96, 128, 32, 32),
			};
			var lsr = new List<Rectangle>() {
				new Rectangle(0, 160, 32, 32),
				new Rectangle(32, 160, 32, 32),
				new Rectangle(64, 160, 32, 32),
				new Rectangle(96, 160, 32, 32),
			};
			var rsr = new List<Rectangle>() {
				new Rectangle(0, 192, 32, 32),
				new Rectangle(32, 192, 32, 32),
				new Rectangle(64, 192, 32, 32),
				new Rectangle(96, 192, 32, 32),
			};

			SetSourceRectangles(usr, dsr, lsr, rsr);
			#endregion

			Name = "Guy";
		}
	}
}
