#region using

using System;

#endregion

namespace MyShare.Kernel.Domain.Exceptions
{
    public class EventsOutOfOrderException : Exception
    {
        public EventsOutOfOrderException(Guid id)
            : base($"Eventstore gave event for aggregate {id} out of order")
        {
        }
    }
}