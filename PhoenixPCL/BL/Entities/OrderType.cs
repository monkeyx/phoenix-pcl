//
// OrderType.cs
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
    [Table("OrderType")]
    public class OrderType : EntityBase
    {
		/// <summary>
		/// Order flags.
		/// </summary>
		public enum OrderFlag {
			Any = 0x0,           
			ThrustMove = 0x1,           
			Movement = 0x2,           
			Transaction = 0x4,           
			StandingOrder = 0x8,           
			Basic = 0x10,           
			Scan = 0x20,           
			Other = 0x40,           
			Issue = 0x80,           
			Create = 0x100,           
			SquadronOrder = 0x200,           
			SquadronStandingOrder = 0x400,           
			TurnType = 0x800,           
			Boarding = 0x1000,           
			PlanetaryInteraction = 0x2000,           
			Macro = 0x4000,           
			Copy = 0x8000,           
			Full = 0xfffff  
		}

		/// <summary>
		/// Position flags.
		/// </summary>
		public enum PositionFlag {
			None = 0x00,           
			GroundParty = 0x01,           
			Ship = 0x02,           
			Starbase = 0x04,           
			Political = 0x08,           
			Platform = 0x10,           
			Agent = 0x20,           
			Debris = 0x40   
		}
		  

		/// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Indexed]
        public string Name { get; set; }

		/// <summary>
		/// Gets the list text.
		/// </summary>
		/// <value>The list text.</value>
		[Ignore]
		public override string ListText { get { return Name; } }

		/// <summary>
		/// Gets the list detail.
		/// </summary>
		/// <value>The list detail.</value>
		[Ignore]
		public override string ListDetail { get { return PositionType; } }

        /// <summary>
        /// Gets or sets the type flag.
        /// </summary>
        /// <value>The type flag.</value>
        public int TypeFlag { get; set; }

		/// <summary>
		/// Gets the type description.
		/// </summary>
		/// <value>The type description.</value>
		public string TypeDescription
		{
			get {
				string description = "";
				foreach(var mask in Enum.GetValues(typeof(OrderFlag))){
					OrderFlag flag = (OrderFlag)mask;
					if ((TypeFlag & (int)flag) != 0) {
						description += flag.ToString () + " ";
					}
				}
				return description.Trim();
			}

		}

        /// <summary>
        /// Gets or sets the position flag.
        /// </summary>
        /// <value>The position flag.</value>
        [Indexed]
        public int Position { get; set; }

		/// <summary>
		/// Gets the type of the position.
		/// </summary>
		/// <value>The type of the position.</value>
		public string PositionType
		{
			get {
				string positionType = "";
				foreach(var mask in Enum.GetValues(typeof(PositionFlag))){
					PositionFlag flag = (PositionFlag)mask;
					if ((Position & (int)flag) != 0) {
						positionType += flag.ToString () + " ";
					}
				}
				return positionType.Trim();
			}

		}

        /// <summary>
        /// Gets or sets the TU cost.
        /// </summary>
        /// <value>The TU cost.</value>
        public int TUCost { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        [Ignore]
        public List<OrderParameterType> Parameters { get; set; }

		/// <summary>
		/// Gets the group that the entity belongs to
		/// </summary>
		/// <value>The group.</value>
		[Ignore]
		public override string Group { 
			get { 
				return Name.Substring(0,1);
			}
		}

		/// <summary>
		/// Determines whether this instance is for position of the specified flag.
		/// </summary>
		/// <returns><c>true</c> if this instance is for the position of the specified flag; otherwise, <c>false</c>.</returns>
		/// <param name="flag">Flag.</param>
		public bool IsForPosition(PositionFlag flag)
		{
			return ((Position & (int)flag) != 0);
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.OrderType"/> class.
        /// </summary>
        public OrderType ()
        {
            Parameters = new List<OrderParameterType>();
        }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.OrderType"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.OrderType"/>.</returns>
		public override string ToString ()
		{
			return Name;
		}
    }

    [Table("OrderParameterType")]
	public class OrderParameterType : EntityBase
    {
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		[PrimaryKey, AutoIncrement]
		public override int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the info.
        /// </summary>
        /// <value>The type of the info.</value>
        public int InfoType { get; set; }

        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>The type of the data.</value>
        public int DataType { get; set; }

        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        /// <value>The order identifier.</value>
        [Indexed]
        public int OrderId { get; set; }

		/// <summary>
		/// Gets the detail.
		/// </summary>
		/// <value>The detail.</value>
		[Ignore]
		public string Detail {
			get {
				return "Info Type: " + InfoType + " Data Type: " + DataType;
			}
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.OrderParameterType"/> class.
        /// </summary>
        public OrderParameterType()
        {
        }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.OrderParameterType"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.OrderParameterType"/>.</returns>
		public override string ToString ()
		{
			return string.Format ("[OrderParameterType: Name={0}, InfoType={1}, DataType={2}]", Name, InfoType, DataType);
		}
    }
}

