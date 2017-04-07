using Contoso.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Abstract.IntegationTests.ServiceCache;

namespace System.Abstract.Tests.ServiceCache.Integration
{
    [TestClass]
    public class EmptyServiceCacheTest : AbstractServiceCacheTest
    {
        protected override IServiceCache CreateServiceCache() { return new EmptyServiceCache(); }
    }
}