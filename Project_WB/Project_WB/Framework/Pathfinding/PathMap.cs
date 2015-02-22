#region File Description
//-----------------------------------------------------------------------------
// Map.cs
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

namespace Project_WB.Framework.Pathfinding
{
	#region Map Tile Type Enum
	public enum MapTileType
	{
		MapEmpty,
		MapBarrier,
		MapStart,
		MapExit
	}
	#endregion

	public class PathMap
	{
		#region Fields

		// Map data
		private List<MapData> maps;
		private MapTileType[,] mapTiles;
		private int currentMap;
		private int numberColumns;
		private int numberRows;

		#endregion

		#region Properties

		/// <summary>
		/// Start positon on the Map
		/// </summary>
		public Point StartTile
		{
			get { return startTile; }
		}
		private Point startTile;

		/// <summary>
		/// End position in the Map
		/// </summary>
		public Point EndTile
		{
			get { return endTile; }
		}
		private Point endTile;

		/// <summary>
		/// Set: reload Map data, Get: has the Map data changed
		/// </summary>
		public bool MapReload
		{
			get { return mapReload; }
			set { mapReload = value; }
		}
		private bool mapReload;

		#endregion

		#region Methods

		public void SetMaps(int startingMap, params MapData[] mapDatas) {
			if (maps == null) {
				maps = new List<MapData>();
			}

			maps.Clear();
			maps.AddRange(mapDatas);

			this.currentMap = startingMap;

			ReloadMap();
		}

		/// <summary>
		/// Returns true if the given map location exists
		/// </summary>
		/// <param name="column">column position(x)</param>
		/// <param name="row">row position(y)</param>
		private bool InMap(int column, int row)
		{
			return (row >= 0 && row < numberRows &&
				column >= 0 && column < numberColumns);
		}

		/// <summary>
		/// Returns true if the given map location exists and is not 
		/// blocked by a barrier
		/// </summary>
		/// <param name="column">column position(x)</param>
		/// <param name="row">row position(y)</param>
		private bool IsOpen(int column, int row)
		{
			return InMap(column, row) && mapTiles[column, row] != MapTileType.MapBarrier;
		}

		/// <summary>
		/// Enumerate all the map locations that can be entered from the given 
		/// map location
		/// </summary>
		public IEnumerable<Point> OpenMapTiles(Point mapLoc)
		{
			if (IsOpen(mapLoc.X, mapLoc.Y + 1))
				yield return new Point(mapLoc.X, mapLoc.Y + 1);
			if (IsOpen(mapLoc.X, mapLoc.Y - 1))
				yield return new Point(mapLoc.X, mapLoc.Y - 1);
			if (IsOpen(mapLoc.X + 1, mapLoc.Y))
				yield return new Point(mapLoc.X + 1, mapLoc.Y);
			if (IsOpen(mapLoc.X - 1, mapLoc.Y))
				yield return new Point(mapLoc.X - 1, mapLoc.Y);
		}

		/// <summary>
		/// Finds the minimum number of tiles it takes to move from Point A to 
		/// Point B if there are no barriers in the way
		/// </summary>
		/// <param name="pointA">Start position</param>
		/// <param name="pointB">End position</param>
		/// <returns>Distance in tiles</returns>
		public static int StepDistance(Point pointA, Point pointB)
		{
			int distanceX = Math.Abs(pointA.X - pointB.X);
			int distanceY = Math.Abs(pointA.Y - pointB.Y);

			return distanceX + distanceY;
		}

		/// <summary>
		/// Finds the minimum number of tiles it takes to move from the current 
		/// position to the end location on the Map if there are no barriers in 
		/// the way
		/// </summary>
		/// <param name="point">Current position</param>
		/// <returns>Distance to end in tiles</returns>
		public int StepDistanceToEnd(Point point)
		{
			return StepDistance(point, endTile);
		}

		/// <summary>
		/// Load the next map
		/// </summary>
		public void CycleMap()
		{
			currentMap = (currentMap + 1) % maps.Count;

			mapReload = true;
		}

		/// <summary>
		/// Reload map data
		/// </summary>
		public void ReloadMap()
		{
			// Set the map height and width
			numberColumns = maps[currentMap].NumberColumns;
			numberRows = maps[currentMap].NumberRows;
			
			// Recreate the tile array
			mapTiles = new MapTileType[maps[currentMap].NumberColumns, maps[currentMap].NumberRows];
			
			// Set the start
			startTile = maps[currentMap].Start;
			mapTiles[startTile.X, startTile.Y] = MapTileType.MapStart;
			
			// Set the end
			endTile = maps[currentMap].End;
			mapTiles[endTile.X, endTile.Y] = MapTileType.MapExit;

			int x = 0;
			int y = 0;
			// Set the barriers
			for (int i = 0; i < maps[currentMap].Barriers.Count; i++)
			{
				x = maps[currentMap].Barriers[i].X;
				y = maps[currentMap].Barriers[i].Y;
				
				mapTiles[x, y] =  MapTileType.MapBarrier;
			}

			mapReload = false;
		}

		#endregion
	}
}
