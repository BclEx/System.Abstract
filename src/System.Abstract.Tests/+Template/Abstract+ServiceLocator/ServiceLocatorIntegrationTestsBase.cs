#region License
/*
The MIT License

Copyright (c) 2008 Sky Morey

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion
using System.Linq;
using System.Collections.Generic;
using System.Abstract.Fakes;
using Xunit;
namespace System.Abstract
{
    public abstract partial class ServiceLocatorIntegrationTestsBase
    {
        protected IServiceLocator Locator { get; private set; }
        protected IServiceRegistrar Registrar { get; private set; }
        protected abstract IServiceLocator CreateServiceLocator();

        public ServiceLocatorIntegrationTestsBase()
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

        //Test if child works

        [Fact, Trait("Category", "Template")]
        public void Resolve_Should_Return_Valid_Instance()
        {
            var serviceType = typeof(TestService);
            var service = Locator.Resolve<ITestService>();
            Assert.NotNull(service);
            Assert.IsType(serviceType, service);
            // non-generic
            var serviceN = Locator.Resolve(typeof(ITestService));
            Assert.NotNull(serviceN);
            Assert.IsType(serviceType, serviceN);
        }

        [Fact, Trait("Category", "Template")]
        public virtual void GenericAndNonGeneric_Resolve_Method_Should_Return_Same_Instance_Type()
        {
            var serviceType = Locator.Resolve<ITestService>().GetType();
            // non-generic
            var serviceTypeN = Locator.Resolve(typeof(ITestService)).GetType();
            Assert.Equal(serviceType, serviceTypeN);
        }

        [Fact, Trait("Category", "Template")]
        public void Asking_For_UnRegistered_Service_Return_Valid_Instance()
        {
            var service = Locator.Resolve<TestServiceN>();
            Assert.NotNull(service);
            // non-generic
            var serviceN = Locator.Resolve(typeof(TestServiceN));
            Assert.NotNull(serviceN);
        }

        [Fact, Trait("Category", "Template")]
        public void Asking_For_Invalid_Service_Should_Raise_Exception()
        {
            Assert.Throws<ServiceLocatorResolutionException>(() =>
            {
                Locator.Resolve<string>();
            });
            // non-generic
            Assert.Throws<ServiceLocatorResolutionException>(() =>
            {
                Locator.Resolve(typeof(string));
            });
        }

        #region Named Instances

        [Fact, Trait("Category", "Template")]
        public void Ask_For_Named_Instance()
        {
            var serviceType = typeof(TestNamedService);
            var serviceType2 = typeof(TestNamedService2);
            var service = Locator.Resolve<ITestNamedService>(serviceType.FullName);
            Assert.IsType(serviceType, service);
            var service2 = Locator.Resolve<ITestNamedService>(serviceType2.FullName);
            Assert.IsType(serviceType2, service2);
            // non-generic
            var serviceN = Locator.Resolve(typeof(ITestNamedService), serviceType.FullName);
            Assert.IsType(serviceType, serviceN);
            var serviceN2 = Locator.Resolve(typeof(ITestNamedService), serviceType2.FullName);
            Assert.IsType(serviceType2, serviceN2);
        }

        //[Fact, Trait("Category", "Template")]
        //public virtual void GenericAndNonGeneric_Resolve_Named_Instance_Should_Return_Same_Instance_Type()
        //{
        //    Assert.Equal(
        //        Locator.Resolve<ITestNamedService>(typeof(TestNamedService).FullName).GetType(),
        //        Locator.Resolve<ITestNamedService>(typeof(TestNamedService)).GetType());
        //}

        [Fact, Trait("Category", "Template")]
        public void Ask_For_Unknown_Service_Should_Throw_Exception()
        {
            Assert.Throws<ServiceLocatorResolutionException>(() =>
            {
                Locator.Resolve<ITestNamedService>("BAD-ID");
            });
            // non-generic
            Assert.Throws<ServiceLocatorResolutionException>(() =>
            {
                Locator.Resolve(typeof(ITestNamedService), "BAD-ID");
            });
        }

        #endregion

        #region ResolveAll

        [Fact, Trait("Category", "Template")]
        public virtual void ResolveAll_Should_Return_All_Registered_UnNamed_Services()
        {
            var services = Locator.ResolveAll<ITestService>();
            Assert.Equal(1, services.Count());
            // non-generic
            var servicesN = Locator.ResolveAll(typeof(ITestService));
            Assert.Equal(1, servicesN.Count());
        }

        [Fact, Trait("Category", "Template")]
        public virtual void ResolveAll_Should_Return_All_Registered_Named_Services()
        {
            var services2 = Locator.ResolveAll<ITestNamedService>();
            Assert.Equal(2, services2.Count());
            // non-generic
            var servicesN2 = Locator.ResolveAll(typeof(ITestNamedService));
            Assert.Equal(2, servicesN2.Count());
        }

        [Fact, Trait("Category", "Template")]
        public virtual void ResolveAll_For_Unknown_Type_Should_Return_Empty_Enumerable()
        {
            var services = Locator.ResolveAll<string>();
            Assert.Equal(0, services.Count());
            // non-generic
            var servicesN = Locator.ResolveAll(typeof(string));
            Assert.Equal(0, servicesN.Count());
        }

        [Fact, Trait("Category", "Template")]
        public virtual void GenericAndNonGeneric_ResolveAll_Should_Return_Same_Instace_Types()
        {
            var services = new List<ITestNamedService>(Locator.ResolveAll<ITestNamedService>());
            // non-generic
            var servicesN = new List<ITestNamedService>(Locator.ResolveAll(typeof(ITestNamedService)).Cast<ITestNamedService>());
            for (int index = 0; index < services.Count; index++)
                Assert.Equal(services[index].GetType(), servicesN[index].GetType());
        }

        #endregion
    }
}