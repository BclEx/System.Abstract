using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog.Abstract;
using System.Abstract.IntegationTests.ServiceLog;

namespace System.Abstract.Tests.ServiceLocator.Integration
{
    [TestClass]
    public class NLogServiceLogTest : AbstractServiceLogTest
    {
        protected override IServiceLog CreateServiceLog() { return new NLogServiceLog("Name"); }
    }
}
