using Microsoft.Extensions.DependencyInjection;
using MyShare.Common.Defaults;
using MyShare.Kernel;

namespace MyShare.Common
{
    public static class Extension
    {
        public static Bootstrap UseCommonn(this Bootstrap bootstrap)
        {
            bootstrap.ServicesCollection.AddSingleton<ISerializer, Serializer>();
            return bootstrap;
        }
    }
}
