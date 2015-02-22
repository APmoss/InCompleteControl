using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace GameStateManagement {
	public class SoundLibrary {
		Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();

		public SoundEffect GetSound(string name) {
			name = name.ToLower();
			if (sounds.ContainsKey(name)) {
				return sounds[name];
			}
			throw new Exception("Sound file not found");
		}

		public void LoadSounds(ContentManager content) {
			// Clear the current collection
			sounds.Clear();

			// Get the directories of all of the sounds contained in the "audio" folder
			string[] soundDirs = Directory.GetFiles(content.RootDirectory + "\\audio", "*.xnb", SearchOption.AllDirectories);

			if (content != null) {
				foreach (var soundDir in soundDirs) {
					// Remove the Content\\ from the beginning to load the item.
					// -soundDir: Content\\audio\\blah.xnb
					// -loadDir: audio\\blah.xnb
					string loadDir = soundDir.Replace("Content\\", string.Empty);
					// Remove the extension (.xnb)
					loadDir = loadDir.Replace(".xnb", string.Empty);

					string key = Path.GetFileNameWithoutExtension(soundDir).ToLower();
					SoundEffect value = content.Load<SoundEffect>(loadDir);
					// Set the sound effect name to the asset name
					value.Name = key;

					sounds.Add(key, value);
				}
			}
		}
	}
}
