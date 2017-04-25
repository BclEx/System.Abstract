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
using System.Messaging;
using Rhino.ServiceBus;
using Rhino.ServiceBus.Actions;
using Rhino.ServiceBus.Config;
using Rhino.ServiceBus.Convertors;
using Rhino.ServiceBus.DataStructures;
using Rhino.ServiceBus.Impl;
using Rhino.ServiceBus.Internal;
using Rhino.ServiceBus.LoadBalancer;
using Rhino.ServiceBus.MessageModules;
using Rhino.ServiceBus.Msmq;
using Rhino.ServiceBus.Msmq.TransportActions;
using ErrorAction = Rhino.ServiceBus.Msmq.TransportActions.ErrorAction;
using IServiceBus = Rhino.ServiceBus.IServiceBus;
using IServiceLocator = Rhino.ServiceBus.Internal.IServiceLocator;
using System.Reflection;
using System.Collections.Generic;
namespace Contoso.Abstract.RhinoServiceBus
{
    internal class ServiceLocatorBuilder : IBusContainerBuilder
    {
        private readonly AbstractRhinoServiceBusConfiguration _config;
        private readonly System.Abstract.IServiceLocator _locator;
        private readonly System.Abstract.IServiceRegistrar _registrar;

        public ServiceLocatorBuilder(System.Abstract.IServiceLocator locator, AbstractRhinoServiceBusConfiguration config)
        {
            _locator = locator;
            _registrar = locator.Registrar;
            _config = config;
            _config.BuildWith(this);
        }

        public void RegisterAll<T>(Predicate<Type> condition)
            where T : class
        {
            _registrar.RegisterByTypeMatch<T>(condition, typeof(T).Assembly);
        }

        public void RegisterAll<T>(params Type[] excludes)
            where T : class
        {
            _registrar.RegisterByTypeMatch<T>(x => (!x.IsAbstract && !x.IsInterface && typeof(T).IsAssignableFrom(x) && !excludes.Contains<Type>(x)), typeof(T).Assembly);
        }

        public void RegisterBus()
        {
            var config = (RhinoServiceBusConfiguration)_config;
            _registrar.Register<IDeploymentAction, CreateQueuesAction>(Guid.NewGuid().ToString());
            _registrar.BehaveAs(ServiceRegistrarLifetime.Singleton).Register<IStartableServiceBus>(l => new DefaultServiceBus(l.Resolve<IServiceLocator>(), l.Resolve<ITransport>(), l.Resolve<ISubscriptionStorage>(), l.Resolve<IReflection>(), l.ResolveAll<IMessageModule>().ToArray(), config.MessageOwners.ToArray(), l.Resolve<IEndpointRouter>()));
            _registrar.Register<IStartable, IStartableServiceBus>();
            _registrar.Register<IServiceBus, IStartableServiceBus>();
        }

        public void RegisterDefaultServices(IEnumerable<Assembly> assemblies)
        {
            _registrar.Register<IServiceLocator, ServiceLocatorAdapter>();
            ServiceLocatorExtensions.RegisterByTypeMatch<IBusConfigurationAware>(_registrar, typeof(IServiceBus).Assembly);
            foreach (var assembly in assemblies)
                _registrar.RegisterByTypeMatch<IBusConfigurationAware>(assembly);
            var locator = _locator.Resolve<IServiceLocator>();
            foreach (var aware in _locator.ResolveAll<IBusConfigurationAware>())
                aware.Configure(_config, this, locator);
            foreach (var messageModule in _config.MessageModules)
                if (!_registrar.HasRegistered(messageModule))
                    _registrar.Register<IMessageModule>(messageModule, messageModule.FullName);
            _registrar.Register<IReflection, DefaultReflection>();
            _registrar.Register<IMessageSerializer>(_config.SerializerType);
            _registrar.Register<IEndpointRouter, EndpointRouter>();
        }

