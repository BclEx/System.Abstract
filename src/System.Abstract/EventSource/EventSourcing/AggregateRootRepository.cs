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
using System.Linq;

namespace System.Abstract.EventSourcing
{
    /// <summary>
    /// IAggregateRootRepository
    /// </summary>
    public interface IAggregateRootRepository
    {
        /// <summary>
        /// Gets the by Id.
        /// </summary>
        /// <typeparam name="TAggregateRoot">The type of the aggregate root.</typeparam>
        /// <param name="aggregateId">The aggregate Id.</param>
        /// <param name="queryOptions">The query options.</param>
        /// <returns>TAggregateRoot.</returns>
        TAggregateRoot GetById<TAggregateRoot>(object aggregateId, AggregateRootQueryOptions queryOptions)
            where TAggregateRoot : AggregateRoot;

        /// <summary>
        /// Gets the many by Ids.
        /// </summary>
        /// <typeparam name="TAggregateRoot">The type of the aggregate root.</typeparam>
        /// <param name="aggregateIds">The aggregate Ids.</param>
        /// <param name="queryOptions">The query options.</param>
        /// <returns>IEnumerable&lt;TAggregateRoot&gt;.</returns>
        IEnumerable<TAggregateRoot> GetManyByIds<TAggregateRoot>(IEnumerable<object> aggregateIds, AggregateRootQueryOptions queryOptions)
            where TAggregateRoot : AggregateRoot;

        /// <summary>
        /// Gets the events by Id.
        /// </summary>
        /// <param name="aggregateId">The aggregate Id.</param>
        /// <returns>IEnumerable&lt;Event&gt;.</returns>
        IEnumerable<Event> GetEventsById(object aggregateId);

        /// <summary>
        /// Saves the specified aggregate.
        /// </summary>
        /// <param name="aggregate">The aggregate.</param>
        void Save(AggregateRoot aggregate);
        /// <summary>
        /// Saves the specified aggregate.
        /// </summary>
        /// <param name="aggregate">The aggregate.</param>
        void Save(IEnumerable<AggregateRoot> aggregate);

        /// <summary>
        /// Makes the snapshot.
        /// </summary>
        /// <param name="aggregate">The aggregate.</param>
        /// <param name="predicate">The predicate.</param>
        void MakeSnapshot(AggregateRoot aggregate, Func<IAggregateRootRepository, AggregateRoot, bool> predicate);
    }

    /// <summary>
    /// AggregateRootRepository
    /// </summary>
    /// <seealso cref="System.Abstract.EventSourcing.IAggregateRootRepository" />
    public class AggregateRootRepository : IAggregateRootRepository
    {
        readonly IEventStore _eventStore;
        readonly IBatchedEventStore _batchedEventStore;
        readonly IAggregateRootSnapshotStore _snapshotStore;
        readonly IBatchedAggregateRootSnapshotStore _batchedSnapshotStore;
        readonly Action<IEnumerable<Event>> _eventDispatcher;
        readonly Func<Type, AggregateRoot> _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateRootRepository" /> class.
        /// </summary>
        /// <param name="eventStore">The event store.</param>
        /// <param name="snapshotStore">The snapshot store.</param>
        /// <param name="eventDispatcher">The event dispatcher.</param>
        /// <param name="factory">The factory.</param>
        /// <exception cref="ArgumentNullException">eventStore</exception>
        /// <exception cref="System.ArgumentNullException">eventStore</exception>
        public AggregateRootRepository(IEventStore eventStore, IAggregateRootSnapshotStore snapshotStore, Action<IEnumerable<Event>> eventDispatcher = null, Func<Type, AggregateRoot> factory = null)
        {
            _eventStore = eventStore ?? throw new ArgumentNullException("eventStore");
            _batchedEventStore = eventStore as IBatchedEventStore;
            _snapshotStore = snapshotStore;
            _batchedSnapshotStore = snapshotStore as IBatchedAggregateRootSnapshotStore;
            _eventDispatcher = eventDispatcher;
            _factory = factory ?? EventSource.DefaultFactory.Factory;
        }

        /// <summary>
        /// Gets the events by ID.
        /// </summary>
        /// <param name="aggregateId">The aggregate Id.</param>
        /// <returns></returns>
        public IEnumerable<Event> GetEventsById(object aggregateId) =>
            _eventStore.GetEventsByID(aggregateId, 0);

