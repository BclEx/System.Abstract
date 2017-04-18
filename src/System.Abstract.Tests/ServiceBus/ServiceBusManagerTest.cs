using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Abstract.Tests.ServiceBus
{
    [TestClass]
    public class ServiceBusManagerTest
    {
        [TestMethod, TestCategory("Core: ServiceBus"), ExpectedException(typeof(ArgumentNullException), "provider")]
        public void Null_Provider_Throws_ArgumentNullException_Exception()
        {
            ServiceBusManager.SetProvider(null);
        }

        [TestMethod, TestCategory("Core: ServiceBus"), ExpectedException(typeof(NullReferenceException), "instance")]
        public void Null_Provider_Throws_InvalidOperation_Exception()
        {
            ServiceBusManager.SetProvider(() => null);
            var service = ServiceBusManager.Current;
        }
    }
}