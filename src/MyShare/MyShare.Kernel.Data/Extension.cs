using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MyShare.Kernel.Data
{
    public static class Extension
    {
        public static Bootstrap UseDataContext(this Bootstrap bootstrap, DbContextOptions options, List<Type> types)
        {
            bootstrap.ServicesCollection.AddSingleton(new DataContext(options, types));
            return bootstrap;
        }
    }
}
