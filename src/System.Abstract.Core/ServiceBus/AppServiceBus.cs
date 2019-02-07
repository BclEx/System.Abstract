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
using System.Linq;
using System.Abstract;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Contoso.Abstract
{
    /// <summary>
    /// Interface IAppServiceBus
    /// </summary>
    /// <seealso cref="System.Abstract.IServiceBus" />
    /// <remark>
    /// An application service bus specific service bus interface
    /// </remark>
    public interface IAppServiceBus : IServiceBus
    {
        /// <summary>
        /// Adds this instance.
        /// </summary>
        /// <typeparam name="TMessageHandler">The type of the message handler.</typeparam>
        /// <returns>IAppServiceBus.</returns>
        IAppServiceBus Add<TMessageHandler>()
            where TMessageHandler : class;
        /// <summary>
        /// Adds the specified message handler type.
        /// </summary>
        /// <param name="messageHandlerType">Type of the message handler.</param>
        /// <returns>IAppServiceBus.</returns>
        IAppServiceBus Add(Type messageHandlerType);
    }

    /// <summary>
    /// Class AppServiceBus.
    /// </summary>
    /// <seealso cref="Contoso.Abstract.IAppServiceBus" />
    /// <remark>
    /// An application service bus implementation
    /// </remark>
    /// <example>
    ///  <code>
    ///ServiceBusManager.SetProvider(() =&gt; new AppServiceBus()
    ///.Add(Handler1)
    ///.Add(Handler2))
    ///.RegisterWithServiceLocator();
    ///ServiceBusManager.Send&lt;Message1&gt;(x =&gt; x.Body = "Message");
    ///</code>
    /// </example>
    public class AppServiceBus : Collection<AppServiceBusRegistration>, IAppServiceBus, ServiceBusManager.IRegisterWithLocator
    {
        readonly Func<Type, IServiceMessageHandler<object>> _messageHandlerFactory;
        readonly Func<IServiceLocator> _locator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppServiceBus" /> class.
        /// </summary>
        public AppServiceBus()
            : this(t => (IServiceMessageHandler<object>)Activator.CreateInstance(t), () => ServiceLocatorManager.Current) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="AppServiceBus" /> class.
        /// </summary>
        /// <param name="messageHandlerFactory">The message handler factory.</param>
        public AppServiceBus(Func<Type, IServiceMessageHandler<object>> messageHandlerFactory)
            : this(messageHandlerFactory, () => ServiceLocatorManager.Current) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="AppServiceBus" /> class.
        /// </summary>
        /// <param name="messageHandlerFactory">The message handler factory.</param>
        /// <param name="locator">The locator.</param>
        /// <exception cref="ArgumentNullException">messageHandlerFactory
        /// or
        /// locator</exception>
        public AppServiceBus(Func<Type, IServiceMessageHandler<object>> messageHandlerFactory, Func<IServiceLocator> locator)
        {
            _messageHandlerFactory = messageHandlerFactory ?? throw new ArgumentNullException(nameof(messageHandlerFactory));
            _locator = locator ?? throw new ArgumentNullException(nameof(locator));
        }

        Action<IServiceLocator, string> ServiceBusManager.IRegisterWithLocator.RegisterWithLocator =>
            (locator, name) => ServiceBusManager.RegisterInstance<IAppServiceBus>(this, name, locator);

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>Throws NotImplementedException.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public object GetService(Type serviceType) =>
            throw new NotImplementedException();

        /// <summary>
        /// Creates a new message.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="messageBuilder">The message builder.</param>
        /// <returns>The newly created message.</returns>
        public TMessage CreateMessage<TMessage>(Action<TMessage> messageBuilder)
            where TMessage : class
        {
            var message = _locator().Resolve<TMessage>();
            messageBuilder?.Invoke(message);
            return message;
        }

        /// <summary>
        /// Adds a message handler.
        /// </summary>
        /// <typeparam name="TMessageHandler">The type of the message handler.</typeparam>
        /// <returns>Fluent</returns>
        public IAppServiceBus Add<TMessageHandler>()
            where TMessageHandler : class =>
            Add(typeof(TMessageHandler));
        /// <summary>
        /// Adds a message handler
        /// </summary>
        /// <param name="messageHandlerType">Type of the message handler.</param>
        /// <returns>Fluent</returns>
        /// <exception cref="InvalidOperationException">Unable find a message handler</exception>
        public IAppServiceBus Add(Type messageHandlerType)
        {
            var messageType = GetMessageTypeFromHandler(messageHandlerType);
            if (messageType == null)
                throw new InvalidOperationException("Unable find a message handler");
            Add(new AppServiceBusRegistration
            {
                MessageHandlerType = messageHandlerType,
                MessageType = messageType,
            });
            return this;
        }

        /// <summary>
        /// Sends a message on the bus
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="messages">The messages.</param>
        /// <returns>Null</returns>
        /// <exception cref="ArgumentNullException">messages</exception>
        public IServiceBusCallback Send(IServiceBusEndpoint destination, params object[] messages)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));
            foreach (var message in messages)
                foreach (var type in GetTypesOfMessageHandlers(message.GetType()))
                    HandleTheMessage(type, message);
            return null;
        }

        void HandleTheMessage(Type type, object message) =>
            _messageHandlerFactory(type)
                .Handle(message);

        IEnumerable<Type> GetTypesOfMessageHandlers(Type messageType) =>
            Items.Where(x => x.MessageType == messageType)
                .Select(x => x.MessageHandlerType);

        /// <summary>
        /// Gets the message type from handler.
        /// </summary>
        /// <param name="messageHandlerType">Type of the message handler.</param>
        /// <returns>Type.</returns>
        static Type GetMessageTypeFromHandler(Type messageHandlerType) =>
            null;
        //var serviceMessageType = typeof(IServiceMessage);
        //var applicationServiceMessageType = typeof(IApplicationServiceMessage);
        //return messageHandlerType.GetInterfaces()
        //    .Where(h => h.IsGenericType && (h.FullName.StartsWith("System.Abstract.IServiceMessageHandler`1") || h.FullName.StartsWith("Contoso.Abstract.IApplicationServiceMessageHandler`1")))
        //    .Select(h => h.GetGenericArguments()[0])
        //    .Where(m => m.GetInterfaces().Any(x => x == serviceMessageType || x == applicationServiceMessageType))
        //    .SingleOrDefault();

        /// <summary>
        /// Replies messages back up the bus.
        /// </summary>
        /// <param name="messages">The messages.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void Reply(params object[] messages) =>
            throw new NotImplementedException();

        /// <summary>
        /// Returns a value back up the bus.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void Return<T>(T value) =>
            throw new NotImplementedException();
    }
}
