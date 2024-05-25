using System;
using System.Collections.Concurrent;
using UniSharp.Tools.Caching.Implementations;

namespace UniSharp.Tools.Caching.Factory
{
    public static class CacheFactory
    {
        private static ConcurrentDictionary<string, ICache> _cacheInstances = new ConcurrentDictionary<string, ICache>();

        public static ICache GetOrCreateCache(CacheType cacheType)
        {
            var options = CacheOptions.Default;
            options.Type = cacheType;

            return GetOrCreateCache(options);
        }

        public static ICache GetOrCreateCache(CacheOptions options = null)
        {
            options ??= CacheOptions.Default;

            string cacheKey = GenerateCacheKey(options);

            return _cacheInstances.GetOrAdd(cacheKey, _ => CreateCache(options));
        }

        private static ICache CreateCache(CacheOptions options)
        {
            switch (options.Type)
            {
                case CacheType.Basic:
                    return new BasicCache(options);
                case CacheType.LRU:
                    return new LRUCache(options);
                case CacheType.LFU:
                    return new LFUCache(options);
                case CacheType.TTL:
                    if (options.CleanupInterval.HasValue)
                    {
                        return new TTLCache(options);
                    }
                    throw new ArgumentException("CleanupInterval must be provided for TTL cache.");
                default:
                    throw new NotImplementedException($"Cache type {options.Type} is not supported.");
            }
        }

        // Generate a unique key for each cache instance based on its configuration
        private static string GenerateCacheKey(CacheOptions options)
        {
            return $"{options.Type}-{options.Capacity}-{options.CleanupInterval}";
        }
    }
}
