using Contoso.Abstract.EventSourcing;
using System.Abstract.EventSourcing;
using System.Abstract.IntegationTests;

namespace System.Abstract.Tests.EventSourcing.Integration
{
    public class FileAggregateRootSnapshotStoreTest : AbstractAggregateRootSnapshotStoreTest
    {
        protected override IAggregateRootSnapshotStore CreateAggregateRootSnapshotStore() { return new FileAggregateRootSnapshotStore(); }
    }
}