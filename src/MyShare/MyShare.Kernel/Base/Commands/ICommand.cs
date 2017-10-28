#region using

using MyShare.Kernel.Base.Messages;

#endregion

namespace MyShare.Kernel.Base.Commands
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