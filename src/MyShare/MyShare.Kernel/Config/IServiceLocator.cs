using System;

namespace MyShare.Kernel.Config
{
    public interface IServiceLocator
    {
        T GetService<T>();
        object GetService(Type type);
    }
}
