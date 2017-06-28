namespace MyShare.Kernel
{
    public class MyShareConfig
    {
        /// <summary>
        /// 默认数据链接ConnectionStrings中的Key
        /// </summary>
        public string ConnectionKey { get; set; }

        public string RedisConn { get;set; }

        /// <summary>
        /// 自定义程序集名称
        /// </summary>
        public string[] Assemblies { get; set; }
    }
}
