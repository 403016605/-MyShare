#region using

using System;
using System.Threading.Tasks;

#endregion

namespace MyShare.Kernel.Snapshots
{
    /// <summary>
    /// 快照存储接口
    /// </summary>
    public interface ISnapshotStore
    {
        /// <summary>
        /// 获取快照
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Snapshot> Get(Guid id);

        /// <summary>
        /// 存储快照
        /// </summary>
        /// <param name="snapshot"></param>
        /// <returns></returns>
        Task Save(Snapshot snapshot);
    }
}