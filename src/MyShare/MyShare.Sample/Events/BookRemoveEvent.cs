using System;
using MyShare.Kernel.Events;

namespace MyShare.Sample.Events
{
    public class BookRemoveEvent : Event
    {
        public BookRemoveEvent(Guid id)
        {
            Id = id;
        }
    }
}