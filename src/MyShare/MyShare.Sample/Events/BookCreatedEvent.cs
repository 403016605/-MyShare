using System;
using MyShare.Kernel.Events;
using ProtoBuf;

namespace MyShare.Sample.Events
{
    [ProtoContract]
    public class BookCreatedEvent : IEvent
    {
        [ProtoMember(1)]
        public Guid Id { get; set; }
        [ProtoMember(2)]
        public int Version { get; set; }
        [ProtoMember(3)]
        public long TimeStamp { get; set; }
        [ProtoMember(4)]
        public string Name { get; set; }

        public BookCreatedEvent(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public BookCreatedEvent()
        {
        }
    }
}
