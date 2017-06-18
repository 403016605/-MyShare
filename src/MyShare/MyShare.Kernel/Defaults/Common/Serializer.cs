using System.IO;
using MyShare.Kernel.Common;

namespace MyShare.Kernel.Defaults.Common
{
    /// <summary>
    /// 序列化的基于Protobuf的默认实现
    /// </summary>
    internal class Serializer : ISerializer
    {

        public T Deserialize<T>(byte[] byteArray)
        {
            using (var memoryStream = new MemoryStream(byteArray))
            {
                var obj = ProtoBuf.Serializer.Deserialize<T>(memoryStream);
                return obj;
            }
        }

        public byte[] Serialize<T>(T obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(memoryStream, obj);
                return memoryStream.ToArray();
            }
        }
    }
}