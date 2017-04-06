#region License
/*
The MIT License

Copyright (c) 2008 Sky Morey

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion
using Moq;
using System.Abstract.Parts;
using System.IO;
using System.Text;
using Xunit;
namespace System.Abstract.EventSourcing
{
	public class AggregateRootTests
    {
        [Fact]
        public void TypeSerializer_ReadObject_Returns_Value()
        {
            var typeSerializerMock = new Mock<ITypeSerializer>();
            typeSerializerMock.Setup(x => x.ReadObject<string>(typeof(PartsExtensionsTests), It.IsAny<Stream>()))
                .Returns("test");
            var typeSerializer = typeSerializerMock.Object;
            //
            Assert.Equal("test", typeSerializer.ReadObject<string>(typeof(PartsExtensionsTests), "123"));
            Assert.Equal("test", typeSerializer.ReadObject<string>(typeof(PartsExtensionsTests), "123", Encoding.UTF8));
            Assert.Equal("test", typeSerializer.ReadObjectBase64<string>(typeof(PartsExtensionsTests), Convert.ToBase64String(new byte[] { 1, 2, 3 })));
        }
    }
}