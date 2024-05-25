using UniSharp.Tools.Caching.Factory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniSharp.Tools.Caching.Implementations
{
    /// <summary>
    /// Least Recently Used (LRU)
    /// The LRU policy evicts the least recently accessed items first.
    /// It's based on the idea that items accessed recently are more likely to be accessed again in the near future.
    /// </summary>
    internal class LRUCache : BasicCache
    {
        private LinkedList<string> _lruList = new LinkedList<string>();
        private readonly object _lock = new();

        public LRUCache(CacheOptions options) : base(options)
        {
        }

        public override object Get(string key)
        {
            return Get<object>(key);
        }

        public override T Get<T>(string key)
        {
            lock (_lock)
            {
                if (_cache.TryGetValue(key, out CacheItem item) && DateTime.Now < item.ExpiryDate)
                {
                    _lruList.Remove(key);
                    _lruList.AddLast(key);
                    return (T)item.Value;
                }
            }
            return default;
        }

        public override Task<object> GetAsync(string key)
        {
            return Task.FromResult(Get<object>(key));
        }

        public override Task<T> GetAsync<T>(string key)
        {
            return Task.FromResult(Get<T>(key));
        }

        public override void Set(string key, object value, TimeSpan? duration = null)
        {
            Set<object>(key, value, duration);
        }

        public override void Set<T>(string key, T value, TimeSpan? duration = null)
        {
            lock (_lock)
            {
                if (_cache.Count >= _options.MaxCapacity && !_cache.ContainsKey(key))
                {
                    var oldestKey = _lruList.First.Value;
                    _cache.TryRemove(oldestKey, out _);
                    _lruList.RemoveFirst();
                }

                duration ??= _options.Expiry;
                var newItem = new CacheItem { Key = key, Value = value, ExpiryDate = DateTime.Now.Add(duration.Value) };
                _cache.AddOrUpdate(key, newItem, (existingKey, existingValue) => newItem);

                _lruList.Remove(key); // Safe operation even if key does not exist
                _lruList.AddLast(key);
            }
        }

        public override Task SetAsync(string key, object value, TimeSpan? duration = null)
        {
            Set(key, value, duration);
            return Task.CompletedTask;
        }

        public override Task SetAsync<T>(string key, T value, TimeSpan? duration = null)
        {
            Set(key, value, duration);
            return Task.CompletedTask;
        }

        public override void Remove(string key)
        {
            lock (_lock)
            {
                if (_cache.TryRemove(key, out _))
                {
                    _lruList.Remove(key);
                }
            }
        }

        public override Task RemoveAsync(string key)
        {
            Remove(key);
            return Task.CompletedTask;
        }
    }
}
