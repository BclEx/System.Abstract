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

namespace System.Abstract
{
    /// <summary>
    /// ServiceCacheSettings
    /// </summary>
    public class ServiceCacheSettings
    {
        /// <summary>
        /// ServiceCacheOptions
        /// </summary>
        [Flags]
        public enum ServiceCacheOptions
        {
            /// <summary>
            /// ReturnsCachedValueOnRemove
            /// </summary>
            ReturnsCachedValueOnRemove = 0x01,
            /// <summary>
            /// UseDBNullWithRegistrations
            /// </summary>
            UseDBNullWithRegistrations = 0x02,
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCacheSettings" /> class.
        /// </summary>
		public ServiceCacheSettings()
        {
            RegionMarker = "@";
            //Dispatcher = new DefaultServiceCacheRegistrationDispatcher();
            Options = ServiceCacheOptions.UseDBNullWithRegistrations;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCacheSettings" /> class.
        /// </summary>
        /// <param name="touchable">The touchable.</param>
		public ServiceCacheSettings(ITouchableCacheItem touchable)
            : this() =>
            Touchable = touchable;

        /// <summary>
        /// Gets or sets the RegionMarker.
        /// </summary>
        /// <value>The region marker.</value>
        public string RegionMarker { get; set; }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>The options.</value>
        public ServiceCacheOptions Options { get; set; }

        /// <summary>
        /// Gets or sets the registration dispatcher.
        /// </summary>
        /// <value>The registration dispatcher.</value>
        public IServiceCacheDispatcher Dispatcher { get; set; }

        /// <summary>
        /// Gets or sets the touchable.
        /// </summary>
        /// <value>The touchable.</value>
        public ITouchableCacheItem Touchable { get; set; }
    }
}
