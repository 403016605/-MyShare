using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using MyShare.Kernel.Configs;

namespace MyShare.Kernel
{
    public interface IMyShareOptions
    {
        Dictionary<string, Type> TypeDict { get; }
        IServiceProvider ServiceProvider { get; }
        IServiceCollection ServicesCollection { get; }
        MyShareConfig MyShareConfig { get; }
    }
}