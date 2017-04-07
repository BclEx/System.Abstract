using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Abstract.Tests.ServiceBus
{
    [TestClass]
    public class ServiceBusMessageExceptionTest
    {
        [TestMethod, ExpectedException(typeof(ServiceBusMessageException), "provider")]
        public void Create_Instance_With_Type()
        {
            var exception = new ServiceBusMessageException(typeof(string));
            Assert.AreEqual(exception.MessageType, typeof(string));
            Assert.AreEqual(exception.Message, "Could not send messageType 'System.String'");
            throw exception;
        }

        [TestMethod, ExpectedException(typeof(ServiceBusMessageException), "provider")]
        public void Create_Instance_With_Type_And_InnerException()
        {
            var operationException = new InvalidOperationException();
            var exception = new ServiceBusMessageException(typeof(string), operationException);
            Assert.AreEqual(exception.MessageType, typeof(string));
            Assert.AreEqual(exception.InnerException, operationException);
            Assert.AreEqual(exception.Message, "Could not send messageType 'System.String'");
            throw exception;
        }
    }
}