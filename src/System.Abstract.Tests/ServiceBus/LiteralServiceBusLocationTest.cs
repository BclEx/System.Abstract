using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Abstract.Tests.ServiceBus
{
    [TestClass]
    public class LiteralServiceBusLocationTest
    {
        [TestMethod, TestCategory("Core: ServiceBus"), ExpectedException(typeof(ArgumentNullException), "literal")]
        public void New_Throws_On_Null_Location()
        {
            var instance = new LiteralServiceBusEndpoint(null);
        }

        [TestMethod, TestCategory("Core: ServiceBus")]
        public void Value_Returns_Value_Set_In_Constructor()
        {
            var instance = new LiteralServiceBusEndpoint("Test");
            Assert.AreEqual("Test", instance.Value);
        }
    }
}