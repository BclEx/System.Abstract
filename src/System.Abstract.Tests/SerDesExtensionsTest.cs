using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

// https://msdn.microsoft.com/en-us/library/hh549175.aspx
namespace System.Abstract.Tests
{
    [TestClass]
    public class SerDesExtensionsTest
    {
        [TestMethod]
        public void Des_Returns_Value()
        {
            var serDes = new Fakes.StubISerDes();
            serDes.DesOf1TypeStream<string>((t, s) => "test");
            //
            Assert.AreEqual("test", serDes.Des<string>(typeof(SerDesExtensionsTest), "123"));
            Assert.AreEqual("test", serDes.Des<string>(typeof(SerDesExtensionsTest), "123", Encoding.UTF8));
            Assert.AreEqual("test", serDes.DesBase64<string>(typeof(SerDesExtensionsTest), Convert.ToBase64String(new byte[] { 1, 2, 3 })));
        }

        [TestMethod]
        public void Ser_Returns_Value()
        {
            var base64 = Convert.ToBase64String(new byte[] { 1, 2, 3 });
            var serDes = new Fakes.StubISerDes();
            serDes.SerOf1TypeStreamM0<string>((t, s, g) =>
            {
                if (g == "123")
                    s.Write(new[] { (byte)'t', (byte)'e', (byte)'s', (byte)'t' }, 0, 4);
                else
                    s.Write(new byte[] { 1, 2, 3 }, 0, 3);
            });
            //
            Assert.AreEqual("test", serDes.Ser<string>(typeof(SerDesExtensionsTest), "123"));
            Assert.AreEqual("test", serDes.Ser<string>(typeof(SerDesExtensionsTest), "123", Encoding.UTF8));
            Assert.AreEqual(base64, serDes.SerBase64<string>(typeof(SerDesExtensionsTest), base64));
        }
    }
}