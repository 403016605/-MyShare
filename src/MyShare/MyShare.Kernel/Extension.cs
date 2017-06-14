#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MyShare.Kernel.Bus;
using MyShare.Kernel.Cache;
using MyShare.Kernel.Commands;
using MyShare.Kernel.Defaults.Bus;
using MyShare.Kernel.Defaults.Cache;
using MyShare.Kernel.Defaults.Domain;
using MyShare.Kernel.Defaults.Events;
using MyShare.Kernel.Domain;
using MyShare.Kernel.Events;
using Scrutor;

#endregion

namespace MyShare.Kernel
{
    public static class Extension
    {
        /// <summary>
        ///     获取IServiceProvider
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceProvider ServiceProvider(this IServiceCollection services)
        {
            return services.BuildServiceProvider();
        }
    }
}