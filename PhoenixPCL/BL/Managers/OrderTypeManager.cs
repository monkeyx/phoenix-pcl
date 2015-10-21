//
// OrderTypeManager.cs
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

using Phoenix.BL.Entities;

namespace Phoenix.BL.Managers
{
    /// <summary>
    /// Order type manager.
    /// </summary>
    public class OrderTypeManager : NexusManager<OrderType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.BL.Managers.OrderTypeManager"/> class.
        /// </summary>
        /// <param name="user">User.</param>
        public OrderTypeManager (User user) : base(user)
        {
        }

		/// <summary>
		/// Gets the order types for position.
		/// </summary>
		/// <param name="positionFlag">Position flag.</param>
		/// <param name="callback">Callback.</param>
		public async void GetOrderTypesForPosition(Position.PositionFlag positionFlag, Action<IEnumerable<OrderType>> callback)
		{
			List<OrderType> orderTypes = await GetDataManager ().GetItems (null, true);
			IEnumerable<OrderType> results = from element in orderTypes
			                                where element.IsForPosition (positionFlag)
			                                select element;
			callback (results);
		}
    }
}

