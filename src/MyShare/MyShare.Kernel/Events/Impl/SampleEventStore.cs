using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyShare.Kernel.Common;
using MyShare.Kernel.Data;
using MyShare.Kernel.Infrastructure;

namespace MyShare.Kernel.Events.Impl
{
    public class SampleEventStore : IEventStore
    {
        private readonly IEventPublisher _publisher;
        private readonly ISerializer _serializer;
        private readonly DataContext _context;
        private readonly IMyShareOptions _myShareOptions;

        public SampleEventStore(IMyShareOptions myShareOptions,IEventPublisher publisher, ISerializer serializer, DataContext context)
        {
            _publisher = publisher;
            _context = context;
            _serializer=serializer;
            _myShareOptions = myShareOptions;
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

                if (_context.Set<EventEntity>().Any(m => m.Id == @event.Id && m.TimeStamp == @event.TimeStamp)==false)
                {
                    _context.Entry(eventEntity).State = EntityState.Added;
                    _context.SaveChanges();
                }


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
            List<IEvent> result = new List<IEvent>();

            var eventEntityies= _context.Set<EventEntity>().Where(m => m.Id == aggregateId && m.Version > fromVersion).ToList();

            if (eventEntityies != null)
            {
                foreach (var eventEntity in eventEntityies)
                {
                    var obj = _serializer.Deserialize(_myShareOptions.TypeDict[eventEntity.EventType],
                        eventEntity.EventContent);
                    result.Add(obj as IEvent);
                }
            }

            return Task.FromResult(result.AsEnumerable());
        }
    }
}
