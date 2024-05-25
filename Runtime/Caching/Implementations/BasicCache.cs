using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using UniSharp.Tools.Caching.Factory;

namespace UniSharp.Tools.Caching.Implementations
{
    internal class BasicCache : ICache
    {
        protected class CacheItem
        {
            public string Key { get; set; }
            public object Value { get; set; }
            public DateTime ExpiryDate { get; set; }
        }

        protected ConcurrentDictionary<string, CacheItem> _cache = new();
        protected CacheOptions _options;

        public BasicCache(CacheOptions options)
        {
            _options = options;

            _cache = new(Environment.ProcessorCount * 2, _options.Capacity);
        }

        public virtual object Get(string key)
        {
            return Get<object>(key);
        }

        public virtual T Get<T>(string key)
        {
            if (_cache.TryGetValue(key, out CacheItem item) && DateTime.Now < item.ExpiryDate)
            {
                return (T)item.Value;
            }
            return default;
        }

        public virtual Task<object> GetAsync(string key)
        {
            return GetAsync<object>(key);
        }

        public virtual Task<T> GetAsync<T>(string key)
        {
            return Task.FromResult(Get<T>(key));
        }

        public virtual void Set(string key, object value, TimeSpan? duration = null)
        {
            Set<object>(key, value, duration);
        }

        public virtual void Set<T>(string key, T value, TimeSpan? duration = null)
        {
            duration ??= _options.Expiry;
            var newItem = new CacheItem { Key = key, Value = value, ExpiryDate = DateTime.Now.Add(duration.Value) };
            _cache.AddOrUpdate(key, newItem, (existingKey, existingValue) => newItem);
        }

        public virtual Task SetAsync(string key, object value, TimeSpan? duration = null)
        {
            return SetAsync<object>(key, value, duration);
        }

        public virtual Task SetAsync<T>(string key, T value, TimeSpan? duration = null)
        {
            Set(key, value, duration);
            return Task.CompletedTask;
        }

        public virtual bool Exists(string key)
        {
            return _cache.TryGetValue(key, out CacheItem item) && DateTime.Now < item.ExpiryDate;
        }

        public virtual void Remove(string key)
        {
            _cache.TryRemove(key, out _);
        }

        public virtual Task<bool> ExistsAsync(string key)
        {
            return Task.FromResult(Exists(key));
        }

        public virtual Task RemoveAsync(string key)
        {
            Remove(key);
            return Task.CompletedTask;
        }
    }
}
