﻿//
// InfoRequest.cs
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
    /// Info request.
    /// </summary>
    public class InfoRequest : NexusRequest<InfoData>
    {
		readonly static string[] IGNORE_INFO = {"_locked", "(null)"};

        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.SAL.InfoRequest"/> class.
        /// </summary>
        /// <param name="UID">User Id.</param>
        /// <param name="Code">Code.</param>
        public InfoRequest (int UID, string Code) : base(UID,Code,"info_data", 0)
        {   
        }

        /// <summary>
        /// Successfully fetched data and should be processed by subclass
        /// </summary>
        /// <param name="xmlReader">Xml reader.</param>
        /// <param name="callback">Callback.</param>
		protected override void Success(XmlReader xmlReader, Action<IEnumerable<InfoData>, Exception> callback)
        {
			Log.WriteLine (Log.Layer.SAL, this.GetType (), "Success");

            List<InfoData> list = new List<InfoData> ();

            string groupName = null;
            int groupId = 0;

            while (xmlReader.Read ()) {
				if (xmlReader.IsStartElement ()) {
					try {
						if (xmlReader.Name == "type") {
							groupName = xmlReader.GetAttribute ("name");
							groupId = Int32.Parse (xmlReader.GetAttribute ("num"));
							Log.WriteLine(Log.Layer.SAL,GetType(),"Group " + groupName + " (" + groupId + ")");
						} else if (xmlReader.Name == "data") {
							string name = xmlReader.GetAttribute ("name");
							if(!IGNORE_INFO.Contains(name)){
								InfoData item = new InfoData () {
									Name = name,
									NexusId = Int32.Parse (xmlReader.GetAttribute ("num")),
									DataType = Int32.Parse (xmlReader.GetAttribute ("type")),
									Group = groupName,
									GroupId = groupId
								};
								list.Add (item);
							}
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

