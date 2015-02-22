using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Project_WB.Framework.Entities.Units {
	class Ballistarius : Unit {
		public Ballistarius() {
			#region SetRectangles
			var dsr = new List<Rectangle>();

			for (int i = 0; i < 4; i++) {
				dsr.Add(new Rectangle(i * 32, 416, 32, 32));
			}

			List<Rectangle> blank = new List<Rectangle>();
			SetSourceRectangles(blank, dsr, blank, blank);
			#endregion

			Name = "Ballistarius";
			Speed = 1f;
			maxHealth = health = 200;
			distance = 4;
			attackDistance = 5;
			rotationalAnimation = true;

			selectCommandVoices.Add("readyfororders");
			selectCommandVoices.Add("vmdready");
			selectCommandVoices.Add("reporting");
			selectCommandVoices.Add("weaponsready");
			moveCommandVoices.Add("vmdmovingout");
			moveCommandVoices.Add("destinationrecieved");
			moveCommandVoices.Add("headingtolocation");
			moveCommandVoices.Add("ordersrecieved");
			moveCommandVoices.Add("unitmoveout");
		}
	}
}
