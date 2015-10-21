//
// PositionManager.cs
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
using Phoenix.DAL;
using Phoenix.SAL;

namespace Phoenix.BL.Managers
{
    /// <summary>
    /// Position manager.
    /// </summary>
    public class PositionManager : NexusManager<Position>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.BL.Managers.PositionManager"/> class.
        /// </summary>
        /// <param name="user">User.</param>
        public PositionManager (User user) : base(user)
        {
        }

		/// <summary>
		/// Gets the positions in star system.
		/// </summary>
		/// <param name="starSystem">Star system.</param>
		/// <param name="callback">Callback.</param>
		public async void GetPositionsInStarSystem(StarSystem starSystem, Action<IEnumerable<Position>> callback)
		{
			List<Position> list = await ((PositionDataManager)GetDataManager ()).GetPositionsInStarSystem (starSystem);
			callback (list);
		}

		/// <summary>
		/// Gets the positions with orders.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public async void GetPositionsWithOrders(Action<IEnumerable<Position>> callback)
		{
			List<Position> list = await ((PositionDataManager)GetDataManager ()).GetPositionsWithOrders ();
			callback (list);
		}

		/// <summary>
		/// Gets the turn report.
		/// </summary>
		/// <param name="positionId">Position identifier.</param>
		/// <param name="callback">Callback.</param>
		public async void GetTurnReport(int positionId, Action<string> callback)
		{
			DataManager<PositionTurn> turnDM = DataManagerFactory.GetManager<PositionTurn> ();
			PositionTurn pt = await turnDM.GetItem (positionId);
			if (pt == null) {
				FetchTurn(positionId, callback);
			} else {
				await Task.Factory.StartNew (() => {
					try {
						string content = Application.DocumentFolder.ReadFile(pt.TurnPath);
						callback(content);
						FetchTurn(positionId, callback);
					}
					catch {
						FetchTurn(positionId, callback);
					}

				});
			}
		}

		private void FetchTurn(int positionId, Action<string> callback)
		{
			DataManager<PositionTurn> turnDM = DataManagerFactory.GetManager<PositionTurn> ();
			TurnRequest request = new TurnRequest (User.Id, User.Code, positionId);
			request.Get ((results, ex) => {
				IEnumerator<PositionTurn> i = results.GetEnumerator ();
				if (i.MoveNext ()) {
					PositionTurn pt = i.Current;
					turnDM.SaveItem (pt);
					callback (pt.Content);
				} else {
					callback("Not Found");
				}
			});
		}
    }
}

