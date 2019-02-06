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
    /// EventSourcingExtensions
    /// </summary>
    public static class EventSourcingExtensions
    {
        /// <summary>
        /// Gets the by ID.
        /// </summary>
        /// <typeparam name="TAggregateRoot">The type of the aggregate root.</typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="aggregateId">The aggregate Id.</param>
        /// <returns>TAggregateRoot.</returns>
        public static TAggregateRoot GetById<TAggregateRoot>(this IAggregateRootRepository repository, object aggregateId)
            where TAggregateRoot : AggregateRoot =>
            repository.GetById<TAggregateRoot>(aggregateId, 0);

        /// <summary>
        /// Gets the by ID.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="aggregateType">Type of the aggregate.</param>
        /// <param name="aggregateId">The aggregate Id.</param>
        /// <param name="queryOptions">The query options.</param>
        /// <returns>AggregateRoot.</returns>
        public static AggregateRoot GetById(this IAggregateRootRepository repository, Type aggregateType, object aggregateId, AggregateRootQueryOptions queryOptions = 0) =>
            repository.GetById<AggregateRoot>(aggregateId, queryOptions);

        /// <summary>
        /// Gets the many by I ds.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="aggregateIds">The aggregate Ids.</param>
        /// <param name="aggregateType">Type of the aggregate.</param>
        /// <param name="queryOptions">The query options.</param>
        /// <returns>IEnumerable&lt;AggregateRoot&gt;.</returns>
        public static IEnumerable<AggregateRoot> GetManyByIds(this IAggregateRootRepository repository, IEnumerable<object> aggregateIds, Type aggregateType, AggregateRootQueryOptions queryOptions = 0) =>
            repository.GetManyByIds<AggregateRoot>(aggregateIds, queryOptions);

        /// <summary>
        /// Makes the snapshot.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="aggregate">The aggregate.</param>
        public static void MakeSnapshot(this IAggregateRootRepository repository, AggregateRoot aggregate) =>
            repository.MakeSnapshot(aggregate, null);

        #region BehaveAs

        /// <summary>
        /// Behaves as.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service">The service.</param>
        /// <returns>T.</returns>
        public static T BehaveAs<T>(this IAggregateRootRepository service)
            where T : class, IAggregateRootRepository
        {
            IServiceWrapper<IAggregateRootRepository> serviceWrapper;
            do
            {
                serviceWrapper = (service as IServiceWrapper<IAggregateRootRepository>);
                if (serviceWrapper != null)
                    service = serviceWrapper.Base;
            } while (serviceWrapper != null);
            return service as T;
        }

        #endregion
    }
}