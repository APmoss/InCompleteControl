﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameStateManagement;
using Microsoft.Xna.Framework.Graphics;

namespace Project_WB.Framework.Particles {
	/// <summary>
	/// The central manager for all particle effects. Controls the main collection of
	/// particles and emitters and the updating and drawing of all.
	/// </summary>
	class ParticleManager {
		#region Fields
		// The total number of particles allows at once
		int maxParticleCount = 512;

		// The collection of particles and emitters
		List<Particle> particles = new List<Particle>();
		List<ParticleEmitter> particleEmitters = new List<ParticleEmitter>();

		Texture2D particleSheet;
		#endregion

		#region Properties
		/// <summary>
		/// The maximum number of particles allowed on the screen at once.
		/// </summary>
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
		public int ParticleEmitterCount {
			get { return particleEmitters.Count; }
		}
		#endregion

		public ParticleManager(Texture2D particleSheet) {
			this.particleSheet = particleSheet;
		}

		#region Methods
		public void Update(GameTime gameTime) {
			for (int i = 0; i < particleEmitters.Count; i++) {
				particleEmitters[i].Update(gameTime);

				if (particleEmitters[i].LifeSpan < TimeSpan.Zero) {
					particleEmitters.RemoveAt(i);
					i--;
				}
			}
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
		public void Draw(GameTime gameTime, ScreenManager screenManager, Rectangle drawableArea) {
			foreach (var particle in particles) {
				if (drawableArea.Contains((int)particle.Position.X, (int)particle.Position.Y)) {
					var particleOrigin = new Vector2(particle.GetNextSourceRectangle().Width / 2, particle.GetNextSourceRectangle().Height / 2);
					screenManager.SpriteBatch.Draw(particleSheet, particle.Position, particle.GetNextSourceRectangle(),
													particle.Tint, particle.RotationDegrees, particleOrigin, particle.Scale, particle.SpriteEffects, 0);
				}
			}
		}

		public void AddParticle(Particle particle) {
			if(particles.Count + 1 <= maxParticleCount) {
				particles.Add(particle);
			}
		}

		public void AddParticleEmitter(ParticleEmitter particleEmitter) {
			particleEmitter.particleManager = this;
			particleEmitters.Add(particleEmitter);
		} 

		/// <summary>
		/// Return a copy if the particle list to prevent modifications.
		/// </summary>
		/// <returns></returns>
		public Particle[] GetParticles() {
			return particles.ToArray();
		}
		#endregion
	}
}
