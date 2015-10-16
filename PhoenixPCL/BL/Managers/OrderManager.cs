//
// OrderManager.cs
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
using System.Linq;

using Phoenix.BL.Entities;
using Phoenix.DAL;
using Phoenix.Util;

namespace Phoenix.BL.Managers
{
	/// <summary>
	/// Order manager.
	/// </summary>
	public class OrderManager : NexusManager<Order>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Phoenix.BL.Managers.OrderManager"/> class.
		/// </summary>
		/// <param name="user">User.</param>
		public OrderManager (User user) : base(user)
		{
		}

		/// <summary>
		/// Alls for position.
		/// </summary>
		/// <param name="positionId">Position identifier.</param>
		/// <param name="callback">Callback.</param>
		public async void AllForPosition(int positionId, Action<List<Order>> callback)
		{
			Log.WriteLine (Log.Layer.BL, this.GetType (), "All Entities");
			List<Order> results = await GetOrderDataManager ().GetOrdersForPosition (positionId);
			callback (results);
		}

		/// <summary>
		/// Fetchs for position.
		/// </summary>
		/// <param name="positionId">Position identifier.</param>
		/// <param name="callback">Callback.</param>
		/// <param name="clearFirst">If set to <c>true</c> clear first.</param>
		public async void FetchForPosition(int positionId, Action<IEnumerable<Order>, Exception> callback, bool clearFirst = true)
		{
			if (clearFirst) {
				Log.WriteLine (Log.Layer.BL, this.GetType (), "Clearing Orders for Position " + positionId);
				await GetOrderDataManager().ClearForPosition (positionId);
			}
			FetchInProgress = true;
			this.callback = callback;
			Log.WriteLine (Log.Layer.BL, this.GetType (), "Fetching Order for Position " + positionId);
			GetRequest (positionId).Fetch (RequestCallback);
		}

		private OrderDataManager GetOrderDataManager()
		{
			return (OrderDataManager)GetDataManager ();
		}
	}
}

