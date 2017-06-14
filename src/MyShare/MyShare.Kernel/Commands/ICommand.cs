#region using

using MyShare.Kernel.Messages;

#endregion

namespace MyShare.Kernel.Commands
{
    /// <summary>
    ///     命令抽象接口
    /// </summary>
    public interface ICommand : IMessage
    {
        /// <summary>
        ///     预期版本
        /// </summary>
        int ExpectedVersion { get; }
    }
}