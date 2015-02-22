using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using MySql.Data.MySqlClient;
using System.Xml;
using System.Data.SqlTypes;

namespace Project_WB.Framework.IO {
	/// <summary>
	/// Contains static methods for accessing storage data and databases.
	/// </summary>
	class IOManager {
		#region Constants
		/// <summary>
		/// The connection string to access the database.
		/// </summary>
		const string CONSTR = "Server=50.22.81.189;Database=bbaker_DBRF;Uid=bbaker_URF;Pwd=Mech~4PAAD;";
		#endregion

		#region Fields
		/// <summary>
		/// The folder name that contains settings files.
		/// </summary>
		public static string SettingsPath = "settings";

		static MySqlConnection db;
		#endregion

		#region Methods
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
		/// Saves and writes over the level file in the settings folder.
		/// </summary>
		/// <param name="settingsFile"></param>
		public static void SaveLevel(LevelFile levelFile) {
			XmlSerializer serializer = new XmlSerializer(typeof(SettingsFile));

			using (TextWriter streamWriter = new StreamWriter("level.xml")) {
				// Serialize the settings file to an xml
				serializer.Serialize(streamWriter, levelFile);
			}
		}

		/// <summary>
		/// Returns or creates the level file used for saving settings data.
		/// </summary>
		/// <returns></returns>
		public static SettingsFile LoadLevel() {
			XmlSerializer serializer = new XmlSerializer(typeof(LevelFile));

			// If the file does not exist, create it with default values
			if (!File.Exists("level.xml")) {
				SaveLevel(new LevelFile());
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

		/// <summary>
		/// Writes to a text file an error formatted with a date and time.
		/// </summary>
		/// <param name="message"></param>
		public static void WriteError(string message) {
			using (TextWriter writer = new StreamWriter(new FileStream("errors.txt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite))) {
				writer.WriteLine(string.Format("{0} - {1} - {2}", DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString(), message));
			}
		}

		/// <summary>
		/// Writes to a text file a log that contains playing data.
		/// </summary>
		/// <param name="message"></param>
		public static void WriteLog(string message) {
			using (TextWriter writer = new StreamWriter(new FileStream("log.txt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite))) {
				writer.WriteLine(string.Format("{0} - {1} - {2}", DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString(), message));
			}
		}

		/// <summary>
		/// Attempts to log in with the parameters specified.
		/// </summary>
		/// <param name="username"></param>
		/// <param name="rawPassword"></param>
		/// <returns></returns>
		public static bool LogIn(string username, string rawPassword, out bool success) {
			success = false;
			string hashedPass = HashString(rawPassword);

			try {
				db = new MySqlConnection(CONSTR);
				db.Open();
			}
			catch {
				db.Close();
				db = null;
				return false;
			}

			MySqlCommand cmd = new MySqlCommand(string.Format("SELECT * FROM UserInfo WHERE UserName = '{0}' AND Password = '{1}'", username, hashedPass), db);

			MySqlDataReader rdr;

			try {
				rdr = cmd.ExecuteReader();
			}
			catch {
				db.Close();
				db = null;
				return false;
			}

			if (rdr.Read()) {
				success = true;

				Session.Username = username;
				Session.FirstName = rdr["FirstName"].ToString();
				Session.LastName = rdr["LastName"].ToString();
				Session.LoginTime = DateTime.Now;
				Session.LoggedIn = true;
			}
			else {
				success = false;
			}

			db.Close();
			db = null;

			return true;
		}

		/// <summary>
		/// Queries the database for the existance of a username.
		/// </summary>
		/// <param name="username"></param>
		/// <param name="exists"></param>
		/// <returns></returns>
		public static bool CheckUsername(string username, out bool exists) {
			exists = false;

			if (db == null) {
				try {
					db = new MySqlConnection(CONSTR);
					db.Open();
				}
				catch {
					db.Close();
					db = null;
					return false;
				}
			}

			MySqlCommand cmd = new MySqlCommand(string.Format("SELECT count(*) FROM UserInfo WHERE UserName = '{0}'", username), db);

			int result = 0;

			try {
				var rdr = cmd.ExecuteReader();

				if (rdr.Read()) {
					result = rdr.GetInt32(0);
				}
			}
			catch {
				db.Close();
				db = null;
				return false;
			}

			db.Close();
			db = null;

			if (result >= 1) {
				exists = true;
			}
			return true;
		}

		/// <summary>
		/// Accesses the database and creates a new user with the parameters passed in.
		/// </summary>
		/// <param name="username"></param>
		/// <param name="rawPassword"></param>
		/// <param name="first"></param>
		/// <param name="last"></param>
		/// <param name="email"></param>
		/// <returns></returns>
		public static bool CreateNewUser(string username, string rawPassword, string first, string last, string email) {
			if (db == null) {
				try {
					db = new MySqlConnection(CONSTR);
					db.Open();
				}
				catch {
					db.Close();
					db = null;
					return false;
				}
			}

			MySqlCommand cmd = new MySqlCommand(string.Format("INSERT INTO UserInfo (UserName, Password, FirstName, LastName, Email) VALUES('{0}', '{1}', '{2}', '{3}', '{4}')",
												username, HashString(rawPassword), first, last, email), db);

			try {
				cmd.ExecuteNonQuery();
			}
			catch {
				db.Close();
				db = null;
				return false;
			}

			db.Close();
			db = null;

			return true;
		}

		public static string HashString(string rawString) {
			var crypt = new System.Security.Cryptography.MD5CryptoServiceProvider();

			byte[] byteData = System.Text.Encoding.ASCII.GetBytes(rawString);

			byteData = crypt.ComputeHash(byteData);

			string hashedString = string.Empty;

			for (int i = 0; i < byteData.Length; i++) {
				hashedString += byteData[i].ToString("x2").ToLower();
			}

			return hashedString;
		}
		
		//Xml connections
		//store XML data into a seperate table
		public void loadXML(string filePath, string name, string password, int saveState) {
			XmlReader reader = XmlReader.Create(filePath);                                                                                                      //create reader for specified xml file
			SqlXml myXML = new SqlXml(reader);                                                                                                                  //connect reader to file
			var command = new MySqlCommand();

			db.Open();                                                                                                                                     //open connection
			//**use REPLACE instead of INSERT to make sure players can save over previous files
			command = new MySqlCommand("REPLACE INTO UserInfo(UserName,Password,SaveState" + saveState + ") VALUES ('" + name + "','" + password + "','" + myXML.Value + "')", db);  //command to insert values
			command.ExecuteNonQuery();                                                                                                                          //execute query
			db.Close();                                                                                                                                    //close connection
		}
		//extract xml data from database into xml file
		public string extractXML(string name, int saveState) {
			string XMlstring;                                                                                                                                   //string to handle the xml values
			var command = new MySqlCommand();

			db.Open();                                                                                                                                     //open connection
			command = new MySqlCommand("SELECT * FROM UserInfo WHERE UserName = '" + name + "'", db);                                                           //command to select from the database
			MySqlDataReader reader = command.ExecuteReader();                                                                                                   //execute the reader
			if (reader.Read())                                                                                                                                  //if reader has a value
			{
				XMlstring = (string)reader["SaveState" + saveState + ""];                                                                                           //get xml string in column Data
				reader.Close();                                                                                                                                 //close reader
			}
			else {
				XMlstring = "Cannot return a value.";                                                                                                            //if reader is null, return a message to alert the user
				reader.Close();                                                                                                                                  //close reader
			}
			db.Close();                                                                                                                                     //close database connection

			return XMlstring;                                                                                                                                    //put out the string of xml data in xml structure
		}
		#endregion
	}
}
