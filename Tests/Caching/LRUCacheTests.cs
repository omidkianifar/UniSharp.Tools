using NUnit.Framework;
using UniSharp.Tools.Caching;
using UniSharp.Tools.Caching.Factory;

namespace UniSharp.Tools.Tests.Caching
{
    public class LRUCacheTests
    {
        private ICache _cache;

        [SetUp]
        public void SetUp()
        {
            // Initialize your cache before each test
            var options = new CacheOptions { Type = CacheType.LRU, MaxCapacity = 2 };
            _cache = CacheFactory.GetOrCreateCache(options);
        }

        [Test]
        public void TestEvictionOrder()
        {
            _cache.Set("key1", "value1");
            _cache.Set("key2", "value2");
            // Access key1 to make it recently used
            var val = _cache.Get("key1");
            // Add another item, causing key2 to be evicted (as key1 was recently accessed)
            _cache.Set("key3", "value3");

            Assert.IsNull(_cache.Get("key2"), "key2 should have been evicted");
            Assert.IsNotNull(_cache.Get("key1"), "key1 should still exist");
            Assert.IsNotNull(_cache.Get("key3"), "key3 should exist");
        }

        [Test]
        public void TestAccessUpdatesOrder()
        {
            _cache.Set("key1", "value1");
            _cache.Set("key2", "value2");
            // Access key1 to make it recently used
            var val = _cache.Get("key1");
            // This should make key1 the most recently used, so adding another item should evict key2
            _cache.Set("key3", "value3");

            Assert.IsNotNull(_cache.Get("key1"), "key1 was recently accessed, should not be evicted");
            Assert.IsNull(_cache.Get("key2"), "key2 should be evicted as the least recently used");
            Assert.IsNotNull(_cache.Get("key3"), "key3 should exist");
        }

    }
}
