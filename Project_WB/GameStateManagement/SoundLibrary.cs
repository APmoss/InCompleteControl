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
			sounds.Clear();

			string[] soundDirs = Directory.GetFiles(content.RootDirectory + "\\audio", "*.xnb", SearchOption.AllDirectories);

			if (content != null) {
				foreach (var soundDir in soundDirs) {
					var contentDir = new DirectoryInfo(content.RootDirectory + "/audio").Name;

					//TODO: CHANGE THIS
					sounds.Add(Path.GetFileNameWithoutExtension(soundDir).ToLower(),
								content.Load<SoundEffect>(Path.Combine(contentDir, "effects", Path.GetFileNameWithoutExtension(soundDir))));
				}
			}
		}
	}
}
