namespace MyShare.Sample
{
    public sealed class ModuleInfo
    {
        public ModuleInfo()
        {
            Description = "模块描述";
            Name = "模块名称";
        }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 模块描述
        /// </summary>
        public string Description { get; }
    }
}
