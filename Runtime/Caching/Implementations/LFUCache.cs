using UniSharp.Tools.Caching.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniSharp.Tools.Caching.Implementations
{
    /// <summary>
    /// Least Frequently Used (LFU)
    /// The LFU policy evicts items that are used less frequently.
    /// Unlike LRU, it favors items that have been accessed multiple times, even if they were not accessed recently.
    /// </summary>
    internal class LFUCache : BasicCache
    {
        private Dictionary<string, int> _accessFrequencies = new Dictionary<string, int>();
        private LinkedList<string> _lruList = new LinkedList<string>(); // To handle ties in frequency
        private readonly object _lock = new();

        public LFUCache(CacheOptions options) : base(options)
        {
            _options = options;

            _cache = new(Environment.ProcessorCount * 2, _options.Capacity);
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
                    IncreaseAccessFrequency(key);
                    return (T)item.Value;
                }
            }
            return default;
        }

        public override void Set(string key, object value, TimeSpan? duration = null)
        {
            Set(key, value, duration);
        }

        public override void Set<T>(string key, T value, TimeSpan? duration = null)
        {
            lock (_lock)
            {
                if (!_cache.ContainsKey(key) && _cache.Count >= _options.MaxCapacity)
                {
                    RemoveLeastFrequentlyUsedItem();
                }

                duration ??= _options.Expiry;
                var newItem = new CacheItem { Key = key, Value = value, ExpiryDate = DateTime.Now.Add(duration.Value) };
                _cache.AddOrUpdate(key, newItem, (existingKey, existingValue) => newItem);
                IncreaseAccessFrequency(key);
            }
        }

        public override void Remove(string key)
        {
            lock (_lock)
            {
                if (_cache.TryRemove(key, out _))
                {
                    _accessFrequencies.Remove(key);
                    _lruList.Remove(key);
                }
            }
        }

        public override Task<object> GetAsync(string key)
        {
            return Task.FromResult(Get<object>(key));
        }

        public override Task<T> GetAsync<T>(string key)
        {
            return Task.FromResult(Get<T>(key));
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

        public override Task RemoveAsync(string key)
        {
            Remove(key);
            return Task.CompletedTask;
        }

        private void IncreaseAccessFrequency(string key)
        {
            if (_accessFrequencies.ContainsKey(key))
            {
                _accessFrequencies[key]++;
            }
            else
            {
                _accessFrequencies[key] = 1;
            }

            _lruList.Remove(key); // Safe operation even if key does not exist
            _lruList.AddLast(key);
        }


        private void RemoveLeastFrequentlyUsedItem()
        {
            var leastFrequentlyUsed = _accessFrequencies.OrderBy(kv => kv.Value).ThenBy(kv => _lruList.Find(kv.Key)).First().Key;

            _cache.TryRemove(leastFrequentlyUsed, out _);
            _accessFrequencies.Remove(leastFrequentlyUsed);
            _lruList.Remove(leastFrequentlyUsed);
        }
    }
}
