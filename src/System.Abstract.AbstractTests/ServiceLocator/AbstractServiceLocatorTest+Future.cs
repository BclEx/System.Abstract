using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace System.Abstract.IntegationTests.ServiceLocator
{
    public abstract partial class AbstractServiceLocatorTest
    {
        protected virtual void RegisterForFutureTests()
        {
            Registrar.Register<ITestService, TestServiceFuture>();
            Registrar.Register<ITestFutureService, TestFutureService>();
            //Registrar.Register<ITestNamedService, TestNamedService>(typeof(TestNamedService).FullName);
            //Registrar.Register<ITestNamedService, TestNamedService2>(typeof(TestNamedService2).FullName);
        }

        [TestMethod]
        public virtual void Future_Registration_Resolve_Should_Return_Valid_Instance()
        {
            var serviceType = typeof(TestServiceFuture);
            var futureServiceType = typeof(TestFutureService);
            RegisterForFutureTests();
            //
            var service = Locator.Resolve<ITestService>();
            Assert.IsNotNull(service);
            Assert.AreSame(serviceType, service.GetType());
            var futureService = Locator.Resolve<ITestFutureService>();
            Assert.IsNotNull(futureService);
            Assert.AreSame(serviceType, futureServiceType);
            // non-generic
            var serviceN = Locator.Resolve(typeof(ITestService));
            Assert.IsNotNull(serviceN);
            Assert.AreSame(serviceType, serviceN.GetType());
            var futureServiceN = Locator.Resolve(typeof(ITestFutureService));
            Assert.IsNotNull(futureServiceN);
            Assert.AreSame(futureServiceType, futureServiceN.GetType());
        }

        [TestMethod]
        public virtual void Future_Registration_Ask_For_Named_Instance()
        {
            RegisterForFutureTests();
            //
            var serviceType = typeof(TestNamedService);
            var serviceType2 = typeof(TestNamedService2);
            var service = Locator.Resolve<ITestNamedService>(serviceType.FullName);
            Assert.AreSame(service.GetType(), serviceType);
            var service2 = Locator.Resolve<ITestNamedService>(serviceType2.FullName);
            Assert.AreSame(service2.GetType(), serviceType2);
            // non-generic
            var serviceN = Locator.Resolve(typeof(ITestNamedService), serviceType.FullName);
            Assert.AreSame(serviceN.GetType(), serviceType);
            var serviceN2 = Locator.Resolve(typeof(ITestNamedService), serviceType2.FullName);
            Assert.AreSame(serviceN2.GetType(), serviceType2);
        }

        [TestMethod]
        public virtual void Future_Registration_ResolveAll_Should_Return_All_Registered_Services()
        {
            RegisterForFutureTests();
            //
            var services = Locator.ResolveAll<ITestNamedService>();
            Assert.AreEqual(2, services.Count());
            // non-generic
            var servicesN = Locator.ResolveAll(typeof(ITestNamedService));
            Assert.AreEqual(2, servicesN.Count());
        }
    }
}