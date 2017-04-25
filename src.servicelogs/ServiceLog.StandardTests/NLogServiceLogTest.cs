using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog.Abstract;
using System.Abstract;
using System.Abstract.AbstractTests.ServiceLog;

namespace ServiceLog.Tests
{
    [TestClass]
    public class NLogServiceLogTest : AbstractServiceLogTest
    {
        protected override IServiceLog CreateServiceLog() { return new NLogServiceLog("Name"); }
    }
}
