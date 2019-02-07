using Contoso.Abstract.EventSourcing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Abstract.AbstractTests;
using System.Abstract.EventSourcing;

namespace System.Abstract.Tests.EventSourcing.Integration
{
    [TestClass]
    public class MemoryAggregateRootSnapshotStoreTest : AbstractAggregateRootSnapshotStoreTest
    {
        protected override IAggregateRootSnapshotStore CreateAggregateRootSnapshotStore() =>
            new MemoryAggregateRootSnapshotStore();

        [TestMethod, TestCategory("EventSource: Memory")]
        public override void CreateMessage_Should_Return_Valid_Instance() => base.CreateMessage_Should_Return_Valid_Instance();
    }
}