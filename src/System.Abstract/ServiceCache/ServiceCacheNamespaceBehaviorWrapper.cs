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
        IServiceCache _parent;
        string _namespace;

        public ServiceCacheNamespaceBehaviorWrapper(IServiceCache parent, string @namespace)
        {
            if (string.IsNullOrEmpty(@namespace))
                throw new ArgumentNullException("@namespace");
            _parent = parent ?? throw new ArgumentNullException("parent");
            _namespace = @namespace;
        }

        // wrapper
        public IServiceCache Base => _parent;

        public object GetService(Type serviceType) => _parent.GetService(serviceType);

        public object this[string name]
        {
            get => _parent[_namespace + name];
            set => _parent[_namespace + name] = value;
        }
        public object Add(object tag, string name, CacheItemPolicy itemPolicy, object value, ServiceCacheByDispatcher dispatch) => _parent.Add(tag, _namespace + name, itemPolicy, value, dispatch);
        public object Get(object tag, string name) => _parent.Get(tag, _namespace + name);
        public object Get(object tag, string name, IServiceCacheRegistration registration, out CacheItemHeader header) => _parent.Get(tag, _namespace + name, registration, out header);
        public object Get(object tag, IEnumerable<string> names) => _parent.Get(tag, names);
        public IEnumerable<CacheItemHeader> Get(object tag, IServiceCacheRegistration registration) => _parent.Get(tag, registration);
        public bool TryGet(object tag, string name, out object value) => _parent.TryGet(tag, name, out value);
        public object Remove(object tag, string name, IServiceCacheRegistration registration) => _parent.Remove(tag, _namespace + name, registration);
        public object Set(object tag, string name, CacheItemPolicy itemPolicy, object value, ServiceCacheByDispatcher dispatch) => _parent.Add(tag, _namespace + name, itemPolicy, value, dispatch);
        public void Touch(object tag, params string[] names) => _parent.Touch(tag, names);

        public string Namespace => _namespace;
        public ServiceCacheSettings Settings => _parent.Settings;
    }
}
