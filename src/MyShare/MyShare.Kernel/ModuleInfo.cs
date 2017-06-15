namespace MyShare.Kernel
{
    public class ModuleInfo
    {
        public ModuleInfo()
        {
            Description = "模块描述";
            Name = this.GetType().Namespace;
        }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// 模块描述
        /// </summary>
        public string Description { get; protected set; }
    }
}
