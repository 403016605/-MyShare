#region using

using System;
using System.Collections.Generic;
using MyShare.Kernel.Base.Events;
using MyShare.Kernel.Domain.Exceptions;
using MyShare.Kernel.Infrastructure;

#endregion

namespace MyShare.Kernel.Domain
{
    /// <summary>
    ///     聚合根抽象类
    /// </summary>
    public abstract class AggregateRoot
    {
        private readonly List<IEvent> _changes = new List<IEvent>();

        public Guid Id { get; protected set; }
        public int Version { get; protected set; }

        public IEvent[] GetUncommittedChanges()
        {
            lock (_changes)
            {
                return _changes.ToArray();
            }
        }

        public IEvent[] FlushUncommitedChanges()
        {
            lock (_changes)
            {
                var changes = _changes.ToArray();
                var i = 0;
                foreach (var @event in changes)
                {
                    if (@event.Id == Guid.Empty && Id == Guid.Empty)
                        throw new AggregateOrEventMissingIdException(GetType(), @event.GetType());

                    if (@event.Id == Guid.Empty)
                        @event.Id = Id;

                    i++;
                    @event.Version = Version + i;
                    @event.TimeStamp = DateTime.Now;
                }
                Version = Version + _changes.Count;
                _changes.Clear();
                return changes;
            }
        }

        public void LoadFromHistory(IEnumerable<IEvent> history)
        {
            foreach (var e in history)
            {
                if (e.Version != Version + 1)
                    throw new EventsOutOfOrderException(e.Id);

                ApplyChange(e, false);
            }
        }

        protected void ApplyChange(IEvent @event)
        {
            ApplyChange(@event, true);
        }

        private void ApplyChange(IEvent @event, bool isNew)
        {
            lock (_changes)
            {
                this.AsDynamic().Apply(@event);
                if (isNew)
                {
                    _changes.Add(@event);
                }
                else
                {
                    Id = @event.Id;
                    Version++;
                }
            }
        }
    }
}