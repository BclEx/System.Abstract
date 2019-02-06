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
using System.Linq;

namespace System.Abstract
{
    /// <summary>
    /// AbstractExtensions
    /// </summary>
    static partial class AbstractExtensions
    {
        /// <summary>
        /// Adds the specified cache.
        /// </summary>
        /// <param name="service">The cache.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns>System.Object.</returns>
        public static object Add(this IServiceCache service, string name, object value) =>
            service.Add(null, name, CacheItemPolicyEx.Default, value, ServiceCacheByDispatcher.Empty);
        /// <summary>
        /// Adds the specified cache.
        /// </summary>
        /// <param name="service">The cache.</param>
        /// <param name="name">The name.</param>
        /// <param name="itemPolicy">The item policy.</param>
        /// <param name="value">The value.</param>
        /// <returns>System.Object.</returns>
        public static object Add(this IServiceCache service, string name, CacheItemPolicyEx itemPolicy, object value) =>
            service.Add(null, name, itemPolicy, value, ServiceCacheByDispatcher.Empty);
        /// <summary>
        /// Adds the specified cache.
        /// </summary>
        /// <param name="service">The cache.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns>System.Object.</returns>
        public static object Add(this IServiceCache service, object tag, string name, object value) =>
            service.Add(tag, name, CacheItemPolicyEx.Default, value, ServiceCacheByDispatcher.Empty);
        /// <summary>
        /// Adds the specified cache.
        /// </summary>
        /// <param name="service">The cache.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="name">The name.</param>
        /// <param name="itemPolicy">The item policy.</param>
        /// <param name="value">The value.</param>
        /// <returns>System.Object.</returns>
        public static object Add(this IServiceCache service, object tag, string name, CacheItemPolicyEx itemPolicy, object value) =>
            service.Add(tag, name, itemPolicy, value, ServiceCacheByDispatcher.Empty);

        /// <summary>
        /// Gets the specified cache.
        /// </summary>
        /// <param name="service">The cache.</param>
        /// <param name="name">The name.</param>
        /// <returns>System.Object.</returns>
        public static object Get(this IServiceCache service, string name) =>
            service.Get(null, name);
        /// <summary>
        /// Gets the specified cache.
        /// </summary>
        /// <param name="service">The cache.</param>
        /// <param name="names">The names.</param>
        /// <returns>System.Object.</returns>
        public static object Get(this IServiceCache service, IEnumerable<string> names) =>
            service.Get(null, names);

        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <param name="service">The cache.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool TryGet(this IServiceCache service, string name, out object value) =>
            service.TryGet(null, name, out value);

        /// <summary>
        /// Removes the specified cache.
        /// </summary>
        /// <param name="service">The cache.</param>
        /// <param name="name">The name.</param>
        /// <returns>System.Object.</returns>
        public static object Remove(this IServiceCache service, string name) =>
            service.Remove(null, name, null);
        /// <summary>
        /// Removes the specified service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="name">The name.</param>
        /// <returns>System.Object.</returns>
        public static object Remove(this IServiceCache service, object tag, string name) =>
            service.Remove(tag, name, null);

        /// <summary>
        /// Sets the specified cache.
        /// </summary>
        /// <param name="service">The cache.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns>System.Object.</returns>
        public static object Set(this IServiceCache service, string name, object value) =>
            service.Set(null, name, CacheItemPolicyEx.Default, value, ServiceCacheByDispatcher.Empty);
        /// <summary>
        /// Sets the specified cache.
        /// </summary>
        /// <param name="service">The cache.</param>
        /// <param name="name">The name.</param>
        /// <param name="itemPolicy">The item policy.</param>
        /// <param name="value">The value.</param>
        /// <returns>System.Object.</returns>
        public static object Set(this IServiceCache service, string name, CacheItemPolicyEx itemPolicy, object value) =>
            service.Set(null, name, itemPolicy, value, ServiceCacheByDispatcher.Empty);
        /// <summary>
        /// Sets the specified cache.
        /// </summary>
        /// <param name="service">The cache.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns>System.Object.</returns>
        public static object Set(this IServiceCache service, object tag, string name, object value) =>
            service.Set(tag, name, CacheItemPolicyEx.Default, value, ServiceCacheByDispatcher.Empty);
        /// <summary>
        /// Sets the specified cache.
        /// </summary>
        /// <param name="service">The cache.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="name">The name.</param>
        /// <param name="itemPolicy">The item policy.</param>
        /// <param name="value">The value.</param>
        /// <returns>System.Object.</returns>
        public static object Set(this IServiceCache service, object tag, string name, CacheItemPolicyEx itemPolicy, object value) =>
            service.Set(tag, name, itemPolicy, value, ServiceCacheByDispatcher.Empty);

