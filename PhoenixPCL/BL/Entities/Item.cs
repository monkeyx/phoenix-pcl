//
// Item.cs
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
using SQLite;

namespace Phoenix.BL.Entities
{
    /// <summary>
    /// Item.
    /// </summary>
    [Table("Item")]
    public class Item : EntityBase
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Indexed]
        public string Name {get; set; }

        /// <summary>
        /// Gets or sets the type of the item.
        /// </summary>
        /// <value>The type of the item.</value>
        [Indexed]
        public string ItemType { get; set; }

        /// <summary>
        /// Gets or sets the mass units.
        /// </summary>
        /// <value>The mass units.</value>
        public int MassUnits { get; set; }

        /// <summary>
        /// Gets or sets the blueprint.
        /// </summary>
        /// <value>The blueprint.</value>
        public int Blueprint { get; set; }

        /// <summary>
        /// Gets or sets the production.
        /// </summary>
        /// <value>The production.</value>
        public int Production { get; set; }

        /// <summary>
        /// Gets or sets the substitute item.
        /// </summary>
        /// <value>The substitute item.</value>
        public int SubstituteItem { get; set; }

        /// <summary>
        /// Gets or sets the substitute ratio.
        /// </summary>
        /// <value>The substitute ratio.</value>
        public float SubstituteRatio { get; set; }

        /// <summary>
        /// Gets or sets the type of the infrastructure environment.
        /// </summary>
        /// <value>The type of the infrastructure environment.</value>
        [Ignore]
        public List<RawMaterial> RawMaterials { get; set; }

        /// <summary>
        /// Gets or sets the race.
        /// </summary>
        /// <value>The race.</value>
        public string Race { get; set; }

        /// <summary>
        /// Gets or sets the type of the sub.
        /// </summary>
        /// <value>The type of the sub.</value>
        public string SubType { get; set; }

        /// <summary>
        /// Gets or sets the tech manual.
        /// </summary>
        /// <value>The tech manual.</value>
        public string TechManual { get; set; }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        /// <value>The properties.</value>
        [Ignore]
        public Dictionary<string, ItemProperty> Properties { get; set; }

        /// <summary>
        /// Adds the property.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public void AddProperty(string key, string value) 
        {
            Properties.Add (key, new ItemProperty () {
                Key = key,
                Value = value,
                ItemId = Id
            });
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <returns>The property.</returns>
        /// <param name="key">Key.</param>
        public string GetProperty(string key)
        {
            ItemProperty p;
            if (Properties.TryGetValue (key, out p)) {
                return p.Value;
            } else {
                return null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.Item"/> class.
        /// </summary>
        public Item ()
        {
            RawMaterials = new List<RawMaterial> ();
            Properties = new Dictionary<string, ItemProperty> ();
        }
    }

    /// <summary>
    /// Item property.
    /// </summary>
    [Table("ItemProperty")]
    public class ItemProperty : EntityBase 
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [PrimaryKey, AutoIncrement]
        public override int Id { get; set; }

        /// <summary>
        /// Gets or sets the item identifier.
        /// </summary>
        /// <value>The item identifier.</value>
        public int ItemId { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.ItemProperty"/> class.
        /// </summary>
        public ItemProperty ()
        {
        }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.ItemProperty"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.ItemProperty"/>.</returns>
		public override string ToString ()
		{
			return string.Format ("[ItemProperty: Key={0}, Value={1}]", Key, Value);
		}
    }

    /// <summary>
    /// Raw material.
    /// </summary>
    [Table("RawMaterial")]
    public class RawMaterial : EntityBase 
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [PrimaryKey, AutoIncrement]
        public override int Id { get; set; }

        /// <summary>
        /// Gets or sets the item identifier.
        /// </summary>
        /// <value>The item identifier.</value>
        public int ItemId { get; set; }

        /// <summary>
        /// Gets or sets the raw material identifier.
        /// </summary>
        /// <value>The raw material identifier.</value>
        public int RawMaterialId { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>The quantity.</value>
        public int Quantity { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.RawMaterial"/> class.
        /// </summary>
        public RawMaterial()
        {
        }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.RawMaterial"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.RawMaterial"/>.</returns>
		public override string ToString ()
		{
			return string.Format ("[RawMaterial:RawMaterialId={0}, Quantity={1}]", RawMaterialId, Quantity);
		}
    }
}

