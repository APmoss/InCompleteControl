using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Project_WB.Framework.Particles.Emitters {
	/// <summary>
	/// Bullet emitter: Heavy.
	/// </summary>
	class HeavyBullet : BulletEmitter {
		public HeavyBullet(TimeSpan emissionFrequency, TimeSpan lifeSpan, Vector2 startPosition, Vector2 endPosition) : base(emissionFrequency, lifeSpan, startPosition, endPosition) {
			for (int i = 0; i < 4; i++) {
				srcRecs.Add(new Rectangle(16 * i, 112, 16, 16));
			}
		}
	}
}
