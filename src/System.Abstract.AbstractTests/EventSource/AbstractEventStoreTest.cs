using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Abstract.EventSourcing;

namespace System.Abstract.AbstractTests
{
    [TestClass]
    public abstract class AbstractEventStoreTest
	{
		protected IEventStore EventStore { get; private set; }
		protected abstract IEventStore CreateEventStore();

        public AbstractEventStoreTest()
		{
			EventStore = CreateEventStore();
		}

        [TestMethod]
		public virtual void CreateMessage_Should_Return_Valid_Instance()
		{
		}
	}
}