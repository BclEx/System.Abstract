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

using System.Collections.Generic;

namespace System.Abstract
{
    /// <summary>
    /// IServiceCache
    /// </summary>
    /// <seealso cref="System.IServiceProvider" />
    public interface IServiceCache : IServiceProvider
	{
        /// <summary>
        /// Gets or sets the <see cref="System.Object" /> with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>System.Object.</returns>
		object this[string name] { get; set; }

        /// <summary>
        /// Adds the specified tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="name">The name.</param>
        /// <param name="itemPolicy">The item policy.</param>
        /// <param name="value">The value.</param>
        /// <param name="dispatch">The dispatch.</param>
        /// <returns>System.Object.</returns>
        object Add(object tag, string name, CacheItemPolicy itemPolicy, object value, ServiceCacheByDispatcher dispatch);

        /// <summary>
        /// Gets the item from cache associated with the key provided.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="name">The name.</param>
        /// <returns>The cached item.</returns>
		object Get(object tag, string name);
        /// <summary>
        /// Gets the specified tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="name">The name.</param>
        /// <param name="registration">The registration.</param>
        /// <param name="header">The header.</param>
        /// <returns>System.Object.</returns>
        object Get(object tag, string name, IServiceCacheRegistration registration, out CacheItemHeader header);
        /// <summary>
        /// Gets the specified tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="names">The names.</param>
        /// <returns>System.Object.</returns>
		object Get(object tag, IEnumerable<string> names);
        /// <summary>
        /// Gets the specified registration.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="registration">The registration.</param>
        /// <returns>IEnumerable&lt;CacheItemHeader&gt;.</returns>
        IEnumerable<CacheItemHeader> Get(object tag, IServiceCacheRegistration registration);

        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		bool TryGet(object tag, string name, out object value);

        /// <summary>
        /// Removes from cache the item associated with the key provided.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="name">The name.</param>
        /// <param name="registration">The registration.</param>
        /// <returns>The item removed from the Cache. If the value in the key parameter is not found, returns null.</returns>
        object Remove(object tag, string name, IServiceCacheRegistration registration);

        /// <summary>
        /// Adds an object into cache based on the parameters provided.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="name">The name.</param>
        /// <param name="itemPolicy">The itemPolicy object.</param>
        /// <param name="value">The value to store in cache.</param>
        /// <param name="dispatch">The dispatch.</param>
        /// <returns>System.Object.</returns>
        object Set(object tag, string name, CacheItemPolicy itemPolicy, object value, ServiceCacheByDispatcher dispatch);

        /// <summary>
        /// Settings
        /// </summary>
        /// <value>The settings.</value>
        ServiceCacheSettings Settings { get; }
	}
}
