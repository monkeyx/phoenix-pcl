//
// PositionDataManager.cs
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
	/// Position data manager.
	/// </summary>
	public class PositionDataManager : DataManager<Position>
	{
		/// <summary>
		/// Gets the positions in star system.
		/// </summary>
		/// <returns>The positions in star system.</returns>
		/// <param name="starSystem">Star system.</param>
		public Task<List<Position>> GetPositionsInStarSystem(StarSystem starSystem)
		{
			return Task<List<Position>>.Factory.StartNew (() => {
				if (starSystem == null) {
					return new List<Position> ();
				}
				return DL.PhoenixDatabase.GetPositionsInStarSystem (starSystem.Id);
			});
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Phoenix.DAL.PositionDataManager"/> class.
		/// </summary>
		public PositionDataManager ()
		{
		}

		/// <summary>
		/// Loads the relationships.
		/// </summary>
		/// <param name="item">Item.</param>
		protected override void LoadRelationships (Position item)
		{
			if (item.StarSystemId > 0) {
				item.StarSystem = DL.PhoenixDatabase.GetItem<StarSystem> (item.StarSystemId);
			}
		}

		/// <summary>
		/// Order results by
		/// </summary>
		/// <returns>The by.</returns>
		protected override string OrderBy ()
		{
			return "Name ASC";
		}
	}
}

