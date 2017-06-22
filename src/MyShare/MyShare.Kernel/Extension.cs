#region using

using System;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace MyShare.Kernel
{
    public static class Extension
    {
        #region IServiceCollection

        /// <summary>
        ///     获取IServiceProvider
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceProvider ServiceProvider(this IServiceCollection services)
        {
            return services.BuildServiceProvider();
        }

        #endregion
    }
}