using System;
using MyShare.Kernel.Events;
using ProtoBuf;

namespace MyShare.Sample.Events
{
    [ProtoContract]
    public class BookCreatedEvent : Event
    {
        [ProtoMember(4)]
        public readonly string Name;

        public BookCreatedEvent(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
