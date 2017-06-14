#region using

using System;
using System.Threading.Tasks;

#endregion

namespace MyShare.Kernel.Domain
{
    public interface ISession
    {
        Task Add<T>(T aggregate) where T : AggregateRoot;
        Task<T> Get<T>(Guid id, int? expectedVersion = null) where T : AggregateRoot;
        Task Commit();
    }
}