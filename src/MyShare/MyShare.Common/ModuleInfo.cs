using System;

namespace MyShare.Common
{
    public sealed class ModuleInfo : Kernel.ModuleInfo
    {
        public ModuleInfo()
        {
            Description = "封装常用工具";
            Name = this.GetType().Namespace;
        }
    }
}
