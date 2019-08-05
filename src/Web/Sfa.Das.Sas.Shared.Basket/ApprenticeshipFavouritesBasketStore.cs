using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Sfa.Das.Sas.Shared.Basket.Interfaces;
using Sfa.Das.Sas.Shared.Basket.Models;

namespace Sfa.Das.Sas.Shared.Basket.Infrastructure
{
    internal class ApprenticeshipFavouritesBasketStore : IApprenticeshipFavouritesBasketStore
    {
        private const string CacheItemPrefix = "EmpFav-";
        private readonly IDistributedCache _cache;
        private readonly ApprenticehipFavouritesBasketStoreConfig _config;

        public ApprenticeshipFavouritesBasketStore(IDistributedCache cache, ApprenticehipFavouritesBasketStoreConfig config)
        {
            _cache = cache;
            _config = config;
        }

        public Task<ApprenticeshipFavouritesBasket> GetAsync(Guid basketId)
        {
            return RetrieveFromCache($"{CacheItemPrefix}{basketId}");
        }

        public Task UpdateAsync(Guid basketId, ApprenticeshipFavouritesBasket basket)
        {
            return SaveToCache($"{CacheItemPrefix}{basketId}", basket, new TimeSpan(_config.BasketSlidingExpiryDays, 0, 0, 0));
        }

        private Task SaveToCache(string key, ApprenticeshipFavouritesBasket item, TimeSpan slidingExpiration)
        {
            var json = JsonConvert.SerializeObject(item);

            var options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = slidingExpiration
            };

            return _cache.SetStringAsync(key, json, options);
        }

        private async Task<ApprenticeshipFavouritesBasket> RetrieveFromCache(string key)
        {
            var json = await _cache.GetStringAsync(key);

            return json == null ? null : JsonConvert.DeserializeObject<ApprenticeshipFavouritesBasket>(json);
        }
    }
}
