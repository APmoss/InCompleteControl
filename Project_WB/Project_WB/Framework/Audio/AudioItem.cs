using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace Project_WB.Framework.Audio {
	abstract class AudioItem {
		string assetName;
		public SoundEffectInstance SoundInstance;

		public bool Decays = false;
		public TimeSpan lifeSpan = TimeSpan.Zero;

		public string SoundEffectName {
			get { return assetName; }
		}

		protected AudioItem(SoundEffect soundEffect) {
			this.assetName = soundEffect.Name;
			this.SoundInstance = soundEffect.CreateInstance();
		}
	}
}
