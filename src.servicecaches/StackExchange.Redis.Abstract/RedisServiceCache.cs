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
using System.Abstract;
using System.Collections.Generic;

namespace StackExchange.Redis.Abstract
{
    /// <summary>
    /// IRedisServiceCache
    /// </summary>
    public interface IRedisServiceCache : IDistributedServiceCache
    {
        /// <summary>
        /// Gets the cache.
        /// </summary>
        IDatabase Cache { get; }
    }

    /// <summary>
    /// RedisServiceCache
    /// </summary>
    public partial class RedisServiceCache : IRedisServiceCache, ServiceCacheManager.ISetupRegistration
    {
        static RedisServiceCache() { ServiceCacheManager.EnsureRegistration(); }
        /// <summary>
        /// Initializes a new instance of the <see cref="RedisServiceCache" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public RedisServiceCache(string configuration)
            : this(ConnectionMultiplexer.Connect(configuration), 0) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="RedisServiceCache" /> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="db">The database.</param>
        public RedisServiceCache(ConnectionMultiplexer connection, int db)
            : this(connection.GetDatabase(0)) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="MemcachedServiceCache" /> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <exception cref="System.ArgumentNullException">connection</exception>
        public RedisServiceCache(IDatabase database)
        {
            if (database == null)
                throw new ArgumentNullException("database");
            Cache = database;
            Settings = new ServiceCacheSettings();
        }

        Action<IServiceLocator, string> ServiceCacheManager.ISetupRegistration.DefaultServiceRegistrar
        {
            get { return (locator, name) => ServiceCacheManager.RegisterInstance<IRedisServiceCache>(this, locator, name); }
        }

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>
        /// A service object of type <paramref name="serviceType"/>.
        /// -or-
        /// null if there is no service object of type <paramref name="serviceType"/>.
        /// </returns>
        public object GetService(Type serviceType) { throw new NotImplementedException(); }

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> with the specified name.
        /// </summary>
        public object this[string name]
        {
            get { return Get(null, name); }
            set { Set(null, name, CacheItemPolicy.Default, value, ServiceCacheByDispatcher.Empty); }
        }

        /// <summary>
        /// Adds the specified tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="name">The name.</param>
        /// <param name="itemPolicy">The item policy.</param>
        /// <param name="value">The value.</param>
        /// <param name="dispatch">The dispatch.</param>
        /// <returns></returns>
        public object Add(object tag, string name, CacheItemPolicy itemPolicy, object value, ServiceCacheByDispatcher dispatch)
        {
            if (itemPolicy == null)
                throw new ArgumentNullException("itemPolicy");
            var updateCallback = itemPolicy.UpdateCallback;
            if (updateCallback != null)
                updateCallback(name, value);
            //
            if (itemPolicy.SlidingExpiration != ServiceCache.NoSlidingExpiration)
                throw new ArgumentOutOfRangeException("itemPolicy.SlidingExpiration", "not supported.");
            if (itemPolicy.RemovedCallback != null)
                throw new ArgumentOutOfRangeException("itemPolicy.RemovedCallback", "not supported.");
            //
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the item from cache associated with the key provided.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        /// The cached item.
        /// </returns>
        public object Get(object tag, string name) { throw new NotSupportedException(); }
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
                throw new ArgumentNullException("registration");
            throw new NotImplementedException();
        }
        /// <summary>
        /// Gets the specified tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="names">The names.</param>
        /// <returns></returns>
        public object Get(object tag, IEnumerable<string> names)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Gets the specified registration.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="registration">The registration.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<CacheItemHeader> Get(object tag, IServiceCacheRegistration registration)
        {
            if (registration == null)
                throw new ArgumentNullException("registration");
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool TryGet(object tag, string name, out object value) { throw new NotSupportedException(); }

        /// <summary>
        /// Removes from cache the item associated with the key provided.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="name">The name.</param>
        /// <param name="registration">The registration.</param>
        /// <returns>
        /// The item removed from the Cache. If the value in the key parameter is not found, returns null.
        /// </returns>
        public object Remove(object tag, string name, IServiceCacheRegistration registration) { throw new NotSupportedException(); }

        /// <summary>
        /// Adds an object into cache based on the parameters provided.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="name">The name.</param>
        /// <param name="itemPolicy">The itemPolicy object.</param>
        /// <param name="value">The value to store in cache.</param>
        /// <param name="dispatch">The dispatch.</param>
        /// <returns></returns>
        public object Set(object tag, string name, CacheItemPolicy itemPolicy, object value, ServiceCacheByDispatcher dispatch)
        {
            if (itemPolicy == null)
                throw new ArgumentNullException("itemPolicy");
            var updateCallback = itemPolicy.UpdateCallback;
            if (updateCallback != null)
                updateCallback(name, value);
            //
            throw new NotImplementedException();
        }

        /// <summary>
        /// Settings
        /// </summary>
        public ServiceCacheSettings Settings { get; private set; }

        #region TouchableCacheItem

        ///// <summary>
        ///// DefaultTouchableCacheItem
        ///// </summary>
        //public class DefaultTouchableCacheItem : ITouchableCacheItem
        //{
        //    private MemcachedServiceCache _parent;
        //    private ITouchableCacheItem _base;
        //    public DefaultTouchableCacheItem(MemcachedServiceCache parent, ITouchableCacheItem @base) { _parent = parent; _base = @base; }

        //    public void Touch(object tag, string[] names)
        //    {
        //        if (names == null || names.Length == 0)
        //            return;
        //        throw new NotSupportedException();
        //    }

        //    public object MakeDependency(object tag, string[] names)
        //    {
        //        if (names == null || names.Length == 0)
        //            return null;
        //        throw new NotSupportedException();
        //    }
        //}

        #endregion

        #region Domain-specific

        /// <summary>
        /// Gets the cache.
        /// </summary>
        public IDatabase Cache { get; private set; }

        #endregion
    }
}
