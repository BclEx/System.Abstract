using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Abstract.AbstractTests.ServiceBus
{
    [TestClass]
    public abstract class AbstractServiceBusTest
    {
        protected IServiceBus Bus { get; private set; }
        protected abstract IServiceBus CreateServiceBus();

        public AbstractServiceBusTest() =>
            Bus = CreateServiceBus();

        [TestMethod]
        public virtual void CreateMessage_Should_Return_Valid_Instance()
        {
            var message = Bus.CreateMessage<TestMessage>(null);
            Assert.IsNotNull(message);
        }

        [TestMethod]
        public virtual void CreateMessage_With_Action_Should_Return_Valid_Instance()
        {
            var message = Bus.CreateMessage<TestMessage>(x => x.Body = "APPLY");
            Assert.IsNotNull(message);
            Assert.AreEqual(message.Body, "APPLY");
        }

        [TestMethod]
        public virtual void Send_Should_Return_Valid_Instance()
        {
        }

        //IServiceBusCallback Send(IServiceBusLocation destination, params IServiceMessage[] messages);
        //public static IServiceBusCallback Send<TMessage>(this IServiceBus serviceBus, IServiceBusLocation destination, Action<TMessage> messageBuilder)
        //    where TMessage : IServiceMessage { return serviceBus.Send(destination, serviceBus.CreateMessage<TMessage>(messageBuilder)); }
        //public static IServiceBusCallback Send<TMessage>(this IServiceBus serviceBus, Action<TMessage> messageBuilder)
        //    where TMessage : IServiceMessage { return serviceBus.Send(null, serviceBus.CreateMessage<TMessage>(messageBuilder)); }
        //public static IServiceBusCallback Send<TMessage>(this IServiceBus serviceBus, string destination, Action<TMessage> messageBuilder)
        //    where TMessage : IServiceMessage { return serviceBus.Send(new LiteralServiceBusLocation(destination), serviceBus.CreateMessage<TMessage>(messageBuilder)); }
        //// send
        //public static IServiceBusCallback Send(this IServiceBus serviceBus, params IServiceMessage[] messages) { return serviceBus.Send(null, messages); }
        //public static IServiceBusCallback Send(this IServiceBus serviceBus, string destination, params IServiceMessage[] messages) { return serviceBus.Send(new LiteralServiceBusLocation(destination), messages); }
    }
}