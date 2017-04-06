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
namespace System.Abstract.Parts.Fakes
{
    public class ServiceTestManager : ServiceManagerBase<IServiceTest, Action<IServiceTest>, ServiceTestManagerDebugger>
    {
        static ServiceTestManager()
        {
            Registration = new ServiceRegistration
            {
                MakeAction = a => x => a(x),
                OnSetup = (service, descriptor) =>
                {
                    return null;
                },
                OnChange = (service, descriptor) =>
                {
                },
                DefaultServiceRegistrar = (service, locator, name) =>
                {
                },
            };
            // default provider
            if (Lazy == null && DefaultServiceProvider != null)
                SetProvider(DefaultServiceProvider);
        }

        public static Lazy<IServiceTest> SetProvider(Func<IServiceTest> provider) { return (Lazy = MakeByProviderProtected(provider, null)); }
        public static Lazy<IServiceTest> SetProvider(Func<IServiceTest> provider, ISetupDescriptor setupDescriptor) { return (Lazy = MakeByProviderProtected(provider, setupDescriptor)); }
        public static Lazy<IServiceTest> MakeByProvider(Func<IServiceTest> provider) { return MakeByProviderProtected(provider, null); }
        public static Lazy<IServiceTest> MakeByProvider(Func<IServiceTest> provider, ISetupDescriptor setupDescriptor) { return MakeByProviderProtected(provider, setupDescriptor); }

        public static IServiceTest Current
        {
            get { return GetCurrent(); }
        }

        public static void EnsureRegistration() { }
        public static ISetupDescriptor GetSetupDescriptor(Lazy<IServiceTest> service) { return GetSetupDescriptorProtected(service, null); }
    }

}