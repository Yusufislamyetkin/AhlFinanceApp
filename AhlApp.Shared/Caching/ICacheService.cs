namespace AhlApp.Shared.Caching
{
    public interface ICacheService
    {
        Task SetCacheAsync<T>(string key, T value, TimeSpan expiration);
        Task<T> GetCacheAsync<T>(string key);
        Task RemoveCacheAsync(string key);
    }
}
