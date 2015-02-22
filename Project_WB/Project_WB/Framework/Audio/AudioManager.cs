using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;

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
		// Multiplier that spreads the position of environmental sounds.
		public int AudioSpreaderMultiplier = 32;
		// The multiplier of the doppler effect for environment sounds.		
		public float DopplerScale = 3;
		public float musicTransitionAlpha = 1;
		// The collection of all audio items. The size is limited based on
		// the maximum number of sound channels.
		List<AudioItem> audioItems = new List<AudioItem>();
		// The currently playing music
		MusicTrack currentSong;
		MusicTrack transitionSong;

		// The audio listeners, that hear around the center of the screen (along with the camera)
		AudioListener leftListener = new AudioListener();
		AudioListener rightListener = new AudioListener();
		// The distance between the left and right listeners
		int listenerDistance = 4;
		// The height of which the listeners are hearing from (in the Z direction)
		int listenerHeight = 5;
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
		/// Returns a copy of the audio items collection
		/// </summary>
		public AudioItem[] AudioItems {
			get { return audioItems.ToArray(); }
		}
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
					
					audioItems.RemoveRange(audioItems.Count - excessSounds, excessSounds);
				}
			}
		}

		#region Volumes
		/// <summary>
		/// The volume that affects the background and other types of music.
		/// </summary>
		public float MusicVolume {
			get { return musicVolume; }
			set {
				musicVolume = MathHelper.Clamp(value, 0, 1);

				foreach (var sound in audioItems) {
					//if(sound is Music)
				}
			}
		}
		/// <summary>
		/// The volume that affects the interface and interaction sounds (and unit commands).
		/// </summary>
		public float InterfaceVolume {
			get { return interfaceVolume; }
			set {
				interfaceVolume = MathHelper.Clamp(value, 0, 1);

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
				environmentVolume = MathHelper.Clamp(value, 0, 1);

				foreach (var sound in audioItems) {
					if (sound is EnvironmentSound) {
						sound.SoundInstance.Volume = environmentVolume;
					}
				}
			}
		}
		/// <summary>
		/// The volume that affects the sounds of all voiceovers and voice lines (Not unit commands).
		/// </summary>
		public float VoiceVolume {
			get { return voiceVolume; }
			set {
				voiceVolume = MathHelper.Clamp(value, 0, 1);

				foreach (var sound in audioItems) {
					//if(sound is voice)
				}
			}
		}
		#endregion
		#endregion

		public AudioManager(Camera2D camera) {
			this.camera = camera;
		}

		#region Methods
		public void Update(GameTime gameTime) {
			// Update the position of the listeners
			var cameraPosition = camera.ToRelativePosition(new Vector2(camera.Viewport.Width / 2, camera.Viewport.Height / 2));
			cameraPosition.X /= AudioSpreaderMultiplier;
			cameraPosition.Y /= AudioSpreaderMultiplier;
			var halfListenerDistance = listenerDistance / 2;
			
			leftListener.Position = new Vector3(cameraPosition.X - halfListenerDistance, cameraPosition.Y, listenerHeight);
			rightListener.Position = new Vector3(cameraPosition.X + halfListenerDistance, cameraPosition.Y, listenerHeight);
			
			leftListener.Velocity = new Vector3(camera.GetCurrentVelocity(), 0);
			rightListener.Velocity = new Vector3(camera.GetCurrentVelocity(), 0);

			if(transitionSong != null && currentSong == null) {
				currentSong = transitionSong;
				transitionSong = null;
				currentSong.SoundInstance.Play();
			}
			else if (transitionSong != null) {
				musicTransitionAlpha -= .01f;
				if (musicTransitionAlpha <= 0) {
					currentSong = transitionSong;
					currentSong.SoundInstance.Play();
					transitionSong = null;
				}
			}
			else if (currentSong != null && transitionSong == null) {
				if (musicTransitionAlpha < 1) {
					musicTransitionAlpha += .01f;
				}
			}
			if (currentSong != null) {
				musicTransitionAlpha = MathHelper.Clamp(musicTransitionAlpha, 0, 1);
				currentSong.SoundInstance.Volume = musicVolume * musicTransitionAlpha;
			}

			for (int i = 0; i < audioItems.Count; i++) {
				// Subtract from the audio item's lifespan
				audioItems[i].LifeSpan -= gameTime.ElapsedGameTime;
				// Prune the audio items that need to be removed
				if ((audioItems[i].Decays && audioItems[i].LifeSpan <= TimeSpan.Zero) || audioItems[i].SoundInstance.State == SoundState.Stopped) {
					// Dispose sound instance resources
					audioItems[i].SoundInstance.Dispose();
					// Remove the audio item
					audioItems.RemoveAt(i);
					// Push back the loop
					i--;
					continue;
				}

				if (audioItems[i] is InterfaceSound) {
					InterfaceSound inS = audioItems[i] as InterfaceSound;
					inS.SoundInstance.Volume = interfaceVolume;
				}
				else if (audioItems[i] is EnvironmentSound) {
					EnvironmentSound es = audioItems[i] as EnvironmentSound;
					es.SoundInstance.Volume = environmentVolume;
					es.SoundInstance.Apply3D(new AudioListener[] { leftListener, rightListener }, es.Emitter);
				}
				//voice
			}
		}

		public void AddSounds(params AudioItem[] sounds) {
			foreach (var sound in sounds) {
				// If the collection of audio items isn't already full
				if (audioItems.Count + 1 <= maxSoundChannels) {
					if (sound is MusicTrack) {
						MusicTrack mt = sound as MusicTrack;
						mt.SoundInstance.Volume = musicVolume;
						transitionSong = mt;
						continue;
					}
					else if (sound is InterfaceSound) {
						InterfaceSound inS = sound as InterfaceSound;
						inS.SoundInstance.Volume = interfaceVolume;
					}
					else if (sound is EnvironmentSound) {
						EnvironmentSound es = sound as EnvironmentSound;
						es.SoundInstance.Volume = environmentVolume;
						es.Emitter.DopplerScale = DopplerScale;
						// Must apply 3D once before playing for the first time to avoid error
						sound.SoundInstance.Apply3D(new AudioListener[] { leftListener, rightListener }, es.Emitter);
					}
					//voice

					sound.SoundInstance.Play();

					audioItems.Add(sound);
				}
				else {
					// Just break the loop if the sound collection is already full
					break;
				}
			}
		}
		#endregion
	}
}
