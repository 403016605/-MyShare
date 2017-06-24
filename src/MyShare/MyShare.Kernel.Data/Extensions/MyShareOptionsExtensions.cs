using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyShare.Kernel.Infrastructure;

namespace MyShare.Kernel.Data.Extensions
{
    public static class MyShareOptionsExtensions
    {
        public static IMyShareOptions UseDataContext(this IMyShareOptions myShareOptions, string sqlConn)
        {
            var assembly=Assembly.GetEntryAssembly();

            List<Type> types = assembly.GetTypes().Where(t=>t.GetTypeInfo().BaseType==typeof(EntityBase)).ToList();

            myShareOptions.ServicesCollection.AddSingleton(new DataContext(new DbContextOptionsBuilder().UseSqlServer(sqlConn).Options, types));
            return myShareOptions;
        }
    }
}
