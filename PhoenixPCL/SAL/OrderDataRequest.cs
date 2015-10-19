//
// OrderDataRequest.cs
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
using System.Xml;
using System.Linq;

using Phoenix.BL.Entities;
using Phoenix.Util;

namespace Phoenix.SAL
{
    /// <summary>
    /// Order data request.
    /// </summary>
    public class OrderDataRequest : NexusRequest<OrderType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.SAL.OrderDataRequest"/> class.
        /// </summary>
        /// <param name="UID">User Id.</param>
        /// <param name="Code">Code.</param>
        public OrderDataRequest (int UID, string Code) : base(UID,Code,"order_data", 0)
        {
        }

        /// <summary>
        /// Successfully fetched data and should be processed by subclass
        /// </summary>
        /// <param name="xmlReader">Xml reader.</param>
        /// <param name="callback">Callback.</param>
		protected override void Success(XmlReader xmlReader, Action<IEnumerable<OrderType>, Exception> callback)
        {
			Log.WriteLine (Log.Layer.SAL, this.GetType (), "Success");

            List<OrderType> list = new List<OrderType> ();

            OrderType item = null;
            while (xmlReader.Read ()) {
				if (xmlReader.IsStartElement ()) {
					try {
						if (xmlReader.Name == "order") {
							item = new OrderType () {
								Name = xmlReader.GetAttribute ("name"),
								Id = Int32.Parse (xmlReader.GetAttribute ("id")),
								TypeFlag = Int32.Parse (xmlReader.GetAttribute ("typeflag")),
								Position = Int32.Parse (xmlReader.GetAttribute ("posflag")),
								TUCost = Int32.Parse (xmlReader.GetAttribute ("tus"))
							};
							list.Add (item);
						} else if (xmlReader.Name == "desc") {
							if(xmlReader.Read())
								item.Description = xmlReader.Value.Trim ();
						} else if (xmlReader.Name == "param") {
							OrderParameterType param = new OrderParameterType () {
								Name = xmlReader.GetAttribute ("name"),
								InfoType = Int32.Parse (xmlReader.GetAttribute ("infotype")),
								DataType = (OrderType.DataTypes) Int32.Parse (xmlReader.GetAttribute ("datatype"))
							};
							item.Parameters.Add (param);
						}
					} catch (Exception e) {
						Log.WriteLine (Log.Layer.SAL, this.GetType (), e);
					}    
				}
			}

			callback (from element in list
				orderby element.Name
				select element, null);
        }
    }
}

