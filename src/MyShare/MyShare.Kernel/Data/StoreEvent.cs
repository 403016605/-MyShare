using System;
using MyShare.Kernel.Events;

namespace MyShare.Kernel.Data
{
    internal class StoreEvent : IEvent, IEntity
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTime TimeStamp { get; set; }

        public byte[] Body { get; set; }
    }
}