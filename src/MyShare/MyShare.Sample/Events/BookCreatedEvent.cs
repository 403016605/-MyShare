using System;
using MyShare.Kernel.Events;

namespace MyShare.Sample.Events
{
    public class BookCreatedEvent : Event
    {
        public readonly string Name;

        public BookCreatedEvent(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
