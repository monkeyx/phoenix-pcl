//
// InfoManager.cs
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
using Phoenix.BL.Managers;
using Phoenix.DAL;

namespace Phoenix.BL.Managers
{
    /// <summary>
    /// Info manager.
    /// </summary>
    public class InfoManager : NexusManager<InfoData>
    {
		/// <summary>
		/// Gets the info data by group identifier and nexus identifier.
		/// </summary>
		/// <param name="groupId">Group identifier.</param>
		/// <param name="nexusId">Nexus identifier.</param>
		/// <param name="callback">Callback.</param>
		public async void GetInfoDataByGroupIdAndNexusId(int groupId, int nexusId, Action<InfoData> callback)
		{
			InfoData data = await GetInfoDataManager ().GetInfoDataByGroupIdAndNexusId (groupId,nexusId);
			callback (data);
		}

		/// <summary>
		/// Gets the info data by group identifier.
		/// </summary>
		/// <param name="groupId">Group identifier.</param>
		/// <param name="callback">Callback.</param>
		public async void GetInfoDataByGroupId(int groupId, Action<List<InfoData>> callback)
		{
			List<InfoData> list = await GetInfoDataManager ().GetInfoDataByGroupId (groupId);
			callback (list);
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.BL.Managers.InfoManager"/> class.
        /// </summary>
        /// <param name="user">User.</param>
        public InfoManager (User user) : base(user)
        {
        }

		private InfoDataManager GetInfoDataManager()
		{
			return (InfoDataManager)GetDataManager ();
		}
    }
}

