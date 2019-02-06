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
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;

namespace System.Abstract
{
    /// <summary>
    /// IServiceCacheDispatcher
    /// </summary>
    public interface IServiceCacheDispatcher
    {
        /// <summary>
        /// Gets the specified cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache">The cache.</param>
        /// <param name="registration">The registration.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="values">The values.</param>
        /// <returns>T.</returns>
        T Get<T>(IServiceCache cache, ServiceCacheRegistration registration, object tag, object[] values);

        /// <summary>
        /// Sends the specified cache.
        /// </summary>
        /// <param name="cache">The cache.</param>
        /// <param name="registration">The registration.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="messages">The messages.</param>
        void Send(IServiceCache cache, ServiceCacheRegistration registration, object tag, params object[] messages);

        /// <summary>
        /// Querys the specified cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache">The cache.</param>
        /// <param name="registration">The registration.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="messages">The messages.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        IEnumerable<T> Query<T>(IServiceCache cache, ServiceCacheRegistration registration, object tag, params object[] messages);

        /// <summary>
        /// Removes the specified cache.
        /// </summary>
        /// <param name="cache">The cache.</param>
        /// <param name="registration">The registration.</param>
        void Remove(IServiceCache cache, ServiceCacheRegistration registration);
    }

    /// <summary>
    /// Class ServiceCacheDispatcher.
    /// </summary>
    public static class ServiceCacheDispatcher
    {
        static readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        const string RegionMarker = "@";

        static class CacheFile
        {
            static string GetFilePathForName(string name) =>
                !string.IsNullOrEmpty(ServiceCacheManager.Directory) ? Path.Combine(ServiceCacheManager.Directory, name + ".txt") : null;

            static void WriteBodyForName(string name, string path) =>
                File.WriteAllText(path, $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\r\n");

            public static void Touch(string name)
            {
                if (string.IsNullOrEmpty(ServiceCacheManager.Directory))
                    return;
                lock (_rwLock)
                {
                    var filePath = GetFilePathForName(name);
                    if (filePath == null)
                        return;
                    try { WriteBodyForName(name, filePath); }
                    catch { };
                }
            }

            public static ChangeMonitor MakeChangeMonitors(IEnumerable<string> names)
            {
                if (string.IsNullOrEmpty(ServiceCacheManager.Directory))
                    return null;
                lock (_rwLock)
                {
                    var filePaths = new List<string>();
                    foreach (var name in names)
                    {
                        var filePath = GetFilePathForName(name);
                        if (filePath == null)
                            continue;
                        filePaths.Add(filePath);
                        try
                        {
                            var filePathAsDirectory = Path.GetDirectoryName(filePath);
                            if (!Directory.Exists(filePathAsDirectory))
                                Directory.CreateDirectory(filePathAsDirectory);
                            if (!File.Exists(filePath))
                                WriteBodyForName(name, filePath);
                        }
                        catch { };
                    }
                    return new HostFileChangeMonitor(filePaths);
                }
            }
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="cache">The cache.</param>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        /// <returns>System.Object.</returns>
        public static object Remove(this ObjectCache cache, ServiceCacheRegistration key, object[] values)
        {
            var name = key.GetNamespace(values);
            return cache.Remove(name);
        }

        /// <summary>
        /// Determines whether [contains] [the specified key].
        /// </summary>
        /// <param name="cache">The cache.</param>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        /// <returns><c>true</c> if [contains] [the specified key]; otherwise, <c>false</c>.</returns>
        public static bool Contains(this ObjectCache cache, ServiceCacheRegistration key, object[] values)
        {
            var name = key.GetNamespace(values);
            return cache.Contains(name);
        }

        /// <summary>
        /// Touches the specified names.
        /// </summary>
        /// <param name="cache">The cache.</param>
        /// <param name="names">The names.</param>
        public static void Touch(this ObjectCache cache, string[] names)
        {
            if (names == null || names.Length == 0)
                return;
            foreach (var x in names)
            {
                if (x.StartsWith("#"))
                {
                    CacheFile.Touch(x);
                    continue;
                }
                var name = x; TryGetRegion(ref name, out var regionName);
                cache.Set(name, string.Empty, ObjectCache.InfiniteAbsoluteExpiration, regionName);
            }
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache">The cache.</param>
        /// <param name="key">The key.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="values">The values.</param>
        /// <returns>T.</returns>
        /// <exception cref="System.ArgumentNullException">key</exception>
        public static T Get<T>(this ObjectCache cache, ServiceCacheRegistration key, object tag, object[] values)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            return AddOrGetExistingUsingLock<T>(cache, key, tag, values);
        }

        /// <summary>
        /// get as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache">The cache.</param>
        /// <param name="key">The key.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="values">The values.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        /// <exception cref="System.ArgumentNullException">key</exception>
        public static async Task<T> GetAsync<T>(this ObjectCache cache, ServiceCacheRegistration key, object tag, object[] values)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            return await AddOrGetExistingUsingLockAsync<T>(cache, key, tag, values);
        }

