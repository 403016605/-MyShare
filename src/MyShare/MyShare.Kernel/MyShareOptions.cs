using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyShare.Kernel.Bus;
using MyShare.Kernel.Cache;
using MyShare.Kernel.Commands;
using MyShare.Kernel.Common;
using MyShare.Kernel.Data;
using MyShare.Kernel.Defaults.Bus;
using MyShare.Kernel.Defaults.Cache;
using MyShare.Kernel.Defaults.Common;
using MyShare.Kernel.Defaults.Domain;
using MyShare.Kernel.Defaults.Events;
using MyShare.Kernel.Domain;
using MyShare.Kernel.Events;

namespace MyShare.Kernel
{
    public sealed class MyShareOptions : IMyShareOptions
    {
        public MyShareConfig MyShareConfig { get; }

        public IServiceCollection ServicesCollection { get; }

        public Dictionary<string, Type> TypeDict { get; internal set; }

        public IServiceProvider ServiceProvider { get; }

        private readonly DbContextOptions _options;

        #region 单例模式

        public static MyShareOptions Instance(IServiceCollection services, IOptions<MyShareConfig> config, DbContextOptions contextOptions)
        {
            services.AddSingleton(contextOptions);
            return new MyShareOptions(services, config);
        }

        private MyShareOptions(IServiceCollection services, IOptions<MyShareConfig> config)
        {
            ServicesCollection = services;
            ServiceProvider = services.BuildServiceProvider();
            TypeDict = new Dictionary<string, Type>();
            MyShareConfig = config.Value;
            _options = ServiceProvider.GetService<DbContextOptions>();
        }

        #endregion

        public IMyShareOptions InitKernel()
        {
            TypeDict = FillDictionary(MyShareConfig.Assemblies);

            ServicesCollection.AddMemoryCache();

            //添加CQRS服务
            ServicesCollection.AddSingleton(new InProcessBus());
            ServicesCollection.AddSingleton<ICommandSender>(y => y.GetService<InProcessBus>());
            ServicesCollection.AddSingleton<IEventPublisher>(y => y.GetService<InProcessBus>());
            ServicesCollection.AddSingleton<IHandlerRegistrar>(y => y.GetService<InProcessBus>());
            ServicesCollection.AddSingleton<ISerializer, Serializer>();
            ServicesCollection.AddSingleton<DataContext>(new DataContext(_options, TypeDict.Values.ToList()));

            ServicesCollection.AddSingleton<IEventStore, SampleEventStore>();

            ServicesCollection.AddScoped<ISession, Session>();
            ServicesCollection.AddScoped<ICache, MemoryCache>();

            ServicesCollection.AddScoped<IRepository>(y => new CacheRepository(new Repository(y.GetService(typeof(IEventStore)) as IEventStore),
                y.GetService(typeof(IEventStore)) as IEventStore, y.GetService<ICache>()));

            RegHandlerType();

            return this;
        }

        #region private

        private static Dictionary<string, Type> FillDictionary(IEnumerable<string> assemblies)
        {
            var assemblyList = assemblies.Select(m => Assembly.Load(new AssemblyName(m))).ToList();

            return assemblyList.SelectMany(asm => asm.GetTypes()).ToDictionary(type => type.FullName);
        }

        private void RegHandlerType()
        {
            var registrar = this.ServicesCollection.BuildServiceProvider().GetService<IHandlerRegistrar>();

            var registerExecutorMethod = registrar
                .GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(mi => mi.Name == "RegisterHandler")
                .Where(mi => mi.IsGenericMethod)
                .Where(mi => mi.GetGenericArguments().Length == 1)
                .Single(mi => mi.GetParameters().Length == 1);

            var executorTypes = TypeDict.Values
                .Select(t => new
                {
                    Type = t,
                    Interfaces = t.GetInterfaces()
                        .Where(i => i.GetTypeInfo().IsGenericType && (i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)
                                                                      || i.GetGenericTypeDefinition() ==
                                                                      typeof(IEventHandler<>)))
                })
                .Where(e => e.Interfaces != null && e.Interfaces.Any());

            foreach (var executorType in executorTypes)
            {
                this.ServicesCollection.AddTransient(executorType.Type);

                foreach (var @interface in executorType.Interfaces)
                {
                    var commandType = @interface.GetGenericArguments()[0];
                    var del = new Func<dynamic, Task>(x =>
                    {
                        dynamic handler = this.ServicesCollection.BuildServiceProvider().GetService(executorType.Type);
                        return handler.Handle(x);
                    });

                    registerExecutorMethod.MakeGenericMethod(commandType).Invoke(registrar, new object[] { del });
                }
            }
        }

        #endregion
    }
}