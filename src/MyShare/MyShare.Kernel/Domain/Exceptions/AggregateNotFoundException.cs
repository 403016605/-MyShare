#region using

using System;

#endregion

namespace MyShare.Kernel.Domain.Exceptions
{
    public class AggregateNotFoundException : System.Exception
    {
        public AggregateNotFoundException(Type t, Guid id)
            : base($"Aggregate {id} of type {t.FullName} was not found")
        {
        }
    }
}