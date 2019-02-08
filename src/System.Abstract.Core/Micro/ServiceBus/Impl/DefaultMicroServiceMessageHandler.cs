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
    /// DefaultMicroServiceMessageHandler
    /// </summary>
    public class DefaultMicroServiceMessageHandler : IServiceMessageHandler
    {
        readonly Func<object> _resolveAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultMicroServiceMessageHandler"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="implementation">The implementation.</param>
        /// <param name="resolveAction">The resolve action.</param>
        public DefaultMicroServiceMessageHandler(Type service, Type implementation, Func<object> resolveAction)
        {
            _resolveAction = resolveAction;
            Implementation = implementation;
            Service = service;
        }

        /// <summary>
        /// Resolves this instance.
        /// </summary>
        /// <returns></returns>
        public object Resolve() { return _resolveAction(); }
        /// <summary>
        /// Gets the implementation.
        /// </summary>
        /// <value>
        /// The implementation.
        /// </value>
        public Type Implementation { get; private set; }
        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <value>
        /// The service.
        /// </value>
        public Type Service { get; private set; }
    }
}