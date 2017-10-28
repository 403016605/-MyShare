#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyShare.Kernel.Base.Events;
using MyShare.Kernel.Domain.Exceptions;
using MyShare.Kernel.Domain.Factories;

#endregion

namespace MyShare.Kernel.Domain
{
    internal class Repository : IRepository
    {
        private readonly IEventStore _eventStore;

        public Repository(IEventStore eventStore)
        {
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        }

        public async Task Save<T>(T aggregate, int? expectedVersion = null) where T : AggregateRoot
        {
            if (expectedVersion != null && (await _eventStore.Get(aggregate.Id, expectedVersion.Value)).Any())
                throw new ConcurrencyException(aggregate.Id);

            var changes = aggregate.FlushUncommitedChanges();
            await _eventStore.Save(changes);
        }

        public Task<T> Get<T>(Guid aggregateId) where T : AggregateRoot
        {
            return LoadAggregate<T>(aggregateId);
        }

        private async Task<T> LoadAggregate<T>(Guid id) where T : AggregateRoot
        {
            var events = await _eventStore.Get(id, -1);
            var eventList = events as IList<IEvent> ?? events.ToList();
            if (!eventList.Any())
                throw new AggregateNotFoundException(typeof(T), id);

            var aggregate = AggregateFactory.CreateAggregate<T>();
            aggregate.LoadFromHistory(eventList);
            return aggregate;
        }
    }
}