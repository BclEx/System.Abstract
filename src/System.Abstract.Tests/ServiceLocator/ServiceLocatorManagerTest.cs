using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Abstract.Tests.ServiceLocator
{
    [TestClass]
    public class ServiceLocatorManagerTest
    {
        [TestMethod, TestCategory("Core: ServiceLocator"), ExpectedException(typeof(ArgumentNullException), "provider")]
        public void Null_Provider_Throws_ArgumentNullException_Exception()
        {
            ServiceLocatorManager.SetProvider(null);
        }

        [TestMethod, TestCategory("Core: ServiceLocator"), ExpectedException(typeof(NullReferenceException), "instance")]
        public void Null_Provider_Throws_InvalidOperation_Exception()
        {
            ServiceLocatorManager.SetProvider(() => null);
            var service = ServiceLocatorManager.Current;
        }
    }
}