#region using

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyShare.Kernel.Cache;
using MyShare.Kernel.Domain;
using MyShare.Kernel.Events;

#endregion

namespace MyShare.Kernel.Defaults.Domain
{
    internal class CacheRepository : IRepository
    {
        private static readonly ConcurrentDictionary<Guid, SemaphoreSlim> Locks =
            new ConcurrentDictionary<Guid, SemaphoreSlim>();

        private readonly ICache _cache;
        private readonly IEventStore _eventStore;
        private readonly IRepository _repository;

        public CacheRepository(IRepository repository, IEventStore eventStore, ICache cache)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));

            _cache.RegisterEvictionCallback(key => Locks.TryRemove(key, out var o));
        }

        public async Task Save<T>(T aggregate, int? expectedVersion = null) where T : AggregateRoot
        {
            var @lock = Locks.GetOrAdd(aggregate.Id, CreateLock);
            await @lock.WaitAsync();
            try
            {
                if (aggregate.Id != Guid.Empty && !_cache.IsTracked(aggregate.Id))
                {
                    _cache.Set(aggregate.Id, aggregate);
                }

                await _repository.Save(aggregate, expectedVersion);
            }
            catch (Exception)
            {
                _cache.Remove(aggregate.Id);
            }
            finally
            {
                @lock.Release();
            }
        }

        public async Task<T> Get<T>(Guid aggregateId) where T : AggregateRoot
        {
            var @lock = Locks.GetOrAdd(aggregateId, CreateLock);
            await @lock.WaitAsync();
            try
            {
                T aggregate;
                if (_cache.IsTracked(aggregateId))
                {
                    aggregate = (T) _cache.Get(aggregateId);
                    var events = await _eventStore.Get(aggregateId, aggregate.Version);
                    if (events.Any() && events.First().Version != aggregate.Version + 1)
                    {
                        _cache.Remove(aggregateId);
                    }
                    else
                    {
                        aggregate.LoadFromHistory(events);
                        return aggregate;
                    }
                }

                aggregate = await _repository.Get<T>(aggregateId);
                _cache.Set(aggregateId, aggregate);
                return aggregate;
            }
            catch (Exception ex)
            {
                _cache.Remove(aggregateId);
                throw;
            }
            finally
            {
                @lock.Release();
            }
        }

        private static SemaphoreSlim CreateLock(Guid _)
        {
            return new SemaphoreSlim(1, 1);
        }
    }
}