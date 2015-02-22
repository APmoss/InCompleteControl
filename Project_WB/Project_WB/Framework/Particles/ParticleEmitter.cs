using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Project_WB.Framework.Particles {
	/// <summary>
	/// A particle emitter that emits a particle at a set frequency.
	/// </summary>
	abstract class ParticleEmitter {
		#region Fields
		public TimeSpan EmissionFrequency = TimeSpan.Zero;
		TimeSpan elapsed = TimeSpan.Zero;
		public TimeSpan LifeSpan = TimeSpan.Zero;
		public int Iterations = 1;

		protected internal ParticleManager particleManager;
		#endregion

		protected ParticleEmitter(TimeSpan emissionFrequency, TimeSpan lifeSpan) {
			this.EmissionFrequency = emissionFrequency;
			this.LifeSpan = lifeSpan;
		}

		#region Methods
		public virtual void Update(GameTime gameTime) {
			elapsed += gameTime.ElapsedGameTime;
			LifeSpan -= gameTime.ElapsedGameTime;

			// Check if the emitter needs to emit particles
			if (elapsed > EmissionFrequency) {
				elapsed -= EmissionFrequency;

				Emit();
			}
		}

		public virtual void Emit() {
			
		}
		#endregion
	}
}
