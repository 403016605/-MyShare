using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MyShare.Kernel.Bus;
using MyShare.Kernel.Cache;
using MyShare.Kernel.Commands;
using MyShare.Kernel.Common;
using MyShare.Kernel.Defaults.Bus;
using MyShare.Kernel.Defaults.Cache;
using MyShare.Kernel.Defaults.Common;
using MyShare.Kernel.Defaults.Domain;
using MyShare.Kernel.Defaults.Events;
using MyShare.Kernel.Domain;
using MyShare.Kernel.Events;
using Scrutor;

namespace MyShare.Kernel
{
    public sealed class MyShareOptions : IMyShareOptions
    {
        public IServiceCollection ServicesCollection { get; }

        public IServiceProvider ServiceProvider { get; }

        #region 单例模式

        public static MyShareOptions Instance(IServiceCollection services)
        {
            return new MyShareOptions(services);
        }

        private MyShareOptions(IServiceCollection services)
        {
            ServicesCollection = services;
            ServiceProvider = services.BuildServiceProvider();
        }

        #endregion

        public IMyShareOptions InitKernel(IDbConnection conn)
        {
            ServicesCollection.AddMemoryCache();

            //添加CQRS服务
            ServicesCollection.AddSingleton(new InProcessBus());
            ServicesCollection.AddSingleton<ICommandSender>(y => y.GetService<InProcessBus>());
            ServicesCollection.AddSingleton<IEventPublisher>(y => y.GetService<InProcessBus>());
            ServicesCollection.AddSingleton<IHandlerRegistrar>(y => y.GetService<InProcessBus>());
            ServicesCollection.AddScoped<ISession, Session>();
            ServicesCollection.AddSingleton<IDbConnection>(conn);
            ServicesCollection.AddSingleton<IEventStore, InMemoryEventStore>();
            ServicesCollection.AddScoped<ICache, MemoryCache>();
            ServicesCollection.AddScoped<IRepository>(y => new CacheRepository(new Repository(y.GetService<IEventStore>()),
                y.GetService<IEventStore>(), y.GetService<ICache>()));

            //注册工具
            ServicesCollection.AddSingleton<ISerializer, Serializer>();
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

        public IMyShareOptions AddBus(Assembly assembly)
        {
            var serviceProvider = ServicesCollection.BuildServiceProvider();
            Register(serviceProvider, assembly);
            return this;
        }

        #region private

        private static void Register(IServiceProvider serviceProvider, Assembly assembly)
        {
            var registrar = serviceProvider.GetService<IHandlerRegistrar>();

            var executorTypes = assembly
                .GetTypes()
                .Select(t => new { Type = t, Interfaces = ResolveMessageHandlerInterface(t) })
                .Where(e => e.Interfaces != null && e.Interfaces.Any());

            foreach (var executorType in executorTypes)
            {
                foreach (var @interface in executorType.Interfaces)
                {
                    InvokeHandler(serviceProvider, @interface, registrar, executorType.Type);
                }
            }
        }

        private static void InvokeHandler(IServiceProvider serviceProvider, Type @interface,
            IHandlerRegistrar registrar, Type executorType)
        {
            var commandType = @interface.GetGenericArguments()[0];

            var registerExecutorMethod = registrar
                .GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(mi => mi.Name == "RegisterHandler")
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

        #endregion
    }
}