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
using System.Linq;
using System.Text;

namespace System.Abstract
{
    /// <summary>
    /// ServiceCache
    /// </summary>
    public static partial class ServiceCache
    {
        /// <summary>
        /// Provides <see cref="System.DateTime" /> instance to be used when no absolute expiration value to be set.
        /// </summary>
        public static readonly DateTime InfiniteAbsoluteExpiration = DateTime.MaxValue.AddDays(-1);

        /// <summary>
        /// Provides <see cref="System.TimeSpan" /> instance to be used when no sliding expiration value to be set.
        /// </summary>
        public static readonly TimeSpan NoSlidingExpiration = TimeSpan.Zero;

        internal static string GetNamespace(IEnumerable<object> values)
        {
            if (values == null || !values.Any())
                return null;
            var b = new StringBuilder();
            foreach (var v in values)
            {
                if (v != null)
                    b.Append(v.ToString());
                b.Append("\\");
            }
            return b.ToString();
        }

        /// <summary>
        /// Touches the specified names.
        /// </summary>
        /// <param name="names">The names.</param>
        public static void Touch(params string[] names) =>
            ServiceCacheManager.Current.Touch(null, names);
        /// <summary>
        /// Touches the specified tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="names">The names.</param>
        public static void Touch(object tag, params string[] names) =>
            ServiceCacheManager.Current.Touch(tag, names);

        #region Registrations

        /// <summary>
        /// Gets the specified cached item.
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <param name="values">The values.</param>
        /// <returns>System.Object.</returns>
        public static object Get(ServiceCacheRegistration registration, object[] values) =>
            ServiceCacheManager.Current.Get<object>(registration, null, values);
        /// <summary>
        /// Gets the specified registration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="registration">The registration.</param>
        /// <param name="values">The values.</param>
        /// <returns>T.</returns>
        public static T Get<T>(ServiceCacheRegistration registration, object[] values) =>
            ServiceCacheManager.Current.Get<T>(registration, null, values);
        /// <summary>
        /// Gets the specified cached item.
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="values">The values.</param>
        /// <returns>System.Object.</returns>
        public static object Get(ServiceCacheRegistration registration, object tag = null, object[] values = null) =>
            ServiceCacheManager.Current.Get<object>(registration, tag, values);
        /// <summary>
        /// Gets the specified registration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="registration">The registration.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="values">The values.</param>
        /// <returns>T.</returns>
        public static T Get<T>(ServiceCacheRegistration registration, object tag = null, object[] values = null) =>
            ServiceCacheManager.Current.Get<T>(registration, tag, values);
        /// <summary>
        /// Gets the many.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="registration">The registration.</param>
        /// <param name="values">The values.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        public static IEnumerable<T> GetMany<T>(ServiceCacheRegistration registration, object[] values) =>
            ServiceCacheManager.Current.Get<IEnumerable<T>>(registration, null, values);
        /// <summary>
        /// Gets the many.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="registration">The registration.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="values">The values.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        public static IEnumerable<T> GetMany<T>(ServiceCacheRegistration registration, object tag = null, object[] values = null) =>
            ServiceCacheManager.Current.Get<IEnumerable<T>>(registration, tag, values);
        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="registration">The registration.</param>
        /// <param name="values">The values.</param>
        /// <returns>IQueryable&lt;T&gt;.</returns>
        public static IQueryable<T> GetQuery<T>(ServiceCacheRegistration registration, object[] values) =>
            ServiceCacheManager.Current.Get<IQueryable<T>>(registration, null, values);
        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="registration">The registration.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="values">The values.</param>
        /// <returns>IQueryable&lt;T&gt;.</returns>
        public static IQueryable<T> GetQuery<T>(ServiceCacheRegistration registration, object tag = null, object[] values = null) =>
            ServiceCacheManager.Current.Get<IQueryable<T>>(registration, tag, values);

        #endregion

        //internal readonly static ObjectCache Cache = MemoryCache.Default;

        //static string _directory;

        ///// <summary>
        ///// Gets or sets the directory.
        ///// </summary>
        ///// <value>The directory.</value>
        ///// <exception cref="System.ArgumentNullException">value</exception>
        //public static string Directory
        //{
        //    get => _directory;
        //    set
        //    {
        //        if (string.IsNullOrEmpty(value))
        //            throw new ArgumentNullException("value");
        //        _directory = value.EndsWith("\\") ? value : value + "\\";
        //    }
        //}

        ///// <summary>
        ///// Gets the specified tag.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="tag">The tag.</param>
        ///// <param name="key">The key.</param>
        ///// <param name="values">The values.</param>
        ///// <returns>T.</returns>
        //public static T Get<T>(object tag, ServiceCacheRegistration key, params object[] values) =>
        //    Cache.Get<T>(key, tag, values);

        ///// <summary>
        ///// Gets the result.
        ///// </summary>
        ///// <param name="tag">The tag.</param>
        ///// <param name="key">The key.</param>
        ///// <param name="values">The values.</param>
        ///// <returns>ServiceCacheResult.</returns>
        //public static ServiceCacheResult GetResult(object tag, ServiceCacheRegistration key, params object[] values) =>
        //    Cache.Get<ServiceCacheResult>(key, tag, values);

