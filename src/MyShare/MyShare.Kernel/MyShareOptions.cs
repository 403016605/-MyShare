using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyShare.Kernel.Bus;
using MyShare.Kernel.Bus.Impl;
using MyShare.Kernel.Cache;
using MyShare.Kernel.Cache.Impl;
using MyShare.Kernel.Commands;
using MyShare.Kernel.Common;
using MyShare.Kernel.Common.Impl;
using MyShare.Kernel.Data;
using MyShare.Kernel.Domain;
using MyShare.Kernel.Domain.Impl;
using MyShare.Kernel.Events;
using MyShare.Kernel.Events.Impl;

namespace MyShare.Kernel
{
    /// <summary>
    /// MyShare启动选项
    /// </summary>
    public sealed class MyShareOptions : IMyShareOptions
    {
        /// <summary>
        /// 配置类参数
        /// </summary>
        public MyShareConfig MyShareConfig { get; }

        /// <summary>
        /// 服务注册
        /// </summary>
        public IServiceCollection ServicesCollection { get; }

        /// <summary>
        /// 类型字典
        /// </summary>
        public Dictionary<string, Type> TypeDict { get; internal set; }

        /// <summary>
        /// 服务获取
        /// </summary>
        public IServiceProvider ServiceProvider { get; }

        private readonly DbContextOptions _options;

        #region 单例模式

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>服务注册</param>
        /// <param name="config"><see cref="IOptions&lt;MyShareConfig&gt;"/>配置信息</param>
        /// <param name="contextOptions"><see cref="DbContextOptions"/>数据库配置信息</param>
        /// <returns></returns>
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
            TypeDict = TypeMap(MyShareConfig.Assemblies);

            ServicesCollection.AddMemoryCache();
            //添加CQRS服务
            ServicesCollection.AddSingleton(new InProcessBus());
            ServicesCollection.AddSingleton<ICommandSender>(y => y.GetService<InProcessBus>());
            ServicesCollection.AddSingleton<IEventPublisher>(y => y.GetService<InProcessBus>());
            ServicesCollection.AddSingleton<IHandlerRegistrar>(y => y.GetService<InProcessBus>());
            ServicesCollection.AddSingleton<ISerializer, Serializer>();
            ServicesCollection.AddSingleton(new DataContext(_options, TypeDict.Values.ToList()));

            ServicesCollection.AddScoped<ICache, MemoryCache>();
            ServicesCollection.AddSingleton<IEventStore, SampleEventStore>();
            ServicesCollection.AddScoped<IRepository, Repository>();
            ServicesCollection.AddScoped<ICacheRepository, CacheRepository>();
            ServicesCollection.AddScoped<ISession, Session>();
            

            //ServicesCollection.AddScoped<IRepository>(y => new CacheRepository(new Repository(y.GetService(typeof(IEventStore)) as IEventStore),
            //    y.GetService(typeof(IEventStore)) as IEventStore, y.GetService<ICache>()));

            RegHandlerType();

            return this;
        }

        #region private

        private static Dictionary<string, Type> TypeMap(IEnumerable<string> assemblies)
        {
            var assemblyList = assemblies.Select(m => Assembly.Load(new AssemblyName(m))).ToList();

            return assemblyList.SelectMany(asm => asm.GetTypes()).ToDictionary(type => type.FullName);
        }

        private void RegHandlerType()
        {
            var registrar = ServicesCollection.BuildServiceProvider().GetService<IHandlerRegistrar>();

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
                ServicesCollection.AddTransient(executorType.Type);

                foreach (var @interface in executorType.Interfaces)
                {
                    var commandType = @interface.GetGenericArguments()[0];
                    registerExecutorMethod.MakeGenericMethod(commandType)
                        .Invoke(registrar,
                                new object[] { new Func<dynamic, Task>(x =>
                                                    {
                                                        dynamic handler = ServicesCollection.BuildServiceProvider().GetService(executorType.Type);
                                                        return handler.Handle(x);
                                                    })
                                }
                        );
                }
            }
        }

        #endregion
    }
}