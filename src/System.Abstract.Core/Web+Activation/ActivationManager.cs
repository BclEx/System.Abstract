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
using System.Linq;
using System.Web.Handlers;
using System.Web;
using System.Reflection;
using System.Collections.Generic;
using System.Web.Configuration;
using System.Configuration;
using System.Web.Hosting;
using System.IO;
using System.Web.Compilation;
#if NET4
[assembly: PreApplicationStartMethod(typeof(ActivationManager), "Run")]
#endif

namespace System.Web
{
    /// <summary>
    /// ActivationManager
    /// </summary>
    public class ActivationManager
    {
        private static bool _hasInited;
        private static List<Assembly> _assemblies;
        private static object _lock = new object();

        static ActivationManager()
        {
            LoadFromConfiguration();
        }

#if NET4
        /// <summary>
        /// InitDisposeCallingModule
        /// </summary>
        public class InitDisposeCallingModule : IHttpModule
        {
            private static int _initializedModuleCount;
            private static object _lock = new object();

            /// <summary>
            /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"/>.
            /// </summary>
            public void Dispose()
            {
                lock (_lock)
                    if (--_initializedModuleCount == 0)
                        ActivationManager.RunShutdownMethods();
            }

            /// <summary>
            /// Initializes a module and prepares it to handle requests.
            /// </summary>
            /// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
            public void Init(HttpApplication context)
            {
                lock (_lock)
                    if (_initializedModuleCount++ == 0)
                        ActivationManager.RunPostStartMethods();
            }
        }
#endif

        /// <summary>
        /// Adds the assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        public static void AddAssemblies(params Assembly[] assemblies)
        {
            if (assemblies == null)
                throw new ArgumentNullException("assemblies");
            Assembly[] newAssemblies;
            if (_assemblies != null)
            {
                newAssemblies = assemblies.Except(_assemblies).ToArray();
                _assemblies.AddRange(newAssemblies);
            }
            else
            {
                newAssemblies = assemblies;
                _assemblies = new List<Assembly>(newAssemblies);
            }
            if (_hasInited)
            {
                RunActivationMethods<PreApplicationStartMethodAttribute>(newAssemblies, x => InvokeMethod((PreApplicationStartMethodAttribute)x));
                RunActivationMethods<PostApplicationStartMethodAttribute>(newAssemblies, x => InvokeMethod((PostApplicationStartMethodAttribute)x));
            }
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public static void Run()
        {
            if (!_hasInited)
            {
                LoadFromConfiguration();
                AddAssemblies(ActivationAssemblies.ToArray());
                RunPreStartMethods();
#if NET4
                if (System.Web.Hosting.HostingEnvironment.IsHosted)
                    Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(InitDisposeCallingModule));
                else
                    RunPostStartMethods();
#else
                RunPostStartMethods();
#endif
                _hasInited = true;
            }
        }

#if !NET4
        /// <summary>
        /// Shutdowns this instance.
        /// </summary>
        public static void Shutdown()
        {
            if (_hasInited)
            {
                RunShutdownMethods();
                _hasInited = false;
            }
        }
#endif

        private static void RunActivationMethods<T>(IEnumerable<Assembly> assemblies, Action<Attribute> action)
            where T : Attribute
        {
            var attributes = assemblies.SelectMany(x => x.GetCustomAttributes(typeof(T), false).OfType<T>());
            foreach (var attribute in attributes)
                action(attribute);
        }

