//
// PositionDataManager.cs
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
	/// Position data manager.
	/// </summary>
	public class PositionDataManager : DataManager<Position>
	{

		/// <summary>
		/// Deletes the note.
		/// </summary>
		/// <returns>The note.</returns>
		/// <param name="positionId">Position identifier.</param>
		public Task DeleteNote(int positionId){
			return Task.Factory.StartNew (() => {
				DL.PhoenixDatabase.DeleteItemById<PositionNote>(positionId);
				Log.WriteLine (Log.Layer.DAL, this.GetType (), "Deleted Note for Position " + positionId);
			});
		}

		/// <summary>
		/// Gets the note.
		/// </summary>
		/// <returns>The note.</returns>
		/// <param name="positionId">Position identifier.</param>
		public Task<PositionNote> GetNote(int positionId)
		{
			return Task<PositionNote>.Factory.StartNew (() => {
				Log.WriteLine (Log.Layer.DAL, this.GetType (), "Get Note for Position " + positionId);
				return DL.PhoenixDatabase.GetItem<PositionNote> (positionId);
			});
		}

		/// <summary>
		/// Saves the note.
		/// </summary>
		/// <returns>The note.</returns>
		/// <param name="positionId">Position identifier.</param>
		/// <param name="note">Note.</param>
		public Task<PositionNote> SaveNote(int positionId, string note){
			return Task<PositionNote>.Factory.StartNew (() => {
				PositionNote pn = DL.PhoenixDatabase.GetItem<PositionNote> (positionId);
				if(pn == null){
					pn = new PositionNote{
						Id = positionId
					};
				}
				pn.Note = note;
				Log.WriteLine (Log.Layer.DAL, this.GetType (), "Save Note for Position " + positionId + "\n" + note);
				DL.PhoenixDatabase.SaveItem<PositionNote>(pn);
				return pn;
			});
		}

		/// <summary>
		/// Gets the type of the positions of.
		/// </summary>
		/// <returns>The positions of type.</returns>
		/// <param name="positionType">Position type.</param>
		public Task<List<Position>> GetPositionsOfType(Position.PositionFlag positionType)
		{
			return Task<List<Position>>.Factory.StartNew (() => {
				return DL.PhoenixDatabase.GetPositionsOfType(positionType);
			});
		}

		/// <summary>
		/// Gets the positions with notes.
		/// </summary>
		/// <returns>The positions with notes.</returns>
		public Task<List<Position>> GetPositionsWithNotes()
		{
			return Task<List<Position>>.Factory.StartNew (() => {
				return DL.PhoenixDatabase.GetPositionsWithNotes();
			});
		}

		/// <summary>
		/// Gets the positions with turns.
		/// </summary>
		/// <returns>The positions with turns.</returns>
		/// <param name="daysAgo">Days ago.</param>
		public Task<List<Position>> GetPositionsWithTurns(int daysAgo)
		{
			return Task<List<Position>>.Factory.StartNew (() => {
				return DL.PhoenixDatabase.GetPositionsWithTurns (daysAgo);
			});
		}

		/// <summary>
		/// Gets the positions in star system.
		/// </summary>
		/// <returns>The positions in star system.</returns>
		/// <param name="starSystem">Star system.</param>
		public Task<List<Position>> GetPositionsInStarSystem(StarSystem starSystem)
		{
			return Task<List<Position>>.Factory.StartNew (() => {
				if (starSystem == null) {
					return new List<Position> ();
				}
				return DL.PhoenixDatabase.GetPositionsInStarSystem (starSystem.Id);
			});
		}

		/// <summary>
		/// Gets the positions with orders.
		/// </summary>
		/// <returns>The positions with orders.</returns>
		public Task<List<Position>> GetPositionsWithOrders()
		{
			return Task<List<Position>>.Factory.StartNew (() => {
				return DL.PhoenixDatabase.GetPositionsWithOrders();
			});
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Phoenix.DAL.PositionDataManager"/> class.
		/// </summary>
		public PositionDataManager ()
		{
		}

		/// <summary>
		/// Loads the relationships.
		/// </summary>
		/// <param name="item">Item.</param>
		protected override void LoadRelationships (Position item)
		{
			if (item.StarSystemId > 0) {
				item.StarSystem = DL.PhoenixDatabase.GetItem<StarSystem> (item.StarSystemId);
			}
			if (item.PositionType == (int)Position.PositionFlag.Starbase) {
				item.Market = DL.PhoenixDatabase.GetItem<MarketBase> (item.Id);
				if (item.Market != null) {
					item.Market.Items = DL.PhoenixDatabase.GetMarketItemsForBase (item.Id);
					foreach (MarketItem mi in item.Market.Items) {
						mi.Item = DL.PhoenixDatabase.GetItem<Item> (mi.ItemId);
						mi.Base = item.Market;
					}
				}
			}
		}

		/// <summary>
		/// Deletes all entities.
		/// </summary>
		protected override void DeleteAllEntities ()
		{
			DL.PhoenixDatabase.ClearTable<Position> ();
			DL.PhoenixDatabase.ClearTable<PositionTurn> ();
		}

		/// <summary>
		/// Deletes the relationships.
		/// </summary>
		/// <param name="item">Item.</param>
		protected override void DeleteRelationships (Position item)
		{
			if (item == null || item.Id == 0)
				return;
			
			Log.WriteLine (Log.Layer.DAL, this.GetType (), "Delete Relationships (" + item.Id + ")");
			DL.PhoenixDatabase.DeleteItemById<PositionTurn> (item.Id);
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

