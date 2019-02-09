#if !NET35 && !PORTABLE
using Contoso.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Abstract.AbstractTests.ServiceCache;

namespace System.Abstract.Tests.ServiceCache.Integration
{
    [TestClass]
    public class SystemServiceCacheTest : AbstractServiceCacheTest
	{
        protected override IServiceCache CreateServiceCache() =>
            new SystemServiceCache((string)null);
	}
}
#endif