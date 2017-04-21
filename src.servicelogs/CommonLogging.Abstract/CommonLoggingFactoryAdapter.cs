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
using Common.Logging.Factory;
using Common.Logging.Simple;

namespace Common.Logging.Abstract
{
    /// <summary>
    /// CommonLoggingFactoryAdapter
    /// </summary>
    internal class CommonLoggingFactoryAdapter : ILoggerFactoryAdapter
    {
        readonly IServiceLog _log;

        #region LogAdapter

        private class LogAdapter : AbstractLogger
        {
            readonly IServiceLog _log;

            public LogAdapter(IServiceLog log)
            {
                _log = log;
            }

            //protected override WriteHandler GetWriteHandler() { return (LogLevel level, object s, Exception ex) => _log.Write(FromLogLevel(level), ex, FromMessage(s)); }

            public override bool IsDebugEnabled
            {
                get { return true; }
            }

            public override bool IsErrorEnabled
            {
                get { return true; }
            }

            public override bool IsFatalEnabled
            {
                get { return true; }
            }

            public override bool IsInfoEnabled
            {
                get { return true; }
            }

            public override bool IsTraceEnabled
            {
                get { return true; }
            }

            public override bool IsWarnEnabled
            {
                get { return true; }
            }

            protected override void WriteInternal(LogLevel level, object message, Exception exception)
            {
                _log.Write(FromLogLevel(level), exception, FromMessage(message));
            }

            private string FromMessage(object s) { var sAsString = (s as string); return (s == null || sAsString != null ? sAsString : s.ToString()); }

            private ServiceLogLevel FromLogLevel(LogLevel level)
            {
                switch (level)
                {
                    case LogLevel.All: return ServiceLogLevel.Debug;
                    case LogLevel.Trace: return ServiceLogLevel.Debug;
                    case LogLevel.Debug: return ServiceLogLevel.Debug;
                    case LogLevel.Info: return ServiceLogLevel.Information;
                    case LogLevel.Warn: return ServiceLogLevel.Warning;
                    case LogLevel.Error: return ServiceLogLevel.Error;
                    case LogLevel.Fatal: return ServiceLogLevel.Fatal;
                    case LogLevel.Off: return ServiceLogLevel.None;
                    default: throw new InvalidOperationException();
                }
            }
        }

        #endregion

        static CommonLoggingFactoryAdapter()
        {
            if (LogManager.Adapter is NoOpLoggerFactoryAdapter)
                LogManager.Adapter = new CommonLoggingFactoryAdapter();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonLoggingFactoryAdapter"/> class.
        /// </summary>
        public CommonLoggingFactoryAdapter()
            : this(ServiceLogManager.Current) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="CommonLoggingFactoryAdapter"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public CommonLoggingFactoryAdapter(IServiceLog log)
        {
            _log = log;
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public ILog GetLogger(string name) { return new LogAdapter(_log.Get(name)); }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public ILog GetLogger(Type type) { return new LogAdapter(_log.Get(type)); }

        /// <summary>
        /// Ensures the registration.
        /// </summary>
        public static void EnsureRegistration() { }
    }
}
