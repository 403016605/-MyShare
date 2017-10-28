using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MyShare.Kernel;
using MyShare.Sample.Queries;

namespace MyShare.Sample.Extensions
{
    public static class MyShareOptionsExtensions
    {
        public static IMyShareOptions UseSample(this IMyShareOptions myShareOptions)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();

            myShareOptions.AddHandlers(currentAssembly);
            myShareOptions.AddBus(currentAssembly);
            myShareOptions.ServicesCollection.AddSingleton<IQueryBook, QueryBook>();
            return myShareOptions;
        }
    }
}
