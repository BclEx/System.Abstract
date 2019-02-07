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

namespace System.IO
{
    /// <summary>
    /// WrapTextReader
    /// </summary>
    /// <seealso cref="System.IO.TextReader" />
    internal class WrapTextReader : TextReader
    {
        /// <summary>
        /// WrapOptions
        /// </summary>
        public enum WrapOptions
        {
            /// <summary>
            /// Default
            /// </summary>
            Default = 0,
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WrapTextReader" /> class.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <param name="options">The options.</param>
        protected WrapTextReader(TextReader r, WrapOptions options)
        {
            R = r;
            Options = options;
        }

        /// <summary>
        /// Gets or sets the R.
        /// </summary>
        /// <value>The R.</value>
        protected TextReader R { get; set; }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>The options.</value>
        protected WrapOptions Options { get; set; }

        /// <summary>
        /// Wraps the specified r.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <returns>TextReader.</returns>
        public static TextReader Wrap(TextReader r) { return Wrap(r, WrapOptions.Default); }
        /// <summary>
        /// Wraps the specified r.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <param name="options">The options.</param>
        /// <returns>TextReader.</returns>
        public static TextReader Wrap(TextReader r, WrapOptions options)
        {
            var rasWrap = r as WrapTextReader;
            return rasWrap != null && rasWrap.Options == options ? r : new WrapTextReader(r, options);
        }

        /// <summary>
        /// Closes the <see cref="T:System.IO.TextReader" /> and releases any system resources associated with the TextReader.
        /// </summary>
        public override void Close() => R.Close();

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.IO.TextReader" /> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing) { if (disposing) R.Dispose(); }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj) { return R.Equals(obj); }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode() => R.GetHashCode();

        int? _pendingC;

        /// <summary>
        /// Reads the next character without changing the state of the reader or the character source. Returns the next available character without actually reading it from the input stream.
        /// </summary>
        /// <returns>An integer representing the next character to be read, or -1 if no more characters are available or the stream does not support seeking.</returns>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextReader" /> is closed.</exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
        public override int Peek()
        {
            var c = R.Peek();
            if (c == -1)
                _pendingC = c = R.Read();
            return c;
        }

        /// <summary>
        /// Reads the next character from the input stream and advances the character position by one character.
        /// </summary>
        /// <returns>The next character from the input stream, or -1 if no more characters are available. The default implementation returns -1.</returns>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextReader" /> is closed.</exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
        public override int Read()
        {
            if (!_pendingC.HasValue)
                return R.Read();
            var c = _pendingC.Value; _pendingC = null; return c;
        }

        /// <summary>
        /// Reads a maximum of <paramref name="count" /> characters from the current stream and writes the data to <paramref name="buffer" />, beginning at <paramref name="index" />.
        /// </summary>
        /// <param name="buffer">When this method returns, contains the specified character array with the values between <paramref name="index" /> and (<paramref name="index" /> + <paramref name="count" /> - 1) replaced by the characters read from the current source.</param>
        /// <param name="index">The place in <paramref name="buffer" /> at which to begin writing.</param>
        /// <param name="count">The maximum number of characters to read. If the end of the stream is reached before <paramref name="count" /> of characters is read into <paramref name="buffer" />, the current method returns.</param>
        /// <returns>The number of characters that have been read. The number will be less than or equal to <paramref name="count" />, depending on whether the data is available within the stream. This method returns zero if called when no more characters are left to read.</returns>
        /// <exception cref="ArgumentNullException">buffer - ArgumentNull_Buffer</exception>
        /// <exception cref="ArgumentOutOfRangeException">index - ArgumentOutOfRange_NeedNonNegNum
        /// or
        /// count - ArgumentOutOfRange_NeedNonNegNum</exception>
        /// <exception cref="ArgumentException">Argument_InvalidOffLen</exception>
        /// <exception cref="T:System.ArgumentNullException">buffer - ArgumentNull_Buffer</exception>
        /// <exception cref="T:System.ArgumentException">index - ArgumentOutOfRange_NeedNonNegNum
        /// or
        /// count - ArgumentOutOfRange_NeedNonNegNum</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">Argument_InvalidOffLen</exception>
        /// <exception cref="T:System.ObjectDisposedException">buffer - ArgumentNull_Buffer</exception>
        /// <exception cref="T:System.IO.IOException">index - ArgumentOutOfRange_NeedNonNegNum
        /// or
        /// count - ArgumentOutOfRange_NeedNonNegNum</exception>
        public override int Read(char[] buffer, int index, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer), "ArgumentNull_Buffer");
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), "ArgumentOutOfRange_NeedNonNegNum");
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "ArgumentOutOfRange_NeedNonNegNum");
            if (buffer.Length - index < count)
                throw new ArgumentException("Argument_InvalidOffLen");
            if (!_pendingC.HasValue)
                return R.Read(buffer, index, count);
            var c = _pendingC.Value; _pendingC = null;
            if (c == -1)
                return 0;
            buffer[index] = (char)c;
            return true ? R.Read(buffer, index + 1, count - 1) + 1 : 1;
        }
    }
}
