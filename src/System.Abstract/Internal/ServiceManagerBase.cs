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

using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace System.Abstract.Internal
{
    /// <summary>
    /// ServiceManagerBase
    /// </summary>
    /// <typeparam name="TService">The type of the service interface.</typeparam>
    /// <typeparam name="TServiceManager">The type of the service.</typeparam>
    /// <typeparam name="TServiceManagerLogger">The type of the service manager logger.</typeparam>
    public abstract partial class ServiceManagerBase<TService, TServiceManager, TServiceManagerLogger>
        where TService : class
        where TServiceManager : class, new()
    {
        static readonly ConditionalWeakTable<Lazy<TService>, ISetupDescriptor> _setupDescriptors = new ConditionalWeakTable<Lazy<TService>, ISetupDescriptor>();
        static readonly object _lock = new object();
        static ServiceRegistration _registration;

        // Force "precise" initialization
        static ServiceManagerBase() =>
            new TServiceManager();

        /// <summary>
        /// Gets or sets the lazy.
        /// </summary>
        /// <value>The lazy.</value>
        public static Lazy<TService> Lazy { get; protected set; }

        /// <summary>
        /// Makes the by provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="setupDescriptor">The setup descriptor.</param>
        /// <returns>Lazy&lt;TService&gt;.</returns>
        /// <exception cref="ArgumentNullException">provider</exception>
        /// <exception cref="System.ArgumentNullException">provider</exception>
        [DebuggerStepThrough]
        public static Lazy<TService> MakeByProvider(Func<TService> provider, ISetupDescriptor setupDescriptor = null)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            var lazy = new Lazy<TService>(provider, LazyThreadSafetyMode.PublicationOnly);
            GetSetupDescriptor(lazy, setupDescriptor);
            return lazy;
        }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>The logger.</value>
        public static TServiceManagerLogger Logger { get; set; }

        /// <summary>
        /// Gets or sets the registration.
        /// </summary>
        /// <value>The registration.</value>
        public static ServiceRegistration Registration
        {
            get => _registration ?? throw new InvalidOperationException("Registration Failed");
            set => _registration = value;
        }

        /// <summary>
        /// Sets the provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="setupDescriptor">The setup descriptor.</param>
        /// <returns>Lazy&lt;TService&gt;.</returns>
        public static Lazy<TService> SetProvider(Func<TService> provider, ISetupDescriptor setupDescriptor = null) =>
            Lazy = MakeByProvider(provider, setupDescriptor);

        /// <summary>
        /// Gets the current.
        /// </summary>
        /// <value>The current.</value>
        /// <exception cref="System.InvalidOperationException">AbstractService undefined. Did you forget to SetProvider?</exception>
        /// <exception cref="System.Exception"></exception>
        public static TService Current
        {
            get
            {
                if (Lazy.IsValueCreated)
                    return Lazy.Value;
                if (InflightValue != null)
                    return InflightValue;
                if (Lazy == null)
                    throw new InvalidOperationException("AbstractService undefined. Did you forget to SetProvider?");
                try { return Lazy.Value; }
                catch (ReflectionTypeLoadException e)
                {
#if NET35
                    var b = new StringBuilder();
                    foreach (var ex in e.LoaderExceptions)
                        b.AppendLine(ex.Message);
                    throw new Exception(b.ToString(), e);
#else
                    throw new AggregateException(e.LoaderExceptions);
#endif
                }
                catch (Exception) { throw; }
            }
        }

        #region Setup

        /// <summary>
        /// IRegisterWithLocator
        /// </summary>
        public interface IRegisterWithLocator
        {
            /// <summary>
            /// Gets the register with locator.
            /// </summary>
            /// <value>The register with locator.</value>
            Action<IServiceLocator, string> RegisterWithLocator { get; }
        }

        /// <summary>
        /// ServiceRegistration
        /// </summary>
        public class ServiceRegistration
        {
            Func<TService> _defaultServiceProvider;

            /// <summary>
            /// Initializes a new instance of the <see cref="ServiceRegistration" /> class.
            /// </summary>
            public ServiceRegistration()
            {
                OnSetup = (service, descriptor) =>
                {
                    if (descriptor != null)
                        foreach (var action in descriptor.Actions)
                            action(service);
                    return service;
                };
                OnChange = (service, descriptor) =>
                {
                    if (descriptor != null)
                        foreach (var action in descriptor.Actions)
                            action(service);
                };
                RegisterWithLocator = (service, locator, name) =>
                {
                    //RegisterInstance(service, locator, name);
                    if (service is IRegisterWithLocator registerWithLocator)
                        registerWithLocator.RegisterWithLocator(locator, name);
                };
            }

            /// <summary>
            /// Gets or sets the DefaultServiceProvider.
            /// </summary>
            /// <value>The DefaultServiceProvider.</value>
            public Func<TService> DefaultServiceProvider
            {
                get => _defaultServiceProvider;
                set
                {
                    _defaultServiceProvider = value;
                    // set default provider
                    if (Lazy == null && value != null)
                        SetProvider(value);
                }
            }

            /// <summary>
            /// Gets or sets the on setup.
            /// </summary>
            /// <value>The on setup.</value>
            public Func<TService, ISetupDescriptor, TService> OnSetup { get; set; }

            /// <summary>
            /// Gets or sets the on change.
            /// </summary>
            /// <value>The on change.</value>
            public Action<TService, ISetupDescriptor> OnChange { get; set; }

            /// <summary>
            /// Gets or sets the on service registrar.
            /// </summary>
            /// <value>The on service registrar.</value>
            public Action<TService, IServiceLocator, string> RegisterWithLocator { get; set; }
        }

        #endregion

        #region IServiceSetup

        /// <summary>
        /// ApplySetupDescriptors
        /// </summary>
        static TService ApplySetupDescriptor(Lazy<TService> service, TService newInstance)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));
            if (newInstance == null)
                throw new NullReferenceException(nameof(newInstance));
            var onSetup = Registration.OnSetup;
            if (onSetup == null)
                return newInstance;
            // find descriptor
            if (_setupDescriptors.TryGetValue(service, out var setupDescriptor))
                _setupDescriptors.Remove(service);
            return onSetup(newInstance, setupDescriptor);
        }

        /// <summary>
        /// ApplyChangeDescriptor
        /// </summary>
        static void ApplyChangeDescriptor(Lazy<TService> service, ISetupDescriptor changeDescriptor)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));
            if (!service.IsValueCreated)
                throw new InvalidOperationException("Service value has not been created yet.");
            Registration.OnChange?.Invoke(service.Value, changeDescriptor);
        }

        /// <summary>
        /// InflightValue
        /// </summary>
        protected static TService InflightValue;

        /// <summary>
        /// GetSetupDescriptor
        /// </summary>
        [DebuggerStepThrough]
        public static ISetupDescriptor GetSetupDescriptor(Lazy<TService> service, ISetupDescriptor firstDescriptor = null)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));
            if (service.IsValueCreated)
                return new SetupDescriptor(Registration, d => ApplyChangeDescriptor(service, d));
            if (_setupDescriptors.TryGetValue(service, out var descriptor))
            {
                if (firstDescriptor == null)
                    return descriptor;
                throw new InvalidOperationException(string.Format(Local.RedefineSetupDescriptorA, service.ToString()));
            }
            lock (_lock)
                if (!_setupDescriptors.TryGetValue(service, out descriptor))
                {
                    descriptor = firstDescriptor ?? new SetupDescriptor(Registration, null);
                    _setupDescriptors.Add(service, descriptor);
                    service.HookValueFactory(valueFactory => ApplySetupDescriptor(service, InflightValue = valueFactory()));
                }
            return descriptor;
        }

        /// <summary>
        /// RegisterInstance
        /// </summary>
        public static void RegisterInstance<T>(T service, string name = null, IServiceLocator locator = null)
            where T : class
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));
            if (name == null) locator.Registrar.RegisterInstance(service);
            else locator.Registrar.RegisterInstance(service, name);
        }

        /// <summary>
        /// RegisterInstance
        /// </summary>
        public static void RegisterInstance(object service, Type serviceType, string name = null, IServiceLocator locator = null)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));
            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));
            if (name == null) locator.Registrar.RegisterInstance(serviceType, service);
            else locator.Registrar.RegisterInstance(serviceType, service, name);
        }

        /// <summary>
        /// ISetupDescriptor
        /// </summary>
        public interface ISetupDescriptor
        {
            /// <summary>
            /// Does the specified action.
            /// </summary>
            /// <param name="action">The action.</param>
            void Do(Action<TService> action);

            /// <summary>
            /// Gets the actions.
            /// </summary>
            /// <value>The actions.</value>
            IEnumerable<Action<TService>> Actions { get; }

            /// <summary>
            /// Registers the with service locator.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="service">The service.</param>
            /// <param name="name">The name.</param>
            /// <param name="locator">The locator.</param>
            [DebuggerStepThrough]
            void RegisterWithServiceLocator<T>(Lazy<TService> service, string name = null, IServiceLocator locator = null)
                where T : class, TService;
            /// <summary>
            /// Registers the with service locator.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="service">The service.</param>
            /// <param name="name">The name.</param>
            /// <param name="locator">The locator.</param>
            [DebuggerStepThrough]
            void RegisterWithServiceLocator<T>(Lazy<TService> service, string name = null, Lazy<IServiceLocator> locator = null)
                where T : class, TService;
            /// <summary>
            /// Registers the with service locator.
            /// </summary>
            /// <param name="service">The service.</param>
            /// <param name="serviceType">Type of the service.</param>
            /// <param name="name">The name.</param>
            /// <param name="locator">The locator.</param>
            [DebuggerStepThrough]
            void RegisterWithServiceLocator(Lazy<TService> service, Type serviceType, string name = null, IServiceLocator locator = null);
            /// <summary>
            /// Registers the with service locator.
            /// </summary>
            /// <param name="service">The service.</param>
            /// <param name="serviceType">Type of the service.</param>
            /// <param name="name">The name.</param>
            /// <param name="locator">The locator.</param>
            [DebuggerStepThrough]
            void RegisterWithServiceLocator(Lazy<TService> service, Type serviceType, string name = null, Lazy<IServiceLocator> locator = null);
        }

        /// <summary>
        /// SetupDescriptor
        /// </summary>
        protected class SetupDescriptor : ISetupDescriptor
        {
            List<Action<TService>> _actions = new List<Action<TService>>();
            ServiceRegistration _registration;
            Action<ISetupDescriptor> _postAction;

            /// <summary>
            /// Initializes a new instance of the <see cref="SetupDescriptor" /> class.
            /// </summary>
            /// <param name="registration">The registration.</param>
            /// <param name="postAction">The post action.</param>
            /// <exception cref="ArgumentNullException">registration - Please ensure EnsureRegistration() has been called previously.</exception>
            /// <exception cref="System.ArgumentNullException">registration;Please ensure EnsureRegistration() has been called previously.</exception>
            public SetupDescriptor(ServiceRegistration registration, Action<ISetupDescriptor> postAction)
            {
                _registration = registration ?? throw new ArgumentNullException(nameof(registration), "Please ensure EnsureRegistration() has been called previously.");
                _postAction = postAction;
            }

            [DebuggerStepThrough]
            void ISetupDescriptor.Do(Action<TService> action)
            {
                if (action == null)
                    throw new ArgumentNullException(nameof(action));
                _actions.Add(action);
                _postAction?.Invoke(this);
            }

            IEnumerable<Action<TService>> ISetupDescriptor.Actions
            {
                [DebuggerStepThrough]
                get => _actions;
            }

            /// <summary>
            /// Registers the with service locator.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="service">The service.</param>
            /// <param name="name">The name.</param>
            /// <param name="locator">The locator.</param>
            /// <exception cref="ArgumentNullException">service</exception>
            /// <exception cref="System.ArgumentNullException">service</exception>
            [DebuggerStepThrough]
            void ISetupDescriptor.RegisterWithServiceLocator<T>(Lazy<TService> service, string name, IServiceLocator locator)
            {
                if (service == null)
                    throw new ArgumentNullException(nameof(service));
                RegisterInstance((T)service.Value, name, locator);
            }
            /// <summary>
            /// Registers the with service locator.
            /// </summary>
            /// <param name="service">The service.</param>
            /// <param name="serviceType">Type of the service.</param>
            /// <param name="name">The name.</param>
            /// <param name="locator">The locator.</param>
            /// <exception cref="ArgumentNullException">service
            /// or
            /// serviceType</exception>
            /// <exception cref="System.ArgumentNullException">service
            /// or
            /// serviceType</exception>
            [DebuggerStepThrough]
            void ISetupDescriptor.RegisterWithServiceLocator(Lazy<TService> service, Type serviceType, string name, IServiceLocator locator)
            {
                if (service == null)
                    throw new ArgumentNullException(nameof(service));
                if (serviceType == null)
                    throw new ArgumentNullException(nameof(serviceType));
                RegisterInstance(service.Value, serviceType, name, locator);
            }
            /// <summary>
            /// Registers the with service locator.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="service">The service.</param>
            /// <param name="name">The name.</param>
            /// <param name="locator">The locator.</param>
            /// <exception cref="ArgumentNullException">service</exception>
            /// <exception cref="System.ArgumentNullException">service</exception>
            [DebuggerStepThrough]
            void ISetupDescriptor.RegisterWithServiceLocator<T>(Lazy<TService> service, string name, Lazy<IServiceLocator> locator)
            {
                if (service == null)
                    throw new ArgumentNullException(nameof(service));
                RegisterInstance((T)service.Value, name, locator.Value);
            }
            /// <summary>
            /// Registers the with service locator.
            /// </summary>
            /// <param name="service">The service.</param>
            /// <param name="serviceType">Type of the service.</param>
            /// <param name="name">The name.</param>
            /// <param name="locator">The locator.</param>
            /// <exception cref="ArgumentNullException">service
            /// or
            /// serviceType</exception>
            /// <exception cref="System.ArgumentNullException">service
            /// or
            /// serviceType</exception>
            [DebuggerStepThrough]
            void ISetupDescriptor.RegisterWithServiceLocator(Lazy<TService> service, Type serviceType, string name, Lazy<IServiceLocator> locator)
            {
                if (service == null)
                    throw new ArgumentNullException(nameof(service));
                if (serviceType == null)
                    throw new ArgumentNullException(nameof(serviceType));
                RegisterInstance(service.Value, serviceType, name, locator.Value);
            }
        }

        #endregion
    }
}
