using System;

namespace MyShare.Kernel.Infrastructure
{
    /// <summary>
    /// 实体抽象类
    /// </summary>
    public abstract class EntityBase
    {
        protected EntityBase(Guid id)
        {
            Id= id;
            CreateTime=DateTime.Now;
            ModifyTime = CreateTime;
        }

        protected EntityBase()
        {
            Id = Guid.NewGuid();
            CreateTime = DateTime.Now;
            ModifyTime = CreateTime;
        }

        /// <summary>
        /// 实体主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 实体版本
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public long TimeStamp { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; protected set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime { get; protected set; }

        public override bool Equals(object obj)
        {
            var compareTo = obj as EntityBase;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(EntityBase a, EntityBase b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(EntityBase a, EntityBase b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + " [Id=" + Id + "]";
        }
    }

    public abstract class ValueObject<T> where T : ValueObject<T>
    {
        public override bool Equals(object obj)
        {
            var valueObject = obj as T;
            return !ReferenceEquals(valueObject, null) && EqualsCore(valueObject);
        }

        protected abstract bool EqualsCore(T other);

        public override int GetHashCode()
        {
            return GetHashCodeCore();
        }

        protected abstract int GetHashCodeCore();

        public static bool operator ==(ValueObject<T> a, ValueObject<T> b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(ValueObject<T> a, ValueObject<T> b)
        {
            return !(a == b);
        }
    }
}