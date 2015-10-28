//
// MarketBase.cs
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

using SQLite.Net.Attributes; 

namespace Phoenix.BL.Entities
{
	/// <summary>
	/// Market base.
	/// </summary>
	[Table("MarketBase")]
	public class MarketBase : EntityBase
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		[PrimaryKey]
		public override int Id { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		[Indexed]
		public string Name { get; set; }

		/// <summary>
		/// Gets the name and identifier.
		/// </summary>
		/// <value>The name and identifier.</value>
		[Ignore]
		public string NameAndId { 
			get { 
				return Name + " (" + Id + ")";
			}
		}

		/// <summary>
		/// Gets the list text.
		/// </summary>
		/// <value>The list text.</value>
		[Ignore]
		public override string ListText { get { return NameAndId; } }

		/// <summary>
		/// Gets the list detail.
		/// </summary>
		/// <value>The list detail.</value>
		[Ignore]
		public override string ListDetail { get { return LocationText; } }

		/// <summary>
		/// Gets the group that the entity belongs to
		/// </summary>
		/// <value>The group.</value>
		[Ignore]
		public override string Group { 
			get { 
				return StarSystemName == null ? "None" : StarSystemName;
			}
		}

		/// <summary>
		/// Gets or sets the affiliation code.
		/// </summary>
		/// <value>The affiliation code.</value>
		[Indexed]
		public string AffiliationCode { get; set; }

		/// <summary>
		/// Gets or sets the star system identifier.
		/// </summary>
		/// <value>The star system identifier.</value>
		[Indexed]
		public int StarSystemId { get; set; }

		/// <summary>
		/// Gets or sets the name of the star system.
		/// </summary>
		/// <value>The name of the star system.</value>
		public string StarSystemName { get; set; }

		/// <summary>
		/// Gets or sets the star system.
		/// </summary>
		/// <value>The star system.</value>
		[Ignore]
		public StarSystem StarSystem { get; set; }

		/// <summary>
		/// Gets or sets the celestial body identifier.
		/// </summary>
		/// <value>The celestial body identifier.</value>
		public int CelestialBodyId { get; set; }

		/// <summary>
		/// Gets or sets the name of the celestial body.
		/// </summary>
		/// <value>The name of the celestial body.</value>
		public string CelestialBodyName { get; set; }

		/// <summary>
		/// Gets or sets the celestial body.
		/// </summary>
		/// <value>The celestial body.</value>
		[Ignore]
		public CelestialBody CelestialBody { get; set; }

		/// <summary>
		/// Gets or sets the hiport.
		/// </summary>
		/// <value>The hiport.</value>
		public int Hiport { get; set; }

		/// <summary>
		/// Gets or sets the patch price.
		/// </summary>
		/// <value>The patch price.</value>
		public int PatchPrice { get; set; }

		/// <summary>
		/// Gets or sets the dock capacity.
		/// </summary>
		/// <value>The dock capacity.</value>
		public int DockCapacity { get; set; }

		/// <summary>
		/// Gets or sets the maintenance complexes.
		/// </summary>
		/// <value>The maintenance complexes.</value>
		public int MaintenanceComplexes { get; set; }

		/// <summary>
		/// Gets or sets the items.
		/// </summary>
		/// <value>The items.</value>
		[Ignore]
		public List<MarketItem> Items { get ;set; } = new List<MarketItem>();

		/// <summary>
		/// Gets the selling.
		/// </summary>
		/// <value>The selling.</value>
		[Ignore]
		public IEnumerable<MarketItem> Selling {
			get {
				return from element in Items
				       where element.SellQuantity > 0
				       select element;
			}
		}

		/// <summary>
		/// Gets the buying.
		/// </summary>
		/// <value>The buying.</value>
		[Ignore]
		public IEnumerable<MarketItem> Buying {
			get {
				return from element in Items
						where element.BuyQuantity > 0
					select element;
			}
		}

		/// <summary>
		/// Gets the location text.
		/// </summary>
		/// <value>The location text.</value>
		[Ignore]
		public string LocationText { 
			get {
				return CelestialBodyName + " (" + CelestialBodyId + ") in " + StarSystemName + " (" + StarSystemId + ")";
			}
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.MarketBase"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.MarketBase"/>.</returns>
		public override string ToString ()
		{
			return string.Format ("{0} {1} ({2})", AffiliationCode, Name, Id);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Phoenix.BL.Entities.MarketBase"/> class.
		/// </summary>
		public MarketBase ()
		{
		}
	}

	/// <summary>
	/// Market item.
	/// </summary>
	[Table("MarketItem")]
	public class MarketItem : EntityBase
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		[PrimaryKey, AutoIncrement]
		public override int Id { get; set; }

		/// <summary>
		/// Gets or sets the base identifier.
		/// </summary>
		/// <value>The base identifier.</value>
		[Indexed]
		public int BaseId { get; set; }

		/// <summary>
		/// Gets or sets the base.
		/// </summary>
		/// <value>The base.</value>
		[Ignore]
		public MarketBase Base { get; set; }

		/// <summary>
		/// Gets or sets the item identifier.
		/// </summary>
		/// <value>The item identifier.</value>
		[Indexed]
		public int ItemId { get; set; }

		/// <summary>
		/// Gets or sets the name of the item.
		/// </summary>
		/// <value>The name of the item.</value>
		public string ItemName { get; set; }

		/// <summary>
		/// Gets or sets the market.
		/// </summary>
		/// <value>The market.</value>
		[Ignore]
		public MarketBase Market { get; set; }

		/// <summary>
		/// Gets the name and identifier.
		/// </summary>
		/// <value>The name and identifier.</value>
		[Ignore]
		public string NameAndId { 
			get { 
				return ItemName + " (" + ItemId + ")";
			}
		}

		/// <summary>
		/// Gets the list text.
		/// </summary>
		/// <value>The list text.</value>
		[Ignore]
		public override string ListText { get { return NameAndId; } }

		/// <summary>
		/// Gets the list detail.
		/// </summary>
		/// <value>The list detail.</value>
		[Ignore]
		public override string ListDetail { get { return Details; } }

		/// <summary>
		/// Gets or sets the item.
		/// </summary>
		/// <value>The item.</value>
		[Ignore]
		public Item Item { get; set; }

		/// <summary>
		/// Gets or sets the sell quantity.
		/// </summary>
		/// <value>The sell quantity.</value>
		public int SellQuantity { get; set; }

		/// <summary>
		/// Gets or sets the sell price.
		/// </summary>
		/// <value>The sell price.</value>
		public float SellPrice { get; set; }

		/// <summary>
		/// Gets or sets the buy quantity.
		/// </summary>
		/// <value>The buy quantity.</value>
		public int BuyQuantity { get; set; }

		/// <summary>
		/// Gets or sets the buy price.
		/// </summary>
		/// <value>The buy price.</value>
		public float BuyPrice { get; set; }

		/// <summary>
		/// Gets the details.
		/// </summary>
		/// <value>The details.</value>
		[Ignore]
		public string Details {
			get {
				List<string> details = new List<string> ();
				if (BuyQuantity > 0) {
					details.Add("Buy: " + BuyQuantity + " @ $" + BuyPrice); 
				}
				if (SellQuantity > 0) {
					details.Add("Sell: " + SellQuantity + " @ $" + SellPrice); 
				}
				return string.Join (", ", details);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Phoenix.BL.Entities.MarketItem"/> class.
		/// </summary>
		public MarketItem ()
		{
		}
	}
}

