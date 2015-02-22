using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Project_WB.Framework.IO {
	class IOManager {
		public static string SettingsPath = "settings";

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
				return serializer.Deserialize(streamReader) as SettingsFile;
			}
		}

		public static void SaveSettings(SettingsFile settingsFile) {
			XmlSerializer serializer = new XmlSerializer(typeof(SettingsFile));

			using (TextWriter streamWriter = new StreamWriter(SettingsPath + "\\settings.xml")) {
				serializer.Serialize(streamWriter, settingsFile);
			}
		}
	}
}
