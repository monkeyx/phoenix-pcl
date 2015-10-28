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
		/// Gets or sets to star system identifier.
		/// </summary>
		/// <value>To star system identifier.</value>
		[Indexed]
		public int ToStarSystemId { get; set; }

		/// <summary>
		/// Gets or sets the path point count.
		/// </summary>
		/// <value>The path point count.</value>
		[Indexed]
		public int PathPointCount { get; set; }

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
			return string.Format ("[NavigationPath: Id={0}, FromStarSystemId={1}, ToStarSystemId={2}, PathPointCount={3}, PathPoints={4}]", Id, FromStarSystemId, ToStarSystemId, PathPointCount, PathPoints);
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
		/// Gets or sets the navigation path identifier.
		/// </summary>
		/// <value>The navigation path identifier.</value>
		[Indexed]
		public int NavigationPathId { get; set; }

		/// <summary>
		/// Gets or sets the jump link identifier.
		/// </summary>
		/// <value>The jump link identifier.</value>
		public int JumpLinkId { get; set; }

		/// <summary>
		/// Gets or sets the jump link.
		/// </summary>
		/// <value>The jump link.</value>
		[Ignore]
		public JumpLink JumpLink { get; set; }

		/// <summary>
		/// Gets or sets to star system identifier.
		/// </summary>
		/// <value>To star system identifier.</value>
		public int ToStarSystemId { get; set; }

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
			return string.Format ("[PathPoint: Id={0}, NavigationPathId={1}, JumpLinkId={2}, JumpLink={3}, ToStarSystemId={4}, FromStarSystemId={5}]", Id, NavigationPathId, JumpLinkId, JumpLink, ToStarSystemId, FromStarSystemId);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Phoenix.BL.Entities.PathPoint"/> class.
		/// </summary>
		public PathPoint ()
		{
		}
	}
}

