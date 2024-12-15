using AhlApp.Shared.Caching;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace AhlApp.Infrastructure.Caching
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task SetCacheAsync<T>(string key, T value, TimeSpan expiration)
        {
            var serializedData = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, serializedData, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            });
        }

        public async Task<T> GetCacheAsync<T>(string key)
        {
            var data = await _cache.GetStringAsync(key);
            return string.IsNullOrEmpty(data) ? default : JsonSerializer.Deserialize<T>(data);
        }

        public async Task RemoveCacheAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
