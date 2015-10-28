//
// StarSystemManager.cs
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

using Phoenix.BL.Entities;
using Phoenix.DAL;
using Phoenix.Util;

namespace Phoenix.BL.Managers
{
    /// <summary>
    /// Star system manager.
    /// </summary>
    public class StarSystemManager : NexusManager<StarSystem>
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.BL.Managers.StarSystemManager"/> class.
        /// </summary>
        /// <param name="user">User.</param>
        public StarSystemManager (User user) : base(user)
        {
        }

		/// <summary>
		/// Requests the callback.
		/// </summary>
		/// <param name="results">Results.</param>
		/// <param name="e">E.</param>
		protected override async void RequestCallback(IEnumerable<StarSystem> results, Exception e)
		{
			if (results == null) {
				callback (new List<StarSystem> (), e == null ? new Exception ("No results received " + GetType()) : e);
				return;
			}

			foreach (StarSystem item in results) {
				try {
					await GetDataManager ().SaveItem (item);
				}
				catch(Exception ex){
					Log.WriteLine (Log.Layer.BL, this.GetType (), ex);
				}
			}
			FetchInProgress = false;
			FetchCompleted = true;

			NavigationPathDataManager pathManager = (NavigationPathDataManager) DataManagerFactory.GetManager<NavigationPath> ();

			foreach (StarSystem ss in results) {
				await pathManager.GeneratePaths (ss.Id, new List<PathPoint> ());
			}

			callback (results, e);
		}
    }
}

