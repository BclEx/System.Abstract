#if NET45
using Contoso.Abstract;
using System.Abstract.IntegationTests.ServiceCache;

namespace Contoso.Abstract
{
    public class SystemServiceCacheTest : AbstractServiceCacheTest
	{
        protected override IServiceCache CreateServiceCache() { return new SystemServiceCache((string)null); }
	}
}
#endif