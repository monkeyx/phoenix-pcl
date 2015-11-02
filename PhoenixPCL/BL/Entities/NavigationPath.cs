//
// NavigationPath.cs
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
using SQLite.Net.Attributes; 

namespace Phoenix.BL.Entities
{
	[Table("NavigationPath")]
	public class NavigationPath : EntityBase
	{
		/// <summary>
		/// Combines the paths.
		/// </summary>
		/// <returns>The paths.</returns>
		/// <param name="a">The alpha component.</param>
		/// <param name="b">The blue component.</param>
		public static List<PathPoint> CombinePaths(NavigationPath a, NavigationPath b)
		{
			List<PathPoint> points = new List<PathPoint> ();
			foreach (PathPoint pp in a.PathPoints) {
				points.Add (new PathPoint {
					ToStarSystemId = pp.ToStarSystemId,
					Distance = pp.Distance,
					FromStarSystemId = pp.FromStarSystemId
				});
			}

			foreach (PathPoint pp in b.PathPoints) {
				points.Add (new PathPoint {
					ToStarSystemId = pp.ToStarSystemId,
					Distance = pp.Distance,
					FromStarSystemId = pp.FromStarSystemId
				});
			}

			return points;
		}

		/// <summary>
		/// Maximum jumps for a navigation path
		/// </summary>
		public const int MAX_JUMPS = 4;

		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		[PrimaryKey, AutoIncrement]
		public override int Id { get; set; }

		/// <summary>
		/// Gets or sets from star system identifier.
		/// </summary>
		/// <value>From star system identifier.</value>
		[Indexed]
		public int FromStarSystemId { get; set; }

		/// <summary>
		/// Gets or sets the name of the from star system.
		/// </summary>
		/// <value>The name of the from star system.</value>
		public string FromStarSystemName { get; set; }

		/// <summary>
		/// Gets or sets to star system identifier.
		/// </summary>
		/// <value>To star system identifier.</value>
		[Indexed]
		public int ToStarSystemId { get; set; }

		/// <summary>
		/// Gets or sets the name of the to star system.
		/// </summary>
		/// <value>The name of the to star system.</value>
		public string ToStarSystemName { get; set; }

		/// <summary>
		/// Gets or sets the total jumps.
		/// </summary>
		/// <value>The total jumps.</value>
		[Indexed]
		public int TotalJumps { get; set; }

		/// <summary>
		/// Gets or sets the path points.
		/// </summary>
		/// <value>The path points.</value>
		[Ignore]
		public List<PathPoint> PathPoints { get; set; } = new List<PathPoint>();

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.NavigationPath"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.NavigationPath"/>.</returns>
		public override string ToString ()
		{
			return string.Format ("{0} ({1}) -> {2} ({3})", FromStarSystemName, FromStarSystemId,ToStarSystemName,ToStarSystemId);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Phoenix.BL.Entities.NavigationPath"/> class.
		/// </summary>
		public NavigationPath ()
		{
		}
	}

	[Table("PathPoint")]
	public class PathPoint : EntityBase 
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		[PrimaryKey, AutoIncrement]
		public override int Id { get; set; }

		/// <summary>
		/// Gets the list text.
		/// </summary>
		/// <value>The list text.</value>
		[Ignore]
		public override string ListText { get { return ToStarSystemName; } }

		/// <summary>
		/// Gets the list detail.
		/// </summary>
		/// <value>The list detail.</value>
		[Ignore]
		public override string ListDetail { get { return DistanceString; } }

		/// <summary>
		/// Gets or sets the navigation path identifier.
		/// </summary>
		/// <value>The navigation path identifier.</value>
		[Indexed]
		public int NavigationPathId { get; set; }

		/// <summary>
		/// Gets or sets to star system identifier.
		/// </summary>
		/// <value>To star system identifier.</value>
		public int ToStarSystemId { get; set; }

		/// <summary>
		/// Gets or sets to star sysytem.
		/// </summary>
		/// <value>To star sysytem.</value>
		[Ignore]
		public StarSystem ToStarSysytem { get; set; }

		/// <summary>
		/// Gets the name of the to star system.
		/// </summary>
		/// <value>The name of the to star system.</value>
		[Ignore]
		public string ToStarSystemName { 
			get {
				return ToStarSysytem == null ? "Unknown" : ToStarSysytem.ToString ();
			}
		}

		/// <summary>
		/// Gets or sets the distance.
		/// </summary>
		/// <value>The distance.</value>
		public int Distance { get; set; }

		/// <summary>
		/// Gets the distance string.
		/// </summary>
		/// <value>The distance string.</value>
		[Ignore]
		public string DistanceString {
			get {
				return Distance.ToString () + (Distance == 1 ? " jump" : " jumps");
			}
		}

		/// <summary>
		/// Gets or sets from star system identifier.
		/// </summary>
		/// <value>From star system identifier.</value>
		public int FromStarSystemId { get; set; }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.PathPoint"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.PathPoint"/>.</returns>
		public override string ToString ()
		{
			return string.Format ("Jump {0} ({1})", ToStarSystemName,ToStarSystemId);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Phoenix.BL.Entities.PathPoint"/> class.
		/// </summary>
		public PathPoint ()
		{
		}
	}
}

