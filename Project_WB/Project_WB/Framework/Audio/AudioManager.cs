using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace Project_WB.Framework.Audio {
	class AudioManager {
		#region Fields
		int maxSoundChannels = 64;
		List<AudioItem> audioItems = new List<AudioItem>();

		float musicVolume = 1;
		float interfaceVolume = 1;
		float environmentVolume = 1;
		float voiceVolume = 1;
		#endregion

		#region Properties
		public int MaxSoundChannels {
			get { return maxSoundChannels; }
			set {
				maxSoundChannels = value;

				if (audioItems.Count > maxSoundChannels) {
					int excessSounds = audioItems.Count - maxSoundChannels;

					audioItems.RemoveRange(audioItems.Count - excessSounds - 1, excessSounds);
				}
			}
		}

		public float MusicVolume {
			get { return musicVolume; }
			set {
				musicVolume = value;

				musicVolume = MathHelper.Clamp(musicVolume, 0, 1);

				foreach (var sound in audioItems) {
					//if(sound is Music)
				}
			}
		}
		public float InterfaceVolume {
			get { return interfaceVolume; }
			set {
				interfaceVolume = value;

				interfaceVolume = MathHelper.Clamp(interfaceVolume, 0, 1);

				foreach (var sound in audioItems) {
					//if(sound is interface)
				}
			}
		}
		public float EnvironmentVolume {
			get { return environmentVolume; }
			set {
				environmentVolume = value;

				environmentVolume = MathHelper.Clamp(environmentVolume, 0, 1);

				foreach (var sound in audioItems) {
					if (sound is EnvironmentSound) {
						sound.SoundInstance.Volume = environmentVolume;
					}
				}
			}
		}
		public float VoiceVolume {
			get { return voiceVolume; }
			set {
				voiceVolume = value;

				voiceVolume = MathHelper.Clamp(voiceVolume, 0, 1);

				foreach (var sound in audioItems) {
					//if(sound is voice)
				}
			}
		}
		#endregion

		#region Methods
		public void Update(GameTime gameTime, Camera2D camera) {

		}
		public void AddSounds(params AudioItem[] audioItems) {

		}
		#endregion
	}
}
