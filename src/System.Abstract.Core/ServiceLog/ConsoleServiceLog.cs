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

using System;
using System.Abstract;
using System.IO;

namespace Contoso.Abstract
{
    /// <summary>
    /// IConsoleServiceLog
    /// </summary>
    /// <seealso cref="System.Abstract.IServiceLog" />
    public interface IConsoleServiceLog : IServiceLog
    {
        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <value>The log.</value>
        TextWriter Log { get; }
    }

    /// <summary>
    /// ConsoleServiceLog
    /// </summary>
    /// <seealso cref="Contoso.Abstract.IConsoleServiceLog" />
    public class ConsoleServiceLog : IConsoleServiceLog, ServiceLogManager.IRegisterWithLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleServiceLog" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ConsoleServiceLog(string name)
        {
            Name = name;
            Log = Console.Out;
        }

        Action<IServiceLocator, string> ServiceLogManager.IRegisterWithLocator.RegisterWithLocator =>
            (locator, name) => ServiceLogManager.RegisterInstance<IConsoleServiceLog>(this, name, locator);

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type <paramref name="serviceType" />.
        /// -or-
        /// null if there is no service object of type <paramref name="serviceType" />.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object GetService(Type serviceType) => throw new NotImplementedException();

        // get
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>IServiceLog.</returns>
        /// <exception cref="System.ArgumentNullException">name</exception>
        public IServiceLog Get(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            return new ConsoleServiceLog(name);
        }
        /// <summary>
        /// Gets the specified name.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>IServiceLog.</returns>
        /// <exception cref="System.ArgumentNullException">type</exception>
        public IServiceLog Get(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            return new ConsoleServiceLog(type.Name);
        }

        // log
        /// <summary>
        /// Writes the specified level.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="s">The s.</param>
        /// <exception cref="System.NullReferenceException">Log</exception>
        public void Write(ServiceLogLevel level, Exception ex, string s)
        {
            if (Log == null)
                throw new NullReferenceException("Log");
            Log.WriteLine($"[{level}] '{Name}' {s}");
            if (ex != null)
                Log.WriteLine($"{ex.GetType().FullName}: {ex.Message} {ex.StackTrace}");
        }

        #region Domain-specific

        /// <summary>
        /// Gets or sets the log.
        /// </summary>
        /// <value>The log.</value>
        public TextWriter Log { get; set; }

        #endregion
    }
}
