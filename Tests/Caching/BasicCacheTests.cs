using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniSharp.Tools.Caching;
using UniSharp.Tools.Caching.Factory;

namespace UniSharp.Tools.Tests.Caching
{
    public class BasicCacheTests
    {
        private ICache _cache;

        [SetUp]
        public void SetUp()
        {
            // Initialize your cache before each test
            var options = new CacheOptions { Type = CacheType.Basic };
            _cache = CacheFactory.GetOrCreateCache(options);
        }

        [Test]
        public void TestAddAndGetItem()
        {
            string key = "testKey";
            string value = "testValue";
            TimeSpan duration = TimeSpan.FromMinutes(5);

            _cache.Set(key, value, duration);
            var result = _cache.Get<string>(key);

            Assert.AreEqual(value, result);
        }

        [Test]
        public void TestItemExpiration()
        {
            string key = "expiringKey";
            string value = "expiringValue";
            TimeSpan duration = TimeSpan.FromMilliseconds(100); // Short duration to test expiration

            _cache.Set(key, value, duration);
            Task.Delay(150).Wait(); // Wait for the item to expire

            var result = _cache.Get<string>(key);

            Assert.IsNull(result);
        }

        [Test]
        public void TestRemoveItem()
        {
            string key = "removableKey";
            string value = "removableValue";
            TimeSpan duration = TimeSpan.FromMinutes(5);

            _cache.Set(key, value, duration);
            _cache.Remove(key);

            var result = _cache.Get<string>(key);

            Assert.IsNull(result);
        }

        [Test]
        public void TestUpdateItem()
        {
            string key = "updateKey";
            string originalValue = "originalValue";
            string updatedValue = "updatedValue";
            TimeSpan duration = TimeSpan.FromMinutes(5);

            _cache.Set(key, originalValue, duration);
            _cache.Set(key, updatedValue, duration); // Update the value

            var result = _cache.Get<string>(key);

            Assert.AreEqual(updatedValue, result);
        }

        [Test]
        public void TestConcurrentWrites()
        {
            string key = "sharedKey";
            int numberOfTasks = 10;
            var tasks = new List<Task>();
            for (int i = 0; i < numberOfTasks; i++)
            {
                string value = $"value{i}";
                tasks.Add(Task.Run(() => _cache.Set(key, value, TimeSpan.FromMinutes(5))));
            }

            Task.WhenAll(tasks).Wait();

            // Retrieve the value after all concurrent writes
            var result = _cache.Get<string>(key);

            // This assertion checks that a value is present, but it cannot predict which write wins
            Assert.IsNotNull(result, "The cache should contain a value for 'sharedKey', but it was null.");
            // Note: This test does not assert which "value" wins because it's unpredictable without thread-safety mechanisms
        }
    }
}
