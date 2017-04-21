using Contoso.Abstract.EventSourcing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Abstract.EventSourcing;
using System.Abstract.AbstractTests;

namespace System.Abstract.Tests.EventSourcing.Integration
{
    [TestClass]
    public class FileEventStoreTest : AbstractEventStoreTest
    {
		protected override IEventStore CreateEventStore() { return new FileEventStore(); }

        [TestMethod, TestCategory("EventSource: File")]
        public override void CreateMessage_Should_Return_Valid_Instance() { base.CreateMessage_Should_Return_Valid_Instance(); }
    }
}