using System;
using System.Collections.Generic;

namespace Project_WB.Framework.IO {
	class Session {
		public static bool OfflineMode = false;
		public static string Username = string.Empty;
		public static string Password = string.Empty;
		public static string PasswordHash = string.Empty;		

		public static void ResetSession() {
			OfflineMode = false;
			Username = string.Empty;
			Password = string.Empty;
			PasswordHash = string.Empty;
		}
	}
}
