using System;

namespace MyShare.Kernel.Exceptions
{
    internal class NoHandlerRegisteredException : Exception
    {
        public NoHandlerRegisteredException(Type t)
            : base($"δע������Ϊ[{t.FullName}]��Handler!")
        {
        }
    }
}