        /// <summary>
        /// Adds the specified name.
        /// </summary>
        /// <param name="cache">The cache.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="itemPolicy">The item policy.</param>
        /// <param name="regionName">Name of the region.</param>
        /// <exception cref="System.InvalidOperationException">Not Service Cache Result</exception>
        public static void Add(this ObjectCache cache, string name, object value, Action<CacheItemPolicy> itemPolicy, string regionName = null)
        {
            var rwLock = _rwLock;
            rwLock.EnterWriteLock();
            try
            {
                if (value is ServiceCacheResult valueResult)
                {
                    itemPolicy?.Invoke(valueResult.ItemPolicy);
                    AddInsideLock(cache, name, valueResult, regionName);
                }
                else throw new InvalidOperationException("Not Service Cache Result");
            }
            finally { rwLock.ExitWriteLock(); }
        }

        /// <summary>
        /// Makes the change monitors.
        /// </summary>
        /// <param name="cache">The cache.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="names">The names.</param>
        /// <returns>IEnumerable&lt;ChangeMonitor&gt;.</returns>
        static IEnumerable<ChangeMonitor> MakeChangeMonitors(ObjectCache cache, object tag, IEnumerable<string> names)
        {
            return names.Select(x =>
            {
                if (x.StartsWith("#")) return new { name = x.Substring(1), regionName = "#" };
                var name = x; TryGetRegion(ref name, out var regionName);
                // add anchor name if not exists
                if (!cache.Contains(name, regionName))
                    cache.Set(name, string.Empty, ObjectCache.InfiniteAbsoluteExpiration, regionName);
                return new { name, regionName };
            }).GroupBy(x => x.regionName).Select(x =>
                x.Key == "#" ? CacheFile.MakeChangeMonitors(x.Select(y => y.name)) :
                cache.CreateCacheEntryChangeMonitor(x.Select(y => y.name), x.Key)
            ).Where(x => x != null).ToList();
        }

        static bool TryGetRegion(ref string name, out string regionName)
        {
            var index = name.IndexOf(RegionMarker);
            if (index == -1)
            {
                regionName = null;
                return false;
            }
            var originalName = name;
            regionName = originalName.Substring(0, index);
            name = originalName.Substring(index + RegionMarker.Length);
            return true;
        }

        //static Task<T> GetUsingLockAsync<T>(ObjectCache cache, ServiceCacheRegistration key, object tag, object[] values)
        //{
        //    var name = key.GetNamespace(values);
        //    _rwLock.EnterReadLock();
        //    var notServiceCacheResultT = typeof(T) != typeof(ServiceCacheResult);
        //    try
        //    {
        //        var value = cache.Get(name);
        //        if (value != null)
        //            return notServiceCacheResultT && value is ServiceCacheResult ? Task.FromResult((T)((ServiceCacheResult)value).Result) : Task.FromResult((T)value);
        //        return null;
        //    }
        //    finally { _rwLock.ExitReadLock(); }
        //}

