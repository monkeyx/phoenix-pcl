//
// OrderTypeDataManager.cs
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
	/// Order type data manager.
	/// </summary>
	public class OrderTypeDataManager : DataManager<OrderType>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Phoenix.DAL.OrderTypeDataManager"/> class.
		/// </summary>
		public OrderTypeDataManager ()
		{
		}

		/// <summary>
		/// Persists the relationships.
		/// </summary>
		/// <param name="item">Item.</param>
		protected override void PersistRelationships (OrderType item)
		{
			Log.WriteLine (Log.Layer.DAL, this.GetType (), "Save Order Type Parameters (" + item.Id + ": " + item.Parameters.Count);
			if (item.Parameters.Count > 0) {
				foreach (OrderParameterType param in item.Parameters) {
					param.OrderId = item.Id;
					DL.PhoenixDatabase.SaveItemIfNew<OrderParameterType> (param);
				}
			}
		}

		/// <summary>
		/// Loads the relationships.
		/// </summary>
		/// <param name="item">Item.</param>
		protected override void LoadRelationships (OrderType item)
		{
			Log.WriteLine (Log.Layer.DAL, this.GetType (), "Load Relationships (" + item.Id + ")");
			item.Parameters = DL.PhoenixDatabase.GetOrderTypeParameters (item.Id);
		}

		/// <summary>
		/// Deletes the relationships.
		/// </summary>
		/// <param name="item">Item.</param>
		protected override void DeleteRelationships (OrderType item)
		{
			Log.WriteLine (Log.Layer.DAL, this.GetType (), "Delete Relationships (" + item.Id + ")");
			DL.PhoenixDatabase.DeleteOrderTypeParameters (item.Id);
		}

		/// <summary>
		/// Deletes all entities.
		/// </summary>
		protected override void DeleteAllEntities ()
		{
			Log.WriteLine (Log.Layer.DAL, this.GetType (), "Delete All Order Types and Order Type Parameters");
			DL.PhoenixDatabase.ClearTable<OrderType> ();
			DL.PhoenixDatabase.ClearTable<OrderParameterType> ();
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

