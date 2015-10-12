//
// BaseNexusRequest.cs
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
using System.IO;
using System.Net;
using System.Xml;

using Phoenix.BL.Entities;

namespace Phoenix.SAL
{
    public interface INexusRequest<T> where T :   IEntity, new()
    {
        HttpStatusCode StatusCode { get; set;}
        void Fetch(Action<List<T>> callback);
    }

    /// <summary>
    /// Nexus request factory.
    /// </summary>
    public static class NexusRequestFactory
    {
        /// <summary>
        /// Creates the request.
        /// </summary>
        /// <returns>The request.</returns>
        /// <param name="user">User.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static INexusRequest<T> CreateRequest<T>(User user) where T : IEntity, new()
        {
            switch (typeof(T).Name) {
            case "GameStatus":
                return (INexusRequest<T>)new GameStatusRequest (user.Id, user.Code);
            case "InfoData":
                return (INexusRequest<T>)new InfoRequest (user.Id, user.Code);
            case "Item":
                return (INexusRequest<T>)new ItemRequest (user.Id, user.Code);
            case "OrderType":
                return (INexusRequest<T>)new OrderDataRequest (user.Id, user.Code);
            case "Position":
                return (INexusRequest<T>)new PositionRequest (user.Id, user.Code);
            case "StarSystem":
                return (INexusRequest<T>)new SystemRequest (user.Id, user.Code);
            default:
                throw new Exception ("Unsupported type"); 
            }
        }
    }

    /// <summary>
    /// Nexus request abstract class.
    /// </summary>
    public abstract class NexusRequest<T> : INexusRequest<T>  where T :   IEntity, new()
    {
        /// <summary>
        /// Base URL for Nexus XML
        /// </summary>
        const string BASE_URL = "http://www.phoenixbse.com/index.php?a=xml";

        /// <summary>
        /// Gets or sets the user interface.
        /// </summary>
        /// <value>The user interface.</value>
        public int UID { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the position identifier.
        /// </summary>
        /// <value>The turn identifier.</value>
        public int PositionId { get; set; }

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>The status code.</value>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.SAL.BaseNexusRequest"/> class.
        /// </summary>
        /// <param name="UID">User interface.</param>
        /// <param name="Code">Code.</param>
        public NexusRequest (int UID, string code, string action, int positionId)
        {
            this.UID = UID;
            this.Code = code;
            this.Action = action;
            this.PositionId = positionId;
        }

        /// <summary>
        /// Fetch data from Nexus
        /// </summary>
        /// <param name="callback">Callback.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public void Fetch(Action<List<T>> callback)
        {
            _callback = callback;
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create (RequestURL());
            httpRequest.BeginGetResponse (new AsyncCallback (FinishWebRequest), httpRequest);
        }

        /// <summary>
        /// Successfully fetched data and should be processed by subclass
        /// </summary>
        /// <param name="xmlReader">Xml reader.</param>
        /// <param name="callback">Callback.</param>
        protected abstract void Success(XmlReader xmlReader, Action<List<T>> callback);

        private Action<List<T>> _callback;

        private string RequestURL()
        {
            return BASE_URL + "&uid=" + UID + "&code=" + Code + "&sa=" + Action + "&tid=" + PositionId + "&pid=" + PositionId;
        }

        private void FinishWebRequest(IAsyncResult result)
        {
            HttpWebResponse httpResponse = (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;
            if (httpResponse.StatusCode == HttpStatusCode.OK) {
                Stream httpResponseStream = httpResponse.GetResponseStream ();
                XmlReader xmlReader = XmlReader.Create (httpResponseStream);
                Success (xmlReader, _callback);

            }
            StatusCode = httpResponse.StatusCode;
        }
    }
}

