#region using

using System;

#endregion

namespace MyShare.Kernel.Domain.Exceptions
{
    public class AggregateNotFoundException : Exception
    {
        public AggregateNotFoundException(Type t, Guid id)
            : base($"类型为 {t.FullName} 的聚合根 {id} 不存在!")
        {
        }
    }
}