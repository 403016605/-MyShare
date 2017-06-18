using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using MyShare.Kernel.Common;
using MyShare.Kernel.Data;
using MyShare.Kernel.Events;

namespace MyShare.Kernel.Defaults.Data
{
    internal class MemoryEventRepository : IEventRepository
    {
        private readonly Dictionary<Guid, List<IEvent>> _inMemoryDb = new Dictionary<Guid, List<IEvent>>();


        private ISerializer _serializer;

        public MemoryEventRepository(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public IEnumerable<IEvent> Get(Guid Id)
        {
            if (_inMemoryDb.TryGetValue(Id, out var events))
            {
                return events;
            };

            return new List<IEvent>();
        }

        public void Save(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                _inMemoryDb.TryGetValue(@event.Id, out var list);
                if (list == null)
                {
                    list = new List<IEvent>();
                    _inMemoryDb.Add(@event.Id, list);
                }
                list.Add(@event);
            }
        }

        public void Save(IEvent @event)
        {
            _inMemoryDb.TryGetValue(@event.Id, out var list);
            if (list == null)
            {
                list = new List<IEvent>();
                _inMemoryDb.Add(@event.Id, list);
            }
            list.Add(@event);
        }
    }


    internal class EventRepository : IEventRepository
    {
        private IDbConnection _conn;

        private ISerializer _serializer;

        public EventRepository(IDbConnection conn, ISerializer serializer)
        {
            _conn = conn;
            _serializer = serializer;
        }

        public IEnumerable<IEvent> Get(Guid Id)
        {
            var storeEvents = _conn.Query<StoreEvent>(Id).AsList();

            return storeEvents.Select(m => _serializer.Deserialize<IEvent>(m.Body));
        }

        public void Save(IEnumerable<IEvent> events)
        {
            _conn.Insert(events.Select(item => new StoreEvent
            {
                Id = item.Id,
                Version = item.Version,
                TimeStamp = item.TimeStamp,
                Body = _serializer.Serialize(item)
            }));
        }

        public void Save(IEvent @event)
        {
            _conn.Insert(new StoreEvent
            {
                Id = @event.Id,
                Version = @event.Version,
                TimeStamp = @event.TimeStamp,
                Body = _serializer.Serialize(@event)
            });
        }
    }
}
