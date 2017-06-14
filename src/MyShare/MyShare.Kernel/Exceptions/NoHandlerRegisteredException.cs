using System;

namespace MyShare.Kernel.Exceptions
{
    internal class NoHandlerRegisteredException : System.Exception
    {
        public NoHandlerRegisteredException(Type t)
            : base((string) $"未注册类型为[{t.FullName}]的Handler!")
        {
        }
    }
}