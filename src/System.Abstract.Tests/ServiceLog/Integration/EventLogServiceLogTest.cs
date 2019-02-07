using Contoso.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Abstract.AbstractTests.ServiceLog;

namespace System.Abstract.Tests.ServiceLog.Integration
{
    [TestClass]
    public class EventLogServiceLogTest : AbstractServiceLogTest
    {
        protected override IServiceLog CreateServiceLog() =>
            new EventLogServiceLog("Default");
    }
}