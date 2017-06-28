using System;

namespace MyShare.Kernel.Infrastructure
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple = false,Inherited = false)]
    public class IgnoreAttribute:System.Attribute
    {
        public IgnoreAttribute()
        {
        }
    }
}
