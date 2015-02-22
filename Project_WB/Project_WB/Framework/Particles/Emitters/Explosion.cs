using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Project_WB.Framework.Particles.Emitters {
	class Explosion : ParticleEmitter {
		Rectangle explosionBounds = Rectangle.Empty;
		List<Rectangle> explosionSrcRecs = new List<Rectangle>();
		List<Rectangle> smokeSrcRecs = new List<Rectangle>();

		Random r = new Random();

		public Explosion(TimeSpan emissionFrequency, TimeSpan lifeSpan, Rectangle explosionBounds) : base(emissionFrequency, lifeSpan) {
			for (int i = 0; i < 8; i++) {
				explosionSrcRecs.Add(new Rectangle(16 * i, 64, 16, 16));
			}
			for (int i = 0; i < 4; i++) {
				smokeSrcRecs.Add(new Rectangle(16 * i, 80, 16, 16));
			}

			this.explosionBounds = explosionBounds;
		}

		public override void Emit() {
			int numExplosions = 1;
			int numSmokes = 5;

			for (int i = 0; i < numExplosions; i++) {
				AnimatedParticle explosion = new AnimatedParticle(explosionSrcRecs);

				explosion.Position.X = r.Next(explosionBounds.Left, explosionBounds.Right);
				explosion.Position.Y = r.Next(explosionBounds.Top, explosionBounds.Bottom);
				explosion.LifeSpan = TimeSpan.FromSeconds(.45);
				explosion.Velocity = new Vector2((float)(r.NextDouble() - .5), (float)(-r.NextDouble() / 2 - .2));
				explosion.TargetAnimationTime = TimeSpan.FromSeconds(.05);

				particleManager.AddParticle(explosion);
			}
			for (int i = 0; i < numSmokes; i++) {
				AnimatedParticle smoke = new AnimatedParticle(smokeSrcRecs);

				smoke.Position.X = r.Next(explosionBounds.Left, explosionBounds.Right);
				smoke.Position.Y = r.Next(explosionBounds.Top, explosionBounds.Bottom);
				smoke.LifeSpan = TimeSpan.FromSeconds(.7);
				smoke.Velocity = new Vector2((float)(r.NextDouble() - .5), (float)(-r.NextDouble() / 2 - .75));
				smoke.TargetAnimationTime = TimeSpan.FromSeconds(.1);

				particleManager.AddParticle(smoke);
			}

			base.Emit();
		}
	}
}
