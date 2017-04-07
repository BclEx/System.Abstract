using Contoso.Abstract;
using System.Abstract.IntegationTests.ServiceCache;

namespace System.Abstract.Tests.ServiceCache.Integration
{
    public class StaticServiceCacheTest : AbstractServiceCacheTest
    {
		protected override IServiceCache CreateServiceCache() { return new StaticServiceCache(); }
    }
}