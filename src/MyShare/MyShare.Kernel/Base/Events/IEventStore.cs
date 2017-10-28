#region using

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace MyShare.Kernel.Base.Events
{
    /// <summary>
    ///     事件存储接口
    /// </summary>
    public interface IEventStore
    {
        /// <summary>
        ///     事件保存
        /// </summary>
        /// <param name="events">事件集合</param>
        /// <returns></returns>
        Task Save(IEnumerable<IEvent> events);

        /// <summary>
        ///     获取事件集合
        /// </summary>
        /// <param name="aggregateId">聚合跟</param>
        /// <param name="fromVersion">起始版本</param>
        /// <returns>事件集合</returns>
        Task<IEnumerable<IEvent>> Get(Guid aggregateId, int fromVersion);
    }
}