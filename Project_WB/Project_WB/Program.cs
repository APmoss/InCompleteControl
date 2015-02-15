using System;

namespace Project_WB {
#if WINDOWS || XBOX
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			using (WB_Game game = new WB_Game())
			{
				game.Run();
			}
		}
	}
#endif
}

