#region using

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
using MyShare.Kernel.Data;
using MyShare.Kernel.Defaults.Bus;
using MyShare.Kernel.Defaults.Cache;
using MyShare.Kernel.Defaults.Domain;
using MyShare.Kernel.Defaults.Events;
using MyShare.Kernel.Domain;
using MyShare.Kernel.Events;
using Scrutor;
using Dapper;

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

        #region Dapper

        private static readonly Dictionary<Type, List<PropertyInfo>> TypeMapper = new Dictionary<Type, List<PropertyInfo>>();

        public static IDbConnection InitEntities(this IDbConnection conn, Assembly assembly)
        {
            var executorTypes = assembly
                    .GetTypes()
                    .Where(e => e.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IEntity)) );

            var flag = BindingFlags.Public | BindingFlags.Instance;

            foreach (var executorType in executorTypes)
            {
                TypeMapper.Add(executorType, executorType.GetProperties(flag).ToList());
            }

            return conn;
        }

        public static int Insert<T>(this IDbConnection conn, IEnumerable<T> entities)
            where T : IEntity
        {
            var type = typeof(T);

            var list = TypeMapper[type].Select(p => p.Name).ToArray();

            var sql = $"INSERT INTO {type.Name}({string.Join(",", list)}) VALUES(@{string.Join(",@", list)})";

            var result = 0;

            foreach (var entity in entities)
            {
                result += conn.Execute(sql, entity);
            }
            return result;
        }

        public static int Insert<T>(this IDbConnection conn, T entity)
            where T : IEntity
        {
            return conn.Insert<T>(new List<T> { entity });
        }

        public static IEnumerable<T> Query<T>(this IDbConnection conn, Guid Id)
            where T : IEntity
        {
            var type = typeof(T);

            var list = TypeMapper[type].Select(p => p.Name).ToArray();

            var sql = $"SELECT {string.Join(",", list)} FROM {type.Name} WHERE Id=@Id";

            return conn.Query<T>(sql, new { Id = Id }).AsList();
        }

        #endregion
    }
}