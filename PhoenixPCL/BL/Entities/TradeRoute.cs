//
// TradeRoute.cs
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
using SQLite.Net.Attributes; 

namespace Phoenix.BL.Entities
{
	[Table("TradeRoute")]
	public class TradeRoute : EntityBase
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		[PrimaryKey, AutoIncrement]
		public override int Id { get; set; }

		/// <summary>
		/// Gets or sets from base identifier.
		/// </summary>
		/// <value>From base identifier.</value>
		[Indexed]
		public int FromBaseId { get; set; }

		/// <summary>
		/// Gets or sets from star system identifier.
		/// </summary>
		/// <value>From star system identifier.</value>
		[Indexed]
		public int FromStarSystemId { get; set; }

		/// <summary>
		/// Gets or sets to base identifier.
		/// </summary>
		/// <value>To base identifier.</value>
		[Indexed]
		public int ToBaseId { get; set; }

		/// <summary>
		/// Gets or sets to star system identifier.
		/// </summary>
		/// <value>To star system identifier.</value>
		[Indexed]
		public int ToStarSystemId { get; set; }

		/// <summary>
		/// Gets or sets the item identifier.
		/// </summary>
		/// <value>The item identifier.</value>
		[Indexed]
		public int ItemId { get; set; }

		/// <summary>
		/// Gets or sets the item mass.
		/// </summary>
		/// <value>The item mass.</value>
		public int ItemMassUnits { get; set; }

		/// <summary>
		/// Gets or sets the navigation path identifier.
		/// </summary>
		/// <value>The navigation path identifier.</value>
		public int NavigationPathId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is life support required.
		/// </summary>
		/// <value><c>true</c> if this instance is life support required; otherwise, <c>false</c>.</value>
		[Indexed]
		public bool IsLifeSupportRequired { get; set; }

		/// <summary>
		/// Gets or sets the total quantity.
		/// </summary>
		/// <value>The total quantity.</value>
		public int TotalQuantity { get; set; }

		/// <summary>
		/// Gets or sets the profit per item.
		/// </summary>
		/// <value>The profit per item.</value>
		public float ProfitPerItem { get; set;} 

		/// <summary>
		/// Gets or sets the profit per mass unit.
		/// </summary>
		/// <value>The profit per mass unit.</value>
		public float ProfitPerMassUnit { get; set; }

		/// <summary>
		/// Gets or sets the rating.
		/// </summary>
		/// <value>The rating.</value>
		[Indexed]
		public float Rating { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Phoenix.BL.Entities.TradeRoute"/> class.
		/// </summary>
		public TradeRoute ()
		{
		}
	}
}

