using System;

namespace MyShare.Sample.Infrastructure.Entities
{
    public abstract class Entity
    {
        protected Entity(Guid id, int version)
        {
            Id = id;
            Version = version;
        }

        public Guid Id { get;  }
        public int Version { get;  }
    }
}