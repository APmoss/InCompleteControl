using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Project_WB.Framework.Particles {
	abstract class ParticleEmitter {
		#region Fields
		public TimeSpan EmissionFrequency = TimeSpan.Zero;
		TimeSpan elapsed = TimeSpan.Zero;
		public int Iterations = 1;
		#endregion

		protected ParticleEmitter(TimeSpan emissionFrequency) {

		}

		#region Methods
		public virtual void Update(GameTime gameTime) {
			elapsed += gameTime.ElapsedGameTime;

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
