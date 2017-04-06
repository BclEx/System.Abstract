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
using Xunit;
using System.Linq;
namespace System.Abstract
{
    public class ServiceDependencyAttributeTests
    {
        private class TestService
        {
            public string NoDependency { get; set; }
            [ServiceDependency("Test")]
            public string Dependency { get; set; }
            public void Method(string noDependency, [ServiceDependency("Test")] string dependency) { }
        }

        //private class TestServiceWithTwoAttributes
        //{
        //    [ServiceDependency, ServiceDependency]
        //    public string Dependency { get; set; }
        //    public void Method([ServiceDependency, ServiceDependency] string dependency) { }
        //}

        //[Fact]
        //[ExpectedException(typeof(ServiceLocatorResolutionException))]
        //public void Resolve_Property_With_Two_Attributes_Should_Throw()
        //{
        //    var property = typeof(TestServiceWithTwoAttributes).GetProperty("Dependency");
        //    ServiceDependencyAttribute.GetServiceDependencies(property);
        //}

        //[Fact]
        //[ExpectedException(typeof(ServiceLocatorResolutionException))]
        //public void Resolve_Parameter_With_Two_Attributes_Should_Throw()
        //{
        //    var parameter = typeof(TestServiceWithTwoAttributes).GetMethod("DependencyMethod").GetParameters()[0];
        //    ServiceDependencyAttribute.GetServiceDependencies(parameter);
        //}

        [Fact]
        public void Resolve_With_No_Dependency()
        {
            var property = typeof(TestService).GetProperty("NoDependency");
            var propertyDependencies = ServiceDependencyAttribute.GetServiceDependencies(property);
            Assert.NotNull(propertyDependencies);
            Assert.Empty(propertyDependencies);
            //
            var parameter = typeof(TestService).GetMethod("Method").GetParameters()[0];
            var parameterDependencies = ServiceDependencyAttribute.GetServiceDependencies(parameter);
            Assert.NotNull(parameterDependencies);
            Assert.Empty(parameterDependencies);
        }

        [Fact]
        public void Resolve_With_Dependency()
        {
            var property = typeof(TestService).GetProperty("Dependency");
            var propertyDependencies = ServiceDependencyAttribute.GetServiceDependencies(property);
            Assert.NotNull(propertyDependencies);
            Assert.Equal("Test", propertyDependencies.First().Name);
            //
            var parameter = typeof(TestService).GetMethod("Method").GetParameters()[1];
            var parameterDependencies = ServiceDependencyAttribute.GetServiceDependencies(parameter);
            Assert.NotNull(parameterDependencies);
            Assert.Equal("Test", parameterDependencies.First().Name);
        }
    }
}