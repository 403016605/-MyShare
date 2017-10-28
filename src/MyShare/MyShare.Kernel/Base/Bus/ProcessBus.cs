#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyShare.Kernel.Base.Commands;
using MyShare.Kernel.Base.Events;
using MyShare.Kernel.Base.Messages;
using MyShare.Kernel.Exceptions;

#endregion

namespace MyShare.Kernel.Base.Bus
{
    public class ProcessBus : IProcessBus
    {
        private readonly Dictionary<Type, List<Func<IMessage, Task>>> _routes =
            new Dictionary<Type, List<Func<IMessage, Task>>>();

        public Task Send<T>(T command) where T : class, ICommand
        {
            var commandType = command.GetType();

            if (!_routes.TryGetValue(commandType, out var handlers))
                throw new NoHandlerRegisteredException(commandType);

            if (handlers.Count != 1)
                throw new MoreThanOneHandlerRegisteredException(commandType);

            return handlers[0](command);
        }

        public Task Publish<T>(T @event) where T : class, IEvent
        {
            return !_routes.TryGetValue(@event.GetType(), out var handlers)
                ? Task.CompletedTask
                : Task.WhenAll(handlers.Select(handler => handler(@event)));
        }

        public void On<T>(Func<T, Task> handler) where T : class, IMessage
        {
            if (!_routes.TryGetValue(typeof(T), out var handlers))
            {
                handlers = new List<Func<IMessage, Task>>();
                _routes.Add(typeof(T), handlers);
            }
            handlers.Add(x => handler((T) x));
        }
    }
}