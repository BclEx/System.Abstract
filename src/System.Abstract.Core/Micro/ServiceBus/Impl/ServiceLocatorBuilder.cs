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

using System;
using System.Abstract;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Contoso.Micro.ServiceBus.Actions;

namespace Contoso.Micro.ServiceBus.Impl
{
    /// <summary>
    /// IServiceLocatorBuilder
    /// </summary>
    public interface IServiceLocatorBuilder
    {
        /// <summary>
        /// Registers all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">The condition.</param>
        void RegisterAll<T>(Predicate<Type> condition);
        /// <summary>
        /// Registers all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="excludes">The excludes.</param>
        void RegisterAll<T>(params Type[] excludes);
        /// <summary>
        /// Registers the bus.
        /// </summary>
        void RegisterBus();
        /// <summary>
        /// Registers the default services.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        void RegisterDefaultServices(IEnumerable<Assembly> assemblies);
        //void RegisterLoggingEndpoint(Uri logEndpoint);
    }

    internal class ServiceLocatorBuilder : IServiceLocatorBuilder
    {
        readonly AbstractMicroServiceBusConfiguration _config;
        readonly IServiceLocator _locator;
        readonly IServiceRegistrar _registrar;

        public ServiceLocatorBuilder(IServiceLocator locator, AbstractMicroServiceBusConfiguration config)
        {
            _locator = locator;
            _registrar = locator.Registrar;
            _config = config;
            _config.BuildWith(this);
        }

        public void RegisterAll<T>(Predicate<Type> condition)
        {
            _registrar.RegisterByTypeMatch<T>(condition, typeof(T).Assembly);
        }

        public void RegisterAll<T>(params Type[] excludes)
        {
            _registrar.RegisterByTypeMatch<T>(x => (!x.IsAbstract && !x.IsInterface && typeof(T).IsAssignableFrom(x) && !excludes.Contains<Type>(x)), typeof(T).Assembly);
        }

        public void RegisterBus()
        {
            _registrar.Register<IMicroDeploymentAction, CreateQueuesAction>(Guid.NewGuid().ToString());
            //_registrar.BehaveAs(ServiceRegistrarLifetime.Singleton).Register<IStartableMicroServiceBus>(l => new DefaultMicroServiceBus(l.Resolve<IServiceLocator>(), l.Resolve<ITransport>(), l.ResolveAll<IMessageModule>().ToArray(), config.MessageOwners.ToArray(), l.Resolve<IEndpointRouter>()));
            _registrar.Register<IMicroServiceBus, IStartableMicroServiceBus>();
        }

        public void RegisterDefaultServices(IEnumerable<Assembly> assemblies)
        {
            //_registrar.Register<IServiceLocator, ServiceLocatorAdapter>();
            //ServiceLocatorExtensions.RegisterByTypeMatch<IBusConfigurationAware>(_registrar, typeof(IServiceBus).Assembly);
            //foreach (var assembly in assemblies)
            //    _registrar.RegisterByTypeMatch<IBusConfigurationAware>(assembly);
            //var locator = _locator.Resolve<IServiceLocator>();
            //foreach (var aware in _locator.ResolveAll<IBusConfigurationAware>())
            //    aware.Configure(_config, this, locator);
            //foreach (var messageModule in _config.MessageModules)
            //    if (!_registrar.HasRegistered(messageModule))
            //        _registrar.Register<IMessageModule>(messageModule, messageModule.FullName);
            //_registrar.Register<IReflection, DefaultReflection>();
            //_registrar.Register<IMessageSerializer>(_config.SerializerType);
            //_registrar.Register<IEndpointRouter, EndpointRouter>();
        }

        //public void RegisterLoggingEndpoint(Uri logEndpoint)
        //{
        //    _registrar.Register<MessageLoggingModule>(l => new MessageLoggingModule(l.Resolve<IEndpointRouter>(), logEndpoint));
        //    _registrar.Register<IMicroDeploymentAction, CreateLogQueueAction>(Guid.NewGuid().ToString());
        //}

        public void RegisterSingleton<T>(Func<T> func)
            where T : class
        {
            _registrar.Register<T>(x => func());
        }

        public void RegisterSingleton<T>(string name, Func<T> func)
            where T : class
        {
            _registrar.Register<T>(x => func(), name);
        }

        //public void WithInterceptor(IConsumerInterceptor interceptor)
        //{
        //    try { _registrar.RegisterInterceptor(new ConsumerInterceptorAdapter(interceptor)); }
        //    catch { }
        //}
    }
}