using Common.Logging.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Abstract;
using System.Abstract.AbstractTests.ServiceLog;

namespace ServiceLog.Tests
{
    [TestClass]
    public class CommonLoggingServiceLogTest : AbstractServiceLogTest
    {
        protected override IServiceLog CreateServiceLog() { return new CommonLoggingServiceLog("Name"); }
    }
}
