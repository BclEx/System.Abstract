using Microsoft.VisualStudio.TestTools.UnitTesting;
using StackExchange.Redis.Abstract;
using System.Abstract;
using System.Abstract.AbstractTests.ServiceCache;

namespace ServiceCache.Tests
{
    [TestClass]
    public class RedisServiceCacheTest : AbstractServiceCacheTest
    {
        protected override IServiceCache CreateServiceCache() { return new RedisServiceCache(""); }
    }
}
