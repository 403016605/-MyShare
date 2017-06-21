using System;
using System.Collections.Generic;
using System.Text;

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
