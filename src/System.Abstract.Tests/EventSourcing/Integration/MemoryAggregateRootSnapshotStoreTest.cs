using Contoso.Abstract.EventSourcing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Abstract.EventSourcing;
using System.Abstract.IntegationTests;

namespace System.Abstract.Tests.EventSourcing.Integration
{
    [TestClass]
    public class MemoryAggregateRootSnapshotStoreTest : AbstractAggregateRootSnapshotStoreTest
    {
		protected override IAggregateRootSnapshotStore CreateAggregateRootSnapshotStore() { return new MemoryAggregateRootSnapshotStore(); }

        [TestMethod, TestCategory("EventSource: Memory")]
        public override void CreateMessage_Should_Return_Valid_Instance() { base.CreateMessage_Should_Return_Valid_Instance(); }
    }
}