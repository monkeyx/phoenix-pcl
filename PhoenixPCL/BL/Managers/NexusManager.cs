//
// BaseNexusManager.cs
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

using Phoenix.DAL;
using Phoenix.SAL;
using Phoenix.BL.Entities;

namespace Phoenix.BL.Managers
{
    /// <summary>
    /// Nexus manager abstract class
    /// </summary>
    public abstract class NexusManager<T> : IEntityManager<T> where T :   EntityBase, new()
    {
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Phoenix.NexusManager`1"/> fetch completed.
        /// </summary>
        /// <value><c>true</c> if fetch completed; otherwise, <c>false</c>.</value>
        public bool FetchCompleted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Phoenix.NexusManager`1"/> fetch in progress.
        /// </summary>
        /// <value><c>true</c> if fetch in progress; otherwise, <c>false</c>.</value>
        public bool FetchInProgress { get; set; }

		/// <summary>
		/// Fetches data from Nexus
		/// </summary>
		/// <param name="callback">Callback.</param>
		public void Fetch(Action<List<T>> callback)
        {
            FetchInProgress = true;
			this.callback = callback;
            GetRequest ().Fetch (RequestCallback);
        }

		/// <summary>
		/// Count entities managed by this instance.
		/// </summary>
		public int Count()
		{
			return GetDataManager ().Count;
		}

        /// <summary>
        /// Gets an entity by its identifier and passes it to the callback
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="callback">Callback.</param>
        public async void Get(int id, Action<T> callback)
        {
            T item = await GetDataManager ().GetItem (id);
            callback (item);
        }

        /// <summary>
        /// Gets the first entity by its identifier and passes it to the callback
        /// </summary>
        /// <param name="callback">Callback.</param>
        public async void First(Action<T> callback)
        {
            T item = await GetDataManager ().GetFirstItem ();
            callback (item);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.NexusManager`1"/> class.
        /// </summary>
        /// <param name="user">User.</param>
        public NexusManager (User user)
        {
            User = user;
            FetchCompleted = false;
            FetchInProgress = false;
        }

        /// <summary>
        /// Gets the data manager.
        /// </summary>
        /// <returns>The data manager.</returns>
        protected DataManager<T> GetDataManager()
        {
            return DataManagerFactory.GetManager<T> ();
        }

        /// <summary>
        /// Gets the request.
        /// </summary>
        /// <returns>The request.</returns>
        protected INexusRequest<T> GetRequest()
        {
            return NexusRequestFactory.CreateRequest<T> (User);
        }

        /// <summary>
        /// Requests the callback.
        /// </summary>
        /// <param name="results">Results.</param>
        private void RequestCallback(List<T> results)
        {
            foreach (T item in results) {
                GetDataManager ().SaveItem (item);
            }
            FetchInProgress = false;
            FetchCompleted = true;
			callback (results);
        }

		private Action<List<T>> callback;
    }
}

