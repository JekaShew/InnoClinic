
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace CommonLibrary.CommonService
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public T? GetData<T>(string key)
        {
            var data = _cache.GetString(key);
            if(data is null)
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(data);
        }

        public void SetData<T>(string key, T value, TimeSpan expirationTime)
        {
            var options = new DistributedCacheEntryOptions
            {
                //AbsoluteExpirationRelativeToNow = expirationTime,
                SlidingExpiration = expirationTime,
            };

            
            if (_cache is not null)
            {
                var data = JsonSerializer.Serialize(value);
                _cache.SetString(key, data, options);
            }
        }

        public void RemoveData(string key)
        {
           _cache.Remove(key);
        }
    }
}
