namespace UniSharp.Tools.Caching.Factory
{
    public enum CacheType
    {
        /// <summary>
        /// Basic Caching witout any Eviction Policies and has no Max-Capacity
        /// </summary>
        Basic,
        /// <summary>
        /// Least Recently Used (LRU)
        /// The LRU policy evicts the least recently accessed items first.
        /// It's based on the idea that items accessed recently are more likely to be accessed again in the near future.
        /// </summary>
        LRU,
        /// <summary>
        /// Least Frequently Used (LFU)
        /// The LFU policy evicts items that are used less frequently.
        /// Unlike LRU, it favors items that have been accessed multiple times, even if they were not accessed recently.
        /// </summary>
        LFU,
        /// <summary>
        /// Time-To-Live (TTL)
        /// A TTL policy automatically removes items after a certain period, regardless of their usage frequency or recency.
        /// </summary>
        TTL
    }
}
