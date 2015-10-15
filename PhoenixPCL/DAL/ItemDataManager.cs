//
// ItemDataManager.cs
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
    /// Item data manager.
    /// </summary>
    public class ItemDataManager : DataManager<Item>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.DAL.ItemDataManager"/> class.
        /// </summary>
        public ItemDataManager ()
        {
        }

        /// <summary>
        /// Persists the relationships.
        /// </summary>
        /// <param name="item">Item.</param>
        protected override void PersistRelationships(Item item)
        {
            if (item != null) {
				Log.WriteLine (Log.Layer.DAL, this.GetType (), "Save Item Properties (" + item.Id + ": " + item.Properties.Count);
                if (item.Properties.Count > 0) {
                    foreach (ItemProperty p in item.Properties.Values) {
                        p.ItemId = item.Id;
                        DL.PhoenixDatabase.SaveItemIfNew<ItemProperty> (p);
                    }
                }
				Log.WriteLine (Log.Layer.DAL, this.GetType (), "Save Item Raw Materials (" + item.Id + ": " + item.RawMaterials.Count);
                if (item.RawMaterials.Count > 0) {
                    foreach (RawMaterial rm in item.RawMaterials) {
                        rm.ItemId = item.Id;
                        DL.PhoenixDatabase.SaveItemIfNew<RawMaterial> (rm);
                    }
                }
            }
        }

        /// <summary>
        /// Loads the relationships.
        /// </summary>
        /// <param name="item">Item.</param>
        protected override void LoadRelationships(Item item)
        {
			Log.WriteLine (Log.Layer.DAL, this.GetType (), "Load Relationships (" + item.Id + ")");
            foreach (ItemProperty ip in DL.PhoenixDatabase.GetItemProperties (item.Id)) {
                item.AddProperty (ip.Key, ip.Value);
            }
            item.RawMaterials = DL.PhoenixDatabase.GetRawMaterials (item.Id);
			foreach (RawMaterial rm in item.RawMaterials) {
				rm.RawMaterialItem = DL.PhoenixDatabase.GetItem<Item> (rm.RawMaterialId);
			}
			if (item.BlueprintId > 0) {
				item.BlueprintItem = DL.PhoenixDatabase.GetItem<Item> (item.BlueprintId);
			}
			if (item.SubstituteItemId > 0) {
				item.SubstituteItem = DL.PhoenixDatabase.GetItem<Item> (item.SubstituteItemId);
			}
        }

        /// <summary>
        /// Deletes the relationships.
        /// </summary>
        /// <param name="item">Item.</param>
        protected override void DeleteRelationships(Item item)
        {
			Log.WriteLine (Log.Layer.DAL, this.GetType (), "Delete Relationships (" + item.Id + ")");
            DL.PhoenixDatabase.DeleteItemProperties (item.Id);
            DL.PhoenixDatabase.DeleteRawMaterials (item.Id);
        }

		/// <summary>
		/// Deletes all entities.
		/// </summary>
		protected override void DeleteAllEntities()
		{
			Log.WriteLine (Log.Layer.DAL, this.GetType (), "Delete All Items, Item Properties and Raw Materials");
			DL.PhoenixDatabase.ClearTable<Item>();
			DL.PhoenixDatabase.ClearTable<ItemProperty>();
			DL.PhoenixDatabase.ClearTable<RawMaterial>();
		}
    }
}

