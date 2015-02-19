using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameStateManagement;
using Microsoft.Xna.Framework.Graphics;

namespace Project_WB.Framework.Particles {
	class ParticleManager {
		//TODO: finish documentation

		#region Fields
		int maxParticleCount = 1024;

		List<Particle> particles = new List<Particle>();

		Texture2D particleSheet;
		#endregion

		#region Properties
		public int MaxParticleCount {
			get { return maxParticleCount; }
			set {
				maxParticleCount = value;

				if (particles.Count > maxParticleCount) {
					int excessParticles = particles.Count - maxParticleCount;

					particles.RemoveRange(particles.Count - excessParticles, excessParticles);
				}	
			}
		}

		public int ParticleCount {
			get { return particles.Count; }
		}
		#endregion

		public ParticleManager(Texture2D particleSheet) {
			this.particleSheet = particleSheet;
		}

		#region Methods
		public void Update(GameTime gameTime) {
			for (int i = 0; i < particles.Count; i++) {
				particles[i].Update(gameTime);

				if (particles[i].LifeSpan < TimeSpan.Zero) {
					particles.RemoveAt(i);
					i--;
				}
			}
		}

		public void Draw(GameTime gameTime, ScreenManager screenManager) {
			foreach (var particle in particles) {
				var particleOrigin = new Vector2(particle.GetNextSourceRectangle().Width / 2, particle.GetNextSourceRectangle().Height / 2);
				screenManager.SpriteBatch.Draw(particleSheet, particle.Position, particle.GetNextSourceRectangle(),
												particle.Tint, particle.RotationDegrees, particleOrigin, particle.Scale, particle.SpriteEffects, 0);
			}
		}

		public void AddParticle(Particle particle) {
			if(particles.Count + 1 <= maxParticleCount) {
				particles.Add(particle);
			}
		}
		#endregion
	}
}
