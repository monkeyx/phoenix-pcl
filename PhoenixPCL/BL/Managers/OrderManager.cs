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
using System.Net;

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
		/// Submits the orders for position.
		/// </summary>
		/// <param name="positionId">Position identifier.</param>
		/// <param name="callback">Callback.</param>
		public async void SubmitOrdersForPosition(int positionId, Action<int, Exception> callback)
		{
			List<Order> orders = await GetOrderDataManager ().GetOrdersForPosition (positionId);
			Log.WriteLine (Log.Layer.BL, this.GetType (), "Submitting " + orders.Count + " orders for Position " + positionId);
			GetRequest (positionId, true).Post (orders, (response, e) => {
				if(e == null){
					if(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created){
						GetOrderDataManager().ClearOrdersForPosition(positionId);
						callback(orders.Count,null);
					}
					else {
						callback(0,new Exception("Error submitting orders. Status Code: " + response.StatusCode));
					}
				}
				else {
					callback(0,e);
					Log.WriteLine (Log.Layer.BL, this.GetType (), e);
				}
			});
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
			GetRequest (positionId).Get (RequestCallback);
		}

		/// <summary>
		/// Deletes the order.
		/// </summary>
		/// <param name="order">Order.</param>
		/// <param name="callback">Callback.</param>
		public async void DeleteOrder(Order order, Action<IEnumerable<Order>> callback)
		{
			await GetDataManager ().DeleteItem (order);
			IEnumerable<Order> list = await GetOrderDataManager ().GetOrdersForPosition (order.PositionId);
			callback (list);
		}

		/// <summary>
		/// Adds the order.
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="orderType">Order type.</param>
		/// <param name="callback">Callback.</param>
		public async void AddOrder(Position position, OrderType orderType, Action<IEnumerable<Order>> callback)
		{
			if (position == null || orderType == null)
				return;
			
			Order order = new Order {
				OrderTypeId = orderType.Id,
				OrderType = orderType,
				PositionId = position.Id
			};

			await GetDataManager ().SaveItem (order);
			IEnumerable<Order> list = await GetOrderDataManager ().GetOrdersForPosition (position.Id);
			callback (list);
		}

		/// <summary>
		/// Saves the order.
		/// </summary>
		/// <param name="order">Order.</param>
		/// <param name="callback">Callback.</param>
		public async void SaveOrder(Order order, Action<IEnumerable<Order>> callback)
		{
			if (order == null)
				return;
			order = await GetDataManager ().SaveItem (order);
			IEnumerable<Order> list = await GetOrderDataManager ().GetOrdersForPosition (order.PositionId);
			callback (list);
		}

		/// <summary>
		/// Clears the orders.
		/// </summary>
		/// <param name="positionId">Position identifier.</param>
		/// <param name="callback">Callback.</param>
		public async void ClearOrders(int positionId, Action<IEnumerable<Order>> callback)
		{
			await GetOrderDataManager ().ClearOrdersForPosition (positionId);
			Order order = Order.ClearPendingOrders (positionId);
			await GetDataManager ().SaveItem (order);
			List<Order> list = new List<Order>();
			list.Add(order);
			callback(list);
		}

		/// <summary>
		/// Requests turn update
		/// </summary>
		/// <param name="positionId">Position identifier.</param>
		/// <param name="callback">Callback.</param>
		public async void RequestUpdate(int positionId, Action<IEnumerable<Order>> callback)
		{
			Order order = Order.RequestUpdate (positionId);
			await GetDataManager ().SaveItem (order);
			List<Order> list = await GetOrderDataManager ().GetOrdersForPosition (positionId);
			callback (list);
		}

		/// <summary>
		/// Requests the callback.
		/// </summary>
		/// <param name="results">Results.</param>
		/// <param name="e">E.</param>
		protected override async void RequestCallback(IEnumerable<Order> results, Exception e)
		{
			foreach (Order item in results) {
				try {
					await GetDataManager ().SaveItem (item);
					item.OrderType = DL.PhoenixDatabase.GetItem<OrderType>(item.OrderTypeId);
				}
				catch(Exception ex){
					Log.WriteLine (Log.Layer.BL, this.GetType (), ex);
				}
			}
			FetchInProgress = false;
			FetchCompleted = true;
			callback (results, e);
		}

		private OrderDataManager GetOrderDataManager()
		{
			return (OrderDataManager)GetDataManager ();
		}
	}
}