        /// <summary>
        /// Ensures the cache dependency.
        /// </summary>
        /// <param name="service">The cache.</param>
        /// <param name="names">The names.</param>
        /// <exception cref="ArgumentNullException">service</exception>
        public static void EnsureCacheDependency(IServiceCache service, IEnumerable<string> names)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));
            if (names != null)
                foreach (var name in names)
                    service.Add(null, name, new CacheItemPolicyEx { AbsoluteExpiration = ServiceCache.InfiniteAbsoluteExpiration }, string.Empty);
        }

        /// <summary>
        /// Touches the specified cache.
        /// </summary>
        /// <param name="service">The cache.</param>
        /// <param name="names">The names.</param>
        public static void Touch(this IServiceCache service, params string[] names) { Touch(service, null, names); }
        /// <summary>
        /// Touches the specified cache.
        /// </summary>
        /// <param name="service">The cache.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="names">The names.</param>
        /// <exception cref="ArgumentNullException">service</exception>
        /// <exception cref="NotSupportedException">Touchables are not supported</exception>
        public static void Touch(this IServiceCache service, object tag, params string[] names)
        {
            if (service == null)
                throw new ArgumentNullException("service");
            var touchable = service.Settings.Touchable;
            if (touchable == null)
                throw new NotSupportedException("Touchables are not supported");
            touchable.Touch(tag, names);
        }

        /// <summary>
        /// Makes the dependency.
        /// </summary>
        /// <param name="service">The cache.</param>
        /// <param name="names">The names.</param>
        /// <returns>CacheItemDependency.</returns>
        /// <exception cref="ArgumentNullException">service</exception>
        /// <exception cref="NotSupportedException">Touchables are not supported</exception>
        public static CacheItemDependency MakeDependency(this IServiceCache service, params string[] names)
        {
            if (service == null)
                throw new ArgumentNullException("service");
            var touchable = service.Settings.Touchable;
            if (touchable == null)
                throw new NotSupportedException("Touchables are not supported");
            return (c, r, tag, values) => touchable.MakeDependency(tag, names);
        }

        #region BehaveAs

        /// <summary>
        /// Behaves as.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service">The cache.</param>
        /// <returns>T.</returns>
        public static T BehaveAs<T>(this IServiceCache service)
            where T : class, IServiceCache
        {
            IServiceWrapper<IServiceCache> serviceWrapper;
            do
            {
                serviceWrapper = (service as IServiceWrapper<IServiceCache>);
                if (serviceWrapper != null)
                    service = serviceWrapper.Base;
            } while (serviceWrapper != null);
            return service as T;
        }

        /// <summary>
        /// Behaves as.
        /// </summary>
        /// <param name="service">The cache.</param>
        /// <param name="namespace">The @namespace.</param>
        /// <returns>IServiceCache.</returns>
        /// <exception cref="ArgumentNullException">service
        /// or
        /// @namespace</exception>
        public static IServiceCache BehaveAs(this IServiceCache service, string @namespace)
        {
            if (service == null)
                throw new ArgumentNullException("service");
            if (@namespace == null)
                throw new ArgumentNullException("@namespace");
            return new ServiceCacheNamespaceBehaviorWrapper(service, @namespace);
        }
        /// <summary>
        /// Behaves as.
        /// </summary>
        /// <param name="service">The cache.</param>
        /// <param name="values">The values.</param>
        /// <param name="namespace">The @namespace.</param>
        /// <returns>IServiceCache.</returns>
        /// <exception cref="ArgumentNullException">service
        /// or
        /// @values</exception>
        public static IServiceCache BehaveAs(this IServiceCache service, IEnumerable<object> values, out string @namespace)
        {
            if (service == null)
                throw new ArgumentNullException("service");
            @namespace = ServiceCache.GetNamespace(values);
            if (@namespace == null)
                throw new ArgumentNullException("@values");
            return new ServiceCacheNamespaceBehaviorWrapper(service, @namespace);
        }

        #endregion

        #region Registrations

        /// <summary>
        /// Gets the specified cached item.
        /// </summary>
        /// <param name="service">The cache.</param>
        /// <param name="registration">The registration.</param>
        /// <param name="values">The values.</param>
        /// <returns>System.Object.</returns>
        public static object Get(this IServiceCache service, ServiceCacheRegistration registration, object[] values) =>
            Get<object>(service, registration, null, values);
        /// <summary>
        /// Gets the specified cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service">The cache.</param>
        /// <param name="registration">The registration.</param>
        /// <param name="values">The values.</param>
        /// <returns>T.</returns>
        public static T Get<T>(this IServiceCache service, ServiceCacheRegistration registration, object[] values) =>
            Get<T>(service, registration, null, values);
        /// <summary>
        /// Gets the specified cached item.
        /// </summary>
        /// <param name="service">The cache.</param>
        /// <param name="registration">The registration.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="values">The values.</param>
        /// <returns>System.Object.</returns>
        public static object Get(this IServiceCache service, ServiceCacheRegistration registration, object tag = null, object[] values = null) =>
            Get<object>(service, registration, tag, values);
        /// <summary>
        /// Gets the many.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service">The cache.</param>
        /// <param name="registration">The registration.</param>
        /// <param name="values">The values.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        public static IEnumerable<T> GetMany<T>(this IServiceCache service, ServiceCacheRegistration registration, object[] values) =>
            Get<IEnumerable<T>>(service, registration, null, values);
        /// <summary>
        /// Gets the dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="service">The cache.</param>
        /// <param name="registration">The registration.</param>
        /// <param name="values">The values.</param>
        /// <returns>IDictionary&lt;TKey, TValue&gt;.</returns>
        public static IDictionary<TKey, TValue> GetMany<TKey, TValue>(this IServiceCache service, ServiceCacheRegistration registration, object[] values) =>
            Get<IDictionary<TKey, TValue>>(service, registration, null, values);
        /// <summary>
        /// Gets the many.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service">The cache.</param>
        /// <param name="registration">The registration.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="values">The values.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        public static IEnumerable<T> GetMany<T>(this IServiceCache service, ServiceCacheRegistration registration, object tag = null, object[] values = null) =>
            Get<IEnumerable<T>>(service, registration, tag, values);
        /// <summary>
        /// Gets the dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="service">The cache.</param>
        /// <param name="registration">The registration.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="values">The values.</param>
        /// <returns>IDictionary&lt;TKey, TValue&gt;.</returns>
        public static IDictionary<TKey, TValue> GetMany<TKey, TValue>(this IServiceCache service, ServiceCacheRegistration registration, object tag = null, object[] values = null) =>
            Get<IDictionary<TKey, TValue>>(service, registration, tag, values);
        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service">The cache.</param>
        /// <param name="registration">The registration.</param>
        /// <param name="values">The values.</param>
        /// <returns>IQueryable&lt;T&gt;.</returns>
        public static IQueryable<T> GetQuery<T>(this IServiceCache service, ServiceCacheRegistration registration, object[] values) =>
                Get<IQueryable<T>>(service, registration, null, values);
        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service">The cache.</param>
        /// <param name="registration">The registration.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="values">The values.</param>
        /// <returns>IQueryable&lt;T&gt;.</returns>
        public static IQueryable<T> GetQuery<T>(this IServiceCache service, ServiceCacheRegistration registration, object tag = null, object[] values = null) =>
            Get<IQueryable<T>>(service, registration, tag, values);

        /// <summary>
        /// Gets the specified cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service">The cache.</param>
        /// <param name="registration">The registration.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="values">The values.</param>
        /// <returns>T.</returns>
        /// <exception cref="ArgumentNullException">service
        /// or
        /// registration</exception>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        public static T Get<T>(this IServiceCache service, ServiceCacheRegistration registration, object tag = null, object[] values = null)
        {
            if (service == null)
                throw new ArgumentNullException("service");
            if (registration == null)
                throw new ArgumentNullException("registration");
            var registrationDispatcher = GetRegistrationDispatcher(service);
            // fetch registration
            var recurses = 0;
            if (!ServiceCacheRegistrar.TryGetValue(registration, ref recurses, out var foundRegistration))
                throw new InvalidOperationException(string.Format(Local.UndefinedServiceCacheRegistrationAB, registration.Registrar != null ? registration.Registrar.AnchorType.ToString() : "{unregistered}", registration.Name));
            if (foundRegistration is ServiceCacheForeignRegistration)
                throw new InvalidOperationException(Local.InvalidDataSource);
            return registrationDispatcher.Get<T>(service, foundRegistration, tag, values);
        }

        /// <summary>
        /// Sends the specified service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="registration">The registration.</param>
        /// <param name="messages">The messages.</param>
        public static void Send(this IServiceCache service, ServiceCacheRegistration registration, object[] messages) =>
            Send(service, registration, null, messages);
        /// <summary>
        /// Sends the specified cache.
        /// </summary>
        /// <param name="service">The cache.</param>
        /// <param name="registration">The registration.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="messages">The messages.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        ///   
        /// <exception cref="System.InvalidOperationException"></exception>
        public static void Send(this IServiceCache service, ServiceCacheRegistration registration, object tag, object[] messages)
        {
            if (service == null)
                throw new ArgumentNullException("service");
            if (registration == null)
                throw new ArgumentNullException("registration");
            if (messages == null)
                throw new ArgumentNullException("messages");
            var dispatcher = GetDispatcher(service);
            // fetch registration
            var recurses = 0;
            IServiceCacheRegistration foundRegistration;
            if (!ServiceCacheRegistrar.TryGetValue(registration, ref recurses, out foundRegistration))
                throw new InvalidOperationException(string.Format(Local.UndefinedServiceCacheRegistrationAB, (registration.Registrar != null ? registration.Registrar.AnchorType.ToString() : "{unregistered}"), registration.Name));
            dispatcher.Send(service, foundRegistration, tag, messages);
        }

        /// <summary>
        /// Querys the specified service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service">The service.</param>
        /// <param name="registration">The registration.</param>
        /// <param name="messages">The messages.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        public static IEnumerable<T> Query<T>(this IServiceCache service, ServiceCacheRegistration registration, object[] messages) { return Query<T>(service, registration, null, messages); }
        /// <summary>
        /// Querys the specified cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service">The cache.</param>
        /// <param name="registration">The registration.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        ///   
        /// <exception cref="System.InvalidOperationException"></exception>
        public static IEnumerable<T> Query<T>(this IServiceCache service, ServiceCacheRegistration registration, object tag, object[] messages)
        {
            if (service == null)
                throw new ArgumentNullException("service");
            if (registration == null)
                throw new ArgumentNullException("registration");
            if (messages == null)
                throw new ArgumentNullException("messages");
            var dispatcher = GetDispatcher(service);
            // fetch registration
            var recurses = 0;
            if (!ServiceCacheRegistrar.TryGetValue(registration, ref recurses, out var foundRegistration))
                throw new InvalidOperationException(string.Format(Local.UndefinedServiceCacheRegistrationAB, registration.Registrar != null ? registration.Registrar.AnchorType.ToString() : "{unregistered}", registration.Name));
            return dispatcher.Query<T>(service, foundRegistration, tag, messages);
        }


        /// <summary>
        /// Removes the specified cache.
        /// </summary>
        /// <param name="cache">The cache.</param>
        /// <param name="registration">The registration.</param>
        public static void Remove(this IServiceCache cache, ServiceCacheRegistration registration)
        {
            if (registration == null)
                throw new ArgumentNullException("registration");
            var dispatcher = GetDispatcher(cache);
            // fetch registration
            var recurses = 0;
            if (!ServiceCacheRegistrar.TryGetValue(registration, ref recurses, out var foundRegistration))
                throw new InvalidOperationException(string.Format(Local.UndefinedServiceCacheRegistrationA, registration.ToString()));
            dispatcher.Remove(cache, foundRegistration);
        }

        static IServiceCacheDispatcher GetDispatcher(IServiceCache cache)
        {
            var settings = cache.Settings ?? throw new NullReferenceException(nameof(cache.Settings));
            return settings.Dispatcher ?? throw new NullReferenceException(nameof(settings.Dispatcher));
        }

        #endregion

        #region Lazy Setup

        /// <summary>
        /// Registers the with service locator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service">The service.</param>
        /// <param name="name">The name.</param>
        /// <returns>Lazy&lt;IServiceCache&gt;.</returns>
        public static Lazy<IServiceCache> RegisterWithServiceLocator<T>(this Lazy<IServiceCache> service, string name = null)
            where T : class, IServiceCache
        { ServiceCacheManager.GetSetupDescriptor(service).RegisterWithServiceLocator<T>(service, name, ServiceLocatorManager.Current); return service; }
        /// <summary>
        /// Registers the with service locator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service">The service.</param>
        /// <param name="locator">The locator.</param>
        /// <param name="name">The name.</param>
        /// <returns>Lazy&lt;IServiceCache&gt;.</returns>
        public static Lazy<IServiceCache> RegisterWithServiceLocator<T>(this Lazy<IServiceCache> service, IServiceLocator locator, string name = null)
            where T : class, IServiceCache
        { ServiceCacheManager.GetSetupDescriptor(service).RegisterWithServiceLocator<T>(service, name, locator); return service; }
        /// <summary>
        /// Registers the with service locator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service">The service.</param>
        /// <param name="locator">The locator.</param>
        /// <param name="name">The name.</param>
        /// <returns>Lazy&lt;IServiceCache&gt;.</returns>
        public static Lazy<IServiceCache> RegisterWithServiceLocator<T>(this Lazy<IServiceCache> service, Lazy<IServiceLocator> locator, string name = null)
            where T : class, IServiceCache
        { ServiceCacheManager.GetSetupDescriptor(service).RegisterWithServiceLocator<T>(service, name, locator); return service; }

        /// <summary>
        /// Registers the with service locator.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <returns>Lazy&lt;IServiceCache&gt;.</returns>
        public static Lazy<IServiceCache> RegisterWithServiceLocator(this Lazy<IServiceCache> service, Type serviceType, string name = null)
        { ServiceCacheManager.GetSetupDescriptor(service).RegisterWithServiceLocator(service, serviceType, name, ServiceLocatorManager.Current); return service; }
        /// <summary>
        /// Registers the with service locator.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="locator">The locator.</param>
        /// <param name="name">The name.</param>
        /// <returns>Lazy&lt;IServiceCache&gt;.</returns>
        public static Lazy<IServiceCache> RegisterWithServiceLocator(this Lazy<IServiceCache> service, Type serviceType, IServiceLocator locator, string name = null)
        { ServiceCacheManager.GetSetupDescriptor(service).RegisterWithServiceLocator(service, serviceType, name, locator); return service; }
        /// <summary>
        /// Registers the with service locator.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="locator">The locator.</param>
        /// <param name="name">The name.</param>
        /// <returns>Lazy&lt;IServiceCache&gt;.</returns>
        public static Lazy<IServiceCache> RegisterWithServiceLocator(this Lazy<IServiceCache> service, Type serviceType, Lazy<IServiceLocator> locator, string name = null)
        { ServiceCacheManager.GetSetupDescriptor(service).RegisterWithServiceLocator(service, serviceType, name, locator); return service; }

        #endregion
    }
}
