//
// IEntityManager.cs
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

using Phoenix.BL.Entities;

namespace Phoenix.BL.Managers
{
    /// <summary>
    /// Entity manager interface
    /// </summary>
    public interface IEntityManager<T> where T :   IEntity, new()
    {
        /// <summary>
        /// Gets an entity by its identifier and passes it to the callback
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="callback">Callback.</param>
        void Get(int id, Action<T> callback);

        /// <summary>
        /// Gets the first entity by its identifier and passes it to the callback
        /// </summary>
        /// <param name="callback">Callback.</param>
        void First(Action<T> callback);
    }
}

