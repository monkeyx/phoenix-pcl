//
// PendingOrdersRequest.cs
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
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

using Phoenix.BL.Entities;
using Phoenix.Util;

namespace Phoenix.SAL
{
	/// <summary>
	/// Pending orders request.
	/// </summary>
	public class PendingOrdersRequest : NexusRequest<Order>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Phoenix.SAL.PendingOrdersRequest"/> class.
		/// </summary>
		/// <param name="UID">User interface.</param>
		/// <param name="Code">Code.</param>
		/// <param name="positionId">Position identifier.</param>
		public PendingOrdersRequest (int UID, string Code, int positionId) : base(UID,Code,"pending_orders", positionId)
		{
		}

		/// <summary>
		/// Successfully fetched data and should be processed by subclass
		/// </summary>
		/// <param name="xmlReader">Xml reader.</param>
		/// <param name="callback">Callback.</param>
		protected override void Success(XmlReader xmlReader, Action<IEnumerable<Order>, Exception> callback)
		{
			Log.WriteLine (Log.Layer.SAL, this.GetType (), "Success");

			List<Order> list = new List<Order> ();

			Order order = null;
			while (xmlReader.Read ()) {
				if (xmlReader.IsStartElement ()) {
					try {
						switch(xmlReader.Name){
						case "order":
							order = new Order{
								PositionId = PositionId,
								OrderTypeId = Int32.Parse(xmlReader.GetAttribute("id")),
								SequencePosition = list.Count
							};
							list.Add(order);
							break;
						case "param":
							string value = null;
							if(xmlReader.Read())
								value = xmlReader.Value.Trim ();
							OrderParameter param = new OrderParameter{
								SequencePosition = order.Parameters.Count,
								Value = value
							};
							order.Parameters.Add(param);
							break;
						}
					} catch (Exception e) {
						Log.WriteLine (Log.Layer.SAL, this.GetType (), e);
					}
				}
			}

			callback (list, null);
		}
	}
}

