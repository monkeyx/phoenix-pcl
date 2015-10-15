//
// StarSystemDataManager.cs
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

using Phoenix.BL.Entities;
using Phoenix.Util;

namespace Phoenix.DAL
{
    /// <summary>
    /// Star system data manager.
    /// </summary>
    public class StarSystemDataManager : DataManager<StarSystem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.DAL.StarSystemDataManager"/> class.
        /// </summary>
        public StarSystemDataManager ()
        {
        }

        /// <summary>
        /// Persists the relationships.
        /// </summary>
        /// <param name="item">Item.</param>
        protected override void PersistRelationships(StarSystem item)
        {
			Log.WriteLine (Log.Layer.DAL, this.GetType (), "Persist Celestial Bodies (" + item.Id + "): " + item.CelestialBodies.Count);
            foreach (CelestialBody cb in item.CelestialBodies) {
                cb.StarSystemId = item.Id;
                DL.PhoenixDatabase.SaveItemIfNew<CelestialBody> (cb);
            }
			Log.WriteLine (Log.Layer.DAL, this.GetType (), "Persist Jump Links (" + item.Id + "): " + item.JumpLinks.Count);
			foreach (JumpLink jl in item.JumpLinks) {
                jl.StarSystemId = item.Id;
                DL.PhoenixDatabase.SaveItemIfNew<JumpLink> (jl);
            }
        }

        /// <summary>
        /// Loads the relationships.
        /// </summary>
        /// <param name="item">Item.</param>
        protected override void LoadRelationships(StarSystem item)
        {
			Log.WriteLine (Log.Layer.DAL, this.GetType (), "Load Relationships (" + item.Id + ")");
            item.CelestialBodies = DL.PhoenixDatabase.GetCelestialBodies (item.Id);
            item.JumpLinks = DL.PhoenixDatabase.GetJumpLinks (item.Id);
			foreach (JumpLink jl in item.JumpLinks) {
				jl.ToStarSysytem = DL.PhoenixDatabase.GetItem<StarSystem> (jl.ToStarSystemId);
			}
        }

        /// <summary>
        /// Deletes the relationships.
        /// </summary>
        /// <param name="item">Item.</param>
        protected override void DeleteRelationships(StarSystem item)
        {
			Log.WriteLine (Log.Layer.DAL, this.GetType (), "Delete Relationships (" + item.Id + ")");
            DL.PhoenixDatabase.DeleteCelestialBodies (item.Id);
            DL.PhoenixDatabase.DeleteJumpLinks (item.Id);
        }

		/// <summary>
		/// Deletes all entities.
		/// </summary>
		protected override void DeleteAllEntities()
		{
			Log.WriteLine (Log.Layer.DAL, this.GetType (), "Delete All Star Systems, Celestial Bodies and Jump Links");
			DL.PhoenixDatabase.ClearTable<StarSystem>();
			DL.PhoenixDatabase.ClearTable<CelestialBody>();
			DL.PhoenixDatabase.ClearTable<JumpLink>();
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

