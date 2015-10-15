//
// SystemRequest.cs
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
    /// System request.
    /// </summary>
    public class SystemRequest : NexusRequest<StarSystem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.SAL.SystemRequest"/> class.
        /// </summary>
        /// <param name="UID">User Id.</param>
        /// <param name="Code">Code.</param>
        public SystemRequest (int UID, string Code) : base(UID,Code,"systems", 0)
        {
        }

        /// <summary>
        /// Successfully fetched data and should be processed by subclass
        /// </summary>
        /// <param name="xmlReader">Xml reader.</param>
        /// <param name="callback">Callback.</param>
		protected override void Success(XmlReader xmlReader, Action<IEnumerable<StarSystem>> callback)
        {
			Log.WriteLine (Log.Layer.SAL, this.GetType (), "Success");

            List<StarSystem> list = new List<StarSystem> ();

            StarSystem item = null;

            while (xmlReader.Read ()) {
				if (xmlReader.IsStartElement ()) {
					try {
						if (xmlReader.Name == "system") {
							item = new StarSystem () {
								Id = Int32.Parse (xmlReader.GetAttribute ("id")),
								Name = xmlReader.GetAttribute ("name")    
							};
							list.Add (item);
						} else if (xmlReader.Name == "cbody") {
							item.CelestialBodies.Add (new CelestialBody () {
								StarSystemId = item.Id,
								Name = xmlReader.GetAttribute ("name"),
								LocalCelestialBodyId = Int32.Parse (xmlReader.GetAttribute ("id")),
								Quad = (StarSystem.QuadType) Int32.Parse (xmlReader.GetAttribute ("quad")),
								Ring = Int32.Parse (xmlReader.GetAttribute ("ring")),
								CBodyType = (CelestialBody.BodyType) Int32.Parse (xmlReader.GetAttribute ("type"))
							});
						} else if (xmlReader.Name == "link") {
							item.JumpLinks.Add (new JumpLink () {
								StarSystemId = item.Id,
								ToStarSystemId = Int32.Parse (xmlReader.GetAttribute ("sys_id")),
								Distance = Int32.Parse (xmlReader.GetAttribute ("dist"))
							});
						}
					} catch (Exception e) {
						Log.WriteLine (Log.Layer.SAL, this.GetType (), e);
					}
				}
            }

			callback (from element in list
				orderby element.Name
				select element);
        }
    }
}

