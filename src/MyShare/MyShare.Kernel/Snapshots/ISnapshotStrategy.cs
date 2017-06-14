#region using

using System;
using MyShare.Kernel.Domain;

#endregion

namespace MyShare.Kernel.Snapshots
{
    /// <summary>
    /// 快照策略
    /// </summary>
    public interface ISnapshotStrategy
    {
        /// <summary>
        /// 是否需要快照
        /// </summary>
        /// <param name="aggregate"></param>
        /// <returns></returns>
        bool ShouldMakeSnapShot(AggregateRoot aggregate);

        /// <summary>
        /// 是否能够快照
        /// </summary>
        /// <param name="aggregateType"></param>
        /// <returns></returns>
        bool IsSnapshotable(Type aggregateType);
    }
}