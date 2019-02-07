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

namespace System.Abstract
{
    /// <summary>
    /// ServiceLocatorManager
    /// </summary>
    public class ServiceLocatorManager : ServiceManagerBase<IServiceLocator, ServiceLocatorManager, ServiceLocatorManagerLogger>
    {
        readonly static Type _ignoreServiceLocatorType = typeof(IIgnoreServiceLocator);

        static ServiceLocatorManager() =>
            Registration = new ServiceRegistration
            {
                OnSetup = (service, descriptor) =>
                {
                    if (!(service.Registrar is IServiceRegistrarBehaviorAccessor behavior) || behavior.RegisterInLocator)
                        RegisterSelfInLocator(service);
                    if (descriptor != null)
                        foreach (var action in descriptor.Actions)
                            action(service);
                    return service;
                },
                OnChange = (service, descriptor) =>
                {
                    if (descriptor != null)
                        foreach (var action in descriptor.Actions)
                            action(service);
                },
            };

        static void RegisterSelfInLocator(IServiceLocator locator) =>
            locator.Registrar.RegisterInstance(locator);

        /// <summary>
        /// Determines whether [has ignore service locator] [the specified instance].
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        ///   <c>true</c> if [has ignore service locator] [the specified instance]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasIgnoreServiceLocator(object instance) =>
            instance == null || HasIgnoreServiceLocator(instance.GetType());
        /// <summary>
        /// Determines whether [has ignore service locator].
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>
        ///   <c>true</c> if [has ignore service locator]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasIgnoreServiceLocator<TService>() =>
            HasIgnoreServiceLocator(typeof(TService));
        /// <summary>
        /// Determines whether [has ignore service locator] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if [has ignore service locator] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasIgnoreServiceLocator(Type type) =>
            type == null || _ignoreServiceLocatorType.IsAssignableFrom(type) || IgnoreServiceLocatorAttribute.HasIgnoreServiceLocator(type);
    }
}
