using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Abstract.Tests.ServiceLog
{
    [TestClass]
    public class ServiceLogManagerTest
    {
        [TestMethod, ExpectedException(typeof(ArgumentNullException), "provider")]
        public void Null_Provider_Throws_ArgumentNullException_Exception()
        {
            ServiceLogManager.SetProvider(null);
        }

        [TestMethod, ExpectedException(typeof(NullReferenceException), "instance")]
        public void Null_Provider_Throws_InvalidOperation_Exception()
        {
            ServiceLogManager.SetProvider(() => null);
            var service = ServiceLogManager.Current;
        }
    }
}