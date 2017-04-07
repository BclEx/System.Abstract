using Contoso.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Abstract.IntegationTests.ServiceLog;
using System.IO;

namespace System.Abstract.Tests.ServiceLog.Integration
{
    [TestClass]
    public class StreamServiceLogTest : AbstractServiceLogTest
    {
        public StreamServiceLogTest()
        {
            Stream = new MemoryStream();
        }

        protected MemoryStream Stream { get; private set; }
        protected override IServiceLog CreateServiceLog() { return new StreamServiceLog("Default", Stream); }
    }
}