        static T AddOrGetExistingUsingLock<T>(ObjectCache cache, ServiceCacheRegistration key, object tag, object[] values)
        {
            var rwLock = key._rwLock ?? _rwLock;
            var name = key.GetNamespace(values);
            rwLock.EnterUpgradeableReadLock();
            var notServiceCacheResultT = typeof(T) != typeof(ServiceCacheResult);
            try
            {
                // double lock test
                var value = cache.Get(name);
                if (value != null)
                    return notServiceCacheResultT && value is ServiceCacheResult ? (T)((ServiceCacheResult)value).Result : (T)value;
                rwLock.EnterWriteLock();
                try
                {
                    value = cache.Get(name);
                    if (value != null)
                        return notServiceCacheResultT && value is ServiceCacheResult ? (T)((ServiceCacheResult)value).Result : (T)value;
                    // create value
                    value = key.BuilderAsync != null ? Task.Run(() => CreateValueAsync<T>(key, tag, values)).Result : CreateValue<T>(key, tag, values);
                    var itemPolicy = (key.ItemPolicy is CacheItemPolicyEx ? ((CacheItemPolicyEx)key.ItemPolicy).ToItemPolicy() : key.ItemPolicy);
                    if (key.CacheTags != null)
                    {
                        var tags = key.CacheTags(tag, values);
                        if (tags != null && tags.Any())
                            foreach (var changeMonitor in MakeChangeMonitors(cache, tag, tags))
                                itemPolicy.ChangeMonitors.Add(changeMonitor);
                    }
                    // add value
                    var valueAsResult = value is ServiceCacheResult ? (ServiceCacheResult)value : new ServiceCacheResult(value);
                    valueAsResult.WeakTag = new WeakReference(tag);
                    valueAsResult.Key = key;
                    valueAsResult.ItemPolicy = itemPolicy;
                    AddInsideLock(cache, name, valueAsResult, key.RegionName);
                    return (T)value;
                }
                finally { rwLock.ExitWriteLock(); }
            }
            finally { rwLock.ExitUpgradeableReadLock(); }
        }

        static Task<T> AddOrGetExistingUsingLockAsync<T>(ObjectCache cache, ServiceCacheRegistration key, object tag, object[] values)
        {
            var rwLock = key._rwLock ?? _rwLock;
            var name = key.GetNamespace(values);
            rwLock.EnterUpgradeableReadLock();
            var notServiceCacheResultT = typeof(T) != typeof(ServiceCacheResult);
            try
            {
                // double lock test
                var value = cache.Get(name);
                if (value != null)
                    return notServiceCacheResultT && value is ServiceCacheResult ? Task.FromResult((T)((ServiceCacheResult)value).Result) : Task.FromResult((T)value);
                rwLock.EnterWriteLock();
                try
                {
                    value = cache.Get(name);
                    if (value != null)
                        return notServiceCacheResultT && value is ServiceCacheResult ? Task.FromResult((T)((ServiceCacheResult)value).Result) : Task.FromResult((T)value);
                    // create value
                    value = key.BuilderAsync != null ? Task.Run(() => CreateValueAsync<T>(key, tag, values)).Result : CreateValue<T>(key, tag, values);
                    var itemPolicy = (key.ItemPolicy is CacheItemPolicyEx ? ((CacheItemPolicyEx)key.ItemPolicy).ToItemPolicy() : key.ItemPolicy);
                    if (key.CacheTags != null)
                    {
                        var tags = key.CacheTags(tag, values);
                        if (tags != null && tags.Any())
                            foreach (var changeMonitor in MakeChangeMonitors(cache, tag, tags))
                                itemPolicy.ChangeMonitors.Add(changeMonitor);
                    }
                    // add value
                    var valueAsResult = value is ServiceCacheResult ? (ServiceCacheResult)value : new ServiceCacheResult(value);
                    valueAsResult.WeakTag = new WeakReference(tag);
                    valueAsResult.Key = key;
                    valueAsResult.ItemPolicy = itemPolicy;
                    AddInsideLock(cache, name, valueAsResult, key.RegionName);
                    return Task.FromResult((T)value);
                }
                finally { rwLock.ExitWriteLock(); }
            }
            finally { rwLock.ExitUpgradeableReadLock(); }
        }

        static void AddInsideLock(ObjectCache cache, string name, ServiceCacheResult value, string regionName)
        {
            try { cache.Add(name, value, value.ItemPolicy, regionName); }
            catch (InvalidOperationException) { }
            catch (Exception e) { Console.WriteLine(e); }
            finally
            {
                if (!string.IsNullOrEmpty(value.ETag))
                {
                    var etagname = value.Key.GetNamespace(new[] { value.ETag });
                    var etagItemPolicy = new CacheItemPolicy { AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration };
                    etagItemPolicy.ChangeMonitors.Add(cache.CreateCacheEntryChangeMonitor(new[] { name }));
                    // ensure base is still exists, then add
                    var baseValue = cache.Get(name);
                    if (baseValue != null)
                        cache.Add(etagname, name, etagItemPolicy);
                }
            }
        }

        static T CreateValue<T>(ServiceCacheRegistration key, object tag, object[] values) =>
            (T)key.Builder(tag, values);

        static async Task<T> CreateValueAsync<T>(ServiceCacheRegistration key, object tag, object[] values) =>
            (T)await key.BuilderAsync(tag, values);
    }
}
