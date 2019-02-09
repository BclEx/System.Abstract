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

using System.Abstract.Internal;

namespace System.Abstract
{
    /// <summary>
    /// ServiceLogManager
    /// </summary>
    public class ServiceLogManager : ServiceManagerBase<IServiceLog, ServiceLogManager, ServiceLogManagerLogger>
    {
        #region EmptyServiceLog

        /// <summary>
        /// EmptyServiceLog
        /// </summary>
        public class EmptyServiceLog : IServiceLog
        {
            /// <summary>
            /// Gets the service object of the specified type.
            /// </summary>
            /// <param name="serviceType">An object that specifies the type of service object to get.</param>
            /// <returns>A service object of type <paramref name="serviceType" />.
            /// -or-
            /// null if there is no service object of type <paramref name="serviceType" />.</returns>
            /// <exception cref="NotImplementedException"></exception>
            public object GetService(Type serviceType) => throw new NotImplementedException();

            // get
            /// <summary>
            /// Gets the name.
            /// </summary>
            /// <value>The name.</value>
            public string Name => null;
            /// <summary>
            /// Gets the specified name.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns>IServiceLog.</returns>
            public IServiceLog Get(string name) => this;
            /// <summary>
            /// Gets the specified name.
            /// </summary>
            /// <param name="type">The type.</param>
            /// <returns>IServiceLog.</returns>
            public IServiceLog Get(Type type) => this;

            // log
            /// <summary>
            /// Writes the specified level.
            /// </summary>
            /// <param name="level">The level.</param>
            /// <param name="ex">The ex.</param>
            /// <param name="s">The s.</param>
            public void Write(ServiceLogLevel level, Exception ex, string s) { }
        }

        #endregion

        /// <summary>
        /// Empty
        /// </summary>
        public static readonly IServiceLog Empty = new EmptyServiceLog();

        /// <summary>
        /// EmptyLazy
        /// </summary>
        public static readonly Lazy<IServiceLog> EmptyLazy = new Lazy<IServiceLog>(() => Empty);

        static ServiceLogManager() =>
            Registration = new ServiceRegistration { };

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IServiceLog Get<T>() =>
            (Lazy ?? EmptyLazy).Value.Get<T>();
        /// <summary>
        /// Gets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static IServiceLog Get(string name) =>
            (Lazy ?? EmptyLazy).Value.Get(name); 
    }
}
