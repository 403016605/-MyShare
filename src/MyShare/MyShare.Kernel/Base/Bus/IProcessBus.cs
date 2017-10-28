using MyShare.Kernel.Base.Commands;
using MyShare.Kernel.Base.Events;

namespace MyShare.Kernel.Base.Bus
{
    /// <summary>
    ///     处理器注册
    /// </summary>
    public interface IProcessBus : IEventPublisher, ICommandSender, IHandlerRegistrar
    {
    }
}