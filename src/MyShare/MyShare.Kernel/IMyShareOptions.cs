using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MyShare.Kernel.Events;

namespace MyShare.Kernel
{

    public interface IMyShareOptions
    {
        IServiceCollection ServicesCollection { get; }

        IServiceProvider ServiceProvider { get; }

        IMyShareOptions InitKernel(IDbConnection conn, List<Type> entityTypes);

        IMyShareOptions AddHandlers(Assembly assembly);

        IMyShareOptions AddBus(Assembly assembly);
    }


}