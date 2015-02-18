using System;

namespace Project_WB {
	/// <summary>
	/// A static class that contains a few commonly used
	/// global variables for ease of access.
	/// </summary>
	public class Stcs {
		/// <summary>
		/// The X resolution (width)
		/// </summary>
		public static int XRes = 1280;
		/// <summary>
		/// The Y resolution (height)
		/// </summary>
		public static int YRes = 720;

		/// <summary>
		/// The version of the program, used internally rather
		/// than publishing/executing versions
		/// </summary>
		public static Version InternalVersion = new Version();
	}
}
