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

using Contoso.Micro.ServiceBus.Modules;
using System;
using System.Abstract;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Reflection;

namespace Contoso.Micro.ServiceBus.Impl
{
    /// <summary>
    /// AbstractMicroServiceBusConfiguration
    /// </summary>
    public abstract class AbstractMicroServiceBusConfiguration
    {
        private readonly List<Type> _messageModules;
        private Action _readConfiguration;
        internal readonly List<Assembly> _assemblies;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractMicroServiceBusConfiguration"/> class.
        /// </summary>
        protected AbstractMicroServiceBusConfiguration()
        {
            Action action = null;
            _messageModules = new List<Type>();
            ConsumeInTransaction = true;
            _assemblies = new List<Assembly>(new[] { typeof(IServiceBus).Assembly });
            if (action == null)
                action = (() => BusConfiguration = (ConfigurationManager.GetSection("bus") as AbstractSection).ServiceBus);
            _readConfiguration = action;
            ThreadCount = 1;
            NumberOfRetries = 5;
        }

        /// <summary>
        /// Adds the message module.
        /// </summary>
        /// <typeparam name="TModule">The type of the module.</typeparam>
        /// <returns></returns>
        public AbstractMicroServiceBusConfiguration AddMessageModule<TModule>()
            where TModule : IMicroMessageModule
        {
            _messageModules.Add(typeof(TModule));
            return this;
        }

        /// <summary>
        /// Applies the configuration.
        /// </summary>
        protected abstract void ApplyConfiguration();

        /// <summary>
        /// Builds the with.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public void BuildWith(IServiceLocatorBuilder builder)
        {
            Builder = builder;
            //Builder.WithInterceptor(new ConsumerInterceptor());
        }

        /// <summary>
        /// Configures this instance.
        /// </summary>
        public virtual void Configure()
        {
            ReadBusConfiguration();
            ApplyConfiguration();
            Builder.RegisterDefaultServices(Assemblies);
        }

        /// <summary>
        /// Disables the queue automatic creation.
        /// </summary>
        /// <returns></returns>
        public AbstractMicroServiceBusConfiguration DisableQueueAutoCreation()
        {
            DisableAutoQueueCreation = true;
            return this;
        }

        /// <summary>
        /// Inserts the message module at first.
        /// </summary>
        /// <typeparam name="TModule">The type of the module.</typeparam>
        /// <returns></returns>
        public AbstractMicroServiceBusConfiguration InsertMessageModuleAtFirst<TModule>()
            where TModule : IMicroMessageModule
        {
            _messageModules.Insert(0, typeof(TModule));
            return this;
        }

        /// <summary>
        /// Reads the bus configuration.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">could not find servicebus configuration element</exception>
        protected virtual void ReadBusConfiguration()
        {
            if (BusConfiguration == null)
            {
                _readConfiguration();
                if (BusConfiguration == null)
                    throw new InvalidOperationException("could not find servicebus configuration element");
            }
        }

        /// <summary>
        /// Uses the configuration.
        /// </summary>
        /// <param name="busConfiguration">The bus configuration.</param>
        /// <returns></returns>
        public AbstractMicroServiceBusConfiguration UseConfiguration(ServiceBusConfiguration busConfiguration)
        {
            BusConfiguration = busConfiguration;
            return this;
        }

        /// <summary>
        /// Uses the standalone configuration file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public AbstractMicroServiceBusConfiguration UseStandaloneConfigurationFile(string fileName)
        {
            _readConfiguration = delegate
            {
                BusConfiguration = (ConfigurationManager.OpenMappedMachineConfiguration(new ConfigurationFileMap(fileName)).GetSection("bus") as AbstractSection).ServiceBus;
            };
            return this;
        }

        /// <summary>
        /// Gets the assemblies.
        /// </summary>
        /// <value>
        /// The assemblies.
        /// </value>
        public IEnumerable<Assembly> Assemblies { get { return _assemblies; } }
        /// <summary>
        /// Gets the builder.
        /// </summary>
        /// <value>
        /// The builder.
        /// </value>
        protected IServiceLocatorBuilder Builder { get; private set; }
        /// <summary>
        /// Gets the bus configuration.
        /// </summary>
        /// <value>
        /// The bus configuration.
        /// </value>
        public ServiceBusConfiguration BusConfiguration { get; private set; }
        /// <summary>
        /// Gets a value indicating whether [consume in transaction].
        /// </summary>
        /// <value>
        /// <c>true</c> if [consume in transaction]; otherwise, <c>false</c>.
        /// </value>
        public bool ConsumeInTransaction { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether [disable automatic queue creation].
        /// </summary>
        /// <value>
        /// <c>true</c> if [disable automatic queue creation]; otherwise, <c>false</c>.
        /// </value>
        public bool DisableAutoQueueCreation { get; set; }
        /// <summary>
        /// Gets or sets the endpoint.
        /// </summary>
        /// <value>
        /// The endpoint.
        /// </value>
        public Uri Endpoint { get; set; }

        /// <summary>
        /// Gets the message modules.
        /// </summary>
        /// <value>
        /// The message modules.
        /// </value>
        public IEnumerable<Type> MessageModules
        {
            get { return new ReadOnlyCollection<Type>(_messageModules); }
        }

        /// <summary>
        /// Gets or sets the number of retries.
        /// </summary>
        /// <value>
        /// The number of retries.
        /// </value>
        public int NumberOfRetries { get; set; }
        /// <summary>
        /// Gets or sets the thread count.
        /// </summary>
        /// <value>
        /// The thread count.
        /// </value>
        public int ThreadCount { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [use flat queue].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use flat queue]; otherwise, <c>false</c>.
        /// </value>
        public bool UseFlatQueue { get; set; }
    }
}