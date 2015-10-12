//
// PositionRequest.cs
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

using Phoenix.BL.Entities;

namespace Phoenix.SAL
{
    /// <summary>
    /// Position request.
    /// </summary>
    public class PositionRequest : NexusRequest<Position>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.SAL.PositionRequest"/> class.
        /// </summary>
        /// <param name="UID">User Id.</param>
        /// <param name="Code">Code.</param>
        public PositionRequest (int UID, string Code) : base(UID,Code,"pos_list", 0)
        {
        }

        /// <summary>
        /// Successfully fetched data and should be processed by subclass
        /// </summary>
        /// <param name="xmlReader">Xml reader.</param>
        /// <param name="callback">Callback.</param>
        protected override void Success(XmlReader xmlReader, Action<List<Position>> callback)
        {
            List<Position> list = new List<Position> ();

            Position item = null;
            while (xmlReader.Read ()) {
                if (xmlReader.Name == "position") {
                    item = new Position () {
                        Name = xmlReader.GetAttribute ("name"),
                        Id = Int32.Parse (xmlReader.GetAttribute ("num"))
                    };
                    list.Add (item);
                } else if (xmlReader.Name == "system_text") {
                    item.SystemText = xmlReader.ReadContentAsString ();
                } else if (xmlReader.Name == "loc_text") {
                    item.LocationText = xmlReader.ReadContentAsString ();
                } else if (xmlReader.Name == "size") {
                    item.Size = xmlReader.ReadContentAsString ();
                } else if (xmlReader.Name == "design") {
                    item.Design = xmlReader.ReadContentAsString ();
                } else if (xmlReader.Name == "class") {
                    item.PositionClass = xmlReader.ReadContentAsString ();
                } else if (xmlReader.Name == "orders") {
                    item.Orders = Boolean.Parse (xmlReader.ReadContentAsString ());
                }
            }
            callback (list);
        }
    }
}

