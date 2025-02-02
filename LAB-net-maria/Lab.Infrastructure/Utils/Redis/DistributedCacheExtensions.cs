using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace Lab.Infrastructure.Utils.Redis
{
    public static class DistributedCacheExtensions
    {
        private static readonly DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                .SetAbsoluteExpiration(TimeSpan.FromHours(1));
        public static async Task SetAsync<T>(this IDistributedCache cache, string key, T value) =>
            await cache.SetAsync(
                key,
                Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value)),
                options);

        public static async Task<T?> GetAsync<T>(this IDistributedCache cache, string key)
        {
            var cachedBytes = await cache.GetAsync(key);

            if (cachedBytes is null)
                return default;

            return JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(cachedBytes));
        }
    }
}
