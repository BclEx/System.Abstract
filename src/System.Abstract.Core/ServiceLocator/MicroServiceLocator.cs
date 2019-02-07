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

namespace Contoso.Abstract
{
    /// <summary>
    /// IMicroServiceLocator
    /// </summary>
    /// <seealso cref="System.Abstract.IServiceLocator" />
    public interface IMicroServiceLocator : IServiceLocator
    {
        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        IDictionary<string, IDictionary<Type, object>> Container { get; }
    }

    /// <summary>
    /// MicroServiceLocator
    /// </summary>
    /// <seealso cref="Contoso.Abstract.IMicroServiceLocator" />
    public class MicroServiceLocator : IMicroServiceLocator, ServiceLocatorManager.IRegisterWithLocator
    {
        IDictionary<string, IDictionary<Type, object>> _containers;
        MicroServiceRegistrar _registrar;

        // used so Type is not a reserved value
        internal class Trampoline { public bool AsSingleton; public Type Type; public Func<IServiceLocator, object> Factory; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MicroServiceLocator" /> class.
        /// </summary>
        public MicroServiceLocator()
            : this(new Dictionary<Type, object>()) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="MicroServiceLocator" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <exception cref="ArgumentNullException">container</exception>
        public MicroServiceLocator(IDictionary<Type, object> container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));
            Container = new Dictionary<string, IDictionary<Type, object>> {
                {string.Empty, container}
            };
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MicroServiceLocator" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <exception cref="ArgumentNullException">container</exception>
        public MicroServiceLocator(IDictionary<string, IDictionary<Type, object>> container) =>
            Container = container ?? throw new ArgumentNullException(nameof(container));

        Action<IServiceLocator, string> ServiceLocatorManager.IRegisterWithLocator.RegisterWithLocator =>
            (locator, name) => ServiceLocatorManager.RegisterInstance<IMicroServiceLocator>(this, name, locator);

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type <paramref name="serviceType" />.
        /// -or-
        /// null if there is no service object of type <paramref name="serviceType" />.</returns>
        public object GetService(Type serviceType) =>
            Resolve(serviceType);

        /// <summary>
        /// Creates the child.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns>IServiceLocator.</returns>
        /// <exception cref="NotSupportedException"></exception>
        public IServiceLocator CreateChild(object tag) =>
            throw new NotSupportedException();

        /// <summary>
        /// Gets the underlying container.
        /// </summary>
        /// <typeparam name="TContainer">The type of the container.</typeparam>
        /// <returns>TContainer.</returns>
        public TContainer GetUnderlyingContainer<TContainer>()
            where TContainer : class =>
            _containers as TContainer;

        // registrar
        /// <summary>
        /// Gets the registrar.
        /// </summary>
        /// <value>The registrar.</value>
        public IServiceRegistrar Registrar =>
            _registrar;

        // resolve
        /// <summary>
        /// Resolves this instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>TService.</returns>
        public TService Resolve<TService>()
            where TService : class =>
            (TService)Resolve(typeof(TService), string.Empty);
        /// <summary>
        /// Resolves the specified name.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="name">The name.</param>
        /// <returns>TService.</returns>
        public TService Resolve<TService>(string name)
            where TService : class =>
            (TService)Resolve(typeof(TService), name);
        /// <summary>
        /// Resolves the specified service type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>System.Object.</returns>
        public object Resolve(Type serviceType) =>
            Resolve(serviceType, string.Empty);
        /// <summary>
        /// Resolves the specified service type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ServiceLocatorResolutionException"></exception>
        public object Resolve(Type serviceType, string name)
        {
            try
            {
                if (!_containers.TryGetValue(name ?? string.Empty, out var container))
                    throw new ArgumentOutOfRangeException($"Could not resolve implementation for [{name ?? "+"}-{serviceType}]");
                // if not registered, then use requested type
                if (!container.TryGetValue(serviceType, out var concrete))
                {
                    if (serviceType.IsInterface)
                        throw new ArgumentOutOfRangeException($"Anonymous registrations for [{name ?? "+"}-{serviceType}] can not be an interface.");
                    concrete = new Trampoline { Type = serviceType };
                }
                // register as null for default constructor
                if (concrete == null)
                    return Activator.CreateInstance(serviceType);
                // try factory
                var factory = concrete as Func<IServiceLocator, object>;
                if (factory != null)
                    return factory(this);
                // try trampoline, then register as factory|singleton
                var trampoline = concrete as Trampoline;
                if (trampoline != null)
                {
                    var trampolineAsType = trampoline.Type;
                    if (trampolineAsType != null)
                    {
                        if (!trampolineAsType.IsInterface)
                        {
                            var constructorInfo = trampolineAsType.GetConstructors()
                                .FirstOrDefault(constructor => constructor.GetParameters().Length > 0);
                            if (constructorInfo == null)
                                factory = l => Activator.CreateInstance(trampolineAsType);
                            else
                            {
                                var args = constructorInfo.GetParameters()
                                    .Select(arg => Resolve(arg.ParameterType))
                                    .ToArray();
                                factory = l => Activator.CreateInstance(trampolineAsType, args);
                            }
                        }
                        else
                            factory = l => Resolve(trampolineAsType, name);
                    }
                    else if ((factory = trampoline.Factory) == null)
                        throw new InvalidOperationException();
                    concrete = factory(this);
                    container[serviceType] = !trampoline.AsSingleton ? factory : concrete;
                }
                return concrete;
            }
            catch (Exception ex) { throw new ServiceLocatorResolutionException(serviceType, ex); }
        }

        /// <summary>
        /// Resolves all.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns></returns>
        public IEnumerable<TService> ResolveAll<TService>()
            where TService : class =>
            ResolveAll(typeof(TService)).Cast<TService>();
        /// <summary>
        /// Resolves all.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>IEnumerable&lt;System.Object&gt;.</returns>
        public IEnumerable<object> ResolveAll(Type serviceType) =>
            _containers.SelectMany(x => x.Value, (a, b) => new { Name = a.Key, Services = b })
                .Where(x => x.Services.Key == serviceType)
                .Select(x => new { ServiceType = x.Services.Key, x.Name })
                .ToList()
                .Select(x => Resolve(x.ServiceType, x.Name))
                .ToList();

        // inject
        /// <summary>
        /// Injects the specified instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns>TService.</returns>
        /// <exception cref="NotSupportedException"></exception>
        public TService Inject<TService>(TService instance)
            where TService : class =>
            throw new NotSupportedException();

        // release and teardown
        /// <summary>
        /// Releases the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public void Release(object instance) { }

        /// <summary>
        /// Tears down.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="instance">The instance.</param>
        public void TearDown<TService>(TService instance)
            where TService : class
        { }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset() { }

        #region Domain specific

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        public IDictionary<string, IDictionary<Type, object>> Container
        {
            get => _containers;
            private set
            {
                _containers = value;
                _registrar = new MicroServiceRegistrar(this, value);
            }
        }

        #endregion

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() { }
    }
}

