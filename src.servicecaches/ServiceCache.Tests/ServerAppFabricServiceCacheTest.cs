using Microsoft.ApplicationServer.Caching.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Abstract;
using System.Abstract.AbstractTests.ServiceCache;

namespace ServiceCache.Tests
{
    [TestClass]
    public class ServerAppFabricServiceCacheTest : AbstractServiceCacheTest
    {
        protected override IServiceCache CreateServiceCache() { return new ServerAppFabricServiceCache(); }
    }
}
