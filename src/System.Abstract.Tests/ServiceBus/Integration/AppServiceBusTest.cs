using Contoso.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Abstract.IntegationTests.ServiceBus;

namespace System.Abstract.Tests.ServiceBus.Integration
{
    [TestClass]
    public class AppServiceBusTest : AbstractServiceBusTest
    {
        protected override IServiceBus CreateServiceBus() { return new AppServiceBus(); }

        [TestMethod, TestCategory("ServiceBus: AppService")]
        public override void CreateMessage_Should_Return_Valid_Instance() { base.CreateMessage_Should_Return_Valid_Instance(); }

        [TestMethod, TestCategory("ServiceBus: AppService")]
        public override void CreateMessage_With_Action_Should_Return_Valid_Instance() { base.CreateMessage_With_Action_Should_Return_Valid_Instance(); }

        [TestMethod, TestCategory("ServiceBus: AppService")]
        public override void Send_Should_Return_Valid_Instance() { base.Send_Should_Return_Valid_Instance(); }
    }
}
