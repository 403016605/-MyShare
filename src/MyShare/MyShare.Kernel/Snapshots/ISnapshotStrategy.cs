#region using

using System;
using MyShare.Kernel.Domain;

#endregion

namespace MyShare.Kernel.Snapshots
{
    /// <summary>
    /// ���ղ���
    /// </summary>
    public interface ISnapshotStrategy
    {
        /// <summary>
        /// �Ƿ���Ҫ����
        /// </summary>
        /// <param name="aggregate"></param>
        /// <returns></returns>
        bool ShouldMakeSnapShot(AggregateRoot aggregate);

        /// <summary>
        /// �Ƿ��ܹ�����
        /// </summary>
        /// <param name="aggregateType"></param>
        /// <returns></returns>
        bool IsSnapshotable(Type aggregateType);
    }
}