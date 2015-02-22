using System;
using System.Collections.Generic;

namespace Project_WB.Framework.IO {
	/// <summary>
	/// The class that is serialized when saving settings data.
	/// </summary>
	[Serializable]
	public class SettingsFile {
		#region Settings Fields
		string fileVersion = "1.0";

		public float MusicVolume = 1.0f;
		public float InterfaceVolume = 1.0f;
		public float EnvironmentVolume = 1.0f;
		public float VoiceVolume = 1.0f;
		#endregion
	}
}
