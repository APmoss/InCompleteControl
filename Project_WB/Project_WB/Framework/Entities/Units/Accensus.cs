using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Project_WB.Framework.Entities.Units {
	class Accensus : Unit {
		public Accensus() {
			#region SetRectangles
			var dsr = new List<Rectangle>();

			for (int i = 0; i < 4; i++) {
				dsr.Add(new Rectangle(i * 32, 448, 32, 32));
			}

			List<Rectangle> blank = new List<Rectangle>();
			SetSourceRectangles(blank, dsr, blank, blank);
			#endregion

			Name = "Accensus";
			Speed = 1.6f;
			Damage = 45;
			maxHealth = health = 150;
			distance = 6;
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
