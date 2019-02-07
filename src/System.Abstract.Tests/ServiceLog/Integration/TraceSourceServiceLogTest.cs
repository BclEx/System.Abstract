using Contoso.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Abstract.AbstractTests.ServiceLog;

namespace System.Abstract.Tests.ServiceLog.Integration
{
    [TestClass]
    public class TraceSourceServiceLogTest : AbstractServiceLogTest
    {
        protected override IServiceLog CreateServiceLog() =>
            new TraceSourceServiceLog("Default");
    }
}