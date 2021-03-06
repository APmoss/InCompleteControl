﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Project_WB.Framework.Entities.Units {
	class Centurion : Unit {
		public Centurion() {
			#region SetRectangles
			var dsr = new List<Rectangle>();

			for (int i = 0; i < 4; i++) {
				dsr.Add(new Rectangle(i * 32, 384, 32, 32));
			}

			List<Rectangle> blank = new List<Rectangle>();
			SetSourceRectangles(blank, dsr, blank, blank);
			#endregion

			Name = "Centurion";
			Speed = 1.5f;
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

			var radialLights = new List<RadialLight>();
			radialLights.Add(new RadialLight(new Vector2(0, -5), new Vector2(.11f, .2f), Color.CornflowerBlue));
			radialLights.Add(new RadialLight(new Vector2(4, -6), new Vector2(.2f, .19f), Color.CornflowerBlue));

			EntityData.Add("radialLights", radialLights);
		}
	}
}
