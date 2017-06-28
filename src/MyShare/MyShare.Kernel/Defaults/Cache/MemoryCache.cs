#region using

using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MyShare.Kernel.Cache;
using MyShare.Kernel.Common;
using MyShare.Kernel.Domain;
using StackExchange.Redis;

#endregion

namespace MyShare.Kernel.Defaults.Cache
{
    internal class MemoryCache : ICache
    {
        private readonly IMemoryCache _cache;

        private readonly MemoryCacheEntryOptions _cacheOptions;

        public MemoryCache()
        {
            _cacheOptions = new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(15)
            };
            _cache = new Microsoft.Extensions.Caching.Memory.MemoryCache(new MemoryCacheOptions());
        }

        public bool IsTracked(Guid id)
        {
            return _cache.TryGetValue(id, out var o) && o != null;
        }

        public void Set(Guid id, AggregateRoot aggregate)
        {
            _cache.Set(id, aggregate, _cacheOptions);
        }

        public AggregateRoot Get(Guid id)
        {
            return (AggregateRoot) _cache.Get(id);
        }

        public void Remove(Guid id)
        {
            _cache.Remove(id);
        }

        public void RegisterEvictionCallback(Action<Guid> action)
        {
            _cacheOptions.RegisterPostEvictionCallback((key, value, reason, state) => { action.Invoke((Guid) key); });
        }
    }

    internal class RedisCache : ICache
    {
        private readonly ISerializer _serializer;

        private readonly MyShareConfig _myShareConfig;

        private IDatabase Get_database()
        {
            return ConnectionMultiplexer.Connect(_myShareConfig.RedisConn).GetDatabase();
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