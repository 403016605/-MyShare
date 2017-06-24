using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace MyShare.Kernel
{
    public interface IMyShareOptions
    {
        IServiceCollection ServicesCollection { get; }

        IServiceProvider ServiceProvider { get; }

        IMyShareOptions Start();

        IMyShareOptions AddHandlers(Assembly assembly);

        IMyShareOptions AddBus(Assembly assembly);
    }
}
