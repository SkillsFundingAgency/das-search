using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sfa.Das.Sas.ApplicationServices.Interfaces;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Configuration;

namespace Sfa.Das.Sas.Infrastructure.Basket
{
    public class ApprenticeshipFavouritesBasketStore : IApprenticeshipFavouritesBasketStore
    {
        private const string CacheItemPrefix = "EmpFav-";
        private readonly IDistributedCache _cache;
        private readonly IApprenticehipFavouritesBasketStoreConfig _config;

        public ApprenticeshipFavouritesBasketStore(IDistributedCache cache, IApprenticehipFavouritesBasketStoreConfig config)
        {
            _cache = cache;
            _config = config;
        }

        public Task<ApprenticeshipFavouritesBasketRead> GetAsync(Guid basketId)
        {
            return RetrieveFromCache($"{CacheItemPrefix}{basketId}");
        }

        public Task UpdateAsync(Guid basketId, ApprenticeshipFavouritesBasketWrite basket)
        {
            return SaveToCache($"{CacheItemPrefix}{basketId}", basket, new TimeSpan(_config.BasketSlidingExpiryDays, 0, 0, 0));
        }

        private Task SaveToCache(string key, ApprenticeshipFavouritesBasketWrite item, TimeSpan slidingExpiration)
        {
            var json = JsonConvert.SerializeObject(item);

            var options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = slidingExpiration
            };

            return _cache.SetStringAsync(key, json, options);
        }

        private async Task<ApprenticeshipFavouritesBasketRead> RetrieveFromCache(string key)
        {
            var json = await _cache.GetStringAsync(key);

            if (json == null)
            {
                return null;
            }

            var jsonObject = JObject.Parse(json);

            var basket = new ApprenticeshipFavouritesBasketRead();

            foreach (var item in jsonObject.Children())
            {
                var apprenticeship = new ApprenticeshipFavouriteRead(item["ApprenticeshipId"].Value<string>())
                {
                    Providers = JsonConvert.DeserializeObject<List<ApprenticeshipProviderFavourite>>(item["Ukprns"].ToString())
                };

            }

            return json == null ? null : basket;
        }



    }
}
