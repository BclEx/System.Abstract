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

using System;
using System.Linq;
using System.Abstract;
using System.Collections.Generic;

namespace Contoso.Abstract
{
    /// <summary>
    /// Interface IStaticServiceCache
    /// </summary>
    /// <seealso cref="System.Abstract.IServiceCache" />
    /// <remark>
    /// An static dictionary specific service cache interface
    /// </remark>
    public interface IStaticServiceCache : IServiceCache
    {
        /// <summary>
        /// Gets the cache.
        /// </summary>
        /// <value>The cache.</value>
        Dictionary<string, object> Cache { get; }
    }

    //: might need to make thread safe
    /// <summary>
    /// StaticServiceCache
    /// </summary>
    /// <seealso cref="Contoso.Abstract.IStaticServiceCache" />
    /// <remark>
    /// Provides a static dictionary adapter for the service cache sub-system.
    /// </remark>
    /// <example>
    /// ServiceCacheManager.SetProvider(() =&gt; new StaticServiceCache())
    /// </example>
    public class StaticServiceCache : IStaticServiceCache, ServiceCacheManager.IRegisterWithLocator
    {
        static readonly Dictionary<string, object> _cache = new Dictionary<string, object>();

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticServiceCache"/> class.
        /// </summary>
        public StaticServiceCache() =>
            Settings = new ServiceCacheSettings(new DefaultFileTouchableCacheItem(this, new DefaultTouchableCacheItem(this, null)));

        Action<IServiceLocator, string> ServiceCacheManager.IRegisterWithLocator.RegisterWithLocator =>
            (locator, name) => ServiceCacheManager.RegisterInstance<IStaticServiceCache>(this, name, locator);

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type <paramref name="serviceType" />.
        /// -or-
        /// null if there is no service object of type <paramref name="serviceType" />.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public object GetService(Type serviceType) { throw new NotImplementedException(); }

        /// <summary>
        /// Gets or sets the <see cref="System.Object" /> with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>System.Object.</returns>
        public object this[string name]
        {
            get => Get(null, name);
            set => Set(null, name, CacheItemPolicy.Default, value, ServiceCacheByDispatcher.Empty);
        }

        /// <summary>
        /// Adds an object into cache based on the parameters provided.
        /// </summary>
        /// <param name="tag">Not used</param>
        /// <param name="name">The key used to identify the item in cache.</param>
        /// <param name="itemPolicy">Not used</param>
        /// <param name="value">The value to store in cache.</param>
        /// <param name="dispatch">Not used</param>
        /// <returns>Last value that what in cache.</returns>
        public object Add(object tag, string name, CacheItemPolicy itemPolicy, object value, ServiceCacheByDispatcher dispatch)
        {
            // TODO: Throw on dependency or other stuff not supported by this simple system
            if (!_cache.TryGetValue(name, out var lastValue))
            {
                _cache[name] = value;
                var registration = dispatch.Registration;
                if (registration != null && registration.UseHeaders)
                {
                    var header = dispatch.Header;
                    header.Item = name;
                    _cache[name + "#"] = header;
                }
                return null;
            }
            return lastValue;
        }

        /// <summary>
        /// Gets the item from cache associated with the key provided.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="name">The key.</param>
        /// <returns>
        /// The cached item.
        /// </returns>
        public object Get(object tag, string name) =>
            _cache.TryGetValue(name, out var value) ? value : null;
        /// <summary>
        /// Gets the specified tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="name">The name.</param>
        /// <param name="registration">The registration.</param>
        /// <param name="header">The header.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public object Get(object tag, string name, IServiceCacheRegistration registration, out CacheItemHeader header)
        {
            if (registration == null)
                throw new ArgumentNullException(nameof(registration));
            header = registration.UseHeaders && _cache.TryGetValue(name + "#", out var value) ? (CacheItemHeader)value : null;
            return _cache.TryGetValue(name, out value) ? value : null;
        }
        /// <summary>
        /// Gets the specified tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="names">The names.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="ArgumentNullException">names</exception>
        public object Get(object tag, IEnumerable<string> names)
        {
            if (names == null)
                throw new ArgumentNullException(nameof(names));
            return names.Select(name => new { name, value = Get(null, name) }).ToDictionary(x => x.name, x => x.value);
        }
        /// <summary>
        /// Gets the specified registration.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="registration">The registration.</param>
        /// <returns>IEnumerable&lt;CacheItemHeader&gt;.</returns>
        /// <exception cref="ArgumentNullException">registration</exception>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<CacheItemHeader> Get(object tag, IServiceCacheRegistration registration)
        {
            if (registration == null)
                throw new ArgumentNullException(nameof(registration));
            var registrationName = registration.AbsoluteName + "#";
            CacheItemHeader value;
            var e = _cache.GetEnumerator();
            while (e.MoveNext())
            {
                var current = e.Current;
                var key = current.Key;
                if (key == null || !key.EndsWith(registrationName) || (value = current.Value as CacheItemHeader) == null)
                    continue;
                yield return value;
            }
        }

        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool TryGet(object tag, string name, out object value) =>
            _cache.TryGetValue(name, out value);

