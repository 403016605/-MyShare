#region using

using System.Threading.Tasks;

#endregion

namespace MyShare.Kernel.Base.Events
{
    /// <summary>
    ///     事件发布者接口
    /// </summary>
    public interface IEventPublisher
    {
        /// <summary>
        ///     事件发布
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="event">时间</param>
        /// <returns></returns>
        Task Publish<T>(T @event) where T : class, IEvent;
    }
}