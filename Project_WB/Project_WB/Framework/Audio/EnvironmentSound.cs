using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Project_WB.Framework.Audio {
	class EnvironmentSound : AudioItem {
		public Vector2 Position = Vector2.Zero;
		AudioEmitter emitter = new AudioEmitter();

		public EnvironmentSound(SoundEffect soundEffect, Vector2 position, bool loops) : base(soundEffect) {
			this.Position = position;
			this.SoundInstance.IsLooped = loops;
		}
		public EnvironmentSound(SoundEffect soundEffect, Vector2 position, TimeSpan lifeSpan, bool loops) : base(soundEffect) {
			this.Position = position;
			this.Decays = true;
			this.lifeSpan = lifeSpan;
			this.SoundInstance.IsLooped = loops;
		}
	}
}
