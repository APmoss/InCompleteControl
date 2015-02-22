using System;
using Microsoft.Xna.Framework.Audio;

namespace Project_WB.Framework.Audio {
	/// <summary>
	/// Abstract class containing items pertaining to all sounds, such as asset name.
	/// </summary>
	abstract class AudioItem {
		#region Fields
		// The name of the sound effect asset when loaded
		protected string assetName;
		/// <summary>
		/// The instance of the sound effect passed in.
		/// With a sound instance, we can modify the sound in more ways.
		/// </summary>
		public SoundEffectInstance SoundInstance;

		// If the sound has a timed lifetime, this should be true
		protected internal bool Decays = false;
		// The lifespan for which the sound will exist.
		protected internal TimeSpan LifeSpan = TimeSpan.Zero;
		#endregion

		#region Properties
		/// <summary>
		/// The asset name of the sound effect.
		/// </summary>
		public string Name {
			get { return assetName; }
		}
		#endregion

		// Protected since this is an abstract class
		protected AudioItem(SoundEffect soundEffect) {
			this.assetName = soundEffect.Name;
			this.SoundInstance = soundEffect.CreateInstance();
		}
	}
}
