//
// InfoData.cs
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
using SQLite.Net.Attributes;

namespace Phoenix.BL.Entities
{
    [Table("info")]
    public class InfoData : EntityBase
    {
        /// <summary>
        /// Gets or sets the group.
        /// </summary>
        /// <value>The group.</value>
        [Indexed]
        public string Group { get; set; }

        /// <summary>
        /// Gets or sets the group I.
        /// </summary>
        /// <value>The group I.</value>
        [Indexed]
        public int GroupID { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Indexed]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the nexus I.
        /// </summary>
        /// <value>The nexus I.</value>
        [Indexed]
        public int NexusID { get; set; }

        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>The type of the data.</value>
        public int DataType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Phoenix.InfoData"/> class.
        /// </summary>
        public InfoData ()
        {
        }
    }
}

