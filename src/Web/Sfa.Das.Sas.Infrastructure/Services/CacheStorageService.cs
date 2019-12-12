using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Sfa.Das.Sas.Infrastructure.Services
{
    public class CacheStorageService : ICacheStorageService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;

        public CacheStorageService(IDistributedCache distributedCache, IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
        }

        public Task DeleteFromCache(string key)
        {
            throw new NotImplementedException();
        }

        public async Task<T> RetrieveFromCache<T>(string key)
        {
            string json;

            var existsInMemory = _memoryCache.TryGetValue(key, out json);

            if (existsInMemory == false)
            {
                json = await _distributedCache.GetStringAsync(key);
                await SaveToMemoryCache(key, json);
            }

            return json == null ? default(T) : JsonConvert.DeserializeObject<T>(json);
        }

        public async Task SaveToCache<T>(string key, T item, TimeSpan absoluteExpiration, TimeSpan slidingExpiration)
        {
            var json = JsonConvert.SerializeObject(item);

            await SaveToDistributedCache(key, json, absoluteExpiration, slidingExpiration);
            await SaveToMemoryCache(key, json);
        }

        private async Task SaveToDistributedCache(string key, string item, TimeSpan absoluteExpiration, TimeSpan slidingExpiration)
        {
            await _distributedCache.SetStringAsync(key, item, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpiration,
                SlidingExpiration = slidingExpiration
            });
        }

        private Task SaveToMemoryCache(string key, string item)
        {
            _memoryCache.Set(key, item, new MemoryCacheEntryOptions()
            {
                Size = 1,
                AbsoluteExpirationRelativeToNow = new TimeSpan(0, 1, 0)
            });

            return Task.CompletedTask;
        }
    }
}
