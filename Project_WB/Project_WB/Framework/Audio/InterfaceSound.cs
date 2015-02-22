using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace Project_WB.Framework.Audio {
	/// <summary>
	/// A sound that plays equally in each stereo channel, such as the game gui.
	/// </summary>
	class InterfaceSound : AudioItem{
		public InterfaceSound(SoundEffect soundEffect) : base(soundEffect) {
		}
	}
}
