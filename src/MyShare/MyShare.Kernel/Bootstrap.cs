using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MyShare.Kernel.Bus;
using MyShare.Kernel.Cache;
using MyShare.Kernel.Commands;
using MyShare.Kernel.Defaults.Bus;
using MyShare.Kernel.Defaults.Cache;
using MyShare.Kernel.Defaults.Domain;
using MyShare.Kernel.Defaults.Events;
using MyShare.Kernel.Domain;
using MyShare.Kernel.Events;
using Scrutor;

namespace MyShare.Kernel
{
    public sealed class Bootstrap
    {
        public IServiceCollection ServicesCollection { get; }

        public static Bootstrap Instance(IServiceCollection services)
        {
            return new Bootstrap(services);
        }

        private Bootstrap(IServiceCollection services)
        {
            ServicesCollection = services;
        }

        /// <summary>
        ///     初始化
        /// </summary>
        /// <returns></returns>
        public Bootstrap Init()
        {
            ServicesCollection.AddMemoryCache();

            //添加CQRS服务
            ServicesCollection.AddSingleton(new InProcessBus());
            ServicesCollection.AddSingleton<ICommandSender>(y => y.GetService<InProcessBus>());
            ServicesCollection.AddSingleton<IEventPublisher>(y => y.GetService<InProcessBus>());
            ServicesCollection.AddSingleton<IHandlerRegistrar>(y => y.GetService<InProcessBus>());
            ServicesCollection.AddScoped<ISession, Session>();
            ServicesCollection.AddSingleton<IEventStore, InMemoryEventStore>();
            ServicesCollection.AddScoped<ICache, MemoryCache>();
            ServicesCollection.AddScoped<IRepository>(y => new CacheRepository(new Repository(y.GetService<IEventStore>()),
                y.GetService<IEventStore>(), y.GetService<ICache>()));

            return this;
        }

        /// <summary>
        ///     扫描指定程序集中所有的commandhandlers和eventhandlers
        /// </summary>
        /// <param name="assembly">指定程序集</param>
        /// <returns></returns>
        public Bootstrap AddHandlers(Assembly assembly)
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

        /// <summary>
        ///     添加总线
        /// </summary>
        /// <param name="typesFromAssemblyContainingMessages"></param>
        /// <returns></returns>
        public Bootstrap AddBus(Assembly assembly)
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