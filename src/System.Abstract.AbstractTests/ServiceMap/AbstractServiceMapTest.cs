using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Abstract.AbstractTests.ServiceMap
{
    [TestClass]
    public abstract class AbstractServiceMapTest
	{
		protected IServiceMap Map { get; private set; }
		protected abstract IServiceMap CreateServiceMap();

        public AbstractServiceMapTest()
		{
            Map = CreateServiceMap();
		}
	}
}