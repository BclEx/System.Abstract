﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Abstract.EventSourcing;

namespace System.Abstract.IntegationTests
{
    [TestClass]
	public abstract class AbstractAggregateRootSnapshotStoreTest
	{
		protected IAggregateRootSnapshotStore AggregateRootSnapshotStore { get; private set; }
		protected abstract IAggregateRootSnapshotStore CreateAggregateRootSnapshotStore();

        public AbstractAggregateRootSnapshotStoreTest()
		{
			AggregateRootSnapshotStore = CreateAggregateRootSnapshotStore();
		}

        [TestMethod, TestCategory("Integration")]
		public void CreateMessage_Should_Return_Valid_Instance()
		{
		}

        [TestMethod]
        public void ATest()
        {
        }
	}
}