using Contoso.Abstract;
using System.Abstract.IntegationTests.ServiceBus;

namespace System.Abstract.Tests.ServiceBus.Integration
{
    public class AppServiceBusTest : AbstractServiceBusTest
    {
        protected override IServiceBus CreateServiceBus() { return new AppServiceBus(); }
    }
}
