//
// DataManager.cs
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
using System.Threading.Tasks;

using Phoenix.BL.Entities;

namespace Phoenix.DAL
{
    /// <summary>
    /// Data manager factory.
    /// </summary>
    public static class DataManagerFactory
    {
        /// <summary>
        /// Gets the manager.
        /// </summary>
        /// <returns>The manager.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static DataManager<T> GetManager<T>() where T : EntityBase, new()
        {
            string typeName = typeof(T).Name;
            if (_managers.Contains(typeName)) {
                return _managers[typeName] as DataManager<T>;
            }
            return null;
        }

        /// <summary>
        /// Registers the manager.
        /// </summary>
        /// <param name="manager">Manager.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        static void RegisterManager<T>(DataManager<T> manager) where T : EntityBase, new()
        {
            string typeName = typeof(T).Name;
            if (_managers.Contains(typeName)) {
                _managers[typeName] = manager;
            } else {
                _managers.Add(typeName, manager);
            }
        }

        /// <summary>
        /// Initializes the <see cref="Phoenix.DAL.DataManagerFactory"/> class.
        /// </summary>
        static DataManagerFactory()
        {
            // register managers
            RegisterManager<GameStatus>(new DataManager<GameStatus>());
            RegisterManager<InfoData>(new DataManager<InfoData>());
            RegisterManager<Item>(new ItemDataManager());
            RegisterManager<OrderType>(new DataManager<OrderType>());
            RegisterManager<Position>(new DataManager<Position>());
            RegisterManager<StarSystem>(new StarSystemDataManager());
            RegisterManager<User>(new DataManager<User>());
        }

        /// <summary>
        /// The managers.
        /// </summary>
        private static readonly System.Collections.IDictionary _managers = new Dictionary<string, object>();
    }

    /// <summary>
    /// Data manager.
    /// </summary>
    public class DataManager<T> where T :   EntityBase, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.DAL.DataManager`1"/> class.
        /// </summary>
        public DataManager ()
        {
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return DL.PhoenixDatabase.CountTable<T>(); }
        }

		/// <summary>
		/// Clear all entities
		/// </summary>
		public Task Clear()
		{
			return Task.Factory.StartNew (() => {
				DL.PhoenixDatabase.ClearTable<T>();
			});
		}

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <returns>The items.</returns>
        /// <param name="progressCallback">Progress callback.</param>
        /// <param name="sort">If set to <c>true</c> sort.</param>
        public Task<List<T>> GetItems(IProgress<int> progressCallback = null, bool sort = false)
        {
            return Task<List<T>>.Factory.StartNew (() => {
                List<T> models = DL.PhoenixDatabase.GetItems<T>(OrderBy());
                if(models.Count > 0){
                    int i = 0;
                    foreach(T item in models){
                        LoadRelationships(item);
						if(progressCallback != null)
                        	progressCallback.Report(i);
                        i++;
                    }
                }
                return models;
            });
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <returns>The item.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="progressCallback">Progress callback.</param>
        public Task<T> GetItem(int id, IProgress<int> progressCallback = null)
        {
            return Task<T>.Factory.StartNew (() => {
                T item = DL.PhoenixDatabase.GetItem<T>(id);
                if(item != null){
                    LoadRelationships(item);
                }
				if(progressCallback != null)
                	progressCallback.Report(1);
                return item;
            });
        }

        /// <summary>
        /// Gets the first item.
        /// </summary>
        /// <returns>The first item.</returns>
        /// <param name="progressCallback">Progress callback.</param>
        public Task<T> GetFirstItem(IProgress<int> progressCallback = null)
        {
            return Task<T>.Factory.StartNew (() => {
                T item = DL.PhoenixDatabase.GetFirstItem<T>(OrderBy());
                if(item != null){
                    LoadRelationships(item);
                }
				if(progressCallback != null)
                	progressCallback.Report(1);
                return item;
            });
        }

        /// <summary>
        /// Saves the item.
        /// </summary>
        /// <returns>The item.</returns>
        /// <param name="item">Item.</param>
        /// <param name="progressCallback">Progress callback.</param>
        public Task<T> SaveItem(T item, IProgress<int> progressCallback = null)
        {
            return Task<T>.Factory.StartNew (() => {
				if(item != null){
					DeleteRelationships(item);
					DL.PhoenixDatabase.SaveItem<T>(item);
					PersistRelationships(item);
				}
				if(progressCallback != null)
                	progressCallback.Report(1);
                return item;
            });
        }

        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <returns>The item.</returns>
        /// <param name="item">Item.</param>
        /// <param name="progressCallback">Progress callback.</param>
        public Task DeleteItem(T item, IProgress<int> progressCallback = null)
        {
            return Task.Factory.StartNew(() => {
                if(item == null){
                    return;
                }
                DL.PhoenixDatabase.DeleteItem(item);
                DeleteRelationships(item);
				if(progressCallback != null)
                	progressCallback.Report(1);
            });
        }

        /// <summary>
        /// Persists the relationships.
        /// </summary>
        /// <param name="item">Item.</param>
        protected virtual void PersistRelationships(T item)
        {
        }

        /// <summary>
        /// Loads the relationships.
        /// </summary>
        /// <param name="item">Item.</param>
        protected virtual void LoadRelationships(T item)
        {
        }

        /// <summary>
        /// Deletes the relationships.
        /// </summary>
        /// <param name="item">Item.</param>
        protected virtual void DeleteRelationships(T item)
        {
        }

        /// <summary>
        /// Order results by
        /// </summary>
        protected virtual string OrderBy()
        {
            return "Id ASC";
        }
    }

}

