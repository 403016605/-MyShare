#region using

using System;

#endregion

namespace MyShare.Kernel.Domain.Exceptions
{
    public class AggregateOrEventMissingIdException : Exception
    {
        public AggregateOrEventMissingIdException(Type aggregateType, Type eventType)
            : base(
                $"An event of type {eventType.FullName} was tried to save from {aggregateType.FullName} but no id where set on either")
        {
        }
    }
}