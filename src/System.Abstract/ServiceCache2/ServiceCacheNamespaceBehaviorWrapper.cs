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

using System.Collections.Generic;

namespace System.Abstract
{
    /// <summary>
    /// ServiceCacheNamespaceBehaviorWrapper
    /// </summary>
    internal struct ServiceCacheNamespaceBehaviorWrapper : IServiceWrapper<IServiceCache>, IServiceCache
    {
        IServiceCache _base;
        string _namespace;

        public ServiceCacheNamespaceBehaviorWrapper(IServiceCache @base, string @namespace)
        {
            _base = @base ?? throw new ArgumentNullException(nameof(@base));
            _namespace = @namespace ?? throw new ArgumentNullException(nameof(@namespace));
        }

        // wrapper
        public IServiceCache Base => _base;

        public object GetService(Type serviceType) => _base.GetService(serviceType);

        public object this[string name]
        {
            get => _base[_namespace + name];
            set => _base[_namespace + name] = value;
        }
        public object Add(object tag, string name, CacheItemPolicyEx itemPolicy, object value, ServiceCacheByDispatcher dispatch) => _base.Add(tag, _namespace + name, itemPolicy, value, dispatch);
        public object Get(object tag, string name) => _base.Get(tag, _namespace + name);
        public object Get(object tag, string name, ServiceCacheRegistration registration, out CacheItemHeader header) => _base.Get(tag, _namespace + name, registration, out header);
        public object Get(object tag, IEnumerable<string> names) => _base.Get(tag, names);
        public IEnumerable<CacheItemHeader> Get(object tag, ServiceCacheRegistration registration) => _base.Get(tag, registration);
        public bool TryGet(object tag, string name, out object value) => _base.TryGet(tag, name, out value);
        public object Remove(object tag, string name, ServiceCacheRegistration registration) => _base.Remove(tag, _namespace + name, registration);
        public object Set(object tag, string name, CacheItemPolicyEx itemPolicy, object value, ServiceCacheByDispatcher dispatch) => _base.Add(tag, _namespace + name, itemPolicy, value, dispatch);
        public void Touch(object tag, params string[] names) => _base.Touch(tag, names);

        public string Namespace => _namespace;
        public ServiceCacheSettings Settings => _base.Settings;
    }
}
