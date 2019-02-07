using Contoso.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Abstract.AbstractTests.ServiceCache;

namespace System.Abstract.Tests.ServiceCache.Integration
{
    [TestClass]
    public class StaticServiceCacheTest : AbstractServiceCacheTest
    {
		protected override IServiceCache CreateServiceCache() =>
            new StaticServiceCache();
    }
}