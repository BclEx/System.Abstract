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
using System.Diagnostics;

namespace Contoso.Abstract
{
    /// <summary>
    /// IEventLogServiceLog
    /// </summary>
    /// <seealso cref="System.Abstract.IServiceLog" />
    public interface IEventLogServiceLog : IServiceLog
    {
        /// <summary>
        /// Gets the log.
        /// </summary>
        EventLog Log { get; }
    }

    /// <summary>
    /// EventLogServiceLog
    /// </summary>
    /// <seealso cref="Contoso.Abstract.IEventLogServiceLog" />
    /// <seealso cref="System.IDisposable" />
    public class EventLogServiceLog : IEventLogServiceLog, IDisposable, ServiceLogManager.IRegisterWithLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogServiceLog" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public EventLogServiceLog(string name)
            : this(name, "default") { }
        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogServiceLog" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="source">The source.</param>
        public EventLogServiceLog(string name, string source)
        {
            if (!EventLog.SourceExists(source))
                EventLog.CreateEventSource(source, name);
            Name = name;
            Log = new EventLog(name) { Source = source };
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogServiceLog" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="machineName">Name of the machine.</param>
        /// <param name="source">The source.</param>
        public EventLogServiceLog(string name, string machineName, string source)
        {
            if (!EventLog.SourceExists(source, machineName))
                EventLog.CreateEventSource(new EventSourceCreationData(source, name) { MachineName = machineName });
            Name = name;
            Log = new EventLog(name, machineName, source);
        }
        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="EventLogServiceLog" /> is reclaimed by garbage collection.
        /// </summary>
        ~EventLogServiceLog()
        {
            try { ((IDisposable)this).Dispose(); }
            catch { }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (Log != null)
            {
                GC.SuppressFinalize(this);
                try { Log.Dispose(); }
                finally { Log = null; }
            }
        }

        Action<IServiceLocator, string> ServiceLogManager.IRegisterWithLocator.RegisterWithLocator =>
            (locator, name) => ServiceLogManager.RegisterInstance<IEventLogServiceLog>(this, name, locator);

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
            return new EventLogServiceLog(name);
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
            return new EventLogServiceLog(type.Name);
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
            var message =
                ex == null ? $"[{level}] '{Name}' message: {s}" :
                $"[{level}] '{Name}' message: {s} exception: {ex.GetType()} {ex.Message} {ex.StackTrace}";
            Log.WriteEntry(message, ToEventLogEntryType(level));
        }

        #region Domain-specific

        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <value>The log.</value>
        public EventLog Log { get; private set; }

        #endregion

        static EventLogEntryType ToEventLogEntryType(ServiceLogLevel level)
        {
            switch (level)
            {
                case ServiceLogLevel.Fatal:
                case ServiceLogLevel.Error: return EventLogEntryType.Error;
                case ServiceLogLevel.Warning: return EventLogEntryType.Warning;
                default: return EventLogEntryType.Information;
            }
        }
    }
}
