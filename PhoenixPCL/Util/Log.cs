//
// Log.cs
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

using Phoenix;

namespace Phoenix.Util
{
	/// <summary>
	/// Log.
	/// </summary>
	public static class Log
	{
		/// <summary>
		/// Layer.
		/// </summary>
		public enum Layer
		{
			UI,
			AL,
			BL,
			DAL,
			DL,
			SAL
		}

		/// <summary>
		/// Gets or sets the logger.
		/// </summary>
		/// <value>The logger.</value>
		public static ILogger Logger { get; set; }

		/// <summary>
		/// Writes the line.
		/// </summary>
		/// <param name="layer">Layer.</param>
		/// <param name="type">Type.</param>
		/// <param name="format">Format.</param>
		/// <param name="arg">Argument.</param>
		public static void WriteLine(Layer layer, Type type, string format, params object[] arg)
		{
			if (Log.IsLayerEnabled(layer)) {
				Logger.WriteLine("[" + layer + " : " + type.Name + "]: " + format, arg);
			}
		}

		/// <summary>
		/// Writes the line.
		/// </summary>
		/// <param name="layer">Layer.</param>
		/// <param name="type">Type.</param>
		/// <param name="value">Value.</param>
		public static void WriteLine(Layer layer, Type type, object value)
		{
			if (Log.IsLayerEnabled(layer)) {
				Logger.WriteLine("[" + layer + " : " + type.Name + "]: " + value);
			}
		}

		/// <summary>
		/// Enables the layer.
		/// </summary>
		/// <param name="layer">Layer.</param>
		public static void EnableLayer(Layer layer)
		{
			Log.EnabledLayers.Add(layer);
		}

		/// <summary>
		/// Disables the layer.
		/// </summary>
		/// <param name="layer">Layer.</param>
		public static void DisableLayer(Layer layer)
		{
			Log.EnabledLayers.Remove(layer);
		}

		/// <summary>
		/// Determines if is layer enabled the specified layer.
		/// </summary>
		/// <returns><c>true</c> if is layer enabled the specified layer; otherwise, <c>false</c>.</returns>
		/// <param name="layer">Layer.</param>
		public static bool IsLayerEnabled(Layer layer)
		{
			return Log.EnabledLayers.Contains(layer);
		}

		/// <summary>
		/// Initializes the <see cref="Phoenix.Util.Log"/> class.
		/// </summary>
		static Log()
		{
			#if UI_LOG_DISABLE
			#else
			Log.EnableLayer(Layer.UI);
			#endif

			#if AL_LOG_DISABLE
			#else
			Log.EnableLayer(Layer.AL);
			#endif

			#if BL_LOG_DISABLE
			#else
			Log.EnableLayer(Layer.BL);
			#endif

			#if DAL_LOG_DISABLE
			#else
			Log.EnableLayer(Layer.DAL);
			#endif

			#if DL_LOG_DISABLE
			#else
			Log.EnableLayer(Layer.DL);
			#endif

			#if SAL_LOG_DISABLE
			#else
			Log.EnableLayer(Layer.SAL);
			#endif
		}

		private static List<Layer> EnabledLayers
		{
			get
			{
				if (_enabledLayers == null) {
					_enabledLayers = new List<Layer>();
				}
				return _enabledLayers;
			}
		}

		private static List<Layer> _enabledLayers;
	}
}

