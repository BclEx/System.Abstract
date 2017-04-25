using log4net.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Abstract;
using System.Abstract.AbstractTests.ServiceLog;

namespace ServiceLog.Tests
{
    [TestClass]
    public class Log4NetServiceLogTest : AbstractServiceLogTest
    {
        protected override IServiceLog CreateServiceLog() { return new Log4NetServiceLog("Name"); }
    }
}
