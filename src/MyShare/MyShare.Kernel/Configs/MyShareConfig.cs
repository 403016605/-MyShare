namespace MyShare.Kernel.Configs
{
    public class MyShareConfig
    {
        /// <summary>
        /// 默认数据链接ConnectionStrings中的Key
        /// </summary>
        public string SqlServerConnStr { get; set; }

        /// <summary>
        /// Redis链接字符串
        /// </summary>
        public string RedisConnStr { get;set; }

        /// <summary>
        /// 自定义程序集名称
        /// </summary>
        public string[] Assemblies { get; set; }
    }
}
