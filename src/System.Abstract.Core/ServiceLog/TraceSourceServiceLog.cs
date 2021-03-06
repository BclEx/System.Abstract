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
using System.Collections.Generic;
using System.Diagnostics;

namespace Contoso.Abstract
{
    /// <summary>
    /// ITraceSourceServiceLog
    /// </summary>
    /// <seealso cref="System.Abstract.IServiceLog" />
    public interface ITraceSourceServiceLog : IServiceLog
    {
        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <value>The log.</value>
        TraceSource Log { get; }
    }

    /// <summary>
    /// TraceSourceServiceLog
    /// </summary>
    /// <seealso cref="Contoso.Abstract.ITraceSourceServiceLog" />
    public class TraceSourceServiceLog : ITraceSourceServiceLog, ServiceLogManager.IRegisterWithLocator
    {
        static readonly Dictionary<string, TraceSource> _logs = new Dictionary<string, TraceSource>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceSourceServiceLog" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public TraceSourceServiceLog(string name)
            : this(name, SourceLevels.Off) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="TraceSourceServiceLog" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="defaultLevel">The default level.</param>
        public TraceSourceServiceLog(string name, SourceLevels defaultLevel)
        {
            Name = name;
            Log = GetAndCache(name, defaultLevel);
        }

        Action<IServiceLocator, string> ServiceLogManager.IRegisterWithLocator.RegisterWithLocator =>
            (locator, name) => ServiceLogManager.RegisterInstance<ITraceSourceServiceLog>(this, name, locator);

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type <paramref name="serviceType" />.
        /// -or-
        /// null if there is no service object of type <paramref name="serviceType" />.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object GetService(Type serviceType) { throw new NotImplementedException(); }

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
            return new TraceSourceServiceLog(name);
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
            return new TraceSourceServiceLog(type.Name);
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
                throw new NullReferenceException(nameof(Log));
            if (ex == null)
                Log.TraceEvent(ToTraceEventType(level), 0, s);
            else
                Log.TraceData(ToTraceEventType(level), 0, new object[] { s, ex });
        }

        #region Domain-specific

        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <value>The log.</value>
        public TraceSource Log { get; private set; }

        #endregion

        static TraceEventType ToTraceEventType(ServiceLogLevel level)
        {
            switch (level)
            {
                case ServiceLogLevel.Fatal: return TraceEventType.Critical;
                case ServiceLogLevel.Error: return TraceEventType.Error;
                case ServiceLogLevel.Warning: return TraceEventType.Warning;
                case ServiceLogLevel.Information: return TraceEventType.Information;
                case ServiceLogLevel.Debug: return TraceEventType.Verbose;
                default: return TraceEventType.Verbose;
            }
        }

        static TraceSource GetAndCache(string name, SourceLevels defaultLevel)
        {
            if (_logs.TryGetValue(name, out var log))
                return log;
            lock (_logs)
            {
                if (_logs.TryGetValue(name, out log))
                    return log;
                log = new TraceSource(name);
                if (!HasDefaultSource(log))
                {
                    var source = new TraceSource("Default", defaultLevel);
                    for (var shortName = ShortenName(name); !string.IsNullOrEmpty(shortName); shortName = ShortenName(shortName))
                    {
                        var source2 = new TraceSource(shortName, defaultLevel);
                        if (!HasDefaultSource(source2))
                        {
                            source = source2;
                            break;
                        }
                    }
                    log.Switch = source.Switch;
                    var listeners = log.Listeners;
                    listeners.Clear();
                    foreach (TraceListener listener in source.Listeners)
                        listeners.Add(listener);
                }
                _logs.Add(name, log);
            }
            return log;
        }

        static string ShortenName(string name)
        {
            var length = name.LastIndexOf('.');
            return length != -1 ? name.Substring(0, length) : null;
        }

        static bool HasDefaultSource(TraceSource source) =>
            source.Listeners.Count == 1 && source.Listeners[0] is DefaultTraceListener && source.Listeners[0].Name == "Default";
    }
}
