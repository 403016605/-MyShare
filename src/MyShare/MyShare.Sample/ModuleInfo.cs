namespace MyShare.Sample
{
    public sealed class ModuleInfo: Kernel.ModuleInfo
    {
        public ModuleInfo()
        {
            Description = "模块描述";
            Name = this.GetType().Namespace;
        }
    }
}
