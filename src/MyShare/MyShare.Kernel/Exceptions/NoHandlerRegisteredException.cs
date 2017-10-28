using System;

namespace MyShare.Kernel.Exceptions
{
    internal class NoHandlerRegisteredException : Exception
    {
        public NoHandlerRegisteredException(Type t)
            : base($"未注册类型为[{t.FullName}]的Handler!")
        {
        }
    }
}