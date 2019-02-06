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
    /// ServiceMapExtensions
    /// </summary>
    static partial class AbstractExtensions
    {
        #region BehaveAs

        /// <summary>
        /// Behaves as.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service">The cache.</param>
        /// <returns>T.</returns>
        public static T BehaveAs<T>(this IServiceMap service)
            where T : class, IServiceMap
        {
            IServiceWrapper<IServiceMap> serviceWrapper;
            do
            {
                serviceWrapper = service as IServiceWrapper<IServiceMap>;
                if (serviceWrapper != null)
                    service = serviceWrapper.Base;
            } while (serviceWrapper != null);
            return service as T;
        }

        #endregion

        #region Lazy Setup

        /// <summary>
        /// Registers the with service locator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service">The service.</param>
        /// <param name="name">The name.</param>
        /// <returns>Lazy&lt;IServiceMap&gt;.</returns>
        public static Lazy<IServiceMap> RegisterWithServiceLocator<T>(this Lazy<IServiceMap> service, string name = null)
            where T : class, IServiceMap
        { ServiceMapManager.GetSetupDescriptor(service).RegisterWithServiceLocator<T>(service, name, ServiceLocatorManager.Current); return service; }
        /// <summary>
        /// Registers the with service locator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service">The service.</param>
        /// <param name="locator">The locator.</param>
        /// <param name="name">The name.</param>
        /// <returns>Lazy&lt;IServiceMap&gt;.</returns>
        public static Lazy<IServiceMap> RegisterWithServiceLocator<T>(this Lazy<IServiceMap> service, IServiceLocator locator = null, string name = null)
            where T : class, IServiceMap
        { ServiceMapManager.GetSetupDescriptor(service).RegisterWithServiceLocator<T>(service, name, locator); return service; }
        /// <summary>
        /// Registers the with service locator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service">The service.</param>
        /// <param name="locator">The locator.</param>
        /// <param name="name">The name.</param>
        /// <returns>Lazy&lt;IServiceMap&gt;.</returns>
        public static Lazy<IServiceMap> RegisterWithServiceLocator<T>(this Lazy<IServiceMap> service, Lazy<IServiceLocator> locator = null, string name = null)
            where T : class, IServiceMap
        { ServiceMapManager.GetSetupDescriptor(service).RegisterWithServiceLocator<T>(service, name, locator); return service; }

        /// <summary>
        /// Registers the with service locator.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <returns>Lazy&lt;IServiceMap&gt;.</returns>
        public static Lazy<IServiceMap> RegisterWithServiceLocator(this Lazy<IServiceMap> service, Type serviceType, string name = null)
        { ServiceMapManager.GetSetupDescriptor(service).RegisterWithServiceLocator(service, serviceType, name, ServiceLocatorManager.Current); return service; }
        /// <summary>
        /// Registers the with service locator.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="locator">The locator.</param>
        /// <param name="name">The name.</param>
        /// <returns>Lazy&lt;IServiceMap&gt;.</returns>
        public static Lazy<IServiceMap> RegisterWithServiceLocator(this Lazy<IServiceMap> service, Type serviceType, IServiceLocator locator = null, string name = null)
        { ServiceMapManager.GetSetupDescriptor(service).RegisterWithServiceLocator(service, serviceType, name, locator); return service; }
        /// <summary>
        /// Registers the with service locator.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="locator">The locator.</param>
        /// <param name="name">The name.</param>
        /// <returns>Lazy&lt;IServiceMap&gt;.</returns>
        public static Lazy<IServiceMap> RegisterWithServiceLocator(this Lazy<IServiceMap> service, Type serviceType, Lazy<IServiceLocator> locator = null, string name = null)
        { ServiceMapManager.GetSetupDescriptor(service).RegisterWithServiceLocator(service, serviceType, name, locator); return service; }

        #endregion
    }
}
