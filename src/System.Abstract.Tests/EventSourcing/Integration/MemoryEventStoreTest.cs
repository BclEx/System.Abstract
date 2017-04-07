﻿using Contoso.Abstract.EventSourcing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Abstract.EventSourcing;
using System.Abstract.IntegationTests;

namespace System.Abstract.Tests.EventSourcing.Integration
{
    [TestClass]
    public class MemoryEventStoreTest : AbstractEventStoreTest
    {
		protected override IEventStore CreateEventStore() { return new MemoryEventStore(); }

        [TestMethod, TestCategory("Memory")]
        public override void CreateMessage_Should_Return_Valid_Instance() { base.CreateMessage_Should_Return_Valid_Instance(); }
    }
}