        ///// <summary>
        ///// Gets the many.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="tag">The tag.</param>
        ///// <param name="key">The key.</param>
        ///// <param name="values">The values.</param>
        ///// <returns>IEnumerable&lt;T&gt;.</returns>
        //public static IEnumerable<T> GetMany<T>(object tag, ServiceCacheRegistration key, params object[] values) =>
        //    Cache.Get<IEnumerable<T>>(key, tag, values);

        ///// <summary>
        ///// Gets the many.
        ///// </summary>
        ///// <typeparam name="TKey">The type of the t key.</typeparam>
        ///// <typeparam name="TValue">The type of the t value.</typeparam>
        ///// <param name="tag">The tag.</param>
        ///// <param name="key">The key.</param>
        ///// <param name="values">The values.</param>
        ///// <returns>IDictionary&lt;TKey, TValue&gt;.</returns>
        //public static IDictionary<TKey, TValue> GetMany<TKey, TValue>(object tag, ServiceCacheRegistration key, params object[] values) =>
        //    Cache.Get<IDictionary<TKey, TValue>>(key, tag, values);

        ///// <summary>
        ///// Gets the query.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="tag">The tag.</param>
        ///// <param name="key">The key.</param>
        ///// <param name="values">The values.</param>
        ///// <returns>IQueryable&lt;T&gt;.</returns>
        //public static IQueryable<T> GetQuery<T>(object tag, ServiceCacheRegistration key, params object[] values) =>
        //    Cache.Get<IQueryable<T>>(key, tag, values);

        ///// <summary>
        ///// get as an asynchronous operation.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="tag">The tag.</param>
        ///// <param name="key">The key.</param>
        ///// <param name="values">The values.</param>
        ///// <returns>Task&lt;T&gt;.</returns>
        //public static async Task<T> GetAsync<T>(object tag, ServiceCacheRegistration key, params object[] values) =>
        //    await Cache.GetAsync<T>(key, tag, values);

        ///// <summary>
        ///// get result as an asynchronous operation.
        ///// </summary>
        ///// <param name="tag">The tag.</param>
        ///// <param name="key">The key.</param>
        ///// <param name="values">The values.</param>
        ///// <returns>Task&lt;ServiceCacheResult&gt;.</returns>
        //public static async Task<ServiceCacheResult> GetResultAsync(object tag, ServiceCacheRegistration key, params object[] values) =>
        //    await Cache.GetAsync<ServiceCacheResult>(key, tag, values);

        ///// <summary>
        ///// get many as an asynchronous operation.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="tag">The tag.</param>
        ///// <param name="key">The key.</param>
        ///// <param name="values">The values.</param>
        ///// <returns>Task&lt;IEnumerable&lt;T&gt;&gt;.</returns>
        //public static async Task<IEnumerable<T>> GetManyAsync<T>(object tag, ServiceCacheRegistration key, params object[] values) =>
        //    await Cache.GetAsync<IEnumerable<T>>(key, tag, values);

        ///// <summary>
        ///// get many as an asynchronous operation.
        ///// </summary>
        ///// <typeparam name="TKey">The type of the t key.</typeparam>
        ///// <typeparam name="TValue">The type of the t value.</typeparam>
        ///// <param name="tag">The tag.</param>
        ///// <param name="key">The key.</param>
        ///// <param name="values">The values.</param>
        ///// <returns>Task&lt;IDictionary&lt;TKey, TValue&gt;&gt;.</returns>
        //public static async Task<IDictionary<TKey, TValue>> GetManyAsync<TKey, TValue>(object tag, ServiceCacheRegistration key, params object[] values) =>
        //    await Cache.GetAsync<IDictionary<TKey, TValue>>(key, tag, values);

        ///// <summary>
        ///// get query as an asynchronous operation.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="tag">The tag.</param>
        ///// <param name="key">The key.</param>
        ///// <param name="values">The values.</param>
        ///// <returns>Task&lt;IQueryable&lt;T&gt;&gt;.</returns>
        //public static async Task<IQueryable<T>> GetQueryAsync<T>(object tag, ServiceCacheRegistration key, params object[] values) =>
        //    await Cache.GetAsync<IQueryable<T>>(key, tag, values);

        ///// <summary>
        ///// Removes the specified key.
        ///// </summary>
        ///// <param name="key">The key.</param>
        ///// <param name="values">The values.</param>
        ///// <returns>System.Object.</returns>
        //public static object Remove(ServiceCacheRegistration key, params object[] values) =>
        //    Cache.Remove(key, values);

        ///// <summary>
        ///// Determines whether [contains] [the specified key].
        ///// </summary>
        ///// <param name="key">The key.</param>
        ///// <param name="values">The values.</param>
        ///// <returns><c>true</c> if [contains] [the specified key]; otherwise, <c>false</c>.</returns>
        //public static bool Contains(ServiceCacheRegistration key, params object[] values) =>
        //    Cache.Contains(key, values);

        ///// <summary>
        ///// Touches the specified names.
        ///// </summary>
        ///// <param name="names">The names.</param>
        //public static void Touch(params string[] names) =>
        //    Cache.Touch(names);
    }
}
