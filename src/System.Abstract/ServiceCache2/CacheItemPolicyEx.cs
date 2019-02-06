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

using System.Runtime.Caching;

namespace System.Abstract
{
    /// <summary>
    /// Class CacheItemPolicyEx.
    /// </summary>
    /// <seealso cref="System.Runtime.Caching.CacheItemPolicy" />
    public class CacheItemPolicyEx : CacheItemPolicy
    {
        /// <summary>
        /// Default
        /// </summary>
        public static readonly CacheItemPolicy Default = new CacheItemPolicy { };
        /// <summary>
        /// Infinite
        /// </summary>
        public static readonly CacheItemPolicy Infinite = new CacheItemPolicy(-1);

        DateTimeOffset _absoluteExpiration;
        TimeSpan _floatingAbsoluteExpiration;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheItemPolicyEx"/> class.
        /// </summary>
        public CacheItemPolicyEx() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheItemPolicyEx"/> class.
        /// </summary>
        /// <param name="floatingAbsoluteMinuteTimeout">The floating absolute minute timeout.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">floatingMinuteTimeout</exception>
        public CacheItemPolicyEx(int floatingAbsoluteMinuteTimeout)
        {
            if (floatingAbsoluteMinuteTimeout < -1)
                throw new ArgumentOutOfRangeException("floatingMinuteTimeout");
            if (floatingAbsoluteMinuteTimeout >= 0)
                _floatingAbsoluteExpiration = new TimeSpan(0, floatingAbsoluteMinuteTimeout, 0);
            else
                _absoluteExpiration = DateTimeOffset.MaxValue;
        }

        /// <summary>
        /// Gets or sets a value that indicates whether a cache entry should be evicted after a specified duration.
        /// </summary>
        /// <value>The absolute expiration.</value>
        /// <exception cref="System.InvalidOperationException">FloatingExpiration already set</exception>
        public new DateTimeOffset AbsoluteExpiration
        {
            get
            {
                if (SlidingExpiration != TimeSpan.Zero)
                    return DateTimeOffset.MinValue;
                if (_absoluteExpiration == DateTime.MinValue && _floatingAbsoluteExpiration == TimeSpan.Zero)
                    return DateTimeOffset.Now.Add(new TimeSpan(1, 0, 0));
                return _floatingAbsoluteExpiration != TimeSpan.Zero ? DateTimeOffset.Now.Add(_floatingAbsoluteExpiration) : _absoluteExpiration;
            }
            set
            {
                if (_floatingAbsoluteExpiration != TimeSpan.Zero)
                    throw new InvalidOperationException("FloatingExpiration already set");
                _absoluteExpiration = value;
            }
        }

        /// <summary>
        /// Gets or sets the DateTime instance that represent the absolute expiration of the item being added to cache.
        /// </summary>
        /// <value>The absolute expiration.</value>
        public TimeSpan FloatingAbsoluteExpiration
        {
            get
            {
                if (SlidingExpiration != TimeSpan.Zero)
                    return TimeSpan.Zero;
                return _floatingAbsoluteExpiration;
            }
            set
            {
                if (_absoluteExpiration != DateTime.MinValue)
                    throw new InvalidOperationException("AbsoluteExpiration already set");
                if (value < TimeSpan.Zero)
                    throw new ArgumentOutOfRangeException(nameof(value));
                _floatingAbsoluteExpiration = value;
            }
        }

        /// <summary>
        /// To the item policy.
        /// </summary>
        /// <returns>CacheItemPolicy.</returns>
        public CacheItemPolicy ToItemPolicy()
        {
            if (_absoluteExpiration == DateTimeOffset.MaxValue)
                return this;
            var r = new CacheItemPolicy
            {
                AbsoluteExpiration = AbsoluteExpiration,
                Priority = Priority,
                RemovedCallback = base.RemovedCallback,
                SlidingExpiration = SlidingExpiration,
                UpdateCallback = UpdateCallback,
            };
            foreach (var x in ChangeMonitors)
                r.ChangeMonitors.Add(x);
            return r;
        }

        Action<object, CacheEntryRemovedArguments> _removedCallback;
        /// <summary>
        /// Gets or sets a reference to a <see cref="T:System.Runtime.Caching.CacheEntryRemovedCallback" /> delegate that is called after an entry is removed from the cache.
        /// </summary>
        /// <value>The removed callback.</value>
        public new Action<object, CacheEntryRemovedArguments> RemovedCallback
        {
            get => _removedCallback;
            set
            {
                _removedCallback = value;
                if (value != null) base.RemovedCallback = args =>
                    value(args.CacheItem.Value is ServiceCacheResult ? ((ServiceCacheResult)args.CacheItem.Value).WeakTag.Target : null, args);
                else base.RemovedCallback = null;
            }
        }
    }
}
