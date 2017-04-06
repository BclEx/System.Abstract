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
using Xunit;
namespace System.Abstract.Parts
{
    public class ServiceManagerBaseTests
    {
        [Fact]
        public void MakeByProviderProtected()
        {
            //var append = new ArraySegment<byte>();
            //var appendCas = new CasResult<ArraySegment<byte>> { Cas = 1, Result = append };

            //// Test : bool Append(string key, ArraySegment<byte> data);
            //var clientMock = new Mock<IMemcachedClient>();
            //var service = new MemcachedServiceCache(clientMock.Object);
            //service.Add(append, "name", CacheItemPolicy.Infinite, "value");
            //clientMock.Verify(x => x.Append("name", It.IsAny<ArraySegment<byte>>()));

            //// Test : CasResult<bool> Append(string key, ulong cas, ArraySegment<byte> data);
            //var clientMock2 = new Mock<IMemcachedClient>();
            //var service2 = new MemcachedServiceCache(clientMock2.Object);
            //service2.Add(appendCas, "name", CacheItemPolicy.Infinite, "value");
            //clientMock2.Verify(x => x.Append("name", appendCas.Cas, It.IsAny<ArraySegment<byte>>()));
        }

        //public void RegisterInstance
        //{
        //    //ServiceBusManagerBase
        //}
    }
}