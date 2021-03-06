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

namespace System.Abstract
{
    /// <summary>
    /// IEventBus
    /// </summary>
    /// <seealso cref="System.Abstract.IServiceBus" />
    public interface IEventBus : IServiceBus { }

    /// <summary>
    /// EventBus
    /// </summary>
    /// <seealso cref="System.Abstract.IEventBus" />
    public class EventBus : IEventBus
    {
        IServiceBus _parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventBus" /> struct.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <exception cref="ArgumentNullException">parent</exception>
        /// <exception cref="System.ArgumentNullException">parent</exception>
        public EventBus(IServiceBus parent) =>
            _parent = parent ?? throw new ArgumentNullException("parent");

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type <paramref name="serviceType" />.
        /// -or-
        /// null if there is no service object of type <paramref name="serviceType" />.</returns>
        public object GetService(Type serviceType) =>
            _parent.GetService(serviceType);

        /// <summary>
        /// Creates the message.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="messageBuilder">The message builder.</param>
        /// <returns>TMessage.</returns>
        public TMessage CreateMessage<TMessage>(Action<TMessage> messageBuilder)
            where TMessage : class =>
            _parent.CreateMessage(messageBuilder);

        /// <summary>
        /// Sends the specified destination.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="messages">The messages.</param>
        /// <returns>IServiceBusCallback.</returns>
        public IServiceBusCallback Send(IServiceBusEndpoint destination, params object[] messages) =>
            _parent.Send(destination, messages);

        /// <summary>
        /// Replies the specified messages.
        /// </summary>
        /// <param name="messages">The messages.</param>
        public void Reply(params object[] messages) =>
            _parent.Send(messages);
    }
}
