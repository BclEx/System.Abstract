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
#if !NET35
using StructureMap.Building.Interception;
using StructureMap.Pipeline;
using System;
using System.Abstract;
using System.Collections.Generic;

namespace StructureMap.Abstract
{
    /// <summary>
    /// Interceptor
    /// </summary>
    internal class Interceptor : IInterceptorPolicy
    {
        readonly IServiceLocatorInterceptor _interceptor;
        readonly IContainer _container;

        public Interceptor(IServiceLocatorInterceptor interceptor, IContainer container)
        {
            _interceptor = interceptor;
            _container = container;
        }

        public IEnumerable<IInterceptor> DetermineInterceptors(Type pluginType, Instance instance)
        {
            if (_interceptor.Match(pluginType))
            {
                var type = instance.GetType();
                _interceptor.ItemCreated(type, _container.Model.For(type).Lifecycle == Lifecycles.Transient);
            }
            return null;
        }

        public string Description
        {
            get { return "ServiceLocatorInterceptor"; }
        }
    }
}
#endif