using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MyShare.Kernel.Data.Extensions
{
    public static class MyShareOptionsExtension
    {
        public static IMyShareOptions UseDataContext(this IMyShareOptions options, DbContextOptions dboptions, List<Type> types)
        {
            options.ServicesCollection.AddSingleton(new DataContext(dboptions, types));
            return options;
        }
    }
}
