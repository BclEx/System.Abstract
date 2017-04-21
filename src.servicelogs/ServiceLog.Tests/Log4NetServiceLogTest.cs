using log4net.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Abstract.IntegationTests.ServiceLog;

namespace System.Abstract.Tests.ServiceLocator.Integration
{
    [TestClass]
    public class Log4NetServiceLogTest : AbstractServiceLogTest
    {
        protected override IServiceLog CreateServiceLog() { return new Log4NetServiceLog("Name"); }
    }
}
