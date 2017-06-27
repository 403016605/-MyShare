using System;

namespace MyShare.Kernel.Common
{
    public interface ISerializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">T类型</typeparam>
        /// <param name="byteArray">二进制</param>
        /// <returns>T类型实例</returns>
        T Deserialize<T>(byte[] byteArray);

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="byteArray">二进制</param>
        /// <param name="toType">类型</param>
        /// <returns></returns>
        object Deserialize(Type toType, byte[] byteArray);

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T">T类型</typeparam>
        /// <param name="obj">T类型实例</param>
        /// <returns>二进制</returns>
        byte[] Serialize<T>(T obj);
    }
}