//
// ItemRequest.cs
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
    /// Item request.
    /// </summary>
    public class ItemRequest : NexusRequest<Item>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.SAL.ItemRequest"/> class.
        /// </summary>
        /// <param name="UID">User Id.</param>
        /// <param name="Code">Code.</param>
        public ItemRequest (int UID, string Code) : base(UID,Code,"items", 0)
        {
        }

        /// <summary>
        /// Successfully fetched data and should be processed by subclass
        /// </summary>
        /// <param name="xmlReader">Xml reader.</param>
        /// <param name="callback">Callback.</param>
		protected override void Success(XmlReader xmlReader, Action<IEnumerable<Item>> callback)
        {
			Log.WriteLine (Log.Layer.SAL, this.GetType (), "Success");

            List<Item> list = new List<Item> ();

            Item item = null;
            RawMaterial rm = null;

            while (xmlReader.Read ()) {
				if (xmlReader.IsStartElement ()) {
					try {
						switch(xmlReader.Name){
						case "items":
							if(xmlReader.HasAttributes) {
								item = new Item () {
									Id = Int32.Parse (xmlReader.GetAttribute ("key"))
								};
								list.Add (item);
							}
							break;
						case "Name":
							item.Name = xmlReader.GetAttribute ("value");
							break;
						case "Type":
							item.ItemType = xmlReader.GetAttribute ("value");
							break;
						case "Mus":
							item.MassUnits = Int32.Parse (xmlReader.GetAttribute ("value"));
							break;
						case "Prod":
							item.Production = Int32.Parse (xmlReader.GetAttribute ("value"));
							break;
						case "Race":
							item.Race = xmlReader.GetAttribute ("value");
							break;
						case "SubType":
							item.SubType = xmlReader.GetAttribute ("value");
							break;
						case "SubItem":
							item.SubstituteItem = Int32.Parse (xmlReader.GetAttribute ("value"));
							break;
						case "SubRatio":
							item.SubstituteRatio = float.Parse (xmlReader.GetAttribute ("value"));
							break;
						case "TechManual":
							item.TechManual = xmlReader.ReadElementContentAsString ();
							break;
						case "RawMaterials":
							if(xmlReader.HasAttributes) {
								rm = new RawMaterial () {
									ItemId = item.Id
								};
								item.RawMaterials.Add (rm);
							}
							break;
						case "Item":
							if(rm != null) {
								rm.RawMaterialId = Int32.Parse (xmlReader.GetAttribute ("value"));
							}
							break;
						case "Quantity":
							if(rm != null) {
								rm.Quantity = Int32.Parse (xmlReader.GetAttribute ("value"));
							}
							break;
						case "Blueprint":
							item.Blueprint = Int32.Parse (xmlReader.GetAttribute ("value"));
							break;
						default:
							if (xmlReader.HasAttributes) {
								item.AddProperty (xmlReader.Name, xmlReader.GetAttribute ("value"));
							}
							break;
						}
					} catch (Exception e) {
						Log.WriteLine (Log.Layer.SAL, this.GetType (), e);
					}
				}
            }

			callback (from element in list
				orderby element.Id
				select element);
        }
    }
}

