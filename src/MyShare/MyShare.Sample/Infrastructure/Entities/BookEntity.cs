using System;

namespace MyShare.Sample.Infrastructure.Entities
{
    public class BookEntity : Entity
    {
        public string Name;
        public BookEntity(Guid id, string name, int version):base(id,version)
        {
            Name = name;
        }
    }
}
