//
// NavigationPathDataManager.cs
//
// Author:
//       Seyed Razavi <monkeyx@gmail.com>
//
// Copyright (c) 2015 Seyed Razavi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Phoenix.BL.Entities;
using Phoenix.Util;

namespace Phoenix.DAL
{
	/// <summary>
	/// Navigation path data manager.
	/// </summary>
	public class NavigationPathDataManager : DataManager<NavigationPath>
	{
		/// <summary>
		/// Gets the path between system.
		/// </summary>
		/// <returns>The path between system.</returns>
		/// <param name="fromStarSystemId">From star system identifier.</param>
		/// <param name="toStarSystemId">To star system identifier.</param>
		public Task<NavigationPath> GetPathBetweenSystem(int fromStarSystemId, int toStarSystemId)
		{
			return Task<NavigationPath>.Factory.StartNew (() => {
				NavigationPath path = GetShortestPath(fromStarSystemId,toStarSystemId);
				LoadRelationships(path);
				Log.WriteLine(Log.Layer.DAL,GetType(),"Shortest path between " + fromStarSystemId + " and " + toStarSystemId + ": " + path);
				return path;
			});
		}

		/// <summary>
		/// Generates the paths.
		/// </summary>
		/// <returns>The paths.</returns>
		/// <param name="startSystemId">Start system identifier.</param>
		/// <param name="path">Path.</param>
		public Task GeneratePaths(int startSystemId, List<PathPoint> path)
		{
			return Task.Factory.StartNew (() => {
				if (path.Count > NavigationPath.MAX_JUMPS) {
					return;
				}
				// Log.WriteLine(Log.Layer.DAL,GetType(),"Generating path from " + startSystemId + " (existing points: " + path.Count + ")");
				int fromSystemId = startSystemId;
				int previousStarSystemId = 0;

				if(path.Count > 0){
					PathPoint last = path[(path.Count - 1)];
					fromSystemId = last.ToStarSystemId;
					previousStarSystemId = last.FromStarSystemId;
				}

				if(fromSystemId != previousStarSystemId){
					List<PathPoint> results = GetPotentialPathPoints(fromSystemId,previousStarSystemId);

					foreach(PathPoint pp in results){
						if(pp.ToStarSystemId != startSystemId){
							List<PathPoint> pathCopy = new List<PathPoint>(path);
							pathCopy.Add(pp);
							if(MakePath(startSystemId,pp.ToStarSystemId,pathCopy) != null && pathCopy.Count < NavigationPath.MAX_JUMPS){
								GeneratePaths(startSystemId,pathCopy);
							}
							pathCopy = ReversePath(pathCopy);
							if(pathCopy != null){
								MakePath(pp.ToStarSystemId,startSystemId,pathCopy);
							}
						}
					}
				}
			});

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Phoenix.DAL.NavigationPathDataManager"/> class.
		/// </summary>
		public NavigationPathDataManager ()
		{
		}

		/// <summary>
		/// Persists the relationships.
		/// </summary>
		/// <param name="item">Item.</param>
		protected override void PersistRelationships (NavigationPath item)
		{
			if (item != null) {
				// Log.WriteLine (Log.Layer.DAL, this.GetType (), "Save path points (" + item.Id + "): " + item.PathPoints.Count);
				item.TotalJumps = item.PathPoints.Count;
				foreach (PathPoint pp in item.PathPoints) {
					pp.NavigationPathId = item.Id;
					DL.PhoenixDatabase.SaveItemIfNew<PathPoint> (pp);
				}
			}
		}

		/// <summary>
		/// Loads the relationships.
		/// </summary>
		/// <param name="item">Item.</param>
		protected override void LoadRelationships (NavigationPath item)
		{
			if (item != null) {
				Log.WriteLine (Log.Layer.DAL, this.GetType (), "Load Relationships (" + item.Id + ")");
				item.PathPoints = DL.PhoenixDatabase.GetPathPoints (item.Id);
				foreach (PathPoint pp in item.PathPoints) {
					pp.ToStarSysytem = DL.PhoenixDatabase.GetItem<StarSystem> (pp.ToStarSystemId);
				}
			}
		}

		/// <summary>
		/// Deletes the relationships.
		/// </summary>
		/// <param name="item">Item.</param>
		protected override void DeleteRelationships (NavigationPath item)
		{
			if (item == null || item.Id == 0)
				return;

			Log.WriteLine (Log.Layer.DAL, this.GetType (), "Delete Relationships (" + item.Id + ")");
			DL.PhoenixDatabase.DeletePathPoints (item.Id);
		}

		/// <summary>
		/// Deletes all entities.
		/// </summary>
		protected override void DeleteAllEntities ()
		{
			Log.WriteLine (Log.Layer.DAL, this.GetType (), "Delete All Navigation Paths and Path Points");
			DL.PhoenixDatabase.ClearTable<NavigationPath> ();
			DL.PhoenixDatabase.ClearTable<PathPoint> ();
		}

		/// <summary>
		/// Order results by
		/// </summary>
		/// <returns>The by.</returns>
		protected override string OrderBy ()
		{
			return "TotalJumps asc";
		}

		// not used yet
		private NavigationPath Extrapolate(int fromSystemId, int toSystemId)
		{
			NavigationPath path = null;
			int inbetweenSystemId = DL.PhoenixDatabase.GetInterconnectingSystem(fromSystemId,toSystemId);
			if(inbetweenSystemId > 0){
				NavigationPath pathA = GetShortestPath(fromSystemId,inbetweenSystemId);
				NavigationPath pathB = GetShortestPath(inbetweenSystemId,toSystemId);
				List<PathPoint> combinedPath = NavigationPath.CombinePaths(pathA,pathB);
				path = MakePath(fromSystemId,toSystemId,combinedPath);

				List<PathPoint> reversePath = ReversePath(combinedPath);
				if(reversePath != null){
					MakePath(toSystemId,fromSystemId,reversePath);
				}
			}
			return path;
		}

		private List<PathPoint> ReversePath(List<PathPoint> path)
		{
			List<PathPoint> reverse = new List<PathPoint> ();

			for (int i = (path.Count - 1); i >= 0; i--) {
				PathPoint pp = path [i];
				int fromId = pp.ToStarSystemId;
				int toId = pp.FromStarSystemId;
				if (DL.PhoenixDatabase.GetJumpLinksBetweenSystems (fromId, toId).Count > 0) {
					PathPoint newPoint = new PathPoint {
						FromStarSystemId = fromId,
						ToStarSystemId = toId,
						Distance = pp.Distance
					};
					reverse.Add (newPoint);
				} else { // no reverse jump
					return null;
				}
			}

			return reverse;
		}

		private NavigationPath MakePath(int fromStarSystemId, int toStarSystemId, List<PathPoint> path)
		{
			NavigationPath shortestPath = GetShortestPath(fromStarSystemId, toStarSystemId);
			if(shortestPath == null || shortestPath.TotalJumps > path.Count){
				StarSystem fromSystem = DL.PhoenixDatabase.GetItem<StarSystem> (fromStarSystemId);
				StarSystem toSystem = DL.PhoenixDatabase.GetItem<StarSystem> (toStarSystemId);
				NavigationPath nav = new NavigationPath {
					FromStarSystemId = fromStarSystemId,
					ToStarSystemId = toStarSystemId,
					FromStarSystemName = fromSystem.Name,
					ToStarSystemName = toSystem.Name,
					PathPoints = path
				};
				SaveItem (nav).Wait();

				return nav;
			}		
			return null;
		}

		private NavigationPath GetShortestPath(int fromStarSystemId, int toStarSystemId)
		{
			List<NavigationPath> paths = DL.PhoenixDatabase.GetNavigationPath(fromStarSystemId,toStarSystemId);
			if(paths.Count > 0){
				NavigationPath path = paths[0];
				return path;
			}
			return null;
		}

		private List<PathPoint> GetPotentialPathPoints(int startSystemId, int previousStarSystemId)
		{
			List<PathPoint> list = new List<PathPoint> ();
			if (startSystemId > 0) {
				List<JumpLink> jumpLinks = DL.PhoenixDatabase.GetJumpLinks (startSystemId);
				foreach (JumpLink jl in jumpLinks) {
					if (previousStarSystemId != jl.ToStarSystemId) {
						list.Add (new PathPoint {
							ToStarSystemId = jl.ToStarSystemId,
							Distance = jl.Distance,
							FromStarSystemId = startSystemId
						});
					}
				}
			}
			return list;
		}
	}
}

