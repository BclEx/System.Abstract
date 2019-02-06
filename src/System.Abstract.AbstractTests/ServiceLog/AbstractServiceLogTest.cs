using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Abstract.AbstractTests.ServiceLog
{
    [TestClass]
    public abstract class AbstractServiceLogTest
	{
		protected IServiceLog Log { get; private set; }
		protected abstract IServiceLog CreateServiceLog();

        public AbstractServiceLogTest() =>
			Log = CreateServiceLog();
	}
}