        /// <summary>
        /// Gets the by ID.
        /// </summary>
        /// <typeparam name="TAggregateRoot">The type of the aggregate root.</typeparam>
        /// <param name="aggregateId">The aggregate Id.</param>
        /// <param name="queryOptions">The query options.</param>
        /// <returns>TAggregateRoot.</returns>
        /// <exception cref="ArgumentNullException">aggregateID</exception>
        /// <exception cref="InvalidOperationException">aggregate</exception>
        public TAggregateRoot GetById<TAggregateRoot>(object aggregateId, AggregateRootQueryOptions queryOptions)
             where TAggregateRoot : AggregateRoot
        {
            if (aggregateId == null)
                throw new ArgumentNullException("aggregateID");
            var aggregate = (_factory(typeof(TAggregateRoot)) as TAggregateRoot);
            if (aggregate == null)
                throw new InvalidOperationException("aggregate");
            // find snapshot
            var loaded = false;
            AggregateRootSnapshot snapshot = null;
            if (_snapshotStore != null)
            {
                var snapshoter = (aggregate as ICanAggregateRootSnapshot);
                if (snapshoter != null && (snapshot = _snapshotStore.GetLatestSnapshot<TAggregateRoot>(aggregateId)) != null)
                {
                    loaded = true;
                    snapshoter.LoadSnapshot(snapshot);
                }
            }
            // load events
            var events = _eventStore.GetEventsByID(aggregateId, (snapshot != null ? snapshot.LastEventSequence : 0));
            loaded |= ((IAggregateRootStateAccessor)aggregate).LoadFromHistory(events);
            return (queryOptions & AggregateRootQueryOptions.UseNullAggregates) == 0  || loaded ? aggregate : null;
        }

        /// <summary>
        /// Gets the many by Ids.
        /// </summary>
        /// <typeparam name="TAggregateRoot">The type of the aggregate root.</typeparam>
        /// <param name="aggregateIds">The aggregate Ids.</param>
        /// <param name="queryOptions">The query options.</param>
        /// <returns></returns>
        public IEnumerable<TAggregateRoot> GetManyByIds<TAggregateRoot>(IEnumerable<object> aggregateIds, AggregateRootQueryOptions queryOptions)
            where TAggregateRoot : AggregateRoot
        {
            if (aggregateIds == null)
                throw new ArgumentNullException(nameof(aggregateIds));
            return aggregateIds.Select(x => GetById<TAggregateRoot>(x, queryOptions)).ToList();
        }

        /// <summary>
        /// Saves the specified aggregate.
        /// </summary>
        /// <param name="aggregate">The aggregate.</param>
        public void Save(AggregateRoot aggregate)
        {
            if (aggregate == null)
                throw new ArgumentNullException("aggregate");
            var accessAggregateState = (IAggregateRootStateAccessor)aggregate;
            var events = accessAggregateState.GetUncommittedChanges();
            _eventStore.SaveEvents(aggregate.AggregateId, events);
            _eventDispatcher?.Invoke(events);
            accessAggregateState.MarkChangesAsCommitted();
            Func<IAggregateRootRepository, AggregateRoot, bool> inlineSnapshotPredicate;
            if (_snapshotStore != null && (inlineSnapshotPredicate = _snapshotStore.InlineSnapshotPredicate) != null && aggregate is ICanAggregateRootSnapshot)
                MakeSnapshot(aggregate, inlineSnapshotPredicate);
        }
        /// <summary>
        /// Saves the specified aggregates.
        /// </summary>
        /// <param name="aggregates">The aggregates.</param>
        public void Save(IEnumerable<AggregateRoot> aggregates)
        {
            if (aggregates == null)
                throw new ArgumentNullException("aggregates");
            foreach (var aggregate in aggregates)
                Save(aggregate);
        }

        /// <summary>
        /// Makes the snapshot.
        /// </summary>
        /// <param name="aggregate">The aggregate.</param>
        /// <param name="predicate">The predicate.</param>
        public void MakeSnapshot(AggregateRoot aggregate, Func<IAggregateRootRepository, AggregateRoot, bool> predicate)
        {
            if (aggregate == null)
                throw new ArgumentNullException("aggregate");
            ICanAggregateRootSnapshot snapshoter;
            if (_snapshotStore != null && (snapshoter = aggregate as ICanAggregateRootSnapshot) != null)
                if (predicate == null || predicate(this, aggregate))
                    _snapshotStore.SaveSnapshot(aggregate.GetType(), snapshoter.GetSnapshot());
        }
    }
}