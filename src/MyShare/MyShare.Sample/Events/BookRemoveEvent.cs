using System;
using MyShare.Kernel.Events;
using ProtoBuf;

namespace MyShare.Sample.Events
{
    [ProtoContract]
    public class BookRemoveEvent : IEvent
    {
        [ProtoMember(1)]
        public Guid Id { get; set; }
        [ProtoMember(2)]
        public int Version { get; set; }
        [ProtoMember(3)]
        public DateTimeOffset TimeStamp { get; set; }
        public BookRemoveEvent(Guid id)
        {
            Id = id;
        }
    }
}