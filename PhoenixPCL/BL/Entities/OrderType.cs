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
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Indexed]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type flag.
        /// </summary>
        /// <value>The type flag.</value>
        public int TypeFlag { get; set; }

        /// <summary>
        /// Gets or sets the position flag.
        /// </summary>
        /// <value>The position flag.</value>
        [Indexed]
        public int PositionFlag { get; set; }

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
        /// Initializes a new instance of the <see cref="Phoenix.OrderType"/> class.
        /// </summary>
        public OrderType ()
        {
            Parameters = new List<OrderParameterType>();
        }
    }

    [Table("OrderParameterType")]
    public class OrderParameterType
    {
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
        /// Initializes a new instance of the <see cref="Phoenix.OrderParameterType"/> class.
        /// </summary>
        public OrderParameterType()
        {
        }

		public override string ToString ()
		{
			return string.Format ("[OrderParameterType: Name={0}, InfoType={1}, DataType={2}]", Name, InfoType, DataType);
		}
    }
}

