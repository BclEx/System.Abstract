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

namespace System.Abstract
{
    /// <summary>
    /// ServiceManagerBase
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <typeparam name="TServiceManagerLogger">The type of the service manager logger.</typeparam>
    public abstract partial class ServiceManagerBase<TService, TServiceManagerLogger>
        where TService : class
    {
        static readonly ConditionalWeakTable<Lazy<TService>, ISetupDescriptor> _setupDescriptors = new ConditionalWeakTable<Lazy<TService>, ISetupDescriptor>();
        static readonly object _lock = new object();

        // Force "precise" initialization
        static ServiceManagerBase() { }

        /// <summary>
        /// Gets or sets the DefaultServiceProvider.
        /// </summary>
        /// <value>The DefaultServiceProvider.</value>
        public static Func<TService> DefaultServiceProvider { get; set; }

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
                throw new ArgumentNullException("provider");
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
        protected static ServiceRegistration Registration { get; set; }

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
        protected class ServiceRegistration
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ServiceRegistration" /> class.
            /// </summary>
            public ServiceRegistration()
            {
                RegisterWithLocator = (service, locator, name) =>
                {
                    //RegisterInstance(service, locator, name);
                    var registerWithLocator = (service as IRegisterWithLocator);
                    if (registerWithLocator != null)
                        registerWithLocator.RegisterWithLocator(locator, name);
                };
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
        /// ApplySetup
        /// </summary>
         static TService ApplySetupDescriptors(Lazy<TService> service, TService newInstance)
        {
            if (service == null)
                throw new ArgumentNullException("ApplySetup: service");
            if (newInstance == null)
                throw new NullReferenceException("ApplySetup: newInstance");
            var registration = Registration;
            if (registration == null)
                throw new NullReferenceException("ApplySetup: Registration");
            var onSetup = registration.OnSetup;
            if (onSetup == null)
                return newInstance;
            // find descriptor
            if (_setupDescriptors.TryGetValue(service, out var setupDescriptor))
                _setupDescriptors.Remove(service);
            return onSetup(newInstance, setupDescriptor);
        }

        /// <summary>
        /// ApplyChanges
        /// </summary>
         static void ApplyChange(Lazy<TService> service, ISetupDescriptor changeDescriptor)
        {
            if (service == null)
                throw new ArgumentNullException("service");
            if (!service.IsValueCreated)
                throw new InvalidOperationException("Service value has not been created yet.");
            var registration = Registration;
            if (registration == null)
                throw new NullReferenceException("Registration");
            registration.OnChange?.Invoke(service.Value, changeDescriptor);
        }

        /// <summary>
        /// InflightValue
        /// </summary>
        protected static TService InflightValue;

        /// <summary>
        /// GetSetupDescriptorProtected
        /// </summary>
        [DebuggerStepThrough]
        public static ISetupDescriptor GetSetupDescriptor(Lazy<TService> service, ISetupDescriptor firstDescriptor = null)
        {
            if (service == null)
                throw new ArgumentNullException("service");
            if (service.IsValueCreated)
                return new SetupDescriptor(Registration, d => ApplyChange(service, d));
            ISetupDescriptor descriptor;
            if (_setupDescriptors.TryGetValue(service, out descriptor))
            {
                if (firstDescriptor == null)
                    return descriptor;
                throw new InvalidOperationException(string.Format(Local.RedefineSetupDescriptorA, service.ToString()));
            }
            lock (_lock)
                if (!_setupDescriptors.TryGetValue(service, out descriptor))
                {
                    descriptor = (firstDescriptor ?? new SetupDescriptor(Registration, null));
                    _setupDescriptors.Add(service, descriptor);
                    service.HookValueFactory(valueFactory => ApplySetupDescriptors(service, InflightValue = valueFactory()));
                }
            return descriptor;
        }

        /// <summary>
        /// RegisterInstance
        /// </summary>
        public static void RegisterInstance<T>(T service, IServiceLocator locator = null, string name = null) //: CHECK name, locator
            where T : class
        {
            if (service == null)
                throw new ArgumentNullException("service");
            if (name == null) locator.Registrar.RegisterInstance<T>(service);
            else locator.Registrar.RegisterInstance<T>(service, name);
        }

        /// <summary>
        /// RegisterInstance
        /// </summary>
        public static void RegisterInstance(object service, Type serviceType, IServiceLocator locator = null, string name = null)//: CHECK name, locator
        {
            if (service == null)
                throw new ArgumentNullException("service");
            if (serviceType == null)
                throw new ArgumentNullException("serviceType");
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
            /// <param name="locator">The locator.</param>
            /// <param name="name">The name.</param>
            [DebuggerStepThrough]
            void RegisterWithServiceLocator<T>(Lazy<TService> service, IServiceLocator locator = null, string name = null) //: CHECK name, locator
                where T : class, TService;
            /// <summary>
            /// Registers the with service locator.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="service">The service.</param>
            /// <param name="locator">The locator.</param>
            /// <param name="name">The name.</param>
            [DebuggerStepThrough]
            void RegisterWithServiceLocator<T>(Lazy<TService> service, Lazy<IServiceLocator> locator = null, string name = null) //: CHECK name, locator
                where T : class, TService;
            /// <summary>
            /// Registers the with service locator.
            /// </summary>
            /// <param name="service">The service.</param>
            /// <param name="serviceType">Type of the service.</param>
            /// <param name="locator">The locator.</param>
            /// <param name="name">The name.</param>
            [DebuggerStepThrough]
            void RegisterWithServiceLocator(Lazy<TService> service, Type serviceType, IServiceLocator locator = null, string name = null); //: CHECK name, locator
            /// <summary>
            /// Registers the with service locator.
            /// </summary>
            /// <param name="service">The service.</param>
            /// <param name="serviceType">Type of the service.</param>
            /// <param name="locator">The locator.</param>
            /// <param name="name">The name.</param>
            [DebuggerStepThrough]
            void RegisterWithServiceLocator(Lazy<TService> service, Type serviceType, Lazy<IServiceLocator> locator = null, string name = null); //: CHECK name, locator
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
                _registration = registration ?? throw new ArgumentNullException("registration", "Please ensure EnsureRegistration() has been called previously.");
                _postAction = postAction;
            }

            [DebuggerStepThrough]
            void ISetupDescriptor.Do(Action<TService> action)
            {
                if (action == null)
                    throw new ArgumentNullException("action");
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
            /// <param name="locator">The locator.</param>
            /// <param name="name">The name.</param>
            /// <exception cref="ArgumentNullException">service</exception>
            /// <exception cref="System.ArgumentNullException">service</exception>
            [DebuggerStepThrough]
            void ISetupDescriptor.RegisterWithServiceLocator<T>(Lazy<TService> service, IServiceLocator locator, string name) //: CHECK name, locator
            {
                if (service == null)
                    throw new ArgumentNullException("service");
                RegisterInstance((T)service.Value, locator, name);
            }
            /// <summary>
            /// Registers the with service locator.
            /// </summary>
            /// <param name="service">The service.</param>
            /// <param name="serviceType">Type of the service.</param>
            /// <param name="locator">The locator.</param>
            /// <param name="name">The name.</param>
            /// <exception cref="ArgumentNullException">service
            /// or
            /// serviceType</exception>
            /// <exception cref="System.ArgumentNullException">service
            /// or
            /// serviceType</exception>
            [DebuggerStepThroughAttribute]
            void ISetupDescriptor.RegisterWithServiceLocator(Lazy<TService> service, Type serviceType, IServiceLocator locator, string name) //: CHECK name, locator
            {
                if (service == null)
                    throw new ArgumentNullException("service");
                if (serviceType == null)
                    throw new ArgumentNullException("serviceType");
                RegisterInstance(service.Value, serviceType, locator, name);
            }
            /// <summary>
            /// Registers the with service locator.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="service">The service.</param>
            /// <param name="locator">The locator.</param>
            /// <param name="name">The name.</param>
            /// <exception cref="ArgumentNullException">service</exception>
            /// <exception cref="System.ArgumentNullException">service</exception>
            [DebuggerStepThrough]
            void ISetupDescriptor.RegisterWithServiceLocator<T>(Lazy<TService> service, Lazy<IServiceLocator> locator, string name) //: CHECK name, locator
            {
                if (service == null)
                    throw new ArgumentNullException("service");
                RegisterInstance((T)service.Value, locator.Value, name);
            }
            /// <summary>
            /// Registers the with service locator.
            /// </summary>
            /// <param name="service">The service.</param>
            /// <param name="serviceType">Type of the service.</param>
            /// <param name="locator">The locator.</param>
            /// <param name="name">The name.</param>
            /// <exception cref="ArgumentNullException">service
            /// or
            /// serviceType</exception>
            /// <exception cref="System.ArgumentNullException">service
            /// or
            /// serviceType</exception>
            [DebuggerStepThroughAttribute]
            void ISetupDescriptor.RegisterWithServiceLocator(Lazy<TService> service, Type serviceType, Lazy<IServiceLocator> locator, string name) //: CHECK name, locator
            {
                if (service == null)
                    throw new ArgumentNullException("service");
                if (serviceType == null)
                    throw new ArgumentNullException("serviceType");
                RegisterInstance(service.Value, serviceType, locator.Value, name);
            }
        }

        #endregion
    }
}
