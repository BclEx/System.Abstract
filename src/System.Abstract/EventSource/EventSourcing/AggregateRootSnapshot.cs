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

namespace System.Abstract.EventSourcing
{
    /// <summary>
    /// AggregateRootSnapshot
    /// </summary>
    public abstract class AggregateRootSnapshot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateRootSnapshot"/> class.
        /// </summary>
        public AggregateRootSnapshot() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateRootSnapshot" /> class.
        /// </summary>
        /// <param name="aggregate">The aggregate.</param>
        public AggregateRootSnapshot(AggregateRoot aggregate)
        {
            AggregateId = aggregate.AggregateId;
            LastEventDate = aggregate.LastEventDate;
            LastEventSequence = aggregate.LastEventSequence;
        }

        /// <summary>
        /// Gets or sets the aggregate Id.
        /// </summary>
        /// <value>The aggregate Id.</value>
        public object AggregateId { get; set; }

        /// <summary>
        /// Gets or sets the last event date.
        /// </summary>
        /// <value>The last event date.</value>
        public DateTime LastEventDate { get; set; }

        /// <summary>
        /// Gets or sets the last event sequence.
        /// </summary>
        /// <value>The last event sequence.</value>
        public int LastEventSequence { get; set; }
    }
}