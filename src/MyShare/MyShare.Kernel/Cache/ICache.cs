#region using

using System;
using MyShare.Kernel.Domain;

#endregion

namespace MyShare.Kernel.Cache
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 是否缓存跟踪
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool IsTracked(Guid id);

        /// <summary>
        /// 添加到缓存中
        /// </summary>
        /// <param name="id"></param>
        /// <param name="aggregate"></param>
        void Set(Guid id, AggregateRoot aggregate);

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AggregateRoot Get(Guid id);

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="id"></param>
        void Remove(Guid id);

        /// <summary>
        /// 缓存移除后的回调
        /// </summary>
        /// <param name="action"></param>
        void RegisterEvictionCallback(Action<Guid> action);
    }
}