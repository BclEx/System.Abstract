using Contoso.Abstract;
using System.Abstract.IntegationTests.ServiceLog;

namespace System.Abstract.Tests.ServiceLog.Integration
{
    public class EventLogServiceLogTest : AbstractServiceLogTest
    {
        protected override IServiceLog CreateServiceLog() { return new EventLogServiceLog("Default"); }
    }
}