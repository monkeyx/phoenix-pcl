//
// OrderDataManager.cs
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
	/// Order data manager.
	/// </summary>
	public class OrderDataManager : DataManager<Order>
	{
		/// <summary>
		/// Copies the orders.
		/// </summary>
		/// <returns>The orders.</returns>
		/// <param name="fromPositionId">From position identifier.</param>
		/// <param name="toPositionId">To position identifier.</param>
		public Task CopyOrders(int fromPositionId, int toPositionId)
		{
			return Task.Factory.StartNew (() => {
				List<Order> models = DL.PhoenixDatabase.GetOrdersForPosition(fromPositionId);
				Log.WriteLine(Log.Layer.DAL,this.GetType(), "Copying Orders from Position " + fromPositionId + " to " + toPositionId + ": " + models.Count);
				foreach(Order o in models){
					LoadRelationships(o);
					Order newOrder = o.Copy(toPositionId);
					DL.PhoenixDatabase.SaveItem<Order>(newOrder);
					PersistRelationships(newOrder);
				}
			});
		}

		/// <summary>
		/// Clears for position.
		/// </summary>
		/// <returns>The for position.</returns>
		/// <param name="positionId">Position identifier.</param>
		public Task ClearForPosition(int positionId)
		{
			return Task.Factory.StartNew (() => {
				DL.PhoenixDatabase.DeleteOrders(positionId);
			});
		}

		/// <summary>
		/// Gets the orders for position.
		/// </summary>
		/// <returns>The orders for position.</returns>
		/// <param name="positionId">Position identifier.</param>
		/// <param name="progressCallback">Progress callback.</param>
		/// <param name="sort">If set to <c>true</c> sort.</param>
		public Task<List<Order>> GetOrdersForPosition(int positionId, IProgress<int> progressCallback = null, bool sort = false)
		{
			return Task<List<Order>>.Factory.StartNew (() => {
				List<Order> models = DL.PhoenixDatabase.GetOrdersForPosition(positionId);
				Log.WriteLine(Log.Layer.DAL,this.GetType(), "Get Orders for Position " + positionId + ": " + models.Count);
				foreach(Order o in models){
					LoadRelationships(o);
				}
				return models;
			});
		}

		/// <summary>
		/// Clears the orders for position.
		/// </summary>
		/// <returns>The orders for position.</returns>
		/// <param name="positionId">Position identifier.</param>
		public Task ClearOrdersForPosition(int positionId)
		{
			return Task.Factory.StartNew (() => {
				DL.PhoenixDatabase.DeleteOrders(positionId);
			});
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Phoenix.DAL.OrderDataManager"/> class.
		/// </summary>
		public OrderDataManager ()
		{
		}

		/// <summary>
		/// Persists the relationships.
		/// </summary>
		/// <param name="item">Item.</param>
		protected override void PersistRelationships (Order item)
		{
			Log.WriteLine (Log.Layer.DAL, this.GetType (), "Save Order Parameters (" + item.Id + "): " + item.Parameters.Count);
			if (item.OrderType == null) {
				item.OrderType = DL.PhoenixDatabase.GetItem<OrderType> (item.OrderTypeId);
				if (item.OrderType != null) {
					item.OrderType.Parameters = DL.PhoenixDatabase.GetOrderTypeParameters (item.OrderTypeId);
				}
			}
			if (item.Parameters.Count > 0) {
				int i = 0;
				foreach (OrderParameter param in item.Parameters) {
					param.OrderId = item.Id;
					param.DisplayValue = param.Value;
					if (item.OrderType != null && item.OrderType.Parameters.Count > i) {
						OrderParameterType paramType = item.OrderType.Parameters [i];
						if (paramType.InfoType > 0) {
							try {
								InfoData info = DL.PhoenixDatabase.GetInfoDataByGroupIdAndNexusId (paramType.InfoType, Int32.Parse (param.Value));
								if(info != null){
									param.DisplayValue = info.ToString();
								}
							}
							catch {
							}
						}
					}
					DL.PhoenixDatabase.SaveItemIfNew<OrderParameter> (param);
					i += 1;
				}
			}
		}

		/// <summary>
		/// Loads the relationships.
		/// </summary>
		/// <param name="item">Item.</param>
		protected override void LoadRelationships (Order item)
		{
			Log.WriteLine (Log.Layer.DAL, this.GetType (), "Load Relationships (" + item.Id + ")");
			item.Parameters = DL.PhoenixDatabase.GetOrderParameters (item.Id);
			item.OrderType = DL.PhoenixDatabase.GetItem<OrderType> (item.OrderTypeId);
			if (item.OrderType != null) {
				item.OrderType.Parameters = DL.PhoenixDatabase.GetOrderTypeParameters (item.OrderTypeId);
			}
		}

		/// <summary>
		/// Deletes the relationships.
		/// </summary>
		/// <param name="item">Item.</param>
		protected override void DeleteRelationships (Order item)
		{
			if (item == null || item.Id == 0)
				return;
			Log.WriteLine (Log.Layer.DAL, this.GetType (), "Delete Relationships (" + item.Id + ")");
			DL.PhoenixDatabase.DeleteOrderParameters (item.Id);
		}

		/// <summary>
		/// Deletes all entities.
		/// </summary>
		protected override void DeleteAllEntities ()
		{
			Log.WriteLine (Log.Layer.DAL, this.GetType (), "Delete All Orders and Order Parameters");
			DL.PhoenixDatabase.ClearTable<Order> ();
			DL.PhoenixDatabase.ClearTable<OrderParameter> ();
		}

		/// <summary>
		/// Order results by
		/// </summary>
		/// <returns>The by.</returns>
		protected override string OrderBy ()
		{
			return "PositionId asc, SequencePosition asc";
		}
	}
}

