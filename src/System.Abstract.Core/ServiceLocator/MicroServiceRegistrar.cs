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
using System.Collections.Generic;
using System.Linq;
using Trampoline = Contoso.Abstract.MicroServiceLocator.Trampoline;

namespace Contoso.Abstract
{
    /// <summary>
    /// IMicroServiceRegistrar
    /// </summary>
    /// <seealso cref="System.Abstract.IServiceRegistrar" />
    public interface IMicroServiceRegistrar : IServiceRegistrar { }

    /// <summary>
    /// MicroServiceRegistrar
    /// </summary>
    /// <seealso cref="Contoso.Abstract.IMicroServiceRegistrar" />
    /// <seealso cref="System.ICloneable" />
    /// <seealso cref="System.Abstract.IServiceRegistrarBehaviorAccessor" />
    internal class MicroServiceRegistrar : IMicroServiceRegistrar, ICloneable, IServiceRegistrarBehaviorAccessor
    {
        MicroServiceLocator _parent;
        IDictionary<string, IDictionary<Type, object>> _containers;
        //IList<IServiceLocatorInterceptor> _interceptors;

        public MicroServiceRegistrar(MicroServiceLocator parent, IDictionary<string, IDictionary<Type, object>> container)
        {
            _parent = parent;
            _containers = container;
            LifetimeForRegisters = ServiceRegistrarLifetime.Transient;
        }

        object ICloneable.Clone() =>
            MemberwiseClone();

        // locator
        public IServiceLocator Locator =>
            _parent;

        // enumerate
        public bool HasRegistered<TService>() =>
            HasRegistered(typeof(TService));
        public bool HasRegistered(Type serviceType) =>
            _containers.Any(x => x.Value.Any(y => y.Key == serviceType));
        public IEnumerable<ServiceRegistration> GetRegistrationsFor(Type serviceType) =>
            _containers.SelectMany(x => x.Value, (a, b) => new { Name = a.Key, Services = b })
            .Where(x => serviceType.IsAssignableFrom(x.Services.Key))
            .Select(x => new ServiceRegistration { ServiceType = x.Services.Key, ImplementationType = x.Services.Value.GetType(), Name = x.Name });
        public IEnumerable<ServiceRegistration> Registrations =>
            _containers.SelectMany(x => x.Value, (a, b) => new { Name = a.Key, Services = b })
            .Select(x => new ServiceRegistration { ServiceType = x.Services.Key, ImplementationType = x.Services.Value.GetType(), Name = x.Name });

        // register type
        public ServiceRegistrarLifetime LifetimeForRegisters { get; private set; }
        public void Register(Type serviceType) =>
            RegisterInternal(serviceType, SetLifestyle(new Trampoline { Type = serviceType }), string.Empty);
        public void Register(Type serviceType, string name) =>
            RegisterInternal(serviceType, SetLifestyle(new Trampoline { Type = serviceType }), name);


        // register implementation
        public void Register<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService =>
            RegisterInternal(typeof(TService), SetLifestyle(new Trampoline { Type = typeof(TImplementation) }), string.Empty);
        public void Register<TService, TImplementation>(string name)
            where TService : class
            where TImplementation : class, TService =>
            RegisterInternal(typeof(TService), SetLifestyle(new Trampoline { Type = typeof(TImplementation) }), name);
        public void Register<TService>(Type implementationType)
           where TService : class =>
            RegisterInternal(typeof(TService), SetLifestyle(new Trampoline { Type = implementationType }), string.Empty);
        public void Register<TService>(Type implementationType, string name)
           where TService : class =>
            RegisterInternal(typeof(TService), SetLifestyle(new Trampoline { Type = implementationType }), name);
        public void Register(Type serviceType, Type implementationType) =>
            RegisterInternal(serviceType, SetLifestyle(new Trampoline { Type = implementationType }), string.Empty);
        public void Register(Type serviceType, Type implementationType, string name) =>
            RegisterInternal(serviceType, SetLifestyle(new Trampoline { Type = implementationType }), name);


        // register instance
        public void RegisterInstance<TService>(TService instance)
            where TService : class
        { EnsureTransientLifestyle(); RegisterInternal(typeof(TService), instance, string.Empty); }
        public void RegisterInstance<TService>(TService instance, string name)
            where TService : class
        { EnsureTransientLifestyle(); RegisterInternal(typeof(TService), instance, name); }
        public void RegisterInstance(Type serviceType, object instance)
        { EnsureTransientLifestyle(); RegisterInternal(serviceType, instance, string.Empty); }
        public void RegisterInstance(Type serviceType, object instance, string name)
        { EnsureTransientLifestyle(); RegisterInternal(serviceType, instance, name); }

        // register method
        public void Register<TService>(Func<IServiceLocator, TService> factoryMethod)
            where TService : class =>
            RegisterInternal(typeof(TService), SetLifestyle(new Trampoline { Factory = l => factoryMethod(l) }), string.Empty);
        public void Register<TService>(Func<IServiceLocator, TService> factoryMethod, string name)
            where TService : class =>
            RegisterInternal(typeof(TService), SetLifestyle(new Trampoline { Factory = l => factoryMethod(l) }), name);
        public void Register(Type serviceType, Func<IServiceLocator, object> factoryMethod) =>
            RegisterInternal(serviceType, SetLifestyle(new Trampoline { Factory = l => factoryMethod(l) }), string.Empty);
        public void Register(Type serviceType, Func<IServiceLocator, object> factoryMethod, string name) =>
            RegisterInternal(serviceType, SetLifestyle(new Trampoline { Factory = l => factoryMethod(l) }), name);

        // interceptor
        public void RegisterInterceptor(IServiceLocatorInterceptor interceptor)
        {
            throw new NotSupportedException();
            //if (_interceptors == null)
            //    _interceptors = new List<IServiceLocatorInterceptor>();
            //_interceptors.Add(interceptor);
        }

        #region Behavior

        bool IServiceRegistrarBehaviorAccessor.RegisterInLocator =>
            true;

        ServiceRegistrarLifetime IServiceRegistrarBehaviorAccessor.Lifetime
        {
            get => LifetimeForRegisters;
            set => LifetimeForRegisters = value;
        }

        #endregion

        void RegisterInternal(Type serviceType, object concrete, string name)
        {
            if (!_containers.TryGetValue(name ?? string.Empty, out var container))
                _containers[name ?? string.Empty] = container = new Dictionary<Type, object>();
            container[serviceType] = concrete;
        }

        void EnsureTransientLifestyle()
        {
            if (LifetimeForRegisters != ServiceRegistrarLifetime.Transient)
                throw new NotSupportedException();
        }

        Trampoline SetLifestyle(Trampoline t)
        {
            // must cast to IServiceRegistrar for behavior wrappers
            switch (LifetimeForRegisters)
            {
                case ServiceRegistrarLifetime.Transient: break; // b.InstancePerDependency();
                case ServiceRegistrarLifetime.Singleton: t.AsSingleton = true; break;
                default: throw new NotSupportedException();
            }
            return t;
        }
    }
}

