#region using

using MyShare.Kernel.Messages;

#endregion

namespace MyShare.Kernel.Events
{
    /// <summary>
    ///     事件处理器接口
    /// </summary>
    /// <typeparam name="T">事件类型</typeparam>
    public interface IEventHandler<in T> : IHandler<T> where T : IEvent
    {
    }
}