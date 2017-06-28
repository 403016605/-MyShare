using Microsoft.Extensions.DependencyInjection;
using MyShare.Kernel;
using MyShare.Sample.Queries;

namespace MyShare.Sample.Extensions
{
    public static class MyShareOptionsExtension
    {
        public static IMyShareOptions UseSample(this IMyShareOptions options)
        {
            options.ServicesCollection.AddSingleton<IQueryBook, QueryBook>();
            return options;
        }
    }
}
