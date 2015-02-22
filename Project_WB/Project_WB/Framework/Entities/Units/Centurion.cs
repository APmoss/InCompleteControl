using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Project_WB.Framework.Entities.Units {
	class Centurion : Unit {
		public Centurion(Texture2D spriteSheet) : base(spriteSheet) {
			#region SetRectangles
			var usr = new List<Rectangle>() {
				new Rectangle(0, 352, 32, 32),
				new Rectangle(32, 352, 32, 32),
				new Rectangle(64, 352, 32, 32),
				new Rectangle(96, 352, 32, 32),
				new Rectangle(128, 352, 32, 32),
				new Rectangle(160, 352, 32, 32),
				new Rectangle(192, 352, 32, 32),
				new Rectangle(224, 352, 32, 32)
			};
			var dsr = new List<Rectangle>() {
				new Rectangle(0, 256, 32, 32),
				new Rectangle(32, 256, 32, 32),
				new Rectangle(64, 256, 32, 32),
				new Rectangle(96, 256, 32, 32),
				new Rectangle(128, 256, 32, 32),
				new Rectangle(160, 256, 32, 32),
				new Rectangle(192, 256, 32, 32),
				new Rectangle(224, 256, 32, 32)
			};
			var lsr = new List<Rectangle>() {
				new Rectangle(0, 288, 32, 32),
				new Rectangle(32, 288, 32, 32),
				new Rectangle(64, 288, 32, 32),
				new Rectangle(96, 288, 32, 32),
				new Rectangle(128, 288, 32, 32),
				new Rectangle(160, 288, 32, 32),
				new Rectangle(192, 288, 32, 32),
				new Rectangle(224, 288, 32, 32)
			};
			var rsr = new List<Rectangle>() {
				new Rectangle(0, 320, 32, 32),
				new Rectangle(32, 320, 32, 32),
				new Rectangle(64, 320, 32, 32),
				new Rectangle(96, 320, 32, 32),
				new Rectangle(128, 320, 32, 32),
				new Rectangle(160, 320, 32, 32),
				new Rectangle(192, 320, 32, 32),
				new Rectangle(224, 320, 32, 32)
			};

			SetSourceRectangles(usr, dsr, lsr, rsr);
			#endregion


		}
	}
}
