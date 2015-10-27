//
// NotificationManager.cs
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
using Phoenix.Util;

namespace Phoenix.BL.Managers
{
	/// <summary>
	/// Notification manager.
	/// </summary>
	public class NotificationManager : NexusManager<Notification>
	{
		/// <summary>
		/// Alls for position.
		/// </summary>
		/// <param name="positionId">Position identifier.</param>
		/// <param name="callback">Callback.</param>
		/// <param name="priority">Priority</param>
		public async void AllForPosition(int positionId, Action<List<Notification>> callback, Notification.NotificationPriority priority = Notification.NotificationPriority.All)
		{
			Log.WriteLine (Log.Layer.BL, this.GetType (), "All Notifications For Position " + positionId);
			List<Notification> results = await GetNotificationDataManager ().GetNotificationsForPosition (positionId);
			callback (results);
		}

		/// <summary>
		/// Deletes the notification.
		/// </summary>
		/// <param name="notification">Notification.</param>
		/// <param name="callback">Callback.</param>
		public async void DeleteNotification(Notification notification, Action<IEnumerable<Notification>> callback)
		{
			await GetDataManager ().DeleteItem (notification);
			IEnumerable<Notification> list = await GetNotificationDataManager ().GetNotificationsForPosition (notification.PositionId);
			callback (list);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Phoenix.BL.Managers.NotificationManager"/> class.
		/// </summary>
		/// <param name="user">User.</param>
		public NotificationManager (User user) : base(user)
		{
		}

		private NotificationDataManager GetNotificationDataManager()
		{
			return (NotificationDataManager)GetDataManager ();
		}
	}
}

