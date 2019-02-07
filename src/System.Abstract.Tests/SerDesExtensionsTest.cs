using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using System.Text;

namespace System.Abstract.Tests
{
    [TestClass]
    public class SerDesExtensionsTest
    {
        [TestMethod, TestCategory("Core")]
        public void Des_Returns_Value()
        {
            var mock = new Mock<ISerDes>();
            mock.Setup(x => x.Des<string>(It.IsAny<Type>(), It.IsAny<Stream>())).Returns("test");
            var serDes = mock.Object;
            //
            Assert.AreEqual("test", serDes.Des<string>(typeof(SerDesExtensionsTest), "123"));
            Assert.AreEqual("test", serDes.Des<string>(typeof(SerDesExtensionsTest), "123", Encoding.UTF8));
            Assert.AreEqual("test", serDes.DesBase64<string>(typeof(SerDesExtensionsTest), Convert.ToBase64String(new byte[] { 1, 2, 3 })));
        }

        [TestMethod, TestCategory("Core")]
        public void Ser_Returns_Value()
        {
            var base64 = Convert.ToBase64String(new byte[] { 1, 2, 3 });
            var mock = new Mock<ISerDes>();
            mock.Setup(x => x.Ser(It.IsAny<Type>(), It.IsAny<Stream>(), It.IsAny<string>())).Callback<Type, Stream, string>((t, s, g) =>
            {
                if (g == "123")
                    s.Write(new[] { (byte)'t', (byte)'e', (byte)'s', (byte)'t' }, 0, 4);
                else
                    s.Write(new byte[] { 1, 2, 3 }, 0, 3);
            });
            var serDes = mock.Object;
            //
            Assert.AreEqual("test", serDes.Ser(typeof(SerDesExtensionsTest), "123"));
            Assert.AreEqual("test", serDes.Ser(typeof(SerDesExtensionsTest), "123", Encoding.UTF8));
            Assert.AreEqual(base64, serDes.SerBase64(typeof(SerDesExtensionsTest), base64));
        }
    }
}