using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Abstract.Fakes;
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
            var serviceLocator = new StubIServiceLocator();
            //
            var resolvedServiceLocator = serviceLocator.GetServiceLocator<IServiceLocator>();
            Assert.IsTrue(typeof(IServiceLocator).IsAssignableFrom(resolvedServiceLocator.GetType()));
        }

        [TestMethod, TestCategory("Core: ServiceLocator")]
        public void ResolveGeneric_With_ServiceType_Returns_Generic()
        {
            var testServiceType = typeof(TestService);
            var serviceLocator = new StubIServiceLocator
            {
                ResolveType = serviceType => new TestService { },
            };
            //
            Assert.AreSame(testServiceType, serviceLocator.Resolve<TestService>(testServiceType).GetType());
        }

        [TestMethod, TestCategory("Core: ServiceLocator")]
        public void ResolveGeneric_With_ServiceType_And_Name_Returns_Generic()
        {
            var testServiceType = typeof(TestService);
            var serviceLocator = new StubIServiceLocator
            {
                ResolveTypeString = (serviceType, name) => new TestService { },
            };
            //
            Assert.AreSame(testServiceType, serviceLocator.Resolve<TestService>(testServiceType, "name").GetType());
        }

        [TestMethod, TestCategory("Core: ServiceLocator")]
        public void ResolveAll_With_ServiceType_Returns_Collection()
        {
            var testServiceType = typeof(TestService);
            var serviceLocator = new StubIServiceLocator
            {
                ResolveAllType = serviceType => new[] { new TestService { } },
            };
            //
            var services = serviceLocator.ResolveAll<TestService>(testServiceType);
            Assert.AreEqual(1, services.Count());
            Assert.AreSame(testServiceType, services.First().GetType());
        }
    }
}