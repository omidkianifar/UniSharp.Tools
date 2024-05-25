using System;
using System.Threading.Tasks;

namespace UniSharp.Tools.Caching
{
    public interface ICache
    {
        object Get(string key);
        T Get<T>(string key);
        Task<object> GetAsync(string key);
        Task<T> GetAsync<T>(string key);

        void Set(string key, object value, TimeSpan? duration = null);
        void Set<T>(string key, T value, TimeSpan? duration = null);
        Task SetAsync(string key, object value, TimeSpan? duration = null);
        Task SetAsync<T>(string key, T value, TimeSpan? duration = null);

        bool Exists(string key);
        Task<bool> ExistsAsync(string key);

        void Remove(string key);
        Task RemoveAsync(string key);
    }
}
