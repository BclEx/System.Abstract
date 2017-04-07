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
using System.Configuration;

namespace Contoso.Micro.ServiceBus.Impl
{
    /// <summary>
    /// MicroServiceBusConfiguration
    /// </summary>
    public class MicroServiceBusConfiguration : AbstractMicroServiceBusConfiguration
    {
        readonly List<MessageOwner> _messageOwners = new List<MessageOwner>();

        /// <summary>
        /// Applies the configuration.
        /// </summary>
        protected override void ApplyConfiguration()
        {
            //Uri uri;
            //bool flag;
            //BusElement bus = BusConfiguration.Bus;
            //if (bus == null)
            //{
            //    throw new ConfigurationErrorsException("Could not find 'bus' node in configuration");
            //}
            //if (bus.NumberOfRetries.HasValue)
            //    base.NumberOfRetries = bus.NumberOfRetries.Value;
            //if (bus.ThreadCount.HasValue)
            //    base.ThreadCount = bus.ThreadCount.Value;
            //var queueIsolationLevel = bus.QueueIsolationLevel;
            //if (!string.IsNullOrEmpty(queueIsolationLevel))
            //    base.queueIsolationLevel = (IsolationLevel)Enum.Parse(typeof(IsolationLevel), queueIsolationLevel);
            //if (bus.ConsumeInTransaction.HasValue)
            //    base.ConsumeInTxn = bus.ConsumeInTransaction.Value;
            //var endpoint = bus.Endpoint;
            //if (!Uri.TryCreate(endpoint, UriKind.Absolute, out uri))
            //    throw new ConfigurationErrorsException("Attribute 'endpoint' on 'bus' has an invalid value '" + endpoint + "'");
            //base.Endpoint = uri;
            //string transactional = bus.Transactional;
            //if (bool.TryParse(transactional, out flag))
            //    base.Transactional = flag ? TransactionalOptions.Transactional : TransactionalOptions.NonTransactional;
            //else if (transactional != null)
            //    throw new ConfigurationErrorsException("Attribute 'transactional' on 'bus' has an invalid value '" + transactional + "'");
            //var assemblies = BusConfiguration.Assemblies;
            //if (assemblies != null)
            //    foreach (Assembly element in assemblies)
            //        Assemblies.Add(element);
        }

        /// <summary>
        /// Configures this instance.
        /// </summary>
        public override void Configure()
        {
            base.Configure();
            Builder.RegisterBus();
        }

        /// <summary>
        /// Reads the bus configuration.
        /// </summary>
        protected override void ReadBusConfiguration()
        {
            base.ReadBusConfiguration();
            new MessageOwnersConfigReader(BusConfiguration, _messageOwners).ReadMessageOwners();
        }

        /// <summary>
        /// Uses the flat queue structure.
        /// </summary>
        /// <returns></returns>
        public AbstractMicroServiceBusConfiguration UseFlatQueueStructure()
        {
            UseFlatQueue = true;
            return this;
        }

        /// <summary>
        /// Gets the message owners.
        /// </summary>
        /// <value>
        /// The message owners.
        /// </value>
        public IEnumerable<MessageOwner> MessageOwners
        {
            get { return _messageOwners; }
        }
    }
}