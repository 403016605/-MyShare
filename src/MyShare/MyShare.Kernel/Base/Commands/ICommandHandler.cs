#region using

using MyShare.Kernel.Base.Messages;

#endregion

namespace MyShare.Kernel.Base.Commands
{
    /// <summary>
    ///     命令处理器接口
    /// </summary>
    /// <typeparam name="T">命令类型</typeparam>
    public interface ICommandHandler<in T> : IHandler<T> where T : ICommand
    {
    }
}