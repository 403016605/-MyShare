using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MyShare.Kernel;
using MyShare.Sample.Queries;

namespace MyShare.Sample.Extensions
{
    public static class MyShareOptionsExtension
    {
        public static IMyShareOptions UseSample(this IMyShareOptions options)
        {
            var a = Assembly.GetEntryAssembly().GetReferencedAssemblies();

            var currentAssembly = typeof(ModuleInfo).GetTypeInfo().Assembly;

            options.AddHandlers(currentAssembly);
            options.AddBus(currentAssembly);
            options.ServicesCollection.AddSingleton<IQueryBook, QueryBook>();
            return options;
        }
    }
}
