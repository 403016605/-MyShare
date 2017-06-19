using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MyShare.Kernel;
using MyShare.Sample.Queries;
using Pomelo.Data.MySql;

namespace MyShare.Sample
{
    public static class Extension
    {
        public static Bootstrap InitSample(this Bootstrap bootstrap)
        {
            var currentAssembly=typeof(ModuleInfo).GetTypeInfo().Assembly;

            bootstrap.AddHandlers(currentAssembly);
            bootstrap.AddBus(currentAssembly);
            bootstrap.ServicesCollection.AddSingleton<IQueryBook, QueryBook>(); 
            return bootstrap;
        }
    }
}
