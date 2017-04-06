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
    /// ServiceLogManager
    /// </summary>
    public class ServiceLogManager : ServiceManagerBase<IServiceLog, Action<IServiceLog>, ServiceLogManagerLogger>
    {
        #region EmptyServiceLog

        /// <summary>
        /// EmptyServiceLog
        /// </summary>
        internal class EmptyServiceLog : IServiceLog
        {
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

            // get
            /// <summary>
            /// Gets the name.
            /// </summary>
            public string Name
            {
                get { return null; }
            }
            /// <summary>
            /// Gets the specified name.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <returns></returns>
            public IServiceLog Get(string name) { return this; }
            /// <summary>
            /// Gets the specified name.
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            public IServiceLog Get(Type type) { return this; }

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

        static ServiceLogManager()
        {
            Registration = new ServiceRegistration
            {
                MakeAction = a => x => a(x),
                OnSetup = (service, descriptor) =>
                {
                    if (descriptor != null)
                        foreach (var action in descriptor.Actions)
                            action(service);
                    return service;
                },
                OnChange = (service, descriptor) =>
                {
                    if (descriptor != null)
                        foreach (var action in descriptor.Actions)
                            action(service);
                },
            };
            // default provider
            if (Lazy == null && DefaultServiceProvider != null)
                SetProvider(DefaultServiceProvider);
        }


        /// <summary>
        /// Sets the provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="setupDescriptor">The setup descriptor.</param>
        /// <returns></returns>
        public static Lazy<IServiceLog> SetProvider(Func<IServiceLog> provider, ISetupDescriptor setupDescriptor = null) { return (Lazy = MakeByProviderProtected(provider, setupDescriptor)); }
        /// <summary>
        /// Makes the by provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="setupDescriptor">The setup descriptor.</param>
        /// <returns></returns>
        public static Lazy<IServiceLog> MakeByProvider(Func<IServiceLog> provider, ISetupDescriptor setupDescriptor = null) { return MakeByProviderProtected(provider, setupDescriptor); }

        /// <summary>
        /// Gets the current.
        /// </summary>
        public static IServiceLog Current
        {
            get { return GetCurrent(); }
        }

        /// <summary>
        /// Ensures the registration.
        /// </summary>
        public static void EnsureRegistration() { }
        /// <summary>
        /// Gets the setup descriptor.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        public static ISetupDescriptor GetSetupDescriptor(Lazy<IServiceLog> service) { return GetSetupDescriptorProtected(service, null); }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IServiceLog Get<T>() { return (ServiceLogManager.Lazy ?? EmptyLazy).Value.Get<T>(); }
        /// <summary>
        /// Gets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static IServiceLog Get(string name) { return (ServiceLogManager.Lazy ?? EmptyLazy).Value.Get(name); }
    }
}