        /// <summary>
        /// Adds an object into cache based on the parameters provided.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="name">The name used to identify the item in cache.</param>
        /// <param name="itemPolicy">The itemPolicy defining caching policies.</param>
        /// <param name="value">The value to store in cache.</param>
        /// <param name="dispatch">The dispatch.</param>
        /// <returns>System.Object.</returns>
        public object Set(object tag, string name, CacheItemPolicy itemPolicy, object value, ServiceCacheByDispatcher dispatch)
        {
            _cache[name] = value;
            var registration = dispatch.Registration;
            if (registration != null && registration.UseHeaders)
            {
                var header = dispatch.Header;
                header.Item = name;
                _cache[name + "#"] = header;
            }
            return value;
        }

        /// <summary>
        /// Removes from cache the item associated with the key provided.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="name">The key.</param>
        /// <param name="registration">The registration.</param>
        /// <returns>The item removed from the Cache. If the value in the key parameter is not found, returns null.</returns>
        public object Remove(object tag, string name, IServiceCacheRegistration registration)
        {
            if (_cache.TryGetValue(name, out var value))
            {
                if (registration != null && registration.UseHeaders)
                    _cache.Remove(name + "#");
                _cache.Remove(name);
                return value;
            }
            return null;
        }

        /// <summary>
        /// Settings
        /// </summary>
        /// <value>The settings.</value>
        public ServiceCacheSettings Settings { get; private set; }

        #region TouchableCacheItem

        /// <summary>
        /// DefaultTouchableCacheItem
        /// </summary>
        /// <seealso cref="System.Abstract.ITouchableCacheItem" />
        public class DefaultTouchableCacheItem : ITouchableCacheItem
        {
            readonly StaticServiceCache _parent;
            readonly ITouchableCacheItem _base;
            /// <summary>
            /// Initializes a new instance of the <see cref="DefaultTouchableCacheItem"/> class.
            /// </summary>
            /// <param name="parent">The parent.</param>
            /// <param name="base">The @base.</param>
            public DefaultTouchableCacheItem(StaticServiceCache parent, ITouchableCacheItem @base) { _parent = parent; _base = @base; }

            /// <summary>
            /// Touches the specified tag.
            /// </summary>
            /// <param name="tag">The tag.</param>
            /// <param name="names">The names.</param>
            public void Touch(object tag, string[] names)
            {
                if (names == null || names.Length == 0)
                    return;
                _cache.Clear();
                if (_base != null)
                    _base.Touch(tag, names);
            }

            /// <summary>
            /// Makes the dependency.
            /// </summary>
            /// <param name="tag">The tag.</param>
            /// <param name="names">The names.</param>
            /// <returns>System.Object.</returns>
            /// <exception cref="NotSupportedException"></exception>
            public object MakeDependency(object tag, string[] names)
            {
                if (names == null || names.Length == 0)
                    return null;
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// DefaultFileTouchableCacheItem
        /// </summary>
        /// <seealso cref="System.Abstract.AbstractFileTouchableCacheItem" />
        public class DefaultFileTouchableCacheItem : AbstractFileTouchableCacheItem
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DefaultFileTouchableCacheItem" /> class.
            /// </summary>
            /// <param name="parent">The parent.</param>
            /// <param name="base">The @base.</param>
            public DefaultFileTouchableCacheItem(StaticServiceCache parent, ITouchableCacheItem @base)
                : base(parent, @base) { }

            /// <summary>
            /// Makes the dependency internal.
            /// </summary>
            /// <param name="tag">The tag.</param>
            /// <param name="names">The names.</param>
            /// <param name="baseDependency">The base dependency.</param>
            /// <returns>System.Object.</returns>
            protected override object MakeDependencyInternal(object tag, string[] names, object baseDependency) => null;
        }

        #endregion

        #region Domain-specific

        /// <summary>
        /// Gets the cache.
        /// </summary>
        /// <value>The cache.</value>
        public Dictionary<string, object> Cache =>
            _cache;

        #endregion
    }
}
