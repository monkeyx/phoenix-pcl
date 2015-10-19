//
// InfoDataManager.cs
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
	/// Info data manager.
	/// </summary>
	public class InfoDataManager : DataManager<InfoData>
	{
		/// <summary>
		/// Gets the info data by group identifier and nexus identifier.
		/// </summary>
		/// <returns>The info data by group identifier and nexus identifier.</returns>
		/// <param name="groupId">Group identifier.</param>
		/// <param name="nexusId">Nexus identifier.</param>
		public Task<InfoData> GetInfoDataByGroupIdAndNexusId(int groupId, int nexusId)
		{
			return Task<InfoData>.Factory.StartNew (() => {
				return DL.PhoenixDatabase.GetInfoDataByGroupIdAndNexusId(groupId,nexusId);
			});
		}

		/// <summary>
		/// Gets the info data by group identifier.
		/// </summary>
		/// <returns>The info data by group identifier.</returns>
		/// <param name="groupId">Group identifier.</param>
		public Task<List<InfoData>> GetInfoDataByGroupId(int groupId)
		{
			return Task<List<InfoData>>.Factory.StartNew (() => {
				return DL.PhoenixDatabase.GetInfoDataByGroupId(groupId);
			});
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Phoenix.DAL.InfoDataManager"/> class.
		/// </summary>
		public InfoDataManager ()
		{
		}
	}
}

