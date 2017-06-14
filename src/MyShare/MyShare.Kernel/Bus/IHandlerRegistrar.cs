#region using

using System;
using System.Threading.Tasks;
using MyShare.Kernel.Messages;

#endregion

namespace MyShare.Kernel.Bus
{
    /// <summary>
    /// 处理器注册
    /// </summary>
    public interface IHandlerRegistrar
    {
        /// <summary>
        /// 注册处理器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        void RegisterHandler<T>(Func<T, Task> handler) where T : class, IMessage;
    }
}