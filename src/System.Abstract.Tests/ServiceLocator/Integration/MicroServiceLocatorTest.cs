using Contoso.Abstract;
using System.Abstract.IntegationTests.ServiceLocator;

namespace System.Abstract.Tests.ServiceLocator.Integration
{
    public class MicroServiceLocatorIntegrationTests : AbstractServiceLocatorTest
    {
        protected override IServiceLocator CreateServiceLocator() { return new MicroServiceLocator(); }
    }
}
