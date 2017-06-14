#region using

using System;
using System.Threading.Tasks;

#endregion

namespace MyShare.Kernel.Domain
{
    public interface IRepository
    {
        Task Save<T>(T aggregate, int? expectedVersion = null) where T : AggregateRoot;
        Task<T> Get<T>(Guid aggregateId) where T : AggregateRoot;
    }
}