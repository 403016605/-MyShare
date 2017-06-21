using System;
using MyShare.Kernel.Events;

namespace MyShare.Kernel.Data
{
    internal class StoreEvent : Event, IEntity
    {
        public byte[] Body { get; set; }
    }
}