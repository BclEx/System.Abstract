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
    /// Class ServiceCacheResult.
    /// </summary>
    public class ServiceCacheResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCacheResult"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        public ServiceCacheResult(object result) => Result = result;

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value>The result.</value>
        public object Result { get; private set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public ServiceCacheRegistration Key { get; set; }

        /// <summary>
        /// Gets or sets the item policy.
        /// </summary>
        /// <value>The item policy.</value>
        public CacheItemPolicy ItemPolicy { get; set; }

        /// <summary>
        /// Gets or sets the weak tag.
        /// </summary>
        /// <value>The weak tag.</value>
        public WeakReference WeakTag { get; set; }
        
        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>The tag.</value>
        public object Tag { get; set; }

        /// <summary>
        /// Gets or sets the e tag.
        /// </summary>
        /// <value>The e tag.</value>
        public string ETag { get; set; }
    }
}
