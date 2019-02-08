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
using System.Abstract.Configuration;
using System.Abstract.Configuration.ServiceBus;
using System.Collections.Generic;
using System.Configuration;

namespace Contoso.Micro.ServiceBus.Impl
{
    /// <summary>
    /// MessageOwnersConfigReader
    /// </summary>
    public class MessageOwnersConfigReader
    {
        private readonly ServiceBusConfiguration _configuration;
        private readonly ICollection<MessageOwner> _messageOwners;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageOwnersConfigReader"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="messageOwners">The message owners.</param>
        public MessageOwnersConfigReader(ServiceBusConfiguration configuration, ICollection<MessageOwner> messageOwners)
        {
            _configuration = configuration;
            _messageOwners = messageOwners;
        }

        /// <summary>
        /// Gets the endpoint scheme.
        /// </summary>
        /// <value>
        /// The endpoint scheme.
        /// </value>
        public string EndpointScheme { get; private set; }

        /// <summary>
        /// Reads the message owners.
        /// </summary>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        /// Could not find 'endpoints' node in configuration
        /// or
        /// Invalid name element in the <endpoints /> element
        /// or
        /// Invalid endpoint url:  + endpointAsString
        /// or
        /// Invalid transactional settings:  + transactionalAsString
        /// </exception>
        public void ReadMessageOwners()
        {
            var endpoints = _configuration.Endpoints;
            if (endpoints == null)
                throw new ConfigurationErrorsException("Could not find 'endpoints' node in configuration");
            foreach (EndpointElement child in endpoints)
            {
                var name = child.Name;
                if (string.IsNullOrEmpty(name))
                    throw new ConfigurationErrorsException("Invalid name element in the <endpoints /> element");

                var endpointAsString = child.Endpoint;
                Uri ownerEndpoint;
                try
                {
                    ownerEndpoint = new Uri(endpointAsString);
                    if (EndpointScheme == null)
                        EndpointScheme = ownerEndpoint.Scheme;
                }
                catch (Exception e) { throw new ConfigurationErrorsException("Invalid endpoint url: " + endpointAsString, e); }

                bool? transactional = null;
                var transactionalAsString = child.Transactional;
                if (!string.IsNullOrEmpty(transactionalAsString))
                {
                    bool value;
                    if (!bool.TryParse(transactionalAsString, out value))
                        throw new ConfigurationErrorsException("Invalid transactional settings: " + transactionalAsString);
                    transactional = value;
                }
                _messageOwners.Add(new MessageOwner { Name = name, Endpoint = ownerEndpoint, Transactional = transactional });
            }
        }
    }
}