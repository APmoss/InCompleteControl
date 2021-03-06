﻿using System;
using Microsoft.Xna.Framework.Audio;

namespace Project_WB.Framework.Audio {
	/// <summary>
	/// A music track that plays a certain song when added to the audio manager.
	/// Only one song can be playing in the audio manager at once.
	/// </summary>
	class MusicTrack : AudioItem {
		#region Properties
		/// <summary>
		/// The length of the track of music.
		/// </summary>
		public TimeSpan SongLength {
			get; protected set;
		}
		#endregion

		public MusicTrack(SoundEffect soundEffect) : base(soundEffect) {
			// Set the song length
			this.SongLength = soundEffect.Duration;
		}
	}
}
