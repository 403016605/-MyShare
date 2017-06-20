using System;
using MyShare.Kernel.Events;
using ProtoBuf;

namespace MyShare.Sample.Events
{
    [ProtoContract]
    public class BookRemoveEvent : Event
    {
        public BookRemoveEvent(Guid id)
        {
            Id = id;
        }
    }
}