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

namespace System.Abstract.Internal
{
    /// <summary>
    /// ServiceManagerLoggerBase
    /// </summary>
    /// <typeparam name="TLoggerFlags">The type of the t logger flags.</typeparam>
    public abstract class ServiceManagerLoggerBase<TLoggerFlags>
        where TLoggerFlags : struct
    {
        IServiceLog _log = null;

        /// <summary>
        /// Gets or sets the log.
        /// </summary>
        /// <value>The log.</value>
        public IServiceLog Log
        {
            get
            {
                if (_log != null)
                    return _log;
                _log = ServiceLogManager.Current;
                Started();
                return _log;
            }
            set { _log = value; }
        }

        /// <summary>
        /// Gets or sets the flags.
        /// </summary>
        /// <value>The flags.</value>
        public TLoggerFlags Flags { get; set; }

        /// <summary>
        /// Debugs the started.
        /// </summary>
        protected abstract void Started();
    }
}
