#region License
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

using System.IO;
using System.Text;
namespace System.Abstract
{
    /// <summary>
    /// AbstractExtensions
    /// </summary>
    public static partial class AbstractExtensions
    {
        /// <summary>
        /// Deserializes the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serDes">The ser DES.</param>
        /// <param name="type">The type.</param>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static T Des<T>(this ISerDes serDes, Type type, string text)
            where T : class { return Des<T>(serDes, type, text, Encoding.Default); }

        /// <summary>
        /// Deserializes the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serDes">The ser DES.</param>
        /// <param name="type">The type.</param>
        /// <param name="text">The text.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static T Des<T>(this ISerDes serDes, Type type, string text, Encoding encoding)
            where T : class
        {
            using (var s = new MemoryStream(encoding.GetBytes(text)))
                return serDes.Des<T>(type, s);
        }
        /// <summary>
        /// Deserializes the specified type from base64.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serDes">The ser DES.</param>
        /// <param name="type">The type.</param>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static T DesBase64<T>(this ISerDes serDes, Type type, string text)
            where T : class
        {
            using (var s = new MemoryStream(Convert.FromBase64String(text)))
                return serDes.Des<T>(type, s);
        }

        /// <summary>
        /// Serializes the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serDes">The ser DES.</param>
        /// <param name="type">The type.</param>
        /// <param name="graph">The graph.</param>
        /// <returns></returns>
        public static string Ser<T>(this ISerDes serDes, Type type, T graph)
            where T : class { return Ser<T>(serDes, type, graph, Encoding.Default); }
        /// <summary>
        /// Serializes the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serDes">The ser DES.</param>
        /// <param name="type">The type.</param>
        /// <param name="graph">The graph.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static string Ser<T>(this ISerDes serDes, Type type, T graph, Encoding encoding)
            where T : class
        {
            using (var s = new MemoryStream())
            {
                serDes.Ser<T>(type, s, graph);
                s.Flush(); s.Position = 0;
                return encoding.GetString(s.ToArray());
            }
        }
        /// <summary>
        /// Serializes the specified type to base64.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serDes">The ser DES.</param>
        /// <param name="type">The type.</param>
        /// <param name="graph">The graph.</param>
        /// <returns></returns>
        public static string SerBase64<T>(this ISerDes serDes, Type type, T graph)
            where T : class
        {
            using (var s = new MemoryStream())
            {
                serDes.Ser<T>(type, s, graph);
                s.Flush(); s.Position = 0;
                return Convert.ToBase64String(s.ToArray());
            }
        }
    }
}
