using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Project_WB.Framework.IO {
	/// <summary>
	/// Contains static methods for accessing storage data and databases.
	/// </summary>
	class IOManager {
		/// <summary>
		/// The folder name that contains settings files.
		/// </summary>
		public static string SettingsPath = "settings";

		/// <summary>
		/// Returns or creates the settings file used for saving settings data.
		/// </summary>
		/// <returns></returns>
		public static SettingsFile LoadSettings() {
			XmlSerializer serializer = new XmlSerializer(typeof(SettingsFile));

			// If the file does not exist, create it with default values
			if (!File.Exists(SettingsPath + "\\settings.xml")) {
				SaveSettings(new SettingsFile());
			}

			// Old version- Had problems with shared file access
			//using (TextReader streamReader = new StreamReader(SettingsPath + "\\settings.xml")) {
			// New version- Get shared file access permissions
			using (TextReader streamReader = new StreamReader(new FileStream(SettingsPath + "\\settings.xml", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))) {
				// Deserialize the xml file to a usable class
				return serializer.Deserialize(streamReader) as SettingsFile;
			}
		}

		/// <summary>
		/// Saves and writes over the settings file in the settings folder.
		/// </summary>
		/// <param name="settingsFile"></param>
		public static void SaveSettings(SettingsFile settingsFile) {
			XmlSerializer serializer = new XmlSerializer(typeof(SettingsFile));

			using (TextWriter streamWriter = new StreamWriter(SettingsPath + "\\settings.xml")) {
				// Serialize the settings file to an xml
				serializer.Serialize(streamWriter, settingsFile);
			}
		}
	}
}
