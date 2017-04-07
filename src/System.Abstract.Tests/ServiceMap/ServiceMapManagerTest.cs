using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Abstract.Tests.ServiceMap
{
    [TestClass]
    public class ServiceMapManagerTest
    {
        [TestMethod, ExpectedException(typeof(ArgumentNullException), "provider")]
        public void Null_Provider_Throws_ArgumentNullException_Exception()
        {
            ServiceMapManager.SetProvider(null);
        }

        [TestMethod, ExpectedException(typeof(NullReferenceException), "instance")]
        public void Null_Provider_Throws_InvalidOperation_Exception()
        {
            ServiceMapManager.SetProvider(() => null);
            var service = ServiceMapManager.Current;
        }
    }
}