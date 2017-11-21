using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MyShare.Kernel.Base.Bus;
using MyShare.Kernel.Base.Cache;
using MyShare.Kernel.Base.Commands;
using MyShare.Kernel.Base.Events;
using MyShare.Kernel.Common;
using MyShare.Kernel.Domain;

namespace MyShare.Kernel
{
    public class MyShareOptions : IMyShareOptions
    {
        public MyShareOptions(IServiceCollection services)
        {
            ServicesCollection = services;

            ServicesCollection.AddSingleton<IMyShareOptions>(y => this);
            ServicesCollection.AddMemoryCache();
            //添加CQRS服务
            ServicesCollection.AddSingleton<IProcessBus, ProcessBus>();
            var bus = ServicesCollection.BuildServiceProvider().GetService<IProcessBus>();
            ServicesCollection.AddSingleton<ICommandSender>(bus);
            ServicesCollection.AddSingleton<IEventPublisher>(bus);
            ServicesCollection.AddSingleton<IHandlerRegistrar>(bus);
            ServicesCollection.AddScoped<ISession, Session>();
            ServicesCollection.AddSingleton<IEventStore, InMemoryEventStore>();
            ServicesCollection.AddScoped<ICache, MemoryCache>();
            ServicesCollection.AddScoped<IRepository>(y => new CacheRepository(
                new Repository(y.GetService<IEventStore>()),
                y.GetService<IEventStore>(), y.GetService<ICache>()));

            //注册工具
            ServicesCollection.AddSingleton<ISerializer, Serializer>();

            Register(GetType().Assembly);
        }

        public IServiceCollection ServicesCollection { get; }

        public IMyShareOptions AddBus(Assembly assembly)
        {
            Register( assembly);
            return this;
        }

        public IMyShareOptions AddHandlers(Assembly assembly)
        {
            ServicesCollection.Scan(scan => scan.FromAssemblies(assembly)
                .AddClasses(classes => classes.Where(x =>
                {
                    var allInterfaces = x.GetInterfaces();
                    return
                        allInterfaces.Any(y => y.GetTypeInfo().IsGenericType &&
                                               y.GetTypeInfo().GetGenericTypeDefinition() ==
                                               typeof(ICommandHandler<>)) ||
                        allInterfaces.Any(y => y.GetTypeInfo().IsGenericType &&
                                               y.GetTypeInfo().GetGenericTypeDefinition() == typeof(IEventHandler<>));
                }))
                .AsSelf()
                .WithTransientLifetime()
            );

            return this;
        }

        public static IMyShareOptions Instance(IServiceCollection services)
        {
            return new MyShareOptions(services);
        }

        #region private

        private void Register(Assembly assembly)
        {
            var serviceProvider = ServicesCollection.BuildServiceProvider();
            var registrar = serviceProvider.GetService<IHandlerRegistrar>();

            var executorTypes = assembly
                .GetTypes()
                .Select(t => new { Type = t, Interfaces = ResolveMessageHandlerInterface(t) })
                .Where(e => e.Interfaces != null && e.Interfaces.Any());

            foreach (var executorType in executorTypes)
                foreach (var @interface in executorType.Interfaces)
                    InvokeHandler(serviceProvider, @interface, registrar, executorType.Type);
        }

        private static void InvokeHandler(IServiceProvider serviceProvider, Type @interface,
            IHandlerRegistrar registrar, Type executorType)
        {
            var commandType = @interface.GetGenericArguments()[0];

            var registerExecutorMethod = registrar
                .GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(mi => mi.Name == "On")
                .Where(mi => mi.IsGenericMethod)
                .Where(mi => mi.GetGenericArguments().Length == 1)
                .Single(mi => mi.GetParameters().Length == 1)
                .MakeGenericMethod(commandType);

            var del = new Func<dynamic, Task>(x =>
            {
                dynamic handler = serviceProvider.GetService(executorType);
                return handler.Handle(x);
            });

            registerExecutorMethod.Invoke(registrar, new object[] { del });
        }

