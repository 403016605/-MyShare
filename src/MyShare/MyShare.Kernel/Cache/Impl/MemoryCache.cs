#region using

using System;
using Microsoft.Extensions.Caching.Memory;
using MyShare.Kernel.Domain;

#endregion

namespace MyShare.Kernel.Cache.Impl
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
}