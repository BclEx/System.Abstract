using Contoso.Abstract.EventSourcing;
using System.Abstract.EventSourcing;
using System.Abstract.IntegationTests;

namespace System.Abstract.Tests.EventSourcing.Integration
{
    public class MemoryEventStoreTest : AbstractEventStoreTest
    {
		protected override IEventStore CreateEventStore() { return new MemoryEventStore(); }
    }
}