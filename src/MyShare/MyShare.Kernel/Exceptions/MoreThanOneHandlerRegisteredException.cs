using System;

namespace MyShare.Kernel.Exceptions
{
    internal class MoreThanOneHandlerRegisteredException : System.Exception
    {
        public MoreThanOneHandlerRegisteredException(Type t)
            : base($"类型为[{t.FullName}]的命令只能有一个Handler!")
        {
        }
    }
}
