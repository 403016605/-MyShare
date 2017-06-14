using System;

namespace MyShare.Kernel.Exceptions
{
    internal class NoHandlerRegisteredException : System.Exception
    {
        public NoHandlerRegisteredException(Type t)
            : base((string) $"δע������Ϊ[{t.FullName}]��Handler!")
        {
        }
    }
}