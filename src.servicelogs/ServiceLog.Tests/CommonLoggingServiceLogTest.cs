using Common.Logging.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Abstract.IntegationTests.ServiceLog;

namespace System.Abstract.Tests.ServiceLocator.Integration
{
    [TestClass]
    public class CommonLoggingServiceLogTest : AbstractServiceLogTest
    {
        protected override IServiceLog CreateServiceLog() { return new CommonLoggingServiceLog("Name"); }
    }
}
