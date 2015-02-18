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
			get; protected set;
		}
		#endregion

		#region Initialization
		public EnvironmentSound(SoundEffect soundEffect, Vector2 position, bool loops) : base(soundEffect) {
			// Initialize the audio emitter
			this.Emitter = new AudioEmitter();
			// Set the position of the emitter
			this.Emitter.Position = new Vector3(position, 0);
			// Set the looping property of the sound instance
			this.SoundInstance.IsLooped = loops;
		}
		public EnvironmentSound(SoundEffect soundEffect, Vector2 position, TimeSpan lifeSpan, bool loops) : base(soundEffect) {
			// Initialize the audio emitter
			this.Emitter = new AudioEmitter();
			// Set the position of the emitter
			this.Emitter.Position = new Vector3(position, 0);
			// Set the looping property of the sound instance
			this.SoundInstance.IsLooped = loops;
			// If they specified a lifespan, that means it will decay
			this.Decays = true;
			// Set the lifespan of the sound
			this.lifeSpan = lifeSpan;
		}
		#endregion
	}
}
