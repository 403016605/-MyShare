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
        Dictionary<string, Type> TypeDict { get; }

        IServiceProvider ServiceProvider { get; }
        IServiceCollection ServicesCollection { get; }


        IMyShareOptions InitKernel(IDbConnection conn, List<Type> entityTypes);

        IMyShareOptions AddHandlers(Assembly assembly);

        IMyShareOptions AddBus(Assembly assembly);
    }


}