using System;
using MyShare.Kernel.Infrastructure;

namespace MyShare.Sample.Infrastructure.Entities
{
    public class BookEntity : EntityBase
    {
        public string Name { get; protected set; }

        public BookEntity(Guid id, string name, int version):base(id)
        {
            Name = name;
            Version = version;
        }
    }
}
