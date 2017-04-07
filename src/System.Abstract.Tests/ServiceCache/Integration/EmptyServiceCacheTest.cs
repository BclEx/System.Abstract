using Contoso.Abstract;
using System.Abstract.IntegationTests.ServiceCache;

namespace System.Abstract.Tests.ServiceCache.Integration
{
    public class EmptyServiceCacheTest : AbstractServiceCacheTest
    {
        protected override IServiceCache CreateServiceCache() { return new EmptyServiceCache(); }
    }
}