        public void RegisterLoadBalancerEndpoint(Uri loadBalancerEndpoint)
        {
            _registrar.Register<LoadBalancerMessageModule>(l => new LoadBalancerMessageModule(loadBalancerEndpoint, l.Resolve<IEndpointRouter>()));
        }

        public void RegisterLoggingEndpoint(Uri logEndpoint)
        {
            _registrar.Register<MessageLoggingModule>(l => new MessageLoggingModule(l.Resolve<IEndpointRouter>(), logEndpoint));
            _registrar.Register<IDeploymentAction, CreateLogQueueAction>(Guid.NewGuid().ToString());
        }

        public void RegisterNoSecurity()
        {
            _registrar.Register<IValueConvertor<WireEncryptedString>, ThrowingWireEncryptedStringConvertor>();
            _registrar.Register<IElementSerializationBehavior, ThrowingWireEncryptedMessageConvertor>();
        }

        public void RegisterPrimaryLoadBalancer()
        {
            var config = (Rhino.ServiceBus.LoadBalancer.LoadBalancerConfiguration)_config;
            _registrar.Register<MsmqLoadBalancer>(l => new MsmqLoadBalancer(l.Resolve<IMessageSerializer>(), l.Resolve<IQueueStrategy>(), l.Resolve<IEndpointRouter>(), config.Endpoint, config.ThreadCount, config.SecondaryLoadBalancer, config.Transactional, l.Resolve<IMessageBuilder<Message>>()) { ReadyForWorkListener = l.Resolve<MsmqReadyForWorkListener>() });
            _registrar.Register<IStartable, MsmqLoadBalancer>();
            _registrar.Register<IDeploymentAction, CreateLoadBalancerQueuesAction>(Guid.NewGuid().ToString());
        }

        public void RegisterReadyForWork()
        {
            var config = (Rhino.ServiceBus.LoadBalancer.LoadBalancerConfiguration)_config;
            _registrar.Register<MsmqReadyForWorkListener>(l => new MsmqReadyForWorkListener(l.Resolve<IQueueStrategy>(), config.ReadyForWork, config.ThreadCount, l.Resolve<IMessageSerializer>(), l.Resolve<IEndpointRouter>(), config.Transactional, l.Resolve<IMessageBuilder<Message>>()));
            _registrar.Register<IDeploymentAction, CreateReadyForWorkQueuesAction>(Guid.NewGuid().ToString());
        }

        public void RegisterSecondaryLoadBalancer()
        {
            var config = (Rhino.ServiceBus.LoadBalancer.LoadBalancerConfiguration)_config;
            _registrar.Register<MsmqSecondaryLoadBalancer>(l => new MsmqSecondaryLoadBalancer(l.Resolve<IMessageSerializer>(), l.Resolve<IQueueStrategy>(), l.Resolve<IEndpointRouter>(), config.Endpoint, config.PrimaryLoadBalancer, config.ThreadCount, config.Transactional, l.Resolve<IMessageBuilder<Message>>()));
            _registrar.Register<IStartable, MsmqSecondaryLoadBalancer>();
            _registrar.Register<IDeploymentAction, CreateLoadBalancerQueuesAction>(Guid.NewGuid().ToString());
        }

        public void RegisterSecurity(byte[] key)
        {
            _registrar.Register<IEncryptionService>(l => new RijndaelEncryptionService(key), "esb.security");
            _registrar.Register<IValueConvertor<WireEncryptedString>>(l => new WireEncryptedStringConvertor(l.Resolve<IEncryptionService>("esb.security")));
            _registrar.Register<IElementSerializationBehavior>(l => new WireEncryptedMessageConvertor(l.Resolve<IEncryptionService>("esb.security")));
        }

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

        public void WithInterceptor(IConsumerInterceptor interceptor)
        {
            try { _registrar.RegisterInterceptor(new ConsumerInterceptorAdapter(interceptor)); }
            catch { }
        }
    }
}