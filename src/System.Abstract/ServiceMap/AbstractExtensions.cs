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
        /// <returns></returns>
        public static T BehaveAs<T>(this IServiceMap service)
            where T : class, IServiceMap
        {
            IServiceWrapper<IServiceMap> serviceWrapper;
            do
            {
                serviceWrapper = (service as IServiceWrapper<IServiceMap>);
                if (serviceWrapper != null)
                    service = serviceWrapper.Parent;
            } while (serviceWrapper != null);
            return (service as T);
        }

        #endregion

        #region Lazy Setup

        public static Lazy<IServiceMap> RegisterWithServiceLocator<T>(this Lazy<IServiceMap> service, string name = null, IServiceLocator locator = null)
            where T : class, IServiceMap { ServiceMapManager.GetSetupDescriptor(service).RegisterWithServiceLocator<T>(service, name, locator); return service; }
        public static Lazy<IServiceMap> RegisterWithServiceLocator(this Lazy<IServiceMap> service, Type serviceType, string name = null, IServiceLocator locator = null) { ServiceMapManager.GetSetupDescriptor(service).RegisterWithServiceLocator(service, serviceType, name, locator); return service; }

        #endregion
    }
}
