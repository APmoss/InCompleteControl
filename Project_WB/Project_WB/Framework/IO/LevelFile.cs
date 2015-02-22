using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Project_WB.Framework.IO {
	/// <summary>
	/// The class that is serialized when saving level data.
	/// </summary>
	[Serializable]
	public class LevelFile {
		#region Level Fields
		string fileVersion = "1.0";

		public int numEntities = 0;
		public int numTeams = 2;
		public int numRemaining = 0;
		public List<Point> EntityPositions = new List<Point>();
		public List<float> HealthValues = new List<float>();
		public int slenderScore = 0;
		#endregion
	}
}
