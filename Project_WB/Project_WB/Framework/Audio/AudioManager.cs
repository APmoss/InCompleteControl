﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Project_WB.Framework.Audio {
	/// <summary>
	/// Manages all audio in an organized, controlled manner. Add sounds using
	/// the AddSounds method, with any AudioItem object.
	/// </summary>
	class AudioManager {
		//TODO: finish documentation
		#region Fields
		// Private variable (description in property)
		int maxSoundChannels = 64;
		// The collection of all audio items. The size is limited based on
		// the maximum number of sound channels.
		List<AudioItem> audioItems = new List<AudioItem>();
		// The audio listener, that hears at the center of the screen (along with the camera)
		AudioListener listener = new AudioListener();
		// A local copy of the camera, so we can tell where to listen from
		Camera2D camera;

		// Various volume modifications
		// These are private variables, descriptions in properties
		float musicVolume = 1;
		float interfaceVolume = 1;
		float environmentVolume = 1;
		float voiceVolume = 1;
		#endregion

		#region Properties
		/// <summary>
		/// The maximum number of concurrent audio channels that can be played.
		/// This can be lowered for performance if necessary.
		/// </summary>
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

		/// <summary>
		/// The volume that affects the background and other types of music.
		/// </summary>
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
		/// <summary>
		/// The volume that affects the interface and interaction sounds.
		/// </summary>
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
		/// <summary>
		/// The volume that affects the sounds in the environment, map, etc.
		/// </summary>
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
		/// <summary>
		/// The volume that affects the sounds of all voiceovers and voice lines.
		/// </summary>
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

		public AudioManager(Camera2D camera) {
			this.camera = camera;

			var relativePosition = camera.ToRelativePosition(new Vector2(camera.Viewport.Width / 2, camera.Viewport.Height / 2));
			this.listener.Position = new Vector3(relativePosition, 0);

			foreach (var sound in audioItems) {
				if (sound is EnvironmentSound) {
					if (sound.SoundInstance.State == SoundState.Playing) {
						//((EnvironmentSound)sound).
					}
				}
			}
		}

		#region Methods
		public void Update(GameTime gameTime) {
			// Update the position of the listener
			var relativePosition = camera.ToRelativePosition(new Vector2(camera.Viewport.Width / 2, camera.Viewport.Height / 2));
			relativePosition.X /= 32;
			relativePosition.Y /= 32;
			listener.Position = new Vector3(relativePosition, 0);
			// TODO: Change velocity
			listener.Velocity = new Vector3(camera.GetCurrentVelocity() * 16, 0);

			foreach (var sound in audioItems) {
				//music
				//interface
				if (sound is EnvironmentSound) {
					sound.SoundInstance.Volume = environmentVolume;
					sound.SoundInstance.Apply3D(listener, ((EnvironmentSound)sound).Emitter);
				}
				//voice
			}
		}
		public void AddSounds(params AudioItem[] audioItems) {
			foreach (var sound in audioItems) {
				//music
				//interface
				if (sound is EnvironmentSound) {
					sound.SoundInstance.Volume = environmentVolume;
					sound.SoundInstance.Apply3D(listener, ((EnvironmentSound)sound).Emitter);
				}
				//voice

				sound.SoundInstance.Play();
			}

			this.audioItems.AddRange(audioItems);
		}
		#endregion
	}
}