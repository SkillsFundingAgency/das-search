using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Sfa.Das.Sas.ApplicationServices.Interfaces;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Configuration;

namespace Sfa.Das.Sas.Infrastructure.Basket
{
    public class ApprenticeshipFavouritesBasketStore : IApprenticeshipFavouritesBasketStore
    {
        private readonly IDistributedCache _cache;
        private readonly IApprenticehipFavouritesBasketStoreConfig _config;

        public ApprenticeshipFavouritesBasketStore(IDistributedCache cache, IApprenticehipFavouritesBasketStoreConfig config)
        {
            _cache = cache;
            _config = config;
        }

        public Task<ApprenticeshipFavouritesBasket> GetAsync(Guid basketId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Guid basketId, ApprenticeshipFavouritesBasket basket)
        {
            return SaveToCache(basketId.ToString(), basket, new TimeSpan(_config.BasketSlidingExpiryDays, 0, 0, 0));
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
