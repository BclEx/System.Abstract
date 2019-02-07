using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Abstract.AbstractTests.ServiceLog;

namespace System.Abstract.Tests.ServiceLog.Integration
{
    [TestClass]
    public class EmptyServiceLogTest : AbstractServiceLogTest
    {
        protected override IServiceLog CreateServiceLog() =>
            new ServiceLogManager.EmptyServiceLog();
    }
}