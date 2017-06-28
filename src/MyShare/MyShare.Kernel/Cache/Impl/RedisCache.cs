using System;
using Microsoft.Extensions.Options;
using MyShare.Kernel.Common;
using MyShare.Kernel.Configs;
using MyShare.Kernel.Domain;
using StackExchange.Redis;

namespace MyShare.Kernel.Cache.Impl
{
    internal class RedisCache : ICache
    {
        private readonly ISerializer _serializer;

        private readonly MyShareConfig _myShareConfig;

        private IDatabase Get_database()
        {
            return ConnectionMultiplexer.Connect(_myShareConfig.RedisConnStr).GetDatabase();
        }

        public RedisCache(IOptions<MyShareConfig> options, ISerializer serializer)
        {
            _serializer = serializer;
            _myShareConfig = options.Value;
        }

        public AggregateRoot Get(Guid id)
        {
            if (Get_database().KeyExists(id.ToString()))
            {
                byte[] val= Get_database().StringGet(id.ToString());
                var obj = _serializer.Deserialize(typeof(AggregateRoot), val);
                return obj as AggregateRoot;
            }
            return null;
        }

        public bool IsTracked(Guid id)
        {
            return Get_database().KeyExists(id.ToString());
        }

        public void RegisterEvictionCallback(Action<Guid> action)
        {
            
        }

        public void Remove(Guid id)
        {
            Get_database().KeyDelete(id.ToString());
        }

        public void Set(Guid id, AggregateRoot aggregate)
        {
            byte[] val = _serializer.Serialize(aggregate);
            Get_database().StringSet(id.ToString(), val);
        }
    }
}