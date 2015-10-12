﻿//
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
            foreach (CelestialBody cb in item.CelestialBodies) {
                cb.StarSystemId = item.Id;
                DL.PhoenixDatabase.SaveItemIfNew<CelestialBody> (cb);
            }
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
            item.CelestialBodies = DL.PhoenixDatabase.GetCelestialBodies (item.Id);
            item.JumpLinks = DL.PhoenixDatabase.GetJumpLinks (item.Id);
        }

        /// <summary>
        /// Deletes the relationships.
        /// </summary>
        /// <param name="item">Item.</param>
        protected override void DeleteRelationships(StarSystem item)
        {
            DL.PhoenixDatabase.DeleteCelestialBodies (item.Id);
            DL.PhoenixDatabase.DeleteJumpLinks (item.Id);
        }
    }
}

