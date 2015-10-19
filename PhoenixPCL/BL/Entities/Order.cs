//
// Order.cs
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
	/// Order.
	/// </summary>
	[Table("Order")]
	public class Order : EntityBase
	{
		/// <summary>
		/// Clears the pending orders.
		/// </summary>
		/// <returns>The pending orders.</returns>
		/// <param name="positionId">Position identifier.</param>
		public static Order ClearPendingOrders(int positionId)
		{
			return new Order {
				PositionId = positionId,
				OrderTypeId = (int) OrderType.SpecialOrders.ClearPendingOrders,
				OrderType = new OrderType{
					Id = (int) OrderType.SpecialOrders.ClearPendingOrders,
					Name = "Clear Pending Orders"
				}
			};
		}

		/// <summary>
		/// Requests the update.
		/// </summary>
		/// <returns>The update.</returns>
		/// <param name="positionId">Position identifier.</param>
		public static Order RequestUpdate(int positionId)
		{
			return new Order {
				PositionId = positionId,
				OrderTypeId = (int) OrderType.SpecialOrders.RequestUpdate,
				OrderType = new OrderType{
					Id = (int) OrderType.SpecialOrders.RequestUpdate,
					Name = "Request Update"
				}
			};
		}

		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		[PrimaryKey, AutoIncrement]
		public override int Id { get; set; }

		/// <summary>
		/// Gets or sets the order type identifier.
		/// </summary>
		/// <value>The order type identifier.</value>
		public int OrderTypeId { get; set; }

		/// <summary>
		/// Gets or sets the type of the order.
		/// </summary>
		/// <value>The type of the order.</value>
		[Ignore]
		public OrderType OrderType { get; set; }

		/// <summary>
		/// Gets the list text.
		/// </summary>
		/// <value>The list text.</value>
		[Ignore]
		public override string ListText { get { return OrderType == null ? OrderTypeId.ToString() : OrderType.ListText; } }

		/// <summary>
		/// Gets the list detail.
		/// </summary>
		/// <value>The list detail.</value>
		[Ignore]
		public override string ListDetail { get { return string.Join(",", Parameters); } }

		/// <summary>
		/// Gets or sets the position identifier.
		/// </summary>
		/// <value>The position identifier.</value>
		[Indexed]
		public int PositionId { get; set; }

		/// <summary>
		/// Gets or sets the parameters.
		/// </summary>
		/// <value>The parameters.</value>
		[Ignore]
		public List<OrderParameter> Parameters { get; set; }

		/// <summary>
		/// Gets the parameter description.
		/// </summary>
		/// <returns>The parameter description.</returns>
		public string GetParameterDescription()
		{
			List<string> list = new List<string> ();
			foreach (OrderParameter param in Parameters) {
				list.Add (param.ToString ());
			}
			return string.Join(",", Parameters);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Phoenix.BL.Entities.Order"/> class.
		/// </summary>
		public Order ()
		{
			Parameters = new List<OrderParameter> ();
		}
	}

	/// <summary>
	/// Order parameter.
	/// </summary>
	[Table("OrderParameter")]
	public class OrderParameter : EntityBase
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		[PrimaryKey, AutoIncrement]
		public override int Id { get; set; }

		/// <summary>
		/// Gets or sets the order identifier.
		/// </summary>
		/// <value>The order identifier.</value>
		[Indexed]
		public int OrderId { get; set; }

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		public string Value { get; set; }

		/// <summary>
		/// Gets or sets the display value.
		/// </summary>
		/// <value>The display value.</value>
		public string DisplayValue { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Phoenix.BL.Entities.OrderParameter"/> class.
		/// </summary>
		public OrderParameter()
		{
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.OrderParameter"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Phoenix.BL.Entities.OrderParameter"/>.</returns>
		public override string ToString ()
		{
			return DisplayValue == null ? Value : DisplayValue;
		}
	}
}

