using Contoso.Abstract;
using System.Abstract.IntegationTests.ServiceLog;

namespace System.Abstract.Tests.ServiceLog.Integration
{
    public class ConsoleServiceLogTest : AbstractServiceLogTest
    {
        protected override IServiceLog CreateServiceLog() { return new ConsoleServiceLog("Default"); }
    }
}