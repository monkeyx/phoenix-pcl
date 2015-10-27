//
// NotificationDataManager.cs
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
	/// Notification data manager.
	/// </summary>
	public class NotificationDataManager : DataManager<Notification>
	{
		/// <summary>
		/// Gets the notifications for position.
		/// </summary>
		/// <returns>The notifications for position.</returns>
		/// <param name="positionId">Position identifier.</param>
		/// <param name="priority">Priority</param>
		/// <param name="progressCallback">Progress callback.</param>
		/// <param name="sort">If set to <c>true</c> sort.</param>
		public Task<List<Notification>> GetNotificationsForPosition(int positionId, Notification.NotificationPriority priority = Notification.NotificationPriority.All, IProgress<int> progressCallback = null, bool sort = false)
		{
			return Task<List<Notification>>.Factory.StartNew (() => {
				List<Notification> models = null;
				if(priority == Notification.NotificationPriority.All){
					models = DL.PhoenixDatabase.GetNotificationsForPosition(positionId);
				}
				else {
					models = DL.PhoenixDatabase.GetNotificationsForPosition(positionId,priority);
				}
				Log.WriteLine(Log.Layer.DAL,this.GetType(), "Get Notifications for Position " + positionId + ": " + models.Count);
				return models;
			});
		} 
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Phoenix.DAL.NotificationDataManager"/> class.
		/// </summary>
		public NotificationDataManager ()
		{
		}

		/// <summary>
		/// Persists the relationships.
		/// </summary>
		/// <param name="item">Item.</param>
		protected override void PersistRelationships (Notification item)
		{
			if (item.Type == Notification.NotificationType.ComplexChange && !string.IsNullOrWhiteSpace (item.ComplexType)) {
				try {
					Item complex = DL.PhoenixDatabase.GetItem<Item> (Int32.Parse (item.ComplexType));
					if(complex != null && complex.ItemType == "Complex"){
						item.ComplexItem = complex;
						item.ComplexTypeName = complex.ToString();
					} else {
						item.ComplexTypeName = "Unknown";
					}
				}
				catch{
				}
			}
			if (item.OrderId > 0) {
				item.Order = DL.PhoenixDatabase.GetItem<OrderType> (item.OrderId);
				if (item.Order != null) {
					item.OrderName = item.Order.Name;
				}
			}
		}

		/// <summary>
		/// Loads the relationships.
		/// </summary>
		/// <param name="item">Item.</param>
		protected override void LoadRelationships (Notification item)
		{
			if (item.Type == Notification.NotificationType.ComplexChange && !string.IsNullOrWhiteSpace (item.ComplexType)) {
				try {
					Item complex = DL.PhoenixDatabase.GetItem<Item> (Int32.Parse (item.ComplexType));
					if(complex != null && complex.ItemType == "Complex"){
						item.ComplexItem = complex;
						item.ComplexTypeName = complex.ToString();
					} else {
						item.ComplexTypeName = "Unknown";
					}
				}
				catch{
				}
			}
			if (item.PositionId > 0) {
				item.Position = DL.PhoenixDatabase.GetItem<Position> (item.PositionId);
			}
			if (item.StarSystemId > 0) {
				item.StarSystem = DL.PhoenixDatabase.GetItem<StarSystem> (item.StarSystemId);
				if (item.StarSystem != null && item.CelestialBodyId > 0) {
					item.CelestialBody = item.StarSystem.GetCelestialBody (item.CelestialBodyId);
				}
			}
			if (item.OrderId > 0) {
				item.Order = DL.PhoenixDatabase.GetItem<OrderType> (item.OrderId);
				if (item.Order != null) {
					item.OrderName = item.Order.Name;
				}
			}
			if (item.ItemId > 0) {
				item.Item = DL.PhoenixDatabase.GetItem<Item> (item.ItemId);
			}
			if (item.BaseId > 0) {
				item.Base = DL.PhoenixDatabase.GetItem<Position> (item.BaseId);
			}
			if (item.DeliverToId > 0) {
				item.DeliverTo = DL.PhoenixDatabase.GetItem<Position> (item.DeliverToId);
			}
			if (item.PickedUpFromId > 0) {
				item.PickedUpFrom = DL.PhoenixDatabase.GetItem<Position> (item.PickedUpFromId);
			}
			if (item.BoughtFromId > 0) {
				item.BoughtFrom = DL.PhoenixDatabase.GetItem<Position> (item.BoughtFromId);
			}
			if (item.SoldToId > 0) {
				item.SoldTo = DL.PhoenixDatabase.GetItem<Position> (item.SoldToId);
			}
			if (item.ByPositionId > 0) {
				item.ByPosition = DL.PhoenixDatabase.GetItem<Position> (item.ByPositionId);
			}
		}

		/// <summary>
		/// Deletes the relationships.
		/// </summary>
		/// <param name="item">Item.</param>
		protected override void DeleteRelationships (Notification item)
		{
			base.DeleteRelationships (item);
		}

		/// <summary>
		/// Deletes all entities.
		/// </summary>
		protected override void DeleteAllEntities ()
		{
			base.DeleteAllEntities ();
		}

		/// <summary>
		/// Order results by
		/// </summary>
		/// <returns>The by.</returns>
		protected override string OrderBy ()
		{
			return "DaysAgo asc";
		}
	}
}

