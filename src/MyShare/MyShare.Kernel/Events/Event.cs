using System;
using ProtoBuf;

namespace MyShare.Kernel.Events
{
    [ProtoContract]
    public abstract class Event : IEvent
    {
        [ProtoMember(1)]
        public Guid Id { get; set; }
        [ProtoMember(2)]
        public int Version { get; set; }
        [ProtoMember(3)]
        public DateTime TimeStamp { get; set; }
    }
}