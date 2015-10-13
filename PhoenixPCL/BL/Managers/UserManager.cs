//
// UserManager.cs
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

using Phoenix.BL.Entities;
using Phoenix.DAL;

namespace Phoenix.BL.Managers
{
    /// <summary>
    /// User manager.
    /// </summary>
    public class UserManager : IEntityManager<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.BL.Managers.UserManager"/> class.
        /// </summary>
        public UserManager ()
        {
        }

        /// <summary>
        /// Save the specified user details
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="code">Code.</param>
        /// <param name="callback">Callback.</param>
        public async void Save(int id, string code, Action<User> callback)
        {
            User user = new User () {
                Id = id,
                Code = code
            };
			await GetDataManager ().Clear ();
            user = await GetDataManager ().SaveItem (user);
            callback (user);
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
        public async void Get(int id, Action<User> callback)
        {
            User user = await GetDataManager ().GetItem (id);
            callback (user);
        }

        /// <summary>
        /// Gets the first entity by its identifier and passes it to the callback
        /// </summary>
        /// <param name="callback">Callback.</param>
        public async void First(Action<User> callback)
        {
            User user = await GetDataManager ().GetFirstItem ();
            callback (user);
        }

        private DataManager<User> GetDataManager()
        {
            return DataManagerFactory.GetManager<User> ();
        }
    }
}

