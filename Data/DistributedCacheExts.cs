using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace WebAPI.Data
{
    public static class DistributedCacheExts
    {
        public static async Task SaveRecord<T>(this IDistributedCache cache, string key, T data, TimeSpan? slidingExp, TimeSpan? absoluteExp) where T : class
        {
            var opts = new DistributedCacheEntryOptions()
            {
                SlidingExpiration = slidingExp ?? TimeSpan.FromMinutes(1),
                AbsoluteExpirationRelativeToNow = absoluteExp ?? TimeSpan.FromMinutes(2)
            };

            string serializedData = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(key, serializedData);
        }

        public static async Task<List<T>> GetRecords<T>(this IDistributedCache cache, string key) where T : class
        {
            string serializedData = await cache.GetStringAsync(key);
            try
            {
                List<T> res = JsonSerializer.Deserialize<List<T>>(serializedData);

                return res;
            }
            catch
            {
                return default(List<T>);
            }
        }
    }
}
