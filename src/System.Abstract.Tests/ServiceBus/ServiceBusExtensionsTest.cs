using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Abstract.Fakes;
using System.Abstract.IntegationTests.ServiceBus;

namespace System.Abstract.Tests.ServiceBus
{
    [TestClass]
    public class ServiceBusExtensionsTest
    {
        [TestMethod, TestCategory("Core: ServiceBus")]
        public void SendGeneric_With_Destination_And_MessageBuilder_Sends()
        {
            var verify = false;
            var message = new TestMessage { Body = "Body" };
            var serviceBus = new StubIServiceBus
            {
                SendIServiceBusEndpointObjectArray = (e, a) => { verify = (e != null && a[0] == message); return null; },
            };
            serviceBus.CreateMessageOf1ActionOfM0<TestMessage>(messageBuilder => message);
            //
            serviceBus.Send<TestMessage>(new LiteralServiceBusEndpoint("dest"), x => x.Body = "...");
            //
            Assert.IsTrue(verify);
        }

        [TestMethod, TestCategory("Core: ServiceBus")]
        public void SendGeneric_With_MessageBuilder_Sends()
        {
            var verify = false;
            var message = new TestMessage { Body = "Body" };
            var serviceBus = new StubIServiceBus
            {
                SendIServiceBusEndpointObjectArray = (e, a) => { verify = (a[0] == message); return null; },
            };
            serviceBus.CreateMessageOf1ActionOfM0<TestMessage>(messageBuilder => message);
            //
            serviceBus.Send<TestMessage>(x => x.Body = "...");
            //
            Assert.IsTrue(verify);
        }

        [TestMethod, TestCategory("Core: ServiceBus")]
        public void SendGeneric_With_StringDestination_And_MessageBuilder_Sends()
        {
            var verify = false;
            var message = new TestMessage { Body = "Body" };
            var serviceBus = new StubIServiceBus
            {
                SendIServiceBusEndpointObjectArray = (e, a) => { verify = (a[0] == message); return null; },
            };
            serviceBus.CreateMessageOf1ActionOfM0<TestMessage>(messageBuilder => message);
            //
            serviceBus.Send<TestMessage>("dest", x => x.Body = "...");
            //
            Assert.IsTrue(verify);
        }

        [TestMethod, TestCategory("Core: ServiceBus")]
        public void Send_With_Message_Sends()
        {
            var verify = false;
            var messages = new[] { new TestMessage { Body = "Body" } };
            var serviceBus = new StubIServiceBus
            {
                SendIServiceBusEndpointObjectArray = (e, a) => { verify = (a == messages); return null; },
            };
            //
            serviceBus.Send(messages);
            //
            Assert.IsTrue(verify);
        }

        [TestMethod, TestCategory("Core: ServiceBus")]
        public void Send_With_StringDestination_And_Message_Sends()
        {
            var verify = false;
            var messages = new[] { new TestMessage { Body = "Body" } };
            var serviceBus = new StubIServiceBus
            {
                SendIServiceBusEndpointObjectArray = (e, a) => { verify = (a == messages); return null; },
            };
            //
            serviceBus.Send("dest", messages);
            //
            Assert.IsTrue(verify);
        }
    }
}