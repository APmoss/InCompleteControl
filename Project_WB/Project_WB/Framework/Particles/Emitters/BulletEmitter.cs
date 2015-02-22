using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Project_WB.Framework.Particles.Emitters {
	class BulletEmitter : ParticleEmitter {
		protected List<Rectangle> srcRecs = new List<Rectangle>();
		protected Vector2 startPosition = Vector2.Zero;
		protected Vector2 endPosition = Vector2.Zero;

		protected BulletEmitter(TimeSpan emissionFrequency, TimeSpan lifeSpan, Vector2 startPosition, Vector2 endPosition) : base(emissionFrequency, lifeSpan) {
			this.startPosition = startPosition;
			this.endPosition = endPosition;
		}

		public override void Emit() {
			AnimatedParticle bullet = new AnimatedParticle(srcRecs);

			bullet.Position = startPosition;
			bullet.Velocity = endPosition - startPosition;
			bullet.Velocity.Normalize();
			bullet.Velocity *= 10;
			double time = (endPosition - startPosition).Length() / bullet.Velocity.Length() * .017;
			bullet.LifeSpan = TimeSpan.FromSeconds(time);
			bullet.TargetAnimationTime = TimeSpan.FromSeconds(.1);

			particleManager.AddParticle(bullet);

			base.Emit();
		}
	}
}
