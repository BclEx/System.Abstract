using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Abstract.AbstractTests.ServiceLocator;
using System.Linq;

namespace System.Abstract.Tests.ServiceLocator
{
    [TestClass]
    public class ServiceLocatorExtensionsTest
    {
        [TestMethod, TestCategory("Core: ServiceLocator")]
        public void GetServiceLocatorGeneric_Returns_Generic()
        {
            var mock = new Mock<IServiceLocator>();
            var serviceLocator = mock.Object;
            //
            var resolvedServiceLocator = serviceLocator.GetServiceLocator<IServiceLocator>();
            Assert.IsTrue(typeof(IServiceLocator).IsAssignableFrom(resolvedServiceLocator.GetType()));
        }

        [TestMethod, TestCategory("Core: ServiceLocator")]
        public void ResolveGeneric_With_ServiceType_Returns_Generic()
        {
            var testServiceType = typeof(TestService);
            var mock = new Mock<IServiceLocator>();
            mock.Setup(x => x.Resolve(It.IsAny<Type>())).Returns(new TestService { });
            var serviceLocator = mock.Object;
            //
            Assert.AreSame(testServiceType, serviceLocator.Resolve<TestService>(testServiceType).GetType());
        }

        [TestMethod, TestCategory("Core: ServiceLocator")]
        public void ResolveGeneric_With_ServiceType_And_Name_Returns_Generic()
        {
            var testServiceType = typeof(TestService);
            var mock = new Mock<IServiceLocator>();
            mock.Setup(x => x.Resolve(It.IsAny<Type>(), It.IsAny<string>())).Returns(new TestService { });
            var serviceLocator = mock.Object;
            //
            Assert.AreSame(testServiceType, serviceLocator.Resolve<TestService>(testServiceType, "name").GetType());
        }

        [TestMethod, TestCategory("Core: ServiceLocator")]
        public void ResolveAll_With_ServiceType_Returns_Collection()
        {
            var testServiceType = typeof(TestService);
            var mock = new Mock<IServiceLocator>();
            mock.Setup(x => x.ResolveAll(It.IsAny<Type>())).Returns(new[] { new TestService { } });
            var serviceLocator = mock.Object;
            //
            var services = serviceLocator.ResolveAll<TestService>(testServiceType);
            Assert.AreEqual(1, services.Count());
            Assert.AreSame(testServiceType, services.First().GetType());
        }
    }
}