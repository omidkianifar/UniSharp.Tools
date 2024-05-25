using NUnit.Framework;
using System;
using System.Threading.Tasks;
using UniSharp.Tools.Caching.Factory;

namespace UniSharp.Tools.Tests.Caching
{
    public class TTLCacheTests
    {
        [Test]
        public async void TestItemExpiration()
        {
            var options = new CacheOptions { Type = CacheType.TTL, CleanupInterval = TimeSpan.FromSeconds(1) };
            var cache = CacheFactory.GetOrCreateCache(options);

            string key = "expiringKey";
            string value = "testValue";
            TimeSpan shortTTL = TimeSpan.FromMilliseconds(2000); // Short TTL to expedite the test

            cache.Set(key, value, shortTTL);

            // Wait for longer than the TTL
            await Task.Delay(shortTTL + TimeSpan.FromMilliseconds(2000));

            var result = cache.Get<string>(key);

            Assert.IsNull(result, "The item should have expired and been removed from the cache.");
        }

        [Test]
        public void TestCleanupMechanismEfficiency()
        {
            var options = new CacheOptions { Type = CacheType.TTL, CleanupInterval = TimeSpan.FromSeconds(1) };
            var cache = CacheFactory.GetOrCreateCache(options);
            int numberOfItems = 1000;
            TimeSpan itemTTL = TimeSpan.FromMilliseconds(10); // Very short TTL

            // Populate the cache with many items
            for (int i = 0; i < numberOfItems; i++)
            {
                cache.Set($"key{i}", $"value{i}", itemTTL);
            }

            // Measure performance or observe the cleanup process
            // This might involve waiting some time and then checking the internal state of the cache,
            // or using profiling tools to assess the impact of the cleanup process on performance.

            // Note: Direct performance measurement and asserting on it can be tricky and flaky,
            // especially in different environments. Use with caution.
        }
    }
}
