using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Abstract.AbstractTests.ServiceBus;

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
            var mock = new Mock<IServiceBus>();
            mock.Setup(x => x.Send(It.IsAny<IServiceBusEndpoint>(), It.IsAny<object[]>())).Callback<IServiceBusEndpoint, object[]>((e, a) =>
            {
                verify = e != null && a[0] == message;
            }).Returns((IServiceBusCallback)null);
            mock.Setup(x => x.CreateMessage(It.IsAny<Action<TestMessage>>())).Returns(message);
            var serviceBus = mock.Object;
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
            var mock = new Mock<IServiceBus>();
            mock.Setup(x => x.Send(It.IsAny<IServiceBusEndpoint>(), It.IsAny<object[]>())).Callback<IServiceBusEndpoint, object[]>((e, a) =>
            {
                verify = e == null && a[0] == message;
            }).Returns((IServiceBusCallback)null);
            mock.Setup(x => x.CreateMessage(It.IsAny<Action<TestMessage>>())).Returns(message);
            var serviceBus = mock.Object;
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
            var mock = new Mock<IServiceBus>();
            mock.Setup(x => x.Send(It.IsAny<IServiceBusEndpoint>(), It.IsAny<object[]>())).Callback<IServiceBusEndpoint, object[]>((e, a) =>
            {
                verify = e != null && a[0] == message;
            }).Returns((IServiceBusCallback)null);
            mock.Setup(x => x.CreateMessage(It.IsAny<Action<TestMessage>>())).Returns(message);
            var serviceBus = mock.Object;
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
            var mock = new Mock<IServiceBus>();
            mock.Setup(x => x.Send(It.IsAny<IServiceBusEndpoint>(), It.IsAny<object[]>())).Callback<IServiceBusEndpoint, object[]>((e, a) =>
            {
                verify = e == null && a == messages;
            }).Returns((IServiceBusCallback)null);
            var serviceBus = mock.Object;
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
            var mock = new Mock<IServiceBus>();
            mock.Setup(x => x.Send(It.IsAny<IServiceBusEndpoint>(), It.IsAny<object[]>())).Callback<IServiceBusEndpoint, object[]>((e, a) =>
            {
                verify = e != null && a == messages;
            }).Returns((IServiceBusCallback)null);
            var serviceBus = mock.Object;
            //
            serviceBus.Send("dest", messages);
            //
            Assert.IsTrue(verify);
        }
    }
}