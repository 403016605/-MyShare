using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyShare.Kernel;
using MyShare.Sample.Queries;

namespace MyShare.Sample.Extensions
{
    public static class MyShareOptionsExtensions
    {
        public static IMyShareOptions UseSample(this IMyShareOptions myShareOptions)
        {
            var currentAssembly = typeof(ModuleInfo).GetTypeInfo().Assembly;

            myShareOptions.AddHandlers(currentAssembly);
            myShareOptions.AddBus(currentAssembly);
            myShareOptions.ServicesCollection.AddSingleton<IQueryBook, QueryBook>();
            return myShareOptions;
        }
    }
}
