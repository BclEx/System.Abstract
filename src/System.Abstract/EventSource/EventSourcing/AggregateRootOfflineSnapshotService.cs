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

using System.Collections.Generic;

namespace System.Abstract.EventSourcing
{
    /// <summary>
    /// IAggregateRootOfflineSnapshotService
    /// </summary>
    public interface IAggregateRootOfflineSnapshotService
    {
        /// <summary>
        /// Makes the snapshots.
        /// </summary>
        /// <param name="aggregateTypes">The aggregate types.</param>
        void MakeSnapshots(IEnumerable<Type> aggregateTypes);
    }

    /// <summary>
    /// AggregateRootOfflineSnapshotService
    /// </summary>
    /// <seealso cref="System.Abstract.EventSourcing.IAggregateRootOfflineSnapshotService" />
    public class AggregateRootOfflineSnapshotService : IAggregateRootOfflineSnapshotService
    {
        readonly IAggregateRootRepository _repository;
        readonly IOfflineSnaphotQuery _snaphotQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateRootOfflineSnapshotService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="snaphotQuery">The snaphot query.</param>
        /// <exception cref="ArgumentNullException">repository
        /// or
        /// snaphotQuery</exception>
        public AggregateRootOfflineSnapshotService(IAggregateRootRepository repository, IOfflineSnaphotQuery snaphotQuery)
        {
            _repository = repository ?? throw new ArgumentNullException("repository");
            _snaphotQuery = snaphotQuery ?? throw new ArgumentNullException("snaphotQuery");
        }

        /// <summary>
        /// Makes the snapshots.
        /// </summary>
        /// <param name="aggregateTypes">The aggregate types.</param>
        public void MakeSnapshots(IEnumerable<Type> aggregateTypes)
        {
            foreach (var item in _snaphotQuery.GetAggregatesToSnapshot(aggregateTypes))
            {
                var aggregate = _repository.GetById(item.Item1, item.AggregateId, AggregateRootQueryOptions.UseNullAggregates);
                if (aggregate != null)
                    _repository.MakeSnapshot(aggregate);
            }
        }
    }
}