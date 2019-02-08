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

namespace Contoso.Micro.ServiceBus.Impl
{
    /// <summary>
    /// IMicroQueueStrategy
    /// </summary>
    public interface IMicroQueueStrategy
    {
        /// <summary>
        /// Initializes the queue.
        /// </summary>
        /// <param name="queueEndpoint">The queue endpoint.</param>
        /// <param name="queueType">Type of the queue.</param>
        /// <returns></returns>
        object[] InitializeQueue(IServiceBusEndpoint queueEndpoint, int queueType);
        /// <summary>
        /// Grants the permissions.
        /// </summary>
        /// <param name="queue">The queue.</param>
        /// <param name="user">The user.</param>
        void GrantPermissions(object queue, string user);
        //Uri CreateSubscriptionQueueUri(Uri subscriptionQueue);
        //IEnumerable<TimeoutInfo> GetTimeoutMessages(OpenedQueue queue);
        //void MoveTimeoutToMainQueue(OpenedQueue queue, string messageId);
        //bool TryMoveMessage(OpenedQueue queue, Message message, SubQueue subQueue, out string msgId);
        //void SendToErrorQueue(OpenedQueue queue, Message message);
        //OpenedQueue OpenSubQueue(OpenedQueue queue, SubQueue subQueue, QueueAccessMode accessMode);
    }
}