        private static void InvokeMethod(PostApplicationStartMethodAttribute attribute)
        {
            var method = attribute.Type.GetMethod(attribute.MethodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            if (method == null)
                throw new ArgumentException(string.Format("The type {0} doesn't have a static method named {1}", attribute.Type, attribute.MethodName));
            try { method.Invoke(null, null); }
            catch (Exception ex) { throw ex.InnerException; }
        }
        private static void InvokeMethod(PreApplicationStartMethodAttribute attribute)
        {
            var method = attribute.Type.GetMethod(attribute.MethodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            if (method == null)
                throw new ArgumentException(string.Format("The type {0} doesn't have a static method named {1}", attribute.Type, attribute.MethodName));
            try { method.Invoke(null, null); }
            catch (Exception ex) { throw ex.InnerException; }
        }
        private static void InvokeMethod(ApplicationShutdownMethodAttribute attribute)
        {
            var method = attribute.Type.GetMethod(attribute.MethodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            if (method == null)
                throw new ArgumentException(string.Format("The type {0} doesn't have a static method named {1}", attribute.Type, attribute.MethodName));
            try { method.Invoke(null, null); }
            catch (Exception ex) { throw ex.InnerException; }
        }

        private static IEnumerable<string> GetAssemblyFiles()
        {
            var path = (HostingEnvironment.IsHosted ? HttpRuntime.BinDirectory : Path.GetDirectoryName(typeof(ActivationManager).Assembly.Location));
            return Directory.GetFiles(path, "*.dll");
        }

        /// <summary>
        /// Gets the assemblies.
        /// </summary>
        public static IEnumerable<Assembly> Assemblies
        {
            get
            {
                if (_assemblies == null)
                    lock (_lock)
                        if (_assemblies == null)
                        {
                            var assemblies = new List<Assembly>();
                            if (HostingEnvironment.IsHosted && BuildManager.CodeAssemblies != null)
                                _assemblies.AddRange(BuildManager.CodeAssemblies.OfType<Assembly>());
                            foreach (var c in GetAssemblyFiles())
                                try { assemblies.Add(Assembly.LoadFrom(c)); }
                                catch { }
                            _assemblies = (IgnoredAssemblies != null ? assemblies.Except(IgnoredAssemblies).ToList() : assemblies);
                        }
                return _assemblies;
            }
        }

        /// <summary>
        /// Gets the activation assemblies.
        /// </summary>
        public static IEnumerable<Assembly> ActivationAssemblies { get; private set; }

        /// <summary>
        /// Gets the ignored assemblies.
        /// </summary>
        public static IEnumerable<Assembly> IgnoredAssemblies { get; private set; }

        private static void LoadFromConfiguration()
        {
            try
            {
                var section = WebExSection.GetSection();
                if (section.IgnoredAssemblies != null)
                    IgnoredAssemblies = section.IgnoredAssemblies
                        .Cast<ConfigurationElementCollectionEx.AssemblyElement>()
                        .Select(x => x.Assembly)
                        .ToList();
                _assemblies = (section.CodeAssemblies ? (List<Assembly>)Assemblies : new List<Assembly>());
                var activationAssemblies = section.ActivationAssemblies
                    .Cast<ConfigurationElementCollectionEx.AssemblyElement>()
                    .Select(x => x.Assembly);
                if (IgnoredAssemblies != null)
                    activationAssemblies = activationAssemblies.Except(IgnoredAssemblies);
                ActivationAssemblies = activationAssemblies.ToList();
            }
            catch { }
        }

        /// <summary>
        /// Runs the post start methods.
        /// </summary>
        public static void RunPostStartMethods() { RunActivationMethods<PostApplicationStartMethodAttribute>(_assemblies, x => InvokeMethod((PostApplicationStartMethodAttribute)x)); }
        /// <summary>
        /// Runs the pre start methods.
        /// </summary>
        public static void RunPreStartMethods() { RunActivationMethods<PreApplicationStartMethodAttribute>(_assemblies, x => InvokeMethod((PreApplicationStartMethodAttribute)x)); }
        /// <summary>
        /// Runs the shutdown methods.
        /// </summary>
        public static void RunShutdownMethods() { RunActivationMethods<ApplicationShutdownMethodAttribute>(_assemblies, x => InvokeMethod((ApplicationShutdownMethodAttribute)x)); }
    }
}
