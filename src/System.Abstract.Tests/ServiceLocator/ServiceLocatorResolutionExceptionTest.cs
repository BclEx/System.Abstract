using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Abstract.Tests.ServiceLocator
{
    [TestClass]
    public class ServiceLocatorResolutionExceptionTest
    {
        [TestMethod, TestCategory("Core: ServiceLocator"), ExpectedException(typeof(ServiceLocatorResolutionException), "provider")]
        public void Create_Instance_With_Type()
        {
            var exception = new ServiceLocatorResolutionException(typeof(string));
            Assert.AreEqual(exception.ServiceType, typeof(string));
            Assert.AreEqual(exception.Message, "Could not resolve serviceType 'System.String'");
            throw exception;
        }

        [TestMethod, TestCategory("Core: ServiceLocator"), ExpectedException(typeof(ServiceLocatorResolutionException), "provider")]
        public void Create_Instance_With_Type_And_InnerException()
        {
            var operationException = new InvalidOperationException();
            var exception = new ServiceLocatorResolutionException(typeof(string), operationException);
            Assert.AreEqual(exception.ServiceType, typeof(string));
            Assert.AreEqual(exception.InnerException, operationException);
            Assert.AreEqual(exception.Message, "Could not resolve serviceType 'System.String'");
            throw exception;
        }
    }
}