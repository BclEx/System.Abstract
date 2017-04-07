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
using System.Configuration;

namespace System.Web.Configuration
{
    /// <summary>
    /// WebExSection
    /// </summary>
    public class WebExSection : ConfigurationSectionEx
    {
        /// <summary>
        /// Gets or sets the activation assemblies.
        /// </summary>
        /// <value>
        /// The activation assemblies.
        /// </value>
        [ConfigurationProperty("activationAssemblies")]
        public ConfigurationElementCollectionEx<ConfigurationElementCollectionEx.AssemblyElement> ActivationAssemblies
        {
            get { return (ConfigurationElementCollectionEx<ConfigurationElementCollectionEx.AssemblyElement>)this["activationAssemblies"]; }
            set { this["activationAssemblies"] = value; }
        }

        /// <summary>
        /// Gets or sets the ignored assemblies.
        /// </summary>
        /// <value>
        /// The ignored assemblies.
        /// </value>
        [ConfigurationProperty("ignoredAssemblies")]
        public ConfigurationElementCollectionEx<ConfigurationElementCollectionEx.AssemblyElement> IgnoredAssemblies
        {
            get { return (ConfigurationElementCollectionEx<ConfigurationElementCollectionEx.AssemblyElement>)this["ignoredAssemblies"]; }
            set { this["ignoredAssemblies"] = value; }
        }

        internal static WebExSection GetSection()
        {
            return ConfigurationManagerEx.GetSection<WebExSection>("webEx", false);
        }

        /// <summary>
        /// Gets or sets a value indicating whether [code assemblies].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [code assemblies]; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty("codeAssemblies", DefaultValue = false)]
        public bool CodeAssemblies
        {
            get { return (bool)this["codeAssemblies"]; }
            set { this["codeAssemblies"] = value; }
        }
    }
}

