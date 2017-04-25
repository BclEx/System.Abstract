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
using System.Globalization;
using Common.Logging.Simple;

namespace Common.Logging.Abstract
{
    /// <summary>
    /// ICommonLoggingServiceLog
    /// </summary>
    public interface ICommonLoggingServiceLog : IServiceLog
    {
        /// <summary>
        /// Gets the log.
        /// </summary>
        ILog Log { get; }
    }

    /// <summary>
    /// CommonLoggingServiceLog
    /// </summary>
    public class CommonLoggingServiceLog : ICommonLoggingServiceLog, ServiceLogManager.ISetupRegistration
    {
        static CommonLoggingServiceLog() { ServiceLogManager.EnsureRegistration(); }
        readonly ILog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonLoggingServiceLog"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public CommonLoggingServiceLog(string name)
            : this(LogManager.GetLogger(name)) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="CommonLoggingServiceLog"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public CommonLoggingServiceLog(object log)
        {
            if (log == null)
                throw new ArgumentNullException("log");
            _log = (log as ILog);
            if (_log == null)
                throw new ArgumentOutOfRangeException("log", "Must be of type log4net.ILog");
            var logAsSimpleLogger = (log as AbstractSimpleLogger);
            Name = (logAsSimpleLogger != null ? logAsSimpleLogger.Name : string.Empty);
        }

        Action<IServiceLocator, string> ServiceLogManager.ISetupRegistration.DefaultServiceRegistrar
        {
            get { return (locator, name) => ServiceLogManager.RegisterInstance<ICommonLoggingServiceLog>(this, locator, name); }
        }

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>
        /// A service object of type <paramref name="serviceType"/>.
        /// -or-
        /// null if there is no service object of type <paramref name="serviceType"/>.
        /// </returns>
        public object GetService(Type serviceType) { throw new NotImplementedException(); }

        // get
        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Gets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public IServiceLog Get(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            return new CommonLoggingServiceLog(LogManager.GetLogger(name));
        }
        /// <summary>
        /// Gets the specified name.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IServiceLog Get(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            return new CommonLoggingServiceLog(LogManager.GetLogger(type));
        }

        // log
        /// <summary>
        /// Writes the specified level.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="s">The s.</param>
        public void Write(ServiceLogLevel level, Exception ex, string s)
        {
            if (_log == null)
                throw new NullReferenceException("Log");
            switch (level)
            {
                case ServiceLogLevel.Fatal: _log.Fatal(s, ex); return;
                case ServiceLogLevel.Error: _log.Error(s, ex); return;
                case ServiceLogLevel.Warning: _log.Warn(s, ex); return;
                case ServiceLogLevel.Information: _log.Info(s, ex); return;
                case ServiceLogLevel.Debug: _log.Debug(s, ex); return;
                default: return;
            }
        }

        #region Domain-specific

        /// <summary>
        /// Gets the log.
        /// </summary>
        public ILog Log { get { return _log; } }

        #endregion
    }
}
