using System;
using System.Collections.Generic;

namespace Project_WB.Framework.IO {
	/// <summary>
	/// The data held when logging in for the current login session.
	/// </summary>
	class Session {
		public static bool LoggedIn = false;
		public static DateTime LoginTime = DateTime.Today;
		public static bool OfflineMode = false;
		public static string Username = string.Empty;
		public static string FirstName = string.Empty;
		public static string LastName = string.Empty;

		public static void ResetSession() {
			LoggedIn = false;
			LoginTime = DateTime.Today;
			OfflineMode = false;
			Username = string.Empty;
			FirstName = string.Empty;
			LastName = string.Empty;
		}
	}
}
