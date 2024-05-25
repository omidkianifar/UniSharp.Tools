using System;

namespace UniSharp.Tools.Caching.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CacheableAttribute : Attribute
    {
        public string CacheKey { get; }
        public TimeSpan? TimeToLive { get; }

        /// <summary>
        /// duration in seceonds
        /// </summary>
        /// <param name="duration"></param>
        public CacheableAttribute(string cacheKey, double duration)
        {
            CacheKey = cacheKey;

            TimeToLive = TimeSpan.FromSeconds(duration);
        }
    }

}
