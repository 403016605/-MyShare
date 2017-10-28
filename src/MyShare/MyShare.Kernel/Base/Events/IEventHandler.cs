#region using

using MyShare.Kernel.Base.Messages;

#endregion

namespace MyShare.Kernel.Base.Events
{
    /// <summary>
    ///     事件处理器接口
    /// </summary>
    /// <typeparam name="T">事件类型</typeparam>
    public interface IEventHandler<in T> : IHandler<T> where T : IEvent
    {
    }
}