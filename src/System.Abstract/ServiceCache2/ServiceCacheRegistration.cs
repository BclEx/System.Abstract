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

using System.Diagnostics;
using System.Runtime.Caching;
using System.Text;
using System.Threading;

namespace System.Abstract
{
    /// <summary>
    /// Class ServiceCacheRegistration.
    /// </summary>
    public class ServiceCacheRegistration
    {
        internal readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCacheRegistration"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="builder">The builder.</param>
        /// <param name="cacheTags">The cache tags.</param>
        public ServiceCacheRegistration(string name, CacheItemBuilder builder, params string[] cacheTags)
            : this(new StackTrace(), name, null, builder, null, cacheTags != null && cacheTags.Length > 0 ? (a, b) => cacheTags : (Func<object, object[], string[]>)null) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCacheRegistration"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="itemPolicy">The item policy.</param>
        /// <param name="builder">The builder.</param>
        /// <param name="cacheTags">The cache tags.</param>
        public ServiceCacheRegistration(string name, CacheItemPolicy itemPolicy, CacheItemBuilder builder, params string[] cacheTags)
            : this(new StackTrace(), name, itemPolicy, builder, null, cacheTags != null && cacheTags.Length > 0 ? (a, b) => cacheTags : (Func<object, object[], string[]>)null) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCacheRegistration"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="builder">The builder.</param>
        /// <param name="cacheTags">The cache tags.</param>
        public ServiceCacheRegistration(string name, CacheItemBuilder builder, Func<object, object[], string[]> cacheTags)
            : this(new StackTrace(), name, null, builder, null, cacheTags) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCacheRegistration"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="minuteTimeout">The minute timeout.</param>
        /// <param name="builder">The builder.</param>
        /// <param name="cacheTags">The cache tags.</param>
        public ServiceCacheRegistration(string name, int minuteTimeout, CacheItemBuilder builder, params string[] cacheTags)
            : this(new StackTrace(), name, new CacheItemPolicyEx(minuteTimeout), builder, null, cacheTags != null && cacheTags.Length > 0 ? (a, b) => cacheTags : (Func<object, object[], string[]>)null) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCacheRegistration"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="minuteTimeout">The minute timeout.</param>
        /// <param name="builder">The builder.</param>
        /// <param name="cacheTags">The cache tags.</param>
        public ServiceCacheRegistration(string name, int minuteTimeout, CacheItemBuilder builder, Func<object, object[], string[]> cacheTags)
            : this(new StackTrace(), name, new CacheItemPolicyEx(minuteTimeout), builder, null, cacheTags) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCacheRegistration"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="itemPolicy">The item policy.</param>
        /// <param name="builder">The builder.</param>
        /// <param name="cacheTags">The cache tags.</param>
        public ServiceCacheRegistration(string name, CacheItemPolicy itemPolicy, CacheItemBuilder builder, Func<object, object[], string[]> cacheTags)
            : this(new StackTrace(), name, itemPolicy, builder, null, cacheTags) { }
        //
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCacheRegistration"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="builder">The builder.</param>
        /// <param name="cacheTags">The cache tags.</param>
        public ServiceCacheRegistration(string name, CacheItemBuilderAsync builder, params string[] cacheTags)
            : this(new StackTrace(), name, null, null, builder, cacheTags != null && cacheTags.Length > 0 ? (a, b) => cacheTags : (Func<object, object[], string[]>)null) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCacheRegistration"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="itemPolicy">The item policy.</param>
        /// <param name="builder">The builder.</param>
        /// <param name="cacheTags">The cache tags.</param>
        public ServiceCacheRegistration(string name, CacheItemPolicy itemPolicy, CacheItemBuilderAsync builder, params string[] cacheTags)
            : this(new StackTrace(), name, itemPolicy, null, builder, cacheTags != null && cacheTags.Length > 0 ? (a, b) => cacheTags : (Func<object, object[], string[]>)null) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCacheRegistration"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="builder">The builder.</param>
        /// <param name="cacheTags">The cache tags.</param>
        public ServiceCacheRegistration(string name, CacheItemBuilderAsync builder, Func<object, object[], string[]> cacheTags)
            : this(new StackTrace(), name, null, null, builder, cacheTags) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCacheRegistration"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="minuteTimeout">The minute timeout.</param>
        /// <param name="builder">The builder.</param>
        /// <param name="cacheTags">The cache tags.</param>
        public ServiceCacheRegistration(string name, int minuteTimeout, CacheItemBuilderAsync builder, params string[] cacheTags)
            : this(new StackTrace(), name, new CacheItemPolicyEx(minuteTimeout), null, builder, cacheTags != null && cacheTags.Length > 0 ? (a, b) => cacheTags : (Func<object, object[], string[]>)null) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCacheRegistration"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="minuteTimeout">The minute timeout.</param>
        /// <param name="builder">The builder.</param>
        /// <param name="cacheTags">The cache tags.</param>
        public ServiceCacheRegistration(string name, int minuteTimeout, CacheItemBuilderAsync builder, Func<object, object[], string[]> cacheTags)
            : this(new StackTrace(), name, new CacheItemPolicyEx(minuteTimeout), null, builder, cacheTags) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCacheRegistration"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="itemPolicy">The item policy.</param>
        /// <param name="builder">The builder.</param>
        /// <param name="cacheTags">The cache tags.</param>
        public ServiceCacheRegistration(string name, CacheItemPolicy itemPolicy, CacheItemBuilderAsync builder, Func<object, object[], string[]> cacheTags)
            : this(new StackTrace(), name, itemPolicy, null, builder, cacheTags) { }
        ServiceCacheRegistration(StackTrace stackTrace, string name, CacheItemPolicy itemPolicy, CacheItemBuilder builder, CacheItemBuilderAsync builderAsync, Func<object, object[], string[]> cacheTags)
        {
            if (builder == null && builderAsync == null)
                throw new ArgumentNullException(nameof(builder));
            var parentName = stackTrace.GetFrame(1).GetMethod().DeclaringType.FullName;
            Name = parentName + ":" + name;
            ItemPolicy = itemPolicy ?? new CacheItemPolicyEx(90);
            Builder = builder;
            BuilderAsync = builderAsync;
            CacheTags = cacheTags;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; internal set; }
        /// <summary>
        /// Gets the name of the region.
        /// </summary>
        /// <value>The name of the region.</value>
        public string RegionName { get; internal set; }
        /// <summary>
        /// Gets the builder.
        /// </summary>
        /// <value>The builder.</value>
        public CacheItemBuilder Builder { get; private set; }
        /// <summary>
        /// Gets the builder asynchronous.
        /// </summary>
        /// <value>The builder asynchronous.</value>
        public CacheItemBuilderAsync BuilderAsync { get; private set; }
        /// <summary>
        /// Gets the item policy.
        /// </summary>
        /// <value>The item policy.</value>
        public CacheItemPolicy ItemPolicy { get; private set; }
        /// <summary>
        /// Gets the cache tags.
        /// </summary>
        /// <value>The cache tags.</value>
        public Func<object, object[], string[]> CacheTags { get; private set; }

        internal string GetNamespace(object[] values)
        {
            if (values == null || values.Length == 0)
                return Name;
            var b = new StringBuilder(Name);
            b.Append(":");
            foreach (var v in values)
            {
                if (v != null)
                    b.Append(v.ToString());
                b.Append("\\");
            }
            return b.ToString();
        }
    }
}
