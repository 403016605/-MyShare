using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MyShare.Kernel.Infrastructure;

namespace MyShare.Kernel.Data
{
    public class DataContext : DbContext
    {
        private readonly List<Type> _types;

        public DataContext(DbContextOptions options, List<Type> types) : base(options)
        {
            _types = types;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityTypes = _types
                .Where(type => !string.IsNullOrEmpty(type.Namespace))
                .Where(type => type.GetTypeInfo().BaseType != null && type.GetTypeInfo().BaseType == typeof(EntityBase));

            foreach (var type in entityTypes)
            {
                modelBuilder.Model.GetOrAddEntityType(type);
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
