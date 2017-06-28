using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MyShare.Kernel
{

    public interface IMyShareOptions
    {
        Dictionary<string, Type> TypeDict { get; }

        IServiceProvider ServiceProvider { get; }
        IServiceCollection ServicesCollection { get; }


        IMyShareOptions InitKernel();

        MyShareConfig MyShareConfig { get; }
    }


}