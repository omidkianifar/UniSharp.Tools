using System;

namespace UniSharp.Tools.Caching.Factory
{
    public class CacheOptions
    {
        public CacheType Type { get; set; } = CacheType.Basic;
        public int Capacity { get; set; } = 100; // Default capacity
        public int MaxCapacity { get; set; } = 100;
        public TimeSpan? Expiry { get; set; } = TimeSpan.FromSeconds(30);
        public TimeSpan? CleanupInterval { get; set; } = TimeSpan.FromMinutes(5); // For TTL cache

        public static CacheOptions Default { get; } = new CacheOptions();
    }
}
