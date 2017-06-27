using System;
using System.Data;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MyShare.Kernel
{

    public interface IMyShareOptions
    {
        IServiceCollection ServicesCollection { get; }

        IServiceProvider ServiceProvider { get; }

        IMyShareOptions InitKernel(IDbConnection conn);

        IMyShareOptions AddHandlers(Assembly assembly);

        IMyShareOptions AddBus(Assembly assembly);
    }
}