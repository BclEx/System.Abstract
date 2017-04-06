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
using System.Abstract.Fakes;
using Xunit;
namespace System.Abstract
{
    public abstract class ServiceRegistrarIntegrationTestsBase
    {
        protected IServiceLocator Locator { get; private set; }
        protected IServiceRegistrar Registrar { get; private set; }
        protected abstract IServiceLocator CreateServiceLocator();

        public ServiceRegistrarIntegrationTestsBase()
        {
            Locator = CreateServiceLocator();
            Registrar = Locator.Registrar;
        }

        #region Enumerate

        [Fact, Trait("Category", "Template")]
        public virtual void HasRegistered()
        {
            var serviceType = typeof(TestService);
            Registrar.Register<ITestService>(serviceType);
            //
            Assert.False(Registrar.HasRegistered<string>());
            Assert.False(Registrar.HasRegistered(typeof(string)));
            Assert.True(Registrar.HasRegistered<ITestService>());
            Assert.True(Registrar.HasRegistered(serviceType));
        }

        [Fact, Trait("Category", "Template")]
        public virtual void GetRegistrationsFor()
        {
            var serviceType = typeof(TestService);
            Registrar.Register<ITestService>(serviceType);
            //
            Assert.False(Registrar.GetRegistrationsFor(typeof(string)).Any());
            Assert.True(Registrar.GetRegistrationsFor(serviceType).Any());
        }

        #endregion

        #region Register Type

        #endregion

        #region Register Implementation

        [Fact, Trait("Category", "Template")]
        public virtual void Register_With_Specified_Type_Should_Return_Same_Type()
        {
            var serviceType = typeof(TestService);
            Registrar.Register<ITestService>(serviceType);
            //
            var service = Locator.Resolve<ITestService>();
            Assert.IsType(serviceType, service);
            var serviceA = Locator.Resolve(serviceType);
            Assert.IsType(serviceType, serviceA);
        }

        [Fact, Trait("Category", "Template")]
        public virtual void Register_With_Implementation_Type_Should_Return_Same_Type()
        {
            var serviceType = typeof(TestService);
            Registrar.Register<ITestService, TestService>();
            var serviceType2 = typeof(TestNamedService);
            Registrar.Register<ITestNamedService, TestNamedService>(serviceType2.FullName);
            //
            var service = Locator.Resolve<ITestService>();
            Assert.IsType(serviceType, service);
            var service2 = Locator.Resolve<ITestNamedService>(serviceType2.FullName);
            Assert.IsType(serviceType2, service2);
            var serviceA = Locator.Resolve(serviceType);
            Assert.IsType(serviceType, serviceA);
            var service2A = Locator.Resolve(serviceType, serviceType2.FullName);
            Assert.IsType(serviceType2, service2A);
        }

        [Fact, Trait("Category", "Template")]
        public virtual void Register_With_Keyed_Type_Should_Return_Same_Type()
        {
            var serviceType = typeof(TestNamedService);
            Registrar.Register(serviceType, serviceType.FullName);
            //
            var service = Locator.Resolve<TestNamedService>(serviceType.FullName);
            Assert.IsType(serviceType, service);
        }

        [Fact, Trait("Category", "Template")]
        public virtual void Register_With_Specified_Service_And_Type_Should_Return_Same_Type()
        {
            var serviceType = typeof(TestService);
            Registrar.Register(serviceType, serviceType);
            //
            var service = Locator.Resolve<ITestService>(serviceType);
            Assert.IsType(serviceType, service);
        }

        [Fact, Trait("Category", "Template")]
        public virtual void Register_With_Specified_Service_Should_Return_Same_Type()
        {
            var serviceType = typeof(TestService);
            Registrar.Register(serviceType, serviceType);
            //
            var service = Locator.Resolve(serviceType);
            Assert.IsType(serviceType, service);
        }

        #endregion

        #region Register Instance

        [Fact, Trait("Category", "Template")]
        public virtual void RegisterInstance_Generic_Should_Return_Same_Object()
        {
            Registrar.RegisterInstance<ITestService>(new TestService());
            //
            var service = Locator.Resolve<ITestService>();
            Assert.IsType<TestService>(service);
        }
        [Fact, Trait("Category", "Template")]
        public virtual void RegisterInstance_GenericNamed_Should_Return_Same_Object()
        {
            Registrar.RegisterInstance<ITestService>(new TestService(), "name");
            //
            var service = Locator.Resolve<ITestService>("name");
            Assert.IsType<TestService>(service);
        }
        [Fact, Trait("Category", "Template")]
        public virtual void RegisterInstance_Should_Return_Same_Object()
        {
            Registrar.RegisterInstance(typeof(ITestService), new TestService());
            //
            var service = Locator.Resolve<ITestService>();
            Assert.IsType<TestService>(service);
        }
        [Fact, Trait("Category", "Template")]
        public virtual void RegisterInstance_Named_Should_Return_Same_Object()
        {
            Registrar.RegisterInstance(typeof(ITestService), new TestService(), "name");
            //
            var service = Locator.Resolve<ITestService>("name");
            Assert.IsType<TestService>(service);
        }

        //
        [Fact, Trait("Category", "Template")]
        public virtual void RegisterInstance_Should_Return_Same_Object_For_Same_Type()
        {
            Registrar.RegisterInstance(new TestService());
            //
            var service = Locator.Resolve<TestService>();
            Assert.IsType<TestService>(service);
        }

        #endregion

        #region Register Method

        [Fact, Trait("Category", "Template")]
        public virtual void Register_Generic_With_FactoryMethod_Should_Return_Result_From_Factory()
        {
            var firstExpectedObject = new TestService();
            var secondExpectedObject = new TestService();
            var hasBeenCalled = false;
            Registrar.Register<ITestService>(x =>
            {
                if (!hasBeenCalled)
                {
                    hasBeenCalled = true;
                    return firstExpectedObject;
                }
                return secondExpectedObject;
            });
            //
            var first = Locator.Resolve<ITestService>();
            Assert.Same(firstExpectedObject, first);
            //
            var second = Locator.Resolve<ITestService>();
            Assert.Same(secondExpectedObject, second);
        }

        [Fact, Trait("Category", "Template")]
        public virtual void Register_With_FactoryMethod_Should_Return_Result_From_Factory()
        {
            var firstExpectedObject = new TestService();
            var secondExpectedObject = new TestService();
            var hasBeenCalled = false;
            Registrar.Register(typeof(ITestService), x =>
            {
                if (!hasBeenCalled)
                {
                    hasBeenCalled = true;
                    return firstExpectedObject;
                }
                return secondExpectedObject;
            });
            //
            var first = Locator.Resolve<ITestService>();
            Assert.Same(firstExpectedObject, first);
            //
            var second = Locator.Resolve<ITestService>();
            Assert.Same(secondExpectedObject, second);
        }

        #endregion
    }
}