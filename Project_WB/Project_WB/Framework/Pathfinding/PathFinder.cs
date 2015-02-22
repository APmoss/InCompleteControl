using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Project_WB.Framework.Pathfinding
{
	/// <summary>
	/// The current status of the pathfinder.
	/// </summary>
	public enum SearchStatus {
		Stopped,
		Searching,
		NoPath,
		PathFound,
	}
	/// <summary>
	/// The current searching method. A very simple description of each-
	/// AStar- What we will use for pathfinding. Fast and efficient.
	/// BestFirst- Comparable to AStar, but sometimes chooses awkward paths.
	/// BreadthFirst- Not as fast, but chooses good paths.
	/// </summary>
	public enum SearchMethod {
		BreadthFirst,
		BestFirst,
		AStar
	}

	/// <summary>
	/// PathFinder contains the necessary elemts for the game's pathfinding.
	/// All units must strategically move from one tile to another, which
	/// means you need pathfinding logic to travel in the right direction.
	/// </summary>
	class PathFinder {
		/// <summary>
		/// A single node of a search structure. Contains the node's position,
		/// distance to the goal, and distance traveled so far.
		/// </summary>
		protected struct SearchNode {
			// The node position in the current map
			public Point Position;
			
			// An estimate of the distance to the goal
			public int DistanceToGoal;
			
			// The total distance traveled from the start
			public int DistanceTraveled;

			public SearchNode(Point mapPosition, int distanceToGoal, int distanceTraveled) {
				Position = mapPosition;
				DistanceToGoal = distanceToGoal;
				DistanceTraveled = distanceTraveled;
			}
		}

		#region Fields
		// Private fields (details in properties)
		SearchStatus searchStatus = SearchStatus.Stopped;
		SearchMethod searchMethod = SearchMethod.AStar;
		// The list of nodes that are able to be searched
		List<SearchNode> openList = new List<SearchNode>();
		// The list of nodes that have already been searched
		List<SearchNode> closedList = new List<SearchNode>();
		// All of the points created so far
		Dictionary<Point, Point> paths = new Dictionary<Point,Point>();
		// The current map we are searching
		PathMap map = new PathMap();        
		#endregion
		
		#region Properties
		/// <summary>
		/// Gets the current search status.
		/// </summary>
		public SearchStatus SearchStatus {
			get { return searchStatus; }
			protected set { searchStatus = value; }
		}
		/// <summary>
		/// Gets or sets the methods of path finding. Each method
		/// is described at the SearchMethod enumeration.
		/// </summary>
		public SearchMethod SearchMethod {
			get { return searchMethod; }
			set { searchMethod = value; }
		}
		/// <summary>
		/// Gets or sets whether the path finder is searching or not.
		/// </summary>
		public bool IsSearching {
			get { return searchStatus == SearchStatus.Searching; }
			set {
				if (value) {
					searchStatus = SearchStatus.Searching;
				}
				else {
					searchStatus = SearchStatus.Stopped;
				}
			}
		}

		/// <summary>
		/// The current number of search steps taken so far.
		/// </summary>
		public int TotalSearchSteps {
			get; protected set;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Immediately finds a path with the specified parameters.
		/// Returns whether a path is possible, setting the solution parameter.
		/// </summary>
		/// <param name="mapData"></param>
		/// <param name="solution"></param>
		/// <returns></returns>
		public bool QuickFind(MapData mapData, out LinkedList<Point> solution) {
			solution = new LinkedList<Point>();

			PathMap pm = new PathMap();
			pm.SetMaps(0, mapData);

			map = pm;
			Reset();

			IsSearching = true;

			while (searchStatus != SearchStatus.PathFound && searchStatus != SearchStatus.NoPath) {
				if (searchStatus == SearchStatus.Searching)
					DoSearchStep();
			}
			if (searchStatus == SearchStatus.PathFound) {
				solution = FinalPath();
				return true;
			}
			else {
				return false;
			}
		}

		/// <summary>
		/// Reset the search
		/// </summary>
		public void Reset() {
			searchStatus = SearchStatus.Stopped;
			TotalSearchSteps = 0;
			openList.Clear();
			closedList.Clear();
			paths.Clear();
			openList.Add(new SearchNode(map.StartTile,
				PathMap.StepDistance(map.StartTile, map.EndTile)
				, 0));
		}

		/// <summary>
		/// This method find the next path node to visit, puts that node on the 
		/// closed list and adds any nodes adjacent to the visited node to the 
		/// open list.
		/// </summary>
		private void DoSearchStep() {
			SearchNode newOpenListNode;

			bool foundNewNode = SelectNodeToVisit(out newOpenListNode);
			if (foundNewNode) {
				Point currentPos = newOpenListNode.Position;
				foreach (Point point in map.OpenMapTiles(currentPos)) {
					SearchNode mapTile = new SearchNode(point, 
						map.StepDistanceToEnd(point), 
						newOpenListNode.DistanceTraveled + 1);
					if (!InList(openList,point) &&
						!InList(closedList,point)) {
						openList.Add(mapTile);
						paths[point] = newOpenListNode.Position;
					}
				}
				if (currentPos == map.EndTile) {
					searchStatus = SearchStatus.PathFound;
				}
				openList.Remove(newOpenListNode);
				closedList.Add(newOpenListNode);
			}
			else {
				searchStatus = SearchStatus.NoPath;
			}
		}

		/// <summary>
		/// Determines if the given Point is inside the SearchNode list given
		/// </summary>
		private static bool InList(List<SearchNode> list, Point point) {
			bool inList = false;
			foreach (SearchNode node in list) {
				if (node.Position == point) {
					inList = true;
				}
			}
			return inList;
		}

		/// <summary>
		/// This Method looks at everything in the open list and chooses the next 
		/// path to visit based on which search type is currently selected.
		/// </summary>
		/// <param name="result">The node to be visited</param>
		/// <returns>Whether or not SelectNodeToVisit found a node to examine
		/// </returns>
		private bool SelectNodeToVisit(out SearchNode result) {
			result = new SearchNode();
			bool success = false;
			float smallestDistance = float.PositiveInfinity;
			float currentDistance = 0f;
			if (openList.Count > 0) {
				switch (searchMethod) {
					// Breadth first search looks at every possible path in the 
					// order that we see them in.
					case SearchMethod.BreadthFirst:
						TotalSearchSteps++;
						result = openList[0];
						success = true;
						break;
					// Best first search always looks at whatever path is closest to
					// the goal regardless of how long that path is.
					case SearchMethod.BestFirst:
						TotalSearchSteps++;
						foreach (SearchNode node in openList) {
							currentDistance = node.DistanceToGoal;
							if(currentDistance < smallestDistance){
								success = true;
								result = node;
								smallestDistance = currentDistance;
							}
						}
						break;
					// A* search uses a heuristic, an estimate, to try to find the 
					// best path to take. As long as the heuristic is admissible, 
					// meaning that it never over-estimates, it will always find 
					// the best path.
					case SearchMethod.AStar:
						TotalSearchSteps++;
						foreach (SearchNode node in openList) {
							currentDistance = Heuristic(node);
							// The heuristic value gives us our optimistic estimate 
							// for the path length, while any path with the same 
							// heuristic value is equally ‘good’ in this case we’re 
							// favoring paths that have the same heuristic value 
							// but are longer.
							if (currentDistance <= smallestDistance) {
								if (currentDistance < smallestDistance) {
									success = true;
									result = node;
									smallestDistance = currentDistance;
								}
								else if (currentDistance == smallestDistance &&
									node.DistanceTraveled > result.DistanceTraveled) {
									success = true;
									result = node;
									smallestDistance = currentDistance;
								}
							}
						}
						break;
				}
			}
			return success;
		}

		/// <summary>
		/// Generates an optimistic estimate of the total path length to the goal 
		/// from the given position.
		/// </summary>
		/// <param name="location">Location to examine</param>
		/// <returns>Path length estimate</returns>
		private static float Heuristic(SearchNode location) {
			return location.DistanceTraveled + location.DistanceToGoal;
		}

		/// <summary>
		/// Generates the path from start to end.
		/// </summary>
		/// <returns>The path from start to end</returns>
		public LinkedList<Point> FinalPath() {
			LinkedList<Point> path = new LinkedList<Point>();
			if (searchStatus == SearchStatus.PathFound) {
				Point curPrev = map.EndTile;
				path.AddFirst(curPrev);
				while (paths.ContainsKey(curPrev)) {
					curPrev = paths[curPrev];
					path.AddFirst(curPrev);
				}
			}
			return path;
		}

		#endregion
	}
}
