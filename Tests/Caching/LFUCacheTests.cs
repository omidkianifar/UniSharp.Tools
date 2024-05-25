using NUnit.Framework;
using UniSharp.Tools.Caching.Factory;

namespace UniSharp.Tools.Tests.Caching
{
    public class LFUCacheTests
    {
        [Test]
        public void TestEvictionOrder()
        {
            var options = new CacheOptions { Type = CacheType.LFU, MaxCapacity = 2 };
            var cache = CacheFactory.GetOrCreateCache(options);

            cache.Set("key1", "value1");
            cache.Set("key2", "value2");
            // Access key1 twice to increase its frequency
            cache.Get("key1");
            cache.Get("key1");
            // Now key1 is the most frequently used, and key2 is the least
            // Adding another item should evict key2
            cache.Set("key3", "value3");

            Assert.IsNull(cache.Get("key2"), "key2 should have been evicted as the least frequently used");
            Assert.IsNotNull(cache.Get("key1"), "key1 should still exist as the most frequently used");
            Assert.IsNotNull(cache.Get("key3"), "key3 should exist");
        }

        [Test]
        public void TestFrequencyUpdateOnGet()
        {
            var options = new CacheOptions { Type = CacheType.LFU, MaxCapacity = 3 };
            var cache = CacheFactory.GetOrCreateCache(options);

            cache.Set("key1", "value1");
            cache.Set("key2", "value2");
            // Access key1 multiple times to increase its frequency
            cache.Get("key1");
            cache.Get("key1");

            // Add more items to potentially trigger eviction
            cache.Set("key3", "value3");
            cache.Set("key4", "value4");

            // key2 should be evicted since it has the lowest frequency
            Assert.IsNull(cache.Get("key2"), "key2 should be evicted as the least frequently used");
            Assert.IsNotNull(cache.Get("key1"), "key1 should still exist");
        }
    }
}
