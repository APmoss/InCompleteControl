using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Project_WB.Framework.Audio {
	/// <summary>
	/// An environment/map sound. This is modified as a 3D sound using
	/// a position and an AudioEmitter, and can also be looped if necessary.
	/// </summary>
	class EnvironmentSound : AudioItem {
		#region Properties
		/// <summary>
		/// Gets the item's emitter, which contains position, velocity, etc.
		/// </summary>
		public AudioEmitter Emitter {
			get; protected internal set;
		}
		#endregion

		#region Initialization
		public EnvironmentSound(SoundEffect soundEffect, Vector2 position, bool loops) : base(soundEffect) {
			// Name the sound
			this.assetName = soundEffect.Name;
			// Initialize the audio emitter
			this.Emitter = new AudioEmitter() {
				Forward = Vector3.Backward
			};
			// Set the position of the emitter
			this.Emitter.Position = new Vector3(position, 0);
			// Set the looping property of the sound instance
			this.SoundInstance.IsLooped = loops;
		}
		// Overloaded with a lifespan to the sound effect
		public EnvironmentSound(SoundEffect soundEffect, Vector2 position, bool loops, TimeSpan lifeSpan) : this(soundEffect, position, loops) {
			// If they specified a lifespan, that means it will decay
			this.Decays = true;
			// Set the lifespan of the sound
			this.LifeSpan = lifeSpan;
		}
		#endregion
	}
}
