using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyShare.Kernel.Common;
using MyShare.Kernel.Data;
using MyShare.Kernel.Events;
using MyShare.Kernel.Infrastructure;

namespace MyShare.Kernel.Defaults.Events
{
    public class SampleEventStore : IEventStore
    {
        //private readonly Dictionary<Guid, List<IEvent>> _inMemoryDb = new Dictionary<Guid, List<IEvent>>();
        private readonly IEventPublisher _publisher;
        private readonly ISerializer _serializer;
        private readonly DataContext _context;

        public SampleEventStore(IEventPublisher publisher, ISerializer serializer, DataContext context)
        {
            _publisher = publisher;
            _context = context;
            _serializer=serializer;
        }

        public async Task Save(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                var eventEntity=new EventEntity(@event.Id)
                {
                    EventType = @event.GetType().FullName,
                    EventContent = _serializer.Serialize(@event),
                    Version = @event.Version,
                    TimeStamp=@event.TimeStamp
                };
                _context.Entry(eventEntity).State=EntityState.Added;
                _context.SaveChanges();


                //_inMemoryDb.TryGetValue(@event.Id, out var list);
                //if (list == null)
                //{
                //    list = new List<IEvent>();
                //    _inMemoryDb.Add(@event.Id, list);
                //}
                //list.Add(@event);

                await _publisher.Publish(@event);
            }
        }

        public Task<IEnumerable<IEvent>> Get(Guid aggregateId, int fromVersion)
        {

            var eventEntityies= _context.Set<EventEntity>().Where(m => m.Id == aggregateId && m.Version > fromVersion).ToList();

            var a=eventEntityies[0];

            var b= _serializer.Deserialize(Type.GetType(a.EventType), a.EventContent);

            return Task.FromResult(
            eventEntityies == null
                ? new List<IEvent>()
                : (IEnumerable<IEvent>) eventEntityies.Select(e => (IEvent)_serializer.Deserialize(Type.GetType(e.EventType),e.EventContent)).ToList());



            //_inMemoryDb.TryGetValue(aggregateId, out var events);
            //return Task.FromResult(events?.Where(x => x.Version > fromVersion) ?? new List<IEvent>());
        }
    }
}
