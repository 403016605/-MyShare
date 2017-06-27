using System;
using MyShare.Kernel.Events;

namespace MyShare.Kernel.Infrastructure
{
    /// <summary>
    /// 事件实体
    /// </summary>
    public class EventEntity : EntityBase
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// 事件类容
        /// </summary>
        public byte[] EventContent { get; set; }

        public EventEntity(Guid id) : base(id)
        {
        }
        public EventEntity() 
        {
        }
    }
}
