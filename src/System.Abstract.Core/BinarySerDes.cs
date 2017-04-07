﻿#region License
/*
The MIT License

Copyright (c) 2008 Sky Morey

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace System.Abstract
{
    /// <summary>
    /// BinarySerDes
    /// </summary>
    public class BinarySerDes : ISerDes
    {
        readonly BinaryFormatter _binaryFormatter = new BinaryFormatter();

        /// <summary>
        /// Deserializes the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// type
        /// or
        /// s
        /// </exception>
        public T Des<T>(Type type, Stream s)
            where T : class
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (s == null)
                throw new ArgumentNullException("s");
            return (_binaryFormatter.Deserialize(s) as T);
        }

        /// <summary>
        /// Deserializes the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// type
        /// or
        /// s
        /// </exception>
        public IEnumerable<T> DesMany<T>(Type type, Stream s)
            where T : class
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (s == null)
                throw new ArgumentNullException("s");
            var graphs = (_binaryFormatter.Deserialize(s) as List<object>);
            if (graphs == null)
                return null;
            return graphs.Cast<T>();
        }

        /// <summary>
        /// Serializes the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <param name="s">The s.</param>
        /// <param name="graph">The graph.</param>
        /// <exception cref="System.ArgumentNullException">
        /// type
        /// or
        /// s
        /// or
        /// graph
        /// </exception>
        public void Ser<T>(Type type, Stream s, T graph)
            where T : class
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (s == null)
                throw new ArgumentNullException("s");
            if (graph == null)
                throw new ArgumentNullException("graph");
            _binaryFormatter.Serialize(s, graph);
        }

        /// <summary>
        /// Sers the many.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <param name="s">The s.</param>
        /// <param name="graphs">The graphs.</param>
        /// <exception cref="System.ArgumentNullException">
        /// type
        /// or
        /// s
        /// or
        /// graphs
        /// </exception>
        public void SerMany<T>(Type type, Stream s, IEnumerable<T> graphs)
            where T : class
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (s == null)
                throw new ArgumentNullException("s");
            if (graphs == null)
                throw new ArgumentNullException("graphs");
            var x = new List<object>(graphs.ToArray());
            _binaryFormatter.Serialize(s, x);
        }
    }
}