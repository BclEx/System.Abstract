using Enyim.Caching.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Abstract;
using System.Abstract.AbstractTests.ServiceCache;

namespace ServiceCache.Tests
{
    [TestClass]
    public class MemcachedServiceCacheTest : AbstractServiceCacheTest
    {
        protected override IServiceCache CreateServiceCache() { return new MemcachedServiceCache(); }
    }
}
