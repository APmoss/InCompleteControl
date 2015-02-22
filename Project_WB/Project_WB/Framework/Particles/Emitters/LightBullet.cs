using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Project_WB.Framework.Particles.Emitters {
	class LightBullet : BulletEmitter {
		public LightBullet(TimeSpan emissionFrequency, TimeSpan lifeSpan, Vector2 startPosition, Vector2 endPosition) : base(emissionFrequency, lifeSpan, startPosition, endPosition) {
			for (int i = 0; i < 4; i++) {
				srcRecs.Add(new Rectangle(16 * i, 144, 16, 16));
			}
		}
	}
}
