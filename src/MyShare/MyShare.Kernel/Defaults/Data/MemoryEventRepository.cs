using System;
using System.Collections.Generic;
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
}