        private static IEnumerable<Type> ResolveMessageHandlerInterface(Type type)
        {
            return type
                .GetInterfaces()
                .Where(i => i.GetTypeInfo().IsGenericType && (i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)
                                                              || i.GetGenericTypeDefinition() ==
                                                              typeof(IEventHandler<>)));
        }

        IMyShareOptions IMyShareOptions.AddSingleton<TService, TImplementation>()
        {
            ServicesCollection.AddSingleton<TService, TImplementation>();
            return this;
        }

        public IMyShareOptions AddScoped<TService>() where TService : class
        {
            ServicesCollection.AddScoped<TService>();
            return this;
        }

        public IMyShareOptions AddScoped(Type serviceType, Type implementationType)
        {
            ServicesCollection.AddScoped(serviceType,  implementationType);
            return this;
        }

        public IMyShareOptions AddScoped(Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            ServicesCollection.AddScoped(serviceType, implementationFactory);
            return this;
        }

        IMyShareOptions IMyShareOptions.AddScoped<TService, TImplementation>()
        {
            ServicesCollection.AddScoped<TService, TImplementation>();
            return this;
        }

        public IMyShareOptions AddScoped(Type serviceType)
        {
            ServicesCollection.AddScoped(serviceType);
            return this;
        }

        public IMyShareOptions AddScoped<TService>(Func<IServiceProvider, TService> implementationFactory) where TService : class
        {
            ServicesCollection.AddScoped(implementationFactory);
            return this;
        }

        IMyShareOptions IMyShareOptions.AddScoped<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory)
        {
            ServicesCollection.AddScoped<TService, TImplementation>(implementationFactory);
            return this;
        }

        IMyShareOptions IMyShareOptions.AddSingleton<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory)
        {
            ServicesCollection.AddSingleton<TService, TImplementation>(implementationFactory);
            return this;
        }

        public IMyShareOptions AddSingleton<TService>(Func<IServiceProvider, TService> implementationFactory) where TService : class
        {
            ServicesCollection.AddSingleton(implementationFactory);
            return this;
        }

        public IMyShareOptions AddSingleton<TService>() where TService : class
        {
            ServicesCollection.AddSingleton<TService>();
            return this;
        }

        public IMyShareOptions AddSingleton(Type serviceType)
        {
            ServicesCollection.AddSingleton(serviceType);
            return this;
        }

        public IMyShareOptions AddSingleton(Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            ServicesCollection.AddSingleton(serviceType, implementationFactory);
            return this;
        }

        public IMyShareOptions AddSingleton(Type serviceType, Type implementationType)
        {
            ServicesCollection.AddSingleton(serviceType, implementationType);
            return this;
        }

        public IMyShareOptions AddSingleton<TService>(TService implementationInstance) where TService : class
        {
            ServicesCollection.AddSingleton(implementationInstance);
            return this;
        }

        public IMyShareOptions AddSingleton(Type serviceType, object implementationInstance)
        {
            ServicesCollection.AddSingleton(serviceType,  implementationInstance);
            return this;
        }

        IMyShareOptions IMyShareOptions.AddTransient<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory)
        {
            ServicesCollection.AddTransient<TService, TImplementation>(implementationFactory);
            return this;
        }

        public IMyShareOptions AddTransient<TService>(Func<IServiceProvider, TService> implementationFactory) where TService : class
        {
            ServicesCollection.AddTransient(implementationFactory);
            return this;
        }

        public IMyShareOptions AddTransient<TService>() where TService : class
        {
            ServicesCollection.AddTransient<TService>();
            return this;
        }

        public IMyShareOptions AddTransient(Type serviceType)
        {
            ServicesCollection.AddTransient(serviceType);
            return this;
        }

        IMyShareOptions IMyShareOptions.AddTransient<TService, TImplementation>()
        {
            ServicesCollection.AddTransient<TService, TImplementation>();
            return this;
        }

        public IMyShareOptions AddTransient(Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            ServicesCollection.AddTransient(serviceType, implementationFactory);
            return this;
        }

        public IMyShareOptions AddTransient(Type serviceType, Type implementationType)
        {
            ServicesCollection.AddTransient(serviceType, implementationType);
            return this;
        }

        #endregion
    }
}