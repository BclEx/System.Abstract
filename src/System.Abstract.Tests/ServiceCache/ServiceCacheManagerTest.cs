using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Abstract.Tests.ServiceCache
{
    [TestClass]
    public class ServiceCacheManagerTest
    {
        [TestMethod, ExpectedException(typeof(ArgumentNullException), "provider")]
        public void Null_Provider_Throws_ArgumentNullException_Exception()
        {
            ServiceCacheManager.SetProvider(null);
        }

        [TestMethod, ExpectedException(typeof(NullReferenceException), "instance")]
        public void Null_Provider_Throws_InvalidOperation_Exception()
        {
            ServiceCacheManager.SetProvider(() => null);
            var service = ServiceCacheManager.Current;
        }
    }
}