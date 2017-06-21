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
            var body = _serializer.Serialize(events.First());

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
