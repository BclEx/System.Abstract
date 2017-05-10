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
#if !NET35
using AutoMapper.Configuration;
#endif
using System;
using System.Abstract;
using System.Globalization;

namespace AutoMapper.Abstract
{
    /// <summary>
    /// IAutoMapperServiceMap
    /// </summary>
    public interface IAutoMapperServiceMap : IServiceMap
    {
          /// <summary>
        /// Gets the configuration.
        /// </summary>
        IConfiguration Configuration { get; }
    }

    /// <summary>
    /// AutoMapperServiceMap
    /// </summary>
    public class AutoMapperServiceMap : IAutoMapperServiceMap, ServiceMapManager.ISetupRegistration
    {
        static AutoMapperServiceMap() { ServiceMapManager.EnsureRegistration(); }
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMapperServiceMap"/> class.
        /// </summary>
        public AutoMapperServiceMap()
            : this((IConfiguration)null) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMapperServiceMap"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <exception cref="System.ArgumentNullException">configuration</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">configuration;Must be of type AutoMapper.IConfiguration</exception>
        public AutoMapperServiceMap(object configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");
            Configuration = (configuration as IConfiguration);
            if (Configuration == null)
                throw new ArgumentOutOfRangeException("configuration", "Must be of type AutoMapper.IConfiguration");
        }

        Action<IServiceLocator, string> ServiceMapManager.ISetupRegistration.DefaultServiceRegistrar
        {
            get { return (locator, name) => ServiceLogManager.RegisterInstance<IAutoMapperServiceMap>(this, locator, name); }
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>
        public object GetService(Type serviceType) { throw new NotImplementedException(); }

        #region Domain-specific

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; private set; }

        #endregion
    }
}
