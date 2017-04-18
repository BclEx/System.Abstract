using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Abstract.Tests.ServiceLocator
{
    [TestClass]
    public class ServiceDependencyAttributeTest
    {
        private class TestServiceDependency
        {
            public string NoDependency { get; set; }
            [ServiceDependency("Test")]
            public string Dependency { get; set; }
            public void Method(string noDependency, [ServiceDependency("Test")] string dependency) { }
        }

        //private class TestServiceDependencyWithTwoAttributes
        //{
        //    [ServiceDependency, ServiceDependency]
        //    public string Dependency { get; set; }
        //    public void Method([ServiceDependency, ServiceDependency] string dependency) { }
        //}

        //[TestMethod, TestCategory("Core: ServiceLocator"), ExpectedException(typeof(ServiceLocatorResolutionException))]
        //public void Resolve_Property_With_Two_Attributes_Should_Throw()
        //{
        //    var property = typeof(TestServiceDependencyWithTwoAttributes).GetProperty("Dependency");
        //    ServiceDependencyAttribute.GetServiceDependencies(property);
        //}

        //[TestMethod, TestCategory("Core: ServiceLocator"), ExpectedException(typeof(ServiceLocatorResolutionException))]
        //public void Resolve_Parameter_With_Two_Attributes_Should_Throw()
        //{
        //    var parameter = typeof(TestServiceDependencyWithTwoAttributes).GetMethod("DependencyMethod").GetParameters()[0];
        //    ServiceDependencyAttribute.GetServiceDependencies(parameter);
        //}

        [TestMethod, TestCategory("Core: ServiceLocator")]
        public void Resolve_With_No_Dependency()
        {
            var property = typeof(TestServiceDependency).GetProperty("NoDependency");
            var propertyDependencies = ServiceDependencyAttribute.GetServiceDependencies(property);
            Assert.IsNotNull(propertyDependencies);
            Assert.IsTrue(propertyDependencies.Count() == 0);
            //
            var parameter = typeof(TestServiceDependency).GetMethod("Method").GetParameters()[0];
            var parameterDependencies = ServiceDependencyAttribute.GetServiceDependencies(parameter);
            Assert.IsNotNull(parameterDependencies);
            Assert.IsTrue(parameterDependencies.Count() == 0);
        }

        [TestMethod, TestCategory("Core: ServiceLocator")]
        public void Resolve_With_Dependency()
        {
            var property = typeof(TestServiceDependency).GetProperty("Dependency");
            var propertyDependencies = ServiceDependencyAttribute.GetServiceDependencies(property);
            Assert.IsNotNull(propertyDependencies);
            Assert.AreEqual("Test", propertyDependencies.First().Name);
            //
            var parameter = typeof(TestServiceDependency).GetMethod("Method").GetParameters()[1];
            var parameterDependencies = ServiceDependencyAttribute.GetServiceDependencies(parameter);
            Assert.IsNotNull(parameterDependencies);
            Assert.AreEqual("Test", parameterDependencies.First().Name);
        }
    }
}