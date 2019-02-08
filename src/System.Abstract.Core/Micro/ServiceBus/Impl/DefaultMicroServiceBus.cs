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

namespace Contoso.Micro.ServiceBus.Impl
{
    /// <summary>
    /// DefaultMicroServiceBus
    /// </summary>
    public class DefaultMicroServiceBus : IMicroServiceBus
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultMicroServiceBus"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="implementation">The implementation.</param>
        /// <param name="resolveAction">The resolve action.</param>
        public DefaultMicroServiceBus(Type service, Type implementation, Func<object> resolveAction)
        {
        }

        /// <summary>
        /// Replies the specified messages.
        /// </summary>
        /// <param name="messages">The messages.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Reply(params object[] messages)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sends the specified messages.
        /// </summary>
        /// <param name="messages">The messages.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Send(params object[] messages)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sends the specified endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="messages">The messages.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Send(IServiceBusEndpoint endpoint, params object[] messages)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the endpoint.
        /// </summary>
        /// <value>
        /// The endpoint.
        /// </value>
        /// <exception cref="System.NotImplementedException"></exception>
        public IServiceBusEndpoint Endpoint
        {
            get { throw new NotImplementedException(); }
        }
    }
}