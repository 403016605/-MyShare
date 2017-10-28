#region using

using System.Threading.Tasks;

#endregion

namespace MyShare.Kernel.Base.Messages
{
    /// <summary>
    ///     消息处理器接口
    /// </summary>
    /// <typeparam name="T">消息类型</typeparam>
    public interface IHandler<in T> where T : IMessage
    {
        /// <summary>
        ///     消息处理函数
        /// </summary>
        /// <param name="message">消息类型</param>
        /// <returns></returns>
        Task Handle(T message);
    }
}