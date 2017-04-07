using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace System.Abstract.IntegationTests.ServiceLocator
{
    [TestClass]
    public abstract partial class AbstractServiceLocatorTest
    {
        protected IServiceLocator Locator { get; private set; }
        protected IServiceRegistrar Registrar { get; private set; }
        protected abstract IServiceLocator CreateServiceLocator();

        public AbstractServiceLocatorTest()
        {
            Locator = CreateServiceLocator();
            Registrar = Locator.Registrar;
            RegisterForTests();
        }

        protected virtual void RegisterForTests()
        {
            Registrar.Register<ITestService, TestService>();
            Registrar.Register<ITestNamedService, TestNamedService>(typeof(TestNamedService).FullName);
            Registrar.Register<ITestNamedService, TestNamedService2>(typeof(TestNamedService2).FullName);
        }

        [TestMethod, TestCategory("Integration")]
        public virtual void Resolve_Should_Return_Valid_Instance()
        {
            var serviceType = typeof(TestService);
            var service = Locator.Resolve<ITestService>();
            Assert.IsNotNull(service);
            Assert.AreSame(serviceType, service.GetType());
            // non-generic
            var serviceN = Locator.Resolve(typeof(ITestService));
            Assert.IsNotNull(serviceN);
            Assert.AreSame(serviceType, serviceN.GetType());
        }

        [TestMethod, TestCategory("Integration")]
        public virtual void GenericAndNonGeneric_Resolve_Method_Should_Return_Same_Instance_Type()
        {
            var serviceType = Locator.Resolve<ITestService>().GetType();
            // non-generic
            var serviceTypeN = Locator.Resolve(typeof(ITestService)).GetType();
            Assert.AreEqual(serviceType, serviceTypeN);
        }

        [TestMethod, TestCategory("Integration")]
        public virtual void Asking_For_UnRegistered_Service_Return_Valid_Instance()
        {
            var service = Locator.Resolve<TestServiceN>();
            Assert.IsNotNull(service);
            // non-generic
            var serviceN = Locator.Resolve(typeof(TestServiceN));
            Assert.IsNotNull(serviceN);
        }

        [TestMethod, TestCategory("Integration"), ExpectedException(typeof(ServiceLocatorResolutionException))]
        public virtual void Asking_For_Invalid_Service_Should_Raise_Exception()
        {
            Locator.Resolve<string>();
        }

        [TestMethod, TestCategory("Integration"), ExpectedException(typeof(ServiceLocatorResolutionException))]
        public virtual void Asking_For_Invalid_Service_Should_Raise_Exception2()
        {
            Locator.Resolve(typeof(string));
        }

        #region Named Instances

        [TestMethod, TestCategory("Integration")]
        public virtual void Ask_For_Named_Instance()
        {
            var serviceType = typeof(TestNamedService);
            var serviceType2 = typeof(TestNamedService2);
            var service = Locator.Resolve<ITestNamedService>(serviceType.FullName);
            Assert.AreSame(serviceType, service.GetType());
            var service2 = Locator.Resolve<ITestNamedService>(serviceType2.FullName);
            Assert.AreSame(serviceType2, service2.GetType());
            // non-generic
            var serviceN = Locator.Resolve(typeof(ITestNamedService), serviceType.FullName);
            Assert.AreSame(serviceType, serviceN.GetType());
            var serviceN2 = Locator.Resolve(typeof(ITestNamedService), serviceType2.FullName);
            Assert.AreSame(serviceType2, serviceN2.GetType());
        }

        [TestMethod, TestCategory("Integration")]
        public virtual void GenericAndNonGeneric_Resolve_Named_Instance_Should_Return_Same_Instance_Type()
        {
            var serviceType1 = Locator.Resolve<ITestNamedService>(typeof(TestNamedService).FullName).GetType();
            var serviceType2 = Locator.Resolve<ITestNamedService>(typeof(TestNamedService)).GetType();
            Assert.AreEqual(serviceType1, serviceType2);
        }

        [TestMethod, TestCategory("Integration"), ExpectedException(typeof(ServiceLocatorResolutionException))]
        public virtual void Ask_For_Unknown_Service_Should_Throw_Exception()
        {
            Locator.Resolve<ITestNamedService>("BAD-ID");
        }

        [TestMethod, TestCategory("Integration"), ExpectedException(typeof(ServiceLocatorResolutionException))]
        public virtual void Ask_For_Unknown_Service_Should_Throw_Exception2()
        {
            Locator.Resolve(typeof(ITestNamedService), "BAD-ID");
        }

        #endregion

        #region ResolveAll

        [TestMethod, TestCategory("Integration")]
        public virtual void ResolveAll_Should_Return_All_Registered_UnNamed_Services()
        {
            var services = Locator.ResolveAll<ITestService>();
            Assert.AreEqual(1, services.Count());
            // non-generic
            var servicesN = Locator.ResolveAll(typeof(ITestService));
            Assert.AreEqual(1, servicesN.Count());
        }

        [TestMethod, TestCategory("Integration")]
        public virtual void ResolveAll_Should_Return_All_Registered_Named_Services()
        {
            var services2 = Locator.ResolveAll<ITestNamedService>();
            Assert.AreEqual(2, services2.Count());
            // non-generic
            var servicesN2 = Locator.ResolveAll(typeof(ITestNamedService));
            Assert.AreEqual(2, servicesN2.Count());
        }

        [TestMethod, TestCategory("Integration")]
        public virtual void ResolveAll_For_Unknown_Type_Should_Return_Empty_Enumerable()
        {
            var services = Locator.ResolveAll<string>();
            Assert.AreEqual(0, services.Count());
            // non-generic
            var servicesN = Locator.ResolveAll(typeof(string));
            Assert.AreEqual(0, servicesN.Count());
        }

        [TestMethod, TestCategory("Integration")]
        public virtual void GenericAndNonGeneric_ResolveAll_Should_Return_Same_Instace_Types()
        {
            var services = new List<ITestNamedService>(Locator.ResolveAll<ITestNamedService>());
            // non-generic
            var servicesN = new List<ITestNamedService>(Locator.ResolveAll(typeof(ITestNamedService)).Cast<ITestNamedService>());
            for (int index = 0; index < services.Count; index++)
                Assert.AreEqual(services[index].GetType(), servicesN[index].GetType());
        }

        #endregion
    }
}