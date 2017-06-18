using System;
using System.Collections.Generic;
using System.Data;
using MyShare.Kernel.Events;
using Dapper;
using MyShare.Kernel.Common;
using System.Linq;

namespace MyShare.Kernel.Data
{
    public interface IEventRepository
    {
        /// <summary>
        /// 聚合事件查询
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        IEnumerable<IEvent> Get(Guid Id);

        /// <summary>
        /// 聚合事件保存
        /// </summary>
        /// <param name="events"></param>
        void Save(IEnumerable<IEvent> events);

        void Save(IEvent @event);

    }

    internal class StoreEvent : IEvent, IEntity
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }

        public byte[] Body { get; set; }
    }

    public interface IEntity
    {

    }

}
