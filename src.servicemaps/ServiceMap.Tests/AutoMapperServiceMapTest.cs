using AutoMapper.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Abstract;
using System.Abstract.AbstractTests.ServiceMap;

namespace ServiceMap.Tests
{
    [TestClass]
    public class AutoMapperServiceMapTest : AbstractServiceMapTest
    {
        protected override IServiceMap CreateServiceMap() { return new AutoMapperServiceMap(); }
    }
}
