using UniSharp.Tools.Caching.Factory;
using System;
using System.Threading;

namespace UniSharp.Tools.Caching.Implementations
{
    /// <summary>
    /// Time-To-Live (TTL)
    /// A TTL policy automatically removes items after a certain period, regardless of their usage frequency or recency.
    /// </summary>
    internal class TTLCache : BasicCache
    {
        private Timer _cleanupTimer;

        public TTLCache(CacheOptions options) : base(options)
        {
            var interval = options.CleanupInterval.Value;
            _cleanupTimer = new Timer(CleanupExpiredItems, null, interval, interval);
        }

        private void CleanupExpiredItems(object state)
        {
            foreach (var key in _cache.Keys)
            {
                if (!_cache.TryGetValue(key, out CacheItem item)) continue;

                if (DateTime.Now >= item.ExpiryDate)
                {
                    _cache.TryRemove(key, out _);
                }
            }
        }

        // Override Dispose or Finalize if necessary to clean up the timer when the cache is disposed or finalized
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cleanupTimer?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~TTLCache()
        {
            Dispose(false);
        }
    }
}
