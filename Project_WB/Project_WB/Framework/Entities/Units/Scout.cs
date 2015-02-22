using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Project_WB.Framework.Entities.Units {
	class Scout : Unit {
		public Scout() {
			#region SetRectangles
			var dsr = new List<Rectangle>();

			for (int i = 0; i < 3; i++) {
				dsr.Add(new Rectangle(i * 32, 480, 32, 32));
			}

			List<Rectangle> blank = new List<Rectangle>();
			SetSourceRectangles(blank, dsr, blank, blank);
			#endregion

			Name = "Scout";
			Speed = 2f;
			Damage = 15;
			maxHealth = health = 50;
			distance = 15;
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
