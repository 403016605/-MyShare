#region using

using System.Threading.Tasks;

#endregion

namespace MyShare.Kernel.Base.Commands
{
    /// <summary>
    ///     命令发送者接口
    /// </summary>
    public interface ICommandSender
    {
        /// <summary>
        ///     命令发送
        /// </summary>
        /// <typeparam name="T">命令类型</typeparam>
        /// <param name="command">命令</param>
        /// <returns></returns>
        Task Send<T>(T command) where T : class, ICommand;
    }
}