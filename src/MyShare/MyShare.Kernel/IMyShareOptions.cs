using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MyShare.Kernel
{
    public interface IMyShareOptions
    {
        IServiceCollection ServicesCollection { get; }

        IMyShareOptions Start();

        IMyShareOptions AddHandlers(Assembly assembly);

        IMyShareOptions AddBus(Assembly assembly);

        #region IServiceCollection

        IMyShareOptions AddScoped<TService>() where TService : class;
        IMyShareOptions AddScoped(Type serviceType, Type implementationType);
        IMyShareOptions AddScoped(Type serviceType, Func<IServiceProvider, object> implementationFactory);
        IMyShareOptions AddScoped<TService, TImplementation>()
           where TService : class
           where TImplementation : class, TService;
        IMyShareOptions AddScoped(Type serviceType);
        IMyShareOptions AddScoped<TService>(Func<IServiceProvider, TService> implementationFactory) where TService : class;
        IMyShareOptions AddScoped<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory)
            where TService : class
            where TImplementation : class, TService;
        IMyShareOptions AddSingleton<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory)
            where TService : class
            where TImplementation : class, TService;
        IMyShareOptions AddSingleton<TService>(Func<IServiceProvider, TService> implementationFactory) where TService : class;
        IMyShareOptions AddSingleton<TService>() where TService : class;
        IMyShareOptions AddSingleton(Type serviceType);
        IMyShareOptions AddSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;
        IMyShareOptions AddSingleton(Type serviceType, Func<IServiceProvider, object> implementationFactory);
        IMyShareOptions AddSingleton(Type serviceType, Type implementationType);
        IMyShareOptions AddSingleton<TService>(TService implementationInstance) where TService : class;
        IMyShareOptions AddSingleton(Type serviceType, object implementationInstance);
        IMyShareOptions AddTransient<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory)
            where TService : class
            where TImplementation : class, TService;
        IMyShareOptions AddTransient<TService>(Func<IServiceProvider, TService> implementationFactory) where TService : class;
        IMyShareOptions AddTransient<TService>() where TService : class;
        IMyShareOptions AddTransient(Type serviceType);
        IMyShareOptions AddTransient<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;
        IMyShareOptions AddTransient(Type serviceType, Func<IServiceProvider, object> implementationFactory);
        IMyShareOptions AddTransient(Type serviceType, Type implementationType);
        #endregion
    }
}