//
// MarketBaseDataManager.cs
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
using Phoenix.Util;

namespace Phoenix.DAL
{
	/// <summary>
	/// Market base data manager.
	/// </summary>
	public class MarketBaseDataManager : DataManager<MarketBase>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Phoenix.DAL.MarketBaseDataManager"/> class.
		/// </summary>
		public MarketBaseDataManager ()
		{
		}

		/// <summary>
		/// Persists the relationships.
		/// </summary>
		/// <param name="item">Item.</param>
		protected override void PersistRelationships (MarketBase item)
		{
			if (item != null) {
				Log.WriteLine (Log.Layer.DAL, this.GetType (), "Save Market Items (" + item.Id + ": " + item.Items.Count);
				foreach (MarketItem mi in item.Items) {
					mi.BaseId = item.Id;
					DL.PhoenixDatabase.SaveItemIfNew<MarketItem> (mi);
				}
			}
		}

		/// <summary>
		/// Loads the relationships.
		/// </summary>
		/// <param name="item">Item.</param>
		protected override void LoadRelationships (MarketBase item)
		{
			if (item != null || item.Id != 0) {
				Log.WriteLine (Log.Layer.DAL, this.GetType (), "Load Relationships (" + item.Id + ")");

				item.StarSystem = DL.PhoenixDatabase.GetItem<StarSystem> (item.StarSystemId);
				if (item.StarSystem != null && item.CelestialBodyId > 0) {
					item.CelestialBody = item.StarSystem.GetCelestialBody (item.CelestialBodyId);
				}

				item.Items = DL.PhoenixDatabase.GetMarketItemsForBase (item.Id);
				foreach (MarketItem mi in item.Items) {
					mi.Item = DL.PhoenixDatabase.GetItem<Item> (mi.ItemId);
					mi.Base = item;
				}
			}
		}

		/// <summary>
		/// Deletes the relationships.
		/// </summary>
		/// <param name="item">Item.</param>
		protected override void DeleteRelationships (MarketBase item)
		{
			if (item != null || item.Id != 0) {
				Log.WriteLine (Log.Layer.DAL, this.GetType (), "Delete Relationships (" + item.Id + ")");
				DL.PhoenixDatabase.DeleteMarketItemsForBase (item.Id);
			}
		}

		/// <summary>
		/// Deletes all entities.
		/// </summary>
		protected override void DeleteAllEntities ()
		{
			Log.WriteLine (Log.Layer.DAL, this.GetType (), "Delete All Market Bases, Market Items and Trade Routes");
			DL.PhoenixDatabase.ClearTable<MarketBase> ();
			DL.PhoenixDatabase.ClearTable<MarketItem> ();
			DL.PhoenixDatabase.ClearTable<TradeRoute>();
		}

		/// <summary>
		/// Order results by
		/// </summary>
		/// <returns>The by.</returns>
		protected override string OrderBy ()
		{
			return "Name ASC";
		}
	}
}

