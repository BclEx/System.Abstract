using System.Abstract.IntegationTests.ServiceLog;

namespace System.Abstract.Tests.ServiceLog.Integration
{
    public class EmptyServiceLogTest : AbstractServiceLogTest
    {
        protected override IServiceLog CreateServiceLog() { return new ServiceLogManager.EmptyServiceLog(); }
    }
}