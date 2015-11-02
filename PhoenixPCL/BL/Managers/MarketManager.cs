//
// MarketManager.cs
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

using Phoenix.BL.Entities;
using Phoenix.DAL;
using Phoenix.Util;

namespace Phoenix.BL.Managers
{
	/// <summary>
	/// Market manager.
	/// </summary>
	public class MarketManager : NexusManager<MarketBase>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Phoenix.BL.Managers.MarketManager"/> class.
		/// </summary>
		/// <param name="user">User.</param>
		public MarketManager (User user) : base(user)
		{
		}

		/// <summary>
		/// Requests the callback.
		/// </summary>
		/// <param name="results">Results.</param>
		protected override async void RequestCallback(IEnumerable<MarketBase> results, Exception e)
		{
			if (results == null) {
				callback (new List<MarketBase> (), e == null ? new Exception ("No results received " + GetType()) : e);
				return;
			}

			Dictionary<int,List<MarketBase>> sellers = new Dictionary<int,List<MarketBase>> ();
			Dictionary<int,List<MarketBase>> buyers = new Dictionary<int,List<MarketBase>> ();

			foreach (MarketBase mb in results) {
				try {
					await GetDataManager ().SaveItem (mb);
					foreach(MarketItem mi in mb.Selling){
						if(mi.SellQuantity > 0){
							List<MarketBase> list;
							if(sellers.ContainsKey(mi.ItemId)){
								list = sellers[mi.ItemId];
							}else{
								list = new List<MarketBase>();
								sellers.Add(mi.ItemId,list);
							}
							list.Add(mb);
						}
						if(mi.BuyQuantity > 0){
							List<MarketBase> list;
							if(buyers.ContainsKey(mi.ItemId)){
								list = buyers[mi.ItemId];
							}else{
								list = new List<MarketBase>();
								buyers.Add(mi.ItemId,list);
							}
							list.Add(mb);
						}
					}
				}
				catch(Exception ex){
					Log.WriteLine (Log.Layer.BL, this.GetType (), ex);
				}
			}
			FetchInProgress = false;
			FetchCompleted = true;

			TradeRouteDataManager tradeManager = (TradeRouteDataManager)DataManagerFactory.GetManager<TradeRoute> ();
			NavigationPathDataManager pathManager = (NavigationPathDataManager)DataManagerFactory.GetManager<NavigationPath> ();
			ItemDataManager itemManager = (ItemDataManager)DataManagerFactory.GetManager<Item> ();

			await tradeManager.Clear ();

			foreach (int itemId in sellers.Keys) {
				Item item = await itemManager.GetItem (itemId);
				if (item != null) {
					foreach (MarketBase seller in sellers[itemId]) {
						MarketItem sellingItem = seller.GetSellingItem (itemId);
						if (sellingItem != null && buyers.ContainsKey (itemId)) {
							foreach (MarketBase buyer in buyers[itemId]) {
								MarketItem buyingItem = buyer.GetBuyingItem (itemId);
								if (buyingItem != null && buyingItem.BuyPrice > sellingItem.SellPrice) {
									NavigationPath path = await pathManager.GetPathBetweenSystem (buyer.StarSystemId, seller.StarSystemId);
									if (path != null) {
										int totalQuantity = (sellingItem.SellQuantity > buyingItem.BuyQuantity) ? buyingItem.BuyQuantity : sellingItem.SellQuantity;
										float profitPerItem = buyingItem.BuyPrice - sellingItem.SellPrice;
										float profitPerMass;
										if (item.MassUnits > 0) {
											profitPerMass = profitPerItem / item.MassUnits;
										} else {
											profitPerMass = profitPerItem;
										}
										float rating = profitPerMass / (float)path.TotalJumps;
										TradeRoute route = new TradeRoute {
											FromBaseId = seller.Id,
											FromStarSystemId = seller.StarSystemId,
											ToBaseId = buyer.Id,
											ToStarSystemId = buyer.StarSystemId,
											ItemId = itemId,
											ItemMassUnits = item.MassUnits,
											NavigationPathId = path.Id,
											IsLifeSupportRequired = item.IsLifeSupportRequired,
											TotalQuantity = totalQuantity,
											ProfitPerItem = profitPerItem,
											ProfitPerMassUnit = profitPerMass,
											Rating = rating
										};
										await tradeManager.SaveItem (route);
									}
								}
							}
						}
					}
				}
			}

			Log.WriteLine (Log.Layer.BL, GetType (), "Trade Routes Generated: " + tradeManager.Count);


			callback (results, e);
		}
	}
}

