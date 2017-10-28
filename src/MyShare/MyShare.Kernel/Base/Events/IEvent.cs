#region using

using System;
using MyShare.Kernel.Base.Messages;

#endregion

namespace MyShare.Kernel.Base.Events
{
    /// <summary>
    ///     事件抽象接口
    /// </summary>
    public interface IEvent : IMessage
    {
        /// <summary>
        ///     聚合根标志
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        ///     聚合根版本
        /// </summary>
        int Version { get; set; }

        /// <summary>
        ///     时间戳
        /// </summary>
        DateTimeOffset TimeStamp { get; set; }
    }
}