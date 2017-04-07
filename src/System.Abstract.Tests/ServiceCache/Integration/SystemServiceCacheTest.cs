#if NET45
using Contoso.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Abstract.IntegationTests.ServiceCache;

namespace Contoso.Abstract
{
    [TestClass]
    public class SystemServiceCacheTest : AbstractServiceCacheTest
	{
        protected override IServiceCache CreateServiceCache() { return new SystemServiceCache((string)null); }
	}
}
#endif