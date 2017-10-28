using System;

namespace MyShare.Kernel.Infrastructure
{
    /// <summary>
    ///     实体抽象类
    /// </summary>
    public abstract class EntityBase
    {
        protected EntityBase(Guid id)
        {
            Id = id;
            CreateTime = DateTime.Now;
            ModifyTime = CreateTime;
        }

        /// <summary>
        ///     实体主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     实体版本
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        public DateTime CreateTime { get; protected set; }

        /// <summary>
        ///     修改时间
        /// </summary>
        public DateTime ModifyTime { get; protected set; }
    }
}