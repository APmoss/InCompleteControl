#region File Description
//-----------------------------------------------------------------------------
// MapData.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion

namespace Project_WB.Framework.Pathfinding {
	/// <summary>
	/// Contains essential data for loading maps for pathfinding.
	/// </summary>
	public class MapData {
		public int NumberRows;
		public int NumberColumns;
		public Point Start;
		public Point End;

		List<Point> barriers = new List<Point>();

		public Point[] Barriers {
			get { return barriers.ToArray(); }
		}

		public MapData(int columns, int rows, Point startPosition,
						Point endPosition, List<Point> barriersList) {

			NumberColumns = columns;
			NumberRows = rows;
			Start = startPosition;
			End = endPosition;
			barriers = barriersList;
		}
	}
}
