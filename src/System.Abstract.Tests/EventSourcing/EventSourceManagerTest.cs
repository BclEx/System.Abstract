using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Abstract.Tests.EventSourcing
{
    [TestClass]
    public class EventSourceManagerTest
    {
        [TestMethod, ExpectedException(typeof(ArgumentNullException), "provider")]
        public void Null_Provider_Throws_ArgumentNullException_Exception()
        {
            EventSourceManager.SetProvider(null);
        }

        [TestMethod, ExpectedException(typeof(NullReferenceException), "instance")]
        public void Null_Provider_Throws_InvalidOperation_Exception()
        {
            EventSourceManager.SetProvider(() => null);
            var service = EventSourceManager.Current;
        